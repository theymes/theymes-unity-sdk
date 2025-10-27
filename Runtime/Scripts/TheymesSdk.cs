using UnityEngine;
using System.Collections.Generic;

namespace Theymes
{
    public class TheymesSdk : MonoBehaviour
    {
        public static event System.Action onOpen;
        public static event System.Action onClose;
        public static event System.Action<int> onUnreadMessageCountUpdated;
        public static event System.Action<int> onUnansweredMessageCountUpdated;

        private static bool isInitialized = false;

        public static void Initialize(string token, string domain)
        {
            // never initialize more than once
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;

            _SetupEventListeners();

            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.Initialize(token, domain);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.Initialize(token, domain);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.Initialize(token, domain);
            #endif

            SetFields(
                new Dictionary<string, object>
                {
                    { "_unitySystemMemory", SystemInfo.systemMemorySize },
                    { "_unityGraphicsMemory", SystemInfo.graphicsMemorySize },
                }
            );
        }

        public static void OpenSupport()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.OpenSupport();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.OpenSupport();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.OpenSupport();
            #endif
        }

        public static void OpenSupport(TheymesConfig config)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.OpenSupport(TheymesJsonHelpers.ConfigToJson(config));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.OpenSupport(TheymesJsonHelpers.ConfigToJson(config));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.OpenSupport(TheymesJsonHelpers.ConfigToJson(config));
            #endif
        }

        public static void OpenResource(string resource)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.OpenResource(resource);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.OpenResource(resource);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.OpenResource(resource);
            #endif
        }

        public static void OpenResource(string resource, TheymesConfig config)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.OpenResource(resource, TheymesJsonHelpers.ConfigToJson(config));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.OpenResource(resource, TheymesJsonHelpers.ConfigToJson(config));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.OpenResource(resource, TheymesJsonHelpers.ConfigToJson(config));
            #endif
        }

        public static void Close()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.Close();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.Close();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.Close();
            #endif
        }

        public static string GetSdkVersion()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.GetSdkVersion();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.GetSdkVersion();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesUnityWebGLBridge.GetSdkVersion();
            #else
            return null;
            #endif
        }

        public static bool IsSupported()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.IsSupported();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.IsSupported();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesUnityWebGLBridge.IsSupported();
            #else
            return false;
            #endif
        }

        public static void RequestNotificationPermission()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RequestNotificationPermission();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RequestNotificationPermission();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.RequestNotificationPermission();
            #endif
        }

        public static void EnableNotifications()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.EnableNotifications();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.EnableNotifications();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.EnableNotifications();
            #endif
        }

        public static void DisableNotifications()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.DisableNotifications();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.DisableNotifications();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.DisableNotifications();
            #endif
        }

        public static int GetUnreadMessageCount()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.GetUnreadMessageCount();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.GetUnreadMessageCount();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesUnityWebGLBridge.GetUnreadMessageCount();
            #else
            return 0;
            #endif
        }

        public static int GetUnansweredMessageCount()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.GetUnansweredMessageCount();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.GetUnansweredMessageCount();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesUnityWebGLBridge.GetUnansweredMessageCount();
            #else
            return 0;
            #endif
        }

        public static void RecordRetention()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RecordRetention();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RecordRetention();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.RecordRetention();
            #endif
        }

        public static void Reset()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.Reset();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.Reset();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.Reset();
            #endif
        }

        public static string GetLanguage()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.GetLanguage();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.GetLanguage();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesUnityWebGLBridge.GetLanguage();
            #else
            return null;
            #endif
        }

        public static void SetLanguage(string language)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.SetLanguage(language);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.SetLanguage(language);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.SetLanguage(language);
            #endif
        }

        public static string GetSignedMetadataToken()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.GetSignedMetadataToken();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.GetSignedMetadataToken();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesUnityWebGLBridge.GetSignedMetadataToken();
            #else
            return null;
            #endif
        }

        public static void SetSignedMetadataToken(string token)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.SetSignedMetadataToken(token);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.SetSignedMetadataToken(token);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.SetSignedMetadataToken(token);
            #endif
        }

        public static TheymesPlayer GetPlayer()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToPlayer(TheymesUnityIosBridge.GetPlayer());
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToPlayer(TheymesUnityAndroidBridge.GetPlayer());
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToPlayer(TheymesUnityWebGLBridge.GetPlayer());
            #else
            return null;
            #endif
        }

        public static void SetPlayer(TheymesPlayer player)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.SetPlayer(TheymesJsonHelpers.PlayerToJson(player));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.SetPlayer(TheymesJsonHelpers.PlayerToJson(player));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.SetPlayer(TheymesJsonHelpers.PlayerToJson(player));
            #endif
        }

        public static List<string> GetTags()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToStringList(TheymesUnityIosBridge.GetTags());
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToStringList(TheymesUnityAndroidBridge.GetTags());
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToStringList(TheymesUnityWebGLBridge.GetTags());
            #else
            return null;
            #endif
        }

        public static void SetTags(IList<string> tags)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.SetTags(TheymesJsonHelpers.StringListToJson(tags));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.SetTags(TheymesJsonHelpers.StringListToJson(tags));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.SetTags(TheymesJsonHelpers.StringListToJson(tags));
            #endif
        }

        public static void AddTag(string tag)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.AddTag(tag);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.AddTag(tag);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.AddTag(tag);
            #endif
        }

        public static void AddTags(IList<string> tags)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.AddTags(TheymesJsonHelpers.StringListToJson(tags));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.AddTags(TheymesJsonHelpers.StringListToJson(tags));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.AddTags(TheymesJsonHelpers.StringListToJson(tags));
            #endif
        }

        public static void RemoveTag(string tag)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RemoveTag(tag);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RemoveTag(tag);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.RemoveTag(tag);
            #endif
        }

        public static void RemoveTags(IList<string> tags)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RemoveTags(TheymesJsonHelpers.StringListToJson(tags));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RemoveTags(TheymesJsonHelpers.StringListToJson(tags));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.RemoveTags(TheymesJsonHelpers.StringListToJson(tags));
            #endif
        }

        public static void RemoveAllTags()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RemoveAllTags();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RemoveAllTags();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.RemoveAllTags();
            #endif
        }

        public static Dictionary<string, object> GetFields()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToDictionary(TheymesUnityIosBridge.GetFields());
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToDictionary(TheymesUnityAndroidBridge.GetFields());
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesJsonHelpers.JsonToDictionary(TheymesUnityWebGLBridge.GetFields());
            #else
            return null;
            #endif
        }

        public static void SetFields(IDictionary<string, object> fields)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.SetFields(TheymesJsonHelpers.DictionaryToJson(fields));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.SetFields(TheymesJsonHelpers.DictionaryToJson(fields));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.SetFields(TheymesJsonHelpers.DictionaryToJson(fields));
            #endif
        }

        public static void AddField(string key, object value)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.AddField(key, TheymesJsonHelpers.ObjectToJson(value));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.AddField(key, TheymesJsonHelpers.ObjectToJson(value));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.AddField(key, TheymesJsonHelpers.ObjectToJson(value));
            #endif
        }

        public static void AddFields(IDictionary<string, object> fields)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.AddFields(TheymesJsonHelpers.DictionaryToJson(fields));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.AddFields(TheymesJsonHelpers.DictionaryToJson(fields));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.AddFields(TheymesJsonHelpers.DictionaryToJson(fields));
            #endif
        }

        public static void RemoveField(string key)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RemoveField(key);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RemoveField(key);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.RemoveField(key);
            #endif
        }

        public static void RemoveFields(IList<string> keys)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RemoveFields(TheymesJsonHelpers.StringListToJson(keys));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RemoveFields(TheymesJsonHelpers.StringListToJson(keys));
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.RemoveFields(TheymesJsonHelpers.StringListToJson(keys));
            #endif
        }

        public static void RemoveAllFields()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RemoveAllFields();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RemoveAllFields();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.RemoveAllFields();
            #endif
        }

        public static void EnableLogging()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.EnableLogging();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.EnableLogging();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.EnableLogging();
            #endif
        }

        public static void DisableLogging()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.DisableLogging();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.DisableLogging();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.DisableLogging();
            #endif
        }

        public static bool IsYoungPlayer()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.IsYoungPlayer();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.IsYoungPlayer();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesUnityWebGLBridge.IsYoungPlayer();
            #else
            return false;
            #endif
        }

        public static void SetYoungPlayer(bool youngPlayer)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.SetYoungPlayer(youngPlayer);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.SetYoungPlayer(youngPlayer);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.SetYoungPlayer(youngPlayer);
            #endif
        }

        public static bool IsPrivacyMode()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.IsPrivacyMode();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.IsPrivacyMode();
            #elif UNITY_WEBGL && !UNITY_EDITOR
            return TheymesUnityWebGLBridge.IsPrivacyMode();
            #else
            return false;
            #endif
        }

        public static void SetPrivacyMode(bool privacyMode)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.SetPrivacyMode(privacyMode);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.SetPrivacyMode(privacyMode);
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.SetPrivacyMode(privacyMode);
            #endif
        }

        public static void RegisterPushToken(string token, string type)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.RegisterPushToken(token, type);
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.RegisterPushToken(token, type);
            #endif
        }

        public static bool IsTheymesNotification(IDictionary<string, string> data)
        {
            if (data == null) {
                return false;
            }

            if (!data.ContainsKey("source") || data["source"] != "theymes") {
                return false;
            }

            if (!data.ContainsKey("id") || !data.ContainsKey("title") || !data.ContainsKey("body")) {
                return false;
            }

            return true;
        }

        public static void HandleNotification(bool opened, IDictionary<string, string> data)
        {
            if (!IsTheymesNotification(data)) {
                return;
            }

            // on iOS the notifications are automatically handled
            #if UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.HandleNotification(opened, TheymesJsonHelpers.DictionaryToJson(data));
            #endif
        }

        public static bool HandlePendingNotificationAction()
        {
            return HandlePendingNotificationAction(null);
        }

        public static bool HandlePendingNotificationAction(TheymesConfig config)
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.HandlePendingNotificationAction(TheymesJsonHelpers.ConfigToJson(config));
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.HandlePendingNotificationAction(TheymesJsonHelpers.ConfigToJson(config));
            #else
            return false;
            #endif
        }

        public static bool HasPendingNotificationAction()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            return TheymesUnityIosBridge.HasPendingNotificationAction();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            return TheymesUnityAndroidBridge.HasPendingNotificationAction();
            #else
            return false;
            #endif
        }

        private static void _SetupEventListeners()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            TheymesUnityIosBridge.SetupEventListeners();
            TheymesUnityIosBridge.onOpen += _TriggerOnOpen;
            TheymesUnityIosBridge.onClose += _TriggerOnClose;
            TheymesUnityIosBridge.onUnreadMessageCountUpdated += _TriggerOnUnreadMessageCountUpdated;
            TheymesUnityIosBridge.onUnansweredMessageCountUpdated += _TriggerOnUnansweredMessageCountUpdated;
            #elif UNITY_ANDROID && !UNITY_EDITOR
            TheymesUnityAndroidBridge.SetupEventListeners();
            TheymesUnityAndroidBridge.onOpen += _TriggerOnOpen;
            TheymesUnityAndroidBridge.onClose += _TriggerOnClose;
            TheymesUnityAndroidBridge.onUnreadMessageCountUpdated += _TriggerOnUnreadMessageCountUpdated;
            TheymesUnityAndroidBridge.onUnansweredMessageCountUpdated += _TriggerOnUnansweredMessageCountUpdated;
            #elif UNITY_WEBGL && !UNITY_EDITOR
            TheymesUnityWebGLBridge.SetupEventListeners();
            TheymesUnityWebGLBridge.onOpen += _TriggerOnOpen;
            TheymesUnityWebGLBridge.onClose += _TriggerOnClose;
            TheymesUnityWebGLBridge.onUnreadMessageCountUpdated += _TriggerOnUnreadMessageCountUpdated;
            TheymesUnityWebGLBridge.onUnansweredMessageCountUpdated += _TriggerOnUnansweredMessageCountUpdated;
            #endif
        }

        private static void _TriggerOnOpen()
        {
            onOpen?.Invoke();
        }

        private static void _TriggerOnClose()
        {
            onClose?.Invoke();
        }

        private static void _TriggerOnUnreadMessageCountUpdated(int count)
        {
            onUnreadMessageCountUpdated?.Invoke(count);
        }

        private static void _TriggerOnUnansweredMessageCountUpdated(int count)
        {
            onUnansweredMessageCountUpdated?.Invoke(count);
        }
    }
}
