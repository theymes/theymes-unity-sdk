#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Theymes
{
    [Preserve]
    internal class TheymesUnityAndroidBridgeEventListener : AndroidJavaProxy
    {
        [Preserve]
        public TheymesUnityAndroidBridgeEventListener() : base("com.theymes.sdk.android.TheymesEventListener") { }

        [Preserve]
        public void onOpen()
        {
            TheymesUnityAndroidBridge.TriggerOnOpen();
        }
        
        [Preserve]
        public void onClose()
        {
            TheymesUnityAndroidBridge.TriggerOnClose();
        }

        [Preserve]
        public void onUpdateUnreadMessageCount(int count)
        {
            TheymesUnityAndroidBridge.TriggerUnreadMessageCountUpdated(count);
        }

        [Preserve]
        public void onUpdateUnansweredMessageCount(int count)
        {
            TheymesUnityAndroidBridge.TriggerUnansweredMessageCountUpdated(count);
        }
    }

    internal static class TheymesUnityAndroidBridge
    {
        public static event System.Action onOpen;
        public static event System.Action onClose;
        public static event System.Action<int> onUnreadMessageCountUpdated;
        public static event System.Action<int> onUnansweredMessageCountUpdated;

        private static AndroidJavaObject unityContext;
        private static AndroidJavaClass bridgeClass;
        private static TheymesUnityAndroidBridgeEventListener listener;

        static TheymesUnityAndroidBridge()
        {
            // Get the Unity current activity context
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                unityContext = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }

            bridgeClass = new AndroidJavaClass("com.theymes.sdk.unity.TheymesAndroidBridge");
        }

        internal static void SetupLifecycleHandler()
        {
            GameObject obj = new GameObject("TheymesAndroidLifecycleHandler");
            obj.hideFlags = HideFlags.HideAndDontSave; // Prevents it from appearing in the hierarchy
            obj.AddComponent<TheymesAndroidLifecycleHandler>();
            Debug.Log("TheymesSDK: Hidden lifecycle tracker created.");
        }

        private class TheymesAndroidLifecycleHandler : MonoBehaviour
        {
            private static AndroidJavaObject _theymesSdk;

            private void Awake()
            {
                DontDestroyOnLoad(gameObject);
            }

            private void OnApplicationPause(bool isPaused)
            {
                if (isPaused)
                {
                    bridgeClass?.CallStatic("setIsInForeground", false);
                    Debug.Log("[TheymesSDK] Game went to background.");
                }
                else
                {
                    bridgeClass?.CallStatic("setIsInForeground", true);
                    Debug.Log("[TheymesSDK] Game came to foreground.");
                }
            }
        }

        public static void Initialize(string token, string domain)
        {
            bridgeClass.CallStatic("initialize", unityContext, token, domain);
            SetupLifecycleHandler();
        }

        public static void OpenSupport()
        {
            bridgeClass.CallStatic("openSupport", unityContext);
        }

        public static void OpenSupport(string configJson)
        {
            bridgeClass.CallStatic("openSupport", unityContext, configJson);
        }

        public static void OpenResource(string resource)
        {
            bridgeClass.CallStatic("openResource", unityContext, resource);
        }

        public static void OpenResource(string resource, string configJson)
        {
            bridgeClass.CallStatic("openResource", unityContext, resource, configJson);
        }

        public static void Close()
        {
            bridgeClass.CallStatic("close");
        }

        public static string GetSdkVersion()
        {
            return bridgeClass.CallStatic<string>("getSdkVersion");
        }

        public static bool IsSupported()
        {
            return bridgeClass.CallStatic<bool>("isSupported");
        }

        public static void RequestNotificationPermission()
        {
            bridgeClass.CallStatic("requestNotificationPermission", unityContext);
        }

        public static void EnableNotifications()
        {
            bridgeClass.CallStatic("enableNotifications");
        }

        public static void DisableNotifications()
        {
            bridgeClass.CallStatic("disableNotifications");
        }

        public static int GetUnreadMessageCount()
        {
            return bridgeClass.CallStatic<int>("getUnreadMessageCount");
        }

        public static int GetUnansweredMessageCount()
        {
            return bridgeClass.CallStatic<int>("getUnansweredMessageCount");
        }

        public static void RecordRetention()
        {
            bridgeClass.CallStatic("recordRetention");
        }

        public static void Reset()
        {
            bridgeClass.CallStatic("reset");
        }

        public static string GetLanguage()
        {
            return bridgeClass.CallStatic<string>("getLanguage");
        }

        public static void SetLanguage(string language)
        {
            bridgeClass.CallStatic("setLanguage", language);
        }

        public static string GetSignedMetadataToken()
        {
            return bridgeClass.CallStatic<string>("getSignedMetadataToken");
        }

        public static void SetSignedMetadataToken(string token)
        {
            bridgeClass.CallStatic("setSignedMetadataToken", token);
        }

        public static string GetPlayer()
        {
            return bridgeClass.CallStatic<string>("getPlayer");
        }

        public static void SetPlayer(string playerJson)
        {
            bridgeClass.CallStatic("setPlayer", playerJson);
        }

        public static string GetTags()
        {
            return bridgeClass.CallStatic<string>("getTags");
        }

        public static void SetTags(string tagsJson)
        {
            bridgeClass.CallStatic("setTags", tagsJson);
        }

        public static void AddTag(string tag)
        {
            bridgeClass.CallStatic("addTag", tag);
        }

        public static void AddTags(string tagsJson)
        {
            bridgeClass.CallStatic("addTags", tagsJson);
        }

        public static void RemoveTag(string tag)
        {
            bridgeClass.CallStatic("removeTag", tag);
        }

        public static void RemoveTags(string tagsJson)
        {
            bridgeClass.CallStatic("removeTags", tagsJson);
        }

        public static void RemoveAllTags()
        {
            bridgeClass.CallStatic("removeAllTags");
        }

        public static string GetFields()
        {
            return bridgeClass.CallStatic<string>("getFields");
        }

        public static void SetFields(string fieldsJson)
        {
            bridgeClass.CallStatic("setFields", fieldsJson);
        }

        public static void AddField(string key, string value)
        {
            bridgeClass.CallStatic("addField", key, value);
        }

        public static void AddFields(string fieldsJson)
        {
            bridgeClass.CallStatic("addFields", fieldsJson);
        }

        public static void RemoveField(string key)
        {
            bridgeClass.CallStatic("removeField", key);
        }

        public static void RemoveFields(string fieldsJson)
        {
            bridgeClass.CallStatic("removeFields", fieldsJson);
        }

        public static void RemoveAllFields()
        {
            bridgeClass.CallStatic("removeAllFields");
        }

        public static void EnableLogging()
        {
            bridgeClass.CallStatic("enableLogging");
        }

        public static void DisableLogging()
        {
            bridgeClass.CallStatic("disableLogging");
        }

        public static bool IsYoungPlayer()
        {
            return bridgeClass.CallStatic<bool>("isYoungPlayer");
        }

        public static void SetYoungPlayer(bool youngPlayer)
        {
            bridgeClass.CallStatic("setYoungPlayer", youngPlayer);
        }

        public static bool IsPrivacyMode()
        {
            return bridgeClass.CallStatic<bool>("isPrivacyMode");
        }

        public static void SetPrivacyMode(bool privacyMode)
        {
            bridgeClass.CallStatic("setPrivacyMode", privacyMode);
        }

        public static void RegisterPushToken(string pushToken, string type)
        {
            bridgeClass.CallStatic("registerPushToken", pushToken, type);
        }

        public static void HandleNotification(bool opened, string dataJson)
        {
            bridgeClass.CallStatic("handleNotification", opened, dataJson);
        }

        public static bool HandlePendingNotificationAction(string configJson)
        {
            return bridgeClass.CallStatic<bool>("handlePendingNotificationAction", unityContext, configJson);
        }

        public static bool HasPendingNotificationAction()
        {
            return bridgeClass.CallStatic<bool>("hasPendingNotificationAction");
        }

        public static void SetupEventListeners()
        {
            if (listener != null) {
                return;
            }

            listener = new TheymesUnityAndroidBridgeEventListener();
            bridgeClass.CallStatic("setEventListener", listener);
        }

        public static void TriggerOnOpen()
        {
            onOpen?.Invoke();
        }

        public static void TriggerOnClose()
        {
            onClose?.Invoke();
        }

        public static void TriggerUnreadMessageCountUpdated(int count)
        {
            onUnreadMessageCountUpdated?.Invoke(count);
        }

        public static void TriggerUnansweredMessageCountUpdated(int count)
        {
            onUnansweredMessageCountUpdated?.Invoke(count);
        }
    }
}
#endif