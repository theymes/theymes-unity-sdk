#if UNITY_IOS && !UNITY_EDITOR
using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Theymes
{
    public static class TheymesUnityIosBridge
    {
        public static event System.Action onOpen;
        public static event System.Action onClose;
        public static event System.Action<int> onUnreadMessageCountUpdated;
        public static event System.Action<int> onUnansweredMessageCountUpdated;

        [DllImport("__Internal")]
        private static extern void TheymesInitializeWithToken(string token, string domain);

        [DllImport("__Internal")]
        private static extern void TheymesOpenSupport();

        [DllImport("__Internal")]
        private static extern void TheymesOpenSupportWithConfig(string config);

        [DllImport("__Internal")]
        private static extern void TheymesOpenResource(string resource);

        [DllImport("__Internal")]
        private static extern void TheymesOpenResourceWithConfig(string resource, string config);

        [DllImport("__Internal")]
        private static extern void TheymesClose();

        [DllImport("__Internal")]
        private static extern IntPtr TheymesGetSdkVersion();

        [DllImport("__Internal")]
        private static extern bool TheymesIsSupported();

        [DllImport("__Internal")]
        private static extern void TheymesRequestNotificationPermission();

        [DllImport("__Internal")]
        private static extern void TheymesEnableNotifications();

        [DllImport("__Internal")]
        private static extern void TheymesDisableNotifications();

        [DllImport("__Internal")]
        private static extern int TheymesGetUnreadMessageCount();

        [DllImport("__Internal")]
        private static extern int TheymesGetUnansweredMessageCount();

        [DllImport("__Internal")]
        private static extern void TheymesRecordRetention();

        [DllImport("__Internal")]
        private static extern void TheymesReset();

        [DllImport("__Internal")]
        private static extern IntPtr TheymesGetLanguage();

        [DllImport("__Internal")]
        private static extern void TheymesSetLanguage(string language);

        [DllImport("__Internal")]
        private static extern IntPtr TheymesGetSignedMetadataToken();

        [DllImport("__Internal")]
        private static extern void TheymesSetSignedMetadataToken(string token);

        [DllImport("__Internal")]
        private static extern IntPtr TheymesGetPlayer();

        [DllImport("__Internal")]
        private static extern void TheymesSetPlayer(string player);

        [DllImport("__Internal")]
        private static extern IntPtr TheymesGetTags();

        [DllImport("__Internal")]
        private static extern void TheymesSetTags(string tags);

        [DllImport("__Internal")]
        private static extern void TheymesAddTag(string tag);

        [DllImport("__Internal")]
        private static extern void TheymesAddTags(string tags);

        [DllImport("__Internal")]
        private static extern void TheymesRemoveTag(string tag);

        [DllImport("__Internal")]
        private static extern void TheymesRemoveTags(string tags);

        [DllImport("__Internal")]
        private static extern void TheymesRemoveAllTags();

        [DllImport("__Internal")]
        private static extern IntPtr TheymesGetFields();

        [DllImport("__Internal")]
        private static extern void TheymesSetFields(string fields);

        [DllImport("__Internal")]
        private static extern void TheymesAddField(string key, string value);

        [DllImport("__Internal")]
        private static extern void TheymesAddFields(string fields);

        [DllImport("__Internal")]
        private static extern void TheymesRemoveField(string field);

        [DllImport("__Internal")]
        private static extern void TheymesRemoveFields(string fields);

        [DllImport("__Internal")]
        private static extern void TheymesRemoveAllFields();

        [DllImport("__Internal")]
        private static extern void TheymesEnableLogging();

        [DllImport("__Internal")]
        private static extern void TheymesDisableLogging();

        [DllImport("__Internal")]
        private static extern bool TheymesIsYoungPlayer();

        [DllImport("__Internal")]
        private static extern void TheymesSetYoungPlayer(bool youngPlayer);

        [DllImport("__Internal")]
        private static extern bool TheymesIsPrivacyMode();

        [DllImport("__Internal")]
        private static extern void TheymesSetPrivacyMode(bool privacyMode);

        [DllImport("__Internal")]
        private static extern void TheymesOnOpen(Action callback);

        [DllImport("__Internal")]
        private static extern void TheymesOnClose(Action callback);

        [DllImport("__Internal")]
        private static extern void TheymesOnUnreadMessageCountUpdated(Action<int> callback);

        [DllImport("__Internal")]
        private static extern void TheymesOnUnansweredMessageCountUpdated(Action<int> callback);

        public static void Initialize(string token, string domain)
        {
            TheymesInitializeWithToken(token, domain);
        }

        public static void OpenSupport()
        {
            TheymesOpenSupport();
        }

        public static void OpenSupport(string configJson)
        {
            TheymesOpenSupportWithConfig(configJson);
        }

        public static void OpenResource(string resource)
        {
            TheymesOpenResource(resource);
        }

        public static void OpenResource(string resource, string configJson)
        {
            TheymesOpenResourceWithConfig(resource, configJson);
        }

        public static void Close()
        {
            TheymesClose();
        }

        public static string GetSdkVersion()
        {
            IntPtr versionPtr = TheymesGetSdkVersion();
            return Marshal.PtrToStringAnsi(versionPtr);
        }

        public static bool IsSupported()
        {
            return TheymesIsSupported();
        }

        public static void RequestNotificationPermission()
        {
            TheymesRequestNotificationPermission();
        }

        public static void EnableNotifications()
        {
            TheymesEnableNotifications();
        }

        public static void DisableNotifications()
        {
            TheymesDisableNotifications();
        }

        public static int GetUnreadMessageCount()
        {
            return TheymesGetUnreadMessageCount();
        }

        public static int GetUnansweredMessageCount()
        {
            return TheymesGetUnansweredMessageCount();
        }

        public static void RecordRetention()
        {
            TheymesRecordRetention();
        }

        public static void Reset()
        {
            TheymesReset();
        }

        public static string GetLanguage()
        {
            IntPtr languagePtr = TheymesGetLanguage();
            return Marshal.PtrToStringAnsi(languagePtr);
        }

        public static void SetLanguage(string language)
        {
            TheymesSetLanguage(language);
        }

        public static string GetSignedMetadataToken()
        {
            IntPtr tokenPtr = TheymesGetSignedMetadataToken();
            return Marshal.PtrToStringAnsi(tokenPtr);
        }

        public static void SetSignedMetadataToken(string token)
        {
            TheymesSetSignedMetadataToken(token);
        }

        public static string GetPlayer()
        {
            IntPtr playerPtr = TheymesGetPlayer();
            return Marshal.PtrToStringAnsi(playerPtr);
        }

        public static void SetPlayer(string playerJson)
        {
            TheymesSetPlayer(playerJson);
        }

        public static string GetTags()
        {
            IntPtr tagsPtr = TheymesGetTags();
            return Marshal.PtrToStringAnsi(tagsPtr);
        }

        public static void SetTags(string tagsJson)
        {
            TheymesSetTags(tagsJson);
        }

        public static void AddTag(string tag)
        {
            TheymesAddTag(tag);
        }

        public static void AddTags(string tagsJson)
        {
            TheymesAddTags(tagsJson);
        }

        public static void RemoveTag(string tag)
        {
            TheymesRemoveTag(tag);
        }

        public static void RemoveTags(string tagsJson)
        {
            TheymesRemoveTags(tagsJson);
        }

        public static void RemoveAllTags()
        {
            TheymesRemoveAllTags();
        }

        public static string GetFields()
        {
            IntPtr fieldsPtr = TheymesGetFields();
            return Marshal.PtrToStringAnsi(fieldsPtr);
        }

        public static void SetFields(string fieldsJson)
        {
            TheymesSetFields(fieldsJson);
        }

        public static void AddField(string key, string value)
        {
            TheymesAddField(key, value);
        }

        public static void AddFields(string fieldsJson)
        {
            TheymesAddFields(fieldsJson);
        }

        public static void RemoveField(string field)
        {
            TheymesRemoveField(field);
        }

        public static void RemoveFields(string fieldsJson)
        {
            TheymesRemoveFields(fieldsJson);
        }

        public static void RemoveAllFields()
        {
            TheymesRemoveAllFields();
        }

        public static void EnableLogging()
        {
            TheymesEnableLogging();
        }

        public static void DisableLogging()
        {
            TheymesDisableLogging();
        }

        public static bool IsYoungPlayer()
        {
            return TheymesIsYoungPlayer();
        }

        public static void SetYoungPlayer(bool youngPlayer)
        {
            TheymesSetYoungPlayer(youngPlayer);
        }

        public static bool IsPrivacyMode()
        {
            return TheymesIsPrivacyMode();
        }

        public static void SetPrivacyMode(bool privacyMode)
        {
            TheymesSetPrivacyMode(privacyMode);
        }

        public static void SetupEventListeners()
        {
            TheymesOnOpen(OnOpen);
            TheymesOnClose(OnClose);
            TheymesOnUnreadMessageCountUpdated(OnUnreadMessageCountUpdated);
            TheymesOnUnansweredMessageCountUpdated(OnUnansweredMessageCountUpdated);
        }

        [MonoPInvokeCallback(typeof(Action))]
        public static void OnOpen()
        {
            onOpen?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        public static void OnClose()
        {
            onClose?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        public static void OnUnreadMessageCountUpdated(int count)
        {
            onUnreadMessageCountUpdated?.Invoke(count);
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        public static void OnUnansweredMessageCountUpdated(int count)
        {
            onUnansweredMessageCountUpdated?.Invoke(count);
        }
    }
}
#endif