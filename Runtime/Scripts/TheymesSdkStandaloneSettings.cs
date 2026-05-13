using System;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Theymes
{
    internal static partial class TheymesSdkStandalone
    {
        private const double MaxSettingsRetryDelaySecs = 86400;
        private const double MaxSettingsPollAtOffsetSecs = 4 * 7 * 24 * 60 * 60;

        private static readonly object settingsLock = new object();
        private static int breadcrumbsMaxSizeInBytes = DefaultBreadcrumbsMaxSizeInBytes;
        private static int breadcrumbsMaxItemCount = DefaultBreadcrumbsMaxItemCount;
        private static double breadcrumbsMaxAgeInSeconds = DefaultBreadcrumbsMaxAgeInSeconds;
        private static bool isFetchingSettings = false;
        private static bool hasCompletedSettingsFetch = false;
        private static int currentSettingsAttempt = 0;

        private static void LoadCachedSettings()
        {
            lock (settingsLock)
            {
                breadcrumbsMaxSizeInBytes = GetCachedPositiveInt(
                    "breadcrumbsMaxSizeInBytes",
                    DefaultBreadcrumbsMaxSizeInBytes
                );
                breadcrumbsMaxItemCount = GetCachedPositiveInt(
                    "breadcrumbsMaxItemCount",
                    DefaultBreadcrumbsMaxItemCount
                );
                breadcrumbsMaxAgeInSeconds = GetCachedPositiveDouble(
                    "breadcrumbsMaxAgeInSeconds",
                    DefaultBreadcrumbsMaxAgeInSeconds
                );
            }
        }

        private static void StartSettingsFetchIfNeeded()
        {
            if (!ShouldFetchSettings())
            {
                LogInfo("Settings do not need to be fetched");
                return;
            }

            _ = FetchSettingsIfNeededAsync();
        }

        private static bool ShouldFetchSettings()
        {
            return isInitialized
                && !isFetchingSettings
                && !hasCompletedSettingsFetch
                && !string.IsNullOrEmpty(token)
                && !string.IsNullOrEmpty(apiDomain);
        }

        private static async Task FetchSettingsIfNeededAsync()
        {
            if (!ShouldFetchSettings())
            {
                LogInfo("Settings do not need to be fetched");
                return;
            }

            if (ShouldSkipSettingsFetchForCachedPollAt())
            {
                return;
            }

            isFetchingSettings = true;
            currentSettingsAttempt = 0;
            LogInfo("Fetching settings");

            while (!hasCompletedSettingsFetch)
            {
                try
                {
                    await FetchSettingsOnceAsync();
                    hasCompletedSettingsFetch = true;
                    isFetchingSettings = false;
                    return;
                }
                catch (Exception ex)
                {
                    LogError($"Error fetching settings: {ex.Message}");
                    currentSettingsAttempt++;

                    var retryDelayMs = GetSettingsRetryDelayMs();
                    LogInfo($"Retrying settings fetch in {retryDelayMs / 1000} seconds (attempt {currentSettingsAttempt})");
                    await Task.Delay(retryDelayMs);
                }
            }

            isFetchingSettings = false;
        }

        private static bool ShouldSkipSettingsFetchForCachedPollAt()
        {
            var nowSecs = NowMs() / 1000.0;
            var settingsNextPollAt = GetCachedDouble("settingsNextPollAt", 0);
            if (settingsNextPollAt <= 0)
            {
                return false;
            }

            var maxNextPollAt = nowSecs + MaxSettingsPollAtOffsetSecs;
            if (settingsNextPollAt > maxNextPollAt)
            {
                settingsNextPollAt = maxNextPollAt;
                StoreDouble("settingsNextPollAt", settingsNextPollAt);
                PlayerPrefs.Save();
            }

            if (nowSecs >= settingsNextPollAt)
            {
                return false;
            }

            var nextFetchAt = DateTimeOffset.FromUnixTimeSeconds((long)Math.Floor(settingsNextPollAt)).UtcDateTime;
            LogInfo($"Settings do not need to be fetched. Next fetch at {nextFetchAt:o}");
            return true;
        }

        private static int GetSettingsRetryDelayMs()
        {
            var delaySeconds = Math.Min(Math.Pow(2.0, currentSettingsAttempt + 5), MaxSettingsRetryDelaySecs);
            return (int)(delaySeconds * 1000);
        }

        private static async Task FetchSettingsOnceAsync()
        {
            var url =
                $"https://{apiDomain}/api/settings?token={Uri.EscapeDataString(token)}&platform={Uri.EscapeDataString(GetPlatform())}&sdkVersion={Uri.EscapeDataString(SdkVersion)}";
            var response = await GetJsonAsync(url);
            LogInfo($"Received settings: {response}");

            var responseJson = SimpleJSON.JSON.Parse(response);
            if (responseJson == null || !responseJson.IsObject)
            {
                throw new InvalidOperationException("Invalid settings response.");
            }

            ApplySettingsResponse(responseJson);
            LogInfo("Settings applied");
        }

        private static async Task<string> GetJsonAsync(string url)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                await SendWebRequestAsync(request);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new InvalidOperationException($"Settings request failed: {request.error}");
                }

                if (request.responseCode < 200 || request.responseCode >= 300)
                {
                    throw new InvalidOperationException(
                        $"Settings request failed with status {request.responseCode}: {request.downloadHandler.text}"
                    );
                }

                return request.downloadHandler.text;
            }
        }

        private static void ApplySettingsResponse(SimpleJSON.JSONNode responseJson)
        {
            var shouldPruneBreadcrumbs = false;

            StoreMaybeDoubleFromResponse(responseJson, "recordRetentionAfterSecs");
            StoreMaybeDoubleFromResponse(responseJson, "resetSessionAfterSecs");
            StoreEnumStringFromResponse(responseJson, "sendPushToken");
            StoreEnumStringFromResponse(responseJson, "sendPlayerRetention");

            int sizeInBytes;
            if (TryGetPositiveInt(responseJson, "breadcrumbsMaxSizeInBytes", out sizeInBytes))
            {
                UpdateBreadcrumbsMaxSizeInBytes(sizeInBytes);
                shouldPruneBreadcrumbs = true;
                LogInfo($"Updated breadcrumbs max size in bytes: {sizeInBytes}");
            }

            int itemCount;
            if (TryGetPositiveInt(responseJson, "breadcrumbsMaxItemCount", out itemCount))
            {
                UpdateBreadcrumbsMaxItemCount(itemCount);
                shouldPruneBreadcrumbs = true;
                LogInfo($"Updated breadcrumbs max item count: {itemCount}");
            }

            double ageInSeconds;
            if (TryGetPositiveDouble(responseJson, "breadcrumbsMaxAgeInSeconds", out ageInSeconds))
            {
                UpdateBreadcrumbsMaxAgeInSeconds(ageInSeconds);
                shouldPruneBreadcrumbs = true;
                LogInfo($"Updated breadcrumbs max age in seconds: {ageInSeconds}");
            }

            double pollIntervalSecs;
            if (TryGetPositiveDouble(responseJson, "settingsPollIntervalSecs", out pollIntervalSecs))
            {
                var nowSecs = NowMs() / 1000.0;
                var nextPollAt = Math.Min(nowSecs + pollIntervalSecs, nowSecs + MaxSettingsPollAtOffsetSecs);
                StoreDouble("settingsNextPollAt", nextPollAt);
                LogInfo($"Updated settings poll interval in seconds: {pollIntervalSecs}");
            }

            PlayerPrefs.Save();

            if (shouldPruneBreadcrumbs)
            {
                PruneBreadcrumbs();
            }
        }

        private static void UpdateBreadcrumbsMaxSizeInBytes(int value)
        {
            lock (settingsLock)
            {
                breadcrumbsMaxSizeInBytes = value;
                StoreInt("breadcrumbsMaxSizeInBytes", value);
            }
        }

        private static void UpdateBreadcrumbsMaxItemCount(int value)
        {
            lock (settingsLock)
            {
                breadcrumbsMaxItemCount = value;
                StoreInt("breadcrumbsMaxItemCount", value);
            }
        }

        private static void UpdateBreadcrumbsMaxAgeInSeconds(double value)
        {
            lock (settingsLock)
            {
                breadcrumbsMaxAgeInSeconds = value;
                StoreDouble("breadcrumbsMaxAgeInSeconds", value);
            }
        }

        private static bool TryGetPositiveInt(SimpleJSON.JSONNode json, string key, out int value)
        {
            value = 0;
            if (!json.HasKey(key) || !json[key].IsNumber)
            {
                return false;
            }

            value = json[key].AsInt;
            return value > 0;
        }

        private static bool TryGetPositiveDouble(SimpleJSON.JSONNode json, string key, out double value)
        {
            value = 0;
            if (!json.HasKey(key) || !json[key].IsNumber)
            {
                return false;
            }

            value = json[key].AsDouble;
            return value > 0;
        }

        private static void StoreMaybeDoubleFromResponse(SimpleJSON.JSONNode json, string key)
        {
            if (!json.HasKey(key))
            {
                return;
            }

            if (json[key].IsNull)
            {
                DeleteCachedValue(key);
                return;
            }

            if (json[key].IsNumber)
            {
                StoreDouble(key, json[key].AsDouble);
            }
        }

        private static void StoreEnumStringFromResponse(SimpleJSON.JSONNode json, string key)
        {
            if (!json.HasKey(key) || !json[key].IsString)
            {
                return;
            }

            var value = json[key].Value;
            if (value == "always" || value == "support-only" || value == "never")
            {
                StoreString(key, value);
            }
        }

        private static int GetCachedPositiveInt(string name, int defaultValue)
        {
            var key = GetConfigKey(name);
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }

            var value = PlayerPrefs.GetInt(key, defaultValue);
            return value > 0 ? value : defaultValue;
        }

        private static double GetCachedPositiveDouble(string name, double defaultValue)
        {
            var value = GetCachedDouble(name, defaultValue);
            return value > 0 ? value : defaultValue;
        }

        private static double GetCachedDouble(string name, double defaultValue)
        {
            var key = GetConfigKey(name);
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }

            var storedValue = PlayerPrefs.GetString(key, null);
            double value;
            return double.TryParse(storedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                ? value
                : defaultValue;
        }

        private static void StoreInt(string name, int value)
        {
            PlayerPrefs.SetInt(GetConfigKey(name), value);
        }

        private static void StoreDouble(string name, double value)
        {
            PlayerPrefs.SetString(GetConfigKey(name), value.ToString(CultureInfo.InvariantCulture));
        }

        private static void StoreString(string name, string value)
        {
            PlayerPrefs.SetString(GetConfigKey(name), value);
        }

        private static void DeleteCachedValue(string name)
        {
            PlayerPrefs.DeleteKey(GetConfigKey(name));
        }

        private static string GetConfigKey(string name)
        {
            return $"theymes.config.{token}.{domain}.{name}";
        }

        private static void LogInfo(string message)
        {
            if (loggingEnabled)
            {
                Console.WriteLine($"Theymes SDK: {message}");
            }
        }

        private static void LogWarning(string message)
        {
            if (loggingEnabled)
            {
                Console.WriteLine($"Theymes SDK: {message}");
            }
        }

        private static void LogError(string message)
        {
            if (loggingEnabled)
            {
                Console.Error.WriteLine($"Theymes SDK: {message}");
            }
        }
    }
}
