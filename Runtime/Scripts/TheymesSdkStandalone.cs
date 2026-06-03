using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Theymes
{
    internal static partial class TheymesSdkStandalone
    {
        private const int DefaultBreadcrumbsMaxSizeInBytes = 512000;
        private const int DefaultBreadcrumbsMaxItemCount = 1000;
        private const double DefaultBreadcrumbsMaxAgeInSeconds = 12 * 60 * 60;
        private const float SignedMetadataTokenExpirationUpdateIntervalSeconds = 60f;

        private static bool isInitialized = false;
        private static bool isInForeground = true;
        private static GameObject lifecycleHandler;
        private static string token;
        private static string domain;
        private static string apiDomain;
        private static string deviceId;
        private static string language;
        private static string signedMetadataToken;
        private static double? signedMetadataTokenExpiresAtSeconds;
        private static TheymesPlayer player;
        private static readonly HashSet<string> tags = new HashSet<string>();
        private static readonly Dictionary<string, object> fields = new Dictionary<string, object>();
        private static readonly List<BreadcrumbEntry> breadcrumbs = new List<BreadcrumbEntry>();
        private static readonly HashSet<string> unsupportedFeatureWarnings = new HashSet<string>();
        private static int currentBreadcrumbBytes = 0;
        private static bool youngPlayer = false;
        private static bool privacyMode = false;
        private static bool loggingEnabled = false;
        private static bool signedMetadataTokenExpirationUpdatesActive = false;
        private static float nextSignedMetadataTokenExpirationUpdateAt = -1f;

        private sealed class BreadcrumbEntry
        {
            public BreadcrumbEntry(string message, long timestampMs, int byteSize)
            {
                this.message = message;
                this.timestampMs = timestampMs;
                this.byteSize = byteSize;
            }

            public readonly string message;
            public readonly long timestampMs;
            public readonly int byteSize;
        }

        public static event Action onOpen;
        public static event Action<int> onSignedMetadataTokenExpirationUpdated;

        public static void Initialize(string token, string domain, InitializeOptions options)
        {
            if (isInitialized)
            {
                return;
            }

            TheymesSdkStandalone.token = token;
            TheymesSdkStandalone.domain = domain;
            TheymesSdkStandalone.apiDomain = ResolveApiDomain(domain, options);
            TheymesSdkStandalone.deviceId = GetOrCreateDeviceId(token, domain);
            isInitialized = true;
            SetupLifecycleHandler();
            LoadCachedSettings();
            PruneBreadcrumbs();
            StartSettingsFetchIfNeeded();
        }

        public static async Task OpenSupportAsync()
        {
            await OpenSupportAsync(null);
        }

        public static async Task OpenSupportAsync(TheymesConfig config)
        {
            await OpenUrlAsync(config, null);
        }

        public static async Task OpenResourceAsync(string resource)
        {
            await OpenResourceAsync(resource, null);
        }

        public static async Task OpenResourceAsync(string resource, TheymesConfig config)
        {
            if (string.IsNullOrEmpty(resource))
            {
                LogError("TheymesSdk.OpenResourceAsync requires a resource shortcut.");
                return;
            }

            await OpenUrlAsync(config, resource);
        }

        public static void Close()
        {
            LogUnsupportedStandaloneFeature("Close");
        }

        public static bool IsSupported()
        {
            return true;
        }

        public static void RequestNotificationPermission()
        {
            LogUnsupportedStandaloneFeature("RequestNotificationPermission");
        }

        public static void EnableNotifications()
        {
            LogUnsupportedStandaloneFeature("EnableNotifications");
        }

        public static void DisableNotifications()
        {
            LogUnsupportedStandaloneFeature("DisableNotifications");
        }

        public static int GetUnreadMessageCount()
        {
            LogUnsupportedStandaloneFeature("GetUnreadMessageCount");
            return 0;
        }

        public static int GetUnansweredMessageCount()
        {
            LogUnsupportedStandaloneFeature("GetUnansweredMessageCount");
            return 0;
        }

        public static void RecordRetention()
        {
            LogUnsupportedStandaloneFeature("RecordRetention");
        }

        public static void Reset()
        {
            language = null;
            signedMetadataToken = null;
            signedMetadataTokenExpiresAtSeconds = null;
            StopSignedMetadataTokenExpirationUpdates();
            player = null;
            tags.Clear();
            fields.Clear();
            ClearBreadcrumbs();
            youngPlayer = false;
            privacyMode = false;
        }

        public static string GetLanguage()
        {
            return language;
        }

        public static void SetLanguage(string language)
        {
            TheymesSdkStandalone.language = language;
        }

        public static string GetSignedMetadataToken()
        {
            return signedMetadataToken;
        }

        public static void SetSignedMetadataToken(string token)
        {
            signedMetadataToken = token;
            signedMetadataTokenExpiresAtSeconds = GetSignedMetadataTokenExpiresAtSeconds(token);
            StopSignedMetadataTokenExpirationUpdates();
            if (!string.IsNullOrEmpty(token))
            {
                StartSignedMetadataTokenExpirationUpdates();
            }
        }

        public static TheymesPlayer GetPlayer()
        {
            return player;
        }

        public static void SetPlayer(TheymesPlayer player)
        {
            TheymesSdkStandalone.player = player;
        }

        public static List<string> GetTags()
        {
            return new List<string>(tags);
        }

        public static void SetTags(IList<string> tags)
        {
            TheymesSdkStandalone.tags.Clear();
            AddTags(tags);
        }

        public static void AddTag(string tag)
        {
            if (tag != null)
            {
                tags.Add(tag);
            }
        }

        public static void AddTags(IList<string> tags)
        {
            if (tags == null)
            {
                return;
            }

            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }

        public static void RemoveTag(string tag)
        {
            if (tag != null)
            {
                tags.Remove(tag);
            }
        }

        public static void RemoveTags(IList<string> tags)
        {
            if (tags == null)
            {
                return;
            }

            foreach (var tag in tags)
            {
                RemoveTag(tag);
            }
        }

        public static void RemoveAllTags()
        {
            tags.Clear();
        }

        public static Dictionary<string, object> GetFields()
        {
            return new Dictionary<string, object>(fields);
        }

        public static void SetFields(IDictionary<string, object> fields)
        {
            TheymesSdkStandalone.fields.Clear();
            AddFields(fields);
        }

        public static void AddField(string key, object value)
        {
            if (key != null)
            {
                fields[key] = value;
            }
        }

        public static void AddFields(IDictionary<string, object> fields)
        {
            if (fields == null)
            {
                return;
            }

            foreach (var field in fields)
            {
                AddField(field.Key, field.Value);
            }
        }

        public static void RemoveField(string key)
        {
            if (key != null)
            {
                fields.Remove(key);
            }
        }

        public static void RemoveFields(IList<string> keys)
        {
            if (keys == null)
            {
                return;
            }

            foreach (var key in keys)
            {
                RemoveField(key);
            }
        }

        public static void RemoveAllFields()
        {
            fields.Clear();
        }

        public static void AddBreadcrumb(string breadcrumb)
        {
            if (breadcrumb == null)
            {
                return;
            }

            var byteSize = Encoding.UTF8.GetByteCount(breadcrumb);
            breadcrumbs.Add(new BreadcrumbEntry(breadcrumb, NowMs(), byteSize));
            currentBreadcrumbBytes += byteSize;
            PruneBreadcrumbs();
        }

        public static void AddBreadcrumbs(IList<string> breadcrumbs)
        {
            if (breadcrumbs == null)
            {
                return;
            }

            foreach (var breadcrumb in breadcrumbs)
            {
                AddBreadcrumb(breadcrumb);
            }
        }

        public static void ClearBreadcrumbs()
        {
            breadcrumbs.Clear();
            currentBreadcrumbBytes = 0;
        }

        public static void EnableLogging()
        {
            loggingEnabled = true;
        }

        public static void DisableLogging()
        {
            loggingEnabled = false;
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

        public static bool IsYoungPlayer()
        {
            return youngPlayer;
        }

        public static void SetYoungPlayer(bool youngPlayer)
        {
            TheymesSdkStandalone.youngPlayer = youngPlayer;
            if (youngPlayer)
            {
                privacyMode = true;
            }
        }

        public static bool IsPrivacyMode()
        {
            return privacyMode;
        }

        public static void SetPrivacyMode(bool privacyMode)
        {
            TheymesSdkStandalone.privacyMode = privacyMode;
        }

        public static void RegisterPushToken(string token, string type)
        {
            LogUnsupportedStandaloneFeature("RegisterPushToken");
        }

        public static bool IsTheymesNotification(IDictionary<string, string> data)
        {
            if (data == null)
            {
                return false;
            }

            if (!data.ContainsKey("source") || data["source"] != "theymes")
            {
                return false;
            }

            if (!data.ContainsKey("id") || !data.ContainsKey("title") || !data.ContainsKey("body"))
            {
                return false;
            }

            return true;
        }

        public static void HandleNotification(bool opened, IDictionary<string, string> data)
        {
            LogUnsupportedStandaloneFeature("HandleNotification");
        }

        public static bool HandlePendingNotificationAction(TheymesConfig config)
        {
            LogUnsupportedStandaloneFeature("HandlePendingNotificationAction");
            return false;
        }

        public static bool HasPendingNotificationAction()
        {
            LogUnsupportedStandaloneFeature("HasPendingNotificationAction");
            return false;
        }

        private static async Task OpenUrlAsync(TheymesConfig config, string shortcut)
        {
            try
            {
                var url = await CreateSessionUrlAsync(config, shortcut);
                Application.OpenURL(url);
                onOpen?.Invoke();
            }
            catch (Exception ex)
            {
                LogError($"Failed to open support URL: {ex}");
            }
        }

        private static async Task<string> CreateSessionUrlAsync(TheymesConfig config, string shortcut)
        {
            if (!isInitialized || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(apiDomain))
            {
                throw new InvalidOperationException("TheymesSdk must be initialized before opening support.");
            }

            var clientTimestampMs = NowMs();
            var request = new SimpleJSON.JSONObject();
            request["token"] = token;
            request["metadata"] = BuildMetadata(config);
            request["sdkVersion"] = TheymesSdk.SdkVersion;
            request["platform"] = GetPlatform();
            request["clientTimestampMs"] = new SimpleJSON.JSONNumber(clientTimestampMs);

            var resolvedLanguage = GetResolvedLanguage(config);
            if (resolvedLanguage != null)
            {
                request["language"] = resolvedLanguage;
            }

            var breadcrumbsJson = BuildBreadcrumbs(clientTimestampMs);
            if (breadcrumbsJson.Count > 0)
            {
                request["breadcrumbs"] = breadcrumbsJson;
            }

            if (shortcut != null)
            {
                request["shortcut"] = shortcut;
            }

            var response = await PostJsonAsync($"https://{domain}/sdk/create-session", request.ToString());
            var responseJson = SimpleJSON.JSON.Parse(response);
            if (responseJson == null || !responseJson.IsObject || responseJson["url"] == null || !responseJson["url"].IsString)
            {
                throw new InvalidOperationException("Invalid create-session response.");
            }

            return responseJson["url"].Value;
        }

        private static SimpleJSON.JSONObject BuildMetadata(TheymesConfig config)
        {
            var metadata = new SimpleJSON.JSONObject();
            var unsigned = new SimpleJSON.JSONObject();
            var resolvedPlayer = config != null && config.player != null ? config.player : player;
            var resolvedTags = GetResolvedTags(config);
            var resolvedFields = GetResolvedFields(config);
            var resolvedSignedMetadataToken = config != null && config.signedMetadataToken != null
                ? config.signedMetadataToken
                : signedMetadataToken;

            if (resolvedPlayer != null)
            {
                unsigned["player"] = TheymesJsonHelpers.PlayerToJsonObject(resolvedPlayer);
            }

            if (resolvedTags.Count > 0)
            {
                unsigned["tags"] = TheymesJsonHelpers.StringListToJsonArray(resolvedTags);
            }

            if (resolvedFields.Count > 0)
            {
                unsigned["fields"] = TheymesJsonHelpers.DictionaryToJsonObject(resolvedFields);
            }

            metadata["unsigned"] = unsigned;
            if (resolvedSignedMetadataToken != null)
            {
                metadata["signed"] = resolvedSignedMetadataToken;
            }

            return metadata;
        }

        private static string GetResolvedLanguage(TheymesConfig config)
        {
            return config != null && config.language != null ? config.language : language;
        }

        private static List<string> GetResolvedTags(TheymesConfig config)
        {
            var resolvedTags = new List<string>(tags);
            if (config == null || config.tags == null)
            {
                return resolvedTags;
            }

            foreach (var tag in config.tags)
            {
                if (tag != null && !resolvedTags.Contains(tag))
                {
                    resolvedTags.Add(tag);
                }
            }

            return resolvedTags;
        }

        private static Dictionary<string, object> GetResolvedFields(TheymesConfig config)
        {
            var resolvedFields = new Dictionary<string, object>(fields);
            if (config == null || config.fields == null)
            {
                return AddBuiltinFields(resolvedFields);
            }

            foreach (var field in config.fields)
            {
                resolvedFields[field.Key] = field.Value;
            }

            return AddBuiltinFields(resolvedFields);
        }

        private static Dictionary<string, object> AddBuiltinFields(Dictionary<string, object> resolvedFields)
        {
            resolvedFields["_platform"] = GetPlatform();
            resolvedFields["_sdkVersion"] = TheymesSdk.SdkVersion;

            foreach (var field in TheymesSdk.GetUnityBuiltinFields())
            {
                resolvedFields[field.Key] = field.Value;
            }

            if (!string.IsNullOrEmpty(SystemInfo.deviceModel))
            {
                resolvedFields["_model"] = SystemInfo.deviceModel;
            }

            if (!string.IsNullOrEmpty(SystemInfo.operatingSystem))
            {
                resolvedFields["_osVersion"] = SystemInfo.operatingSystem;
            }

            if (!string.IsNullOrEmpty(Application.version))
            {
                resolvedFields["_appVersion"] = Application.version;
            }

            if (youngPlayer)
            {
                resolvedFields["_youngPlayer"] = true;
            }

            if (privacyMode || youngPlayer)
            {
                resolvedFields["_privacyMode"] = true;
            }

            return resolvedFields;
        }

        private static SimpleJSON.JSONArray BuildBreadcrumbs(long clientTimestampMs)
        {
            PruneBreadcrumbs(clientTimestampMs);
            var result = new SimpleJSON.JSONArray();
            foreach (var breadcrumb in breadcrumbs)
            {
                var item = new SimpleJSON.JSONArray();
                item.Add(breadcrumb.message);
                item.Add(new SimpleJSON.JSONNumber(breadcrumb.timestampMs));
                result.Add(item);
            }

            return result;
        }

        private static async Task<string> PostJsonAsync(string url, string json)
        {
            var body = Encoding.UTF8.GetBytes(json);
            using (var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
            {
                LogInfo("Creating support session before opening browser");
                request.uploadHandler = new UploadHandlerRaw(body);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                await SendWebRequestAsync(request);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    throw new InvalidOperationException($"Create support session request failed: {request.error}");
                }

                if (request.responseCode < 200 || request.responseCode >= 300)
                {
                    throw new InvalidOperationException(
                        $"Create support session request failed with status {request.responseCode}: {request.downloadHandler.text}"
                    );
                }

                return request.downloadHandler.text;
            }
        }

        private static Task SendWebRequestAsync(UnityWebRequest request)
        {
            var completion = new TaskCompletionSource<bool>();
            var operation = request.SendWebRequest();
            operation.completed += _ => completion.TrySetResult(true);
            return completion.Task;
        }

        private static string ResolveApiDomain(string domain, InitializeOptions options)
        {
            if (options != null && !string.IsNullOrEmpty(options.apiDomain))
            {
                return options.apiDomain;
            }

            if (string.IsNullOrEmpty(domain))
            {
                return null;
            }

            var dotIndex = domain.IndexOf(".", StringComparison.Ordinal);
            if (dotIndex < 0)
            {
                return null;
            }

            return domain.Substring(0, dotIndex) + ".sdk" + domain.Substring(dotIndex);
        }

        private static string GetOrCreateDeviceId(string token, string domain)
        {
            var key = $"theymes.deviceId.{token}.{domain}";
            var storedDeviceId = PlayerPrefs.GetString(key, null);
            if (!string.IsNullOrEmpty(storedDeviceId))
            {
                return storedDeviceId;
            }

            var newDeviceId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString(key, newDeviceId);
            PlayerPrefs.Save();
            return newDeviceId;
        }

        private static string GetPlatform()
        {
#if UNITY_EDITOR && UNITY_IOS
            return "ios";
#elif UNITY_EDITOR && UNITY_ANDROID
            return "android";
#elif UNITY_EDITOR && UNITY_WEBGL
            return "web";
#elif UNITY_STANDALONE_WIN
            return "win";
#elif UNITY_STANDALONE_OSX
            return "macos";
#elif UNITY_STANDALONE_LINUX
            return "linux";
#else
            return "web";
#endif
        }

        private static long NowMs()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        private static void SetupLifecycleHandler()
        {
            if (lifecycleHandler != null)
            {
                return;
            }

            lifecycleHandler = new GameObject("TheymesStandaloneLifecycleHandler");
            lifecycleHandler.hideFlags = HideFlags.HideAndDontSave;
            UnityEngine.Object.DontDestroyOnLoad(lifecycleHandler);
            lifecycleHandler.AddComponent<TheymesStandaloneLifecycleHandler>();
        }

        private static void TickSignedMetadataTokenExpirationUpdates()
        {
            if (!signedMetadataTokenExpirationUpdatesActive || !isInForeground)
            {
                return;
            }

            if (Time.realtimeSinceStartup < nextSignedMetadataTokenExpirationUpdateAt)
            {
                return;
            }

            if (!EmitSignedMetadataTokenExpirationUpdate())
            {
                StopSignedMetadataTokenExpirationUpdates();
                return;
            }

            nextSignedMetadataTokenExpirationUpdateAt = Time.realtimeSinceStartup + SignedMetadataTokenExpirationUpdateIntervalSeconds;
        }

        private static void StartSignedMetadataTokenExpirationUpdates()
        {
            StopSignedMetadataTokenExpirationUpdates();

            if (string.IsNullOrEmpty(signedMetadataToken) || !isInForeground)
            {
                return;
            }

            if (!EmitSignedMetadataTokenExpirationUpdate())
            {
                return;
            }

            signedMetadataTokenExpirationUpdatesActive = true;
            nextSignedMetadataTokenExpirationUpdateAt = Time.realtimeSinceStartup + SignedMetadataTokenExpirationUpdateIntervalSeconds;
        }

        private static void StopSignedMetadataTokenExpirationUpdates()
        {
            signedMetadataTokenExpirationUpdatesActive = false;
            nextSignedMetadataTokenExpirationUpdateAt = -1f;
        }

        private static bool EmitSignedMetadataTokenExpirationUpdate()
        {
            var expiresInSeconds = GetSignedMetadataTokenExpiresInSeconds();
            if (!expiresInSeconds.HasValue)
            {
                return false;
            }

            onSignedMetadataTokenExpirationUpdated?.Invoke(expiresInSeconds.Value);
            return true;
        }

        private static int? GetSignedMetadataTokenExpiresInSeconds()
        {
            if (!signedMetadataTokenExpiresAtSeconds.HasValue)
            {
                return null;
            }

            var expiresInSeconds = signedMetadataTokenExpiresAtSeconds.Value - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return (int)Math.Floor(expiresInSeconds);
        }

        private static double? GetSignedMetadataTokenExpiresAtSeconds(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var parts = token.Split('.');
            if (parts.Length < 2)
            {
                return null;
            }

            try
            {
                var base64 = parts[1].Replace('-', '+').Replace('_', '/');
                var remainder = base64.Length % 4;
                if (remainder == 2)
                {
                    base64 += "==";
                }
                else if (remainder == 3)
                {
                    base64 += "=";
                }
                else if (remainder != 0)
                {
                    return null;
                }

                var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                var payload = SimpleJSON.JSON.Parse(payloadJson);
                if (payload == null || !payload.IsObject || !payload.HasKey("exp") || !payload["exp"].IsNumber)
                {
                    return null;
                }

                return payload["exp"].AsDouble;
            }
            catch
            {
                return null;
            }
        }

        private static void PruneBreadcrumbs()
        {
            PruneBreadcrumbs(NowMs());
        }

        private static void PruneBreadcrumbs(long nowMs)
        {
            var minTimestampMs = nowMs - (long)(breadcrumbsMaxAgeInSeconds * 1000);
            while (breadcrumbs.Count > 0 && breadcrumbs[0].timestampMs < minTimestampMs)
            {
                RemoveOldestBreadcrumb();
            }

            while (breadcrumbs.Count > breadcrumbsMaxItemCount)
            {
                RemoveOldestBreadcrumb();
            }

            while (breadcrumbs.Count > 0 && currentBreadcrumbBytes > breadcrumbsMaxSizeInBytes)
            {
                RemoveOldestBreadcrumb();
            }
        }

        private static void RemoveOldestBreadcrumb()
        {
            var breadcrumb = breadcrumbs[0];
            breadcrumbs.RemoveAt(0);
            currentBreadcrumbBytes -= breadcrumb.byteSize;
        }

        private static void LogUnsupportedStandaloneFeature(string feature)
        {
            if (!loggingEnabled)
            {
                return;
            }

            if (!unsupportedFeatureWarnings.Add(feature))
            {
                return;
            }

            if (IsSupportedNativeEditorTarget())
            {
                LogWarning(
                    $"{feature} does nothing while running in the Unity Editor for {GetPlatformDisplayName()}. " +
                    $"Run the game in the native {GetPlatformDisplayName()} build to test this feature."
                );
                return;
            }

            LogWarning($"{feature} is not supported on {GetPlatformDisplayName()}.");
        }

        private static bool IsSupportedNativeEditorTarget()
        {
#if UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL
            return true;
#else
            return false;
#endif
        }

        private static string GetPlatformDisplayName()
        {
            switch (GetPlatform())
            {
                case "win":
                    return "Windows";
                case "macos":
                    return "macOS";
                case "linux":
                    return "Linux";
                case "ios":
                    return "iOS";
                case "android":
                    return "Android";
                case "web":
                    return "WebGL";
                default:
                    return GetPlatform();
            }
        }

        private sealed class TheymesStandaloneLifecycleHandler : MonoBehaviour
        {
            private void OnApplicationPause(bool isPaused)
            {
                isInForeground = !isPaused;
                if (isPaused)
                {
                    StopSignedMetadataTokenExpirationUpdates();
                }
                else
                {
                    StartSignedMetadataTokenExpirationUpdates();
                }
            }

            private void Update()
            {
                TickSignedMetadataTokenExpirationUpdates();
            }
        }
    }
}
