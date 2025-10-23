#import <Foundation/Foundation.h>

// Ensure C linkage for compatibility with Unity's DllImport
#ifdef __cplusplus
extern "C"
{
#endif

  typedef void (*OnOpenClose)(void);
  typedef void (*OnMessageCountUpdated)(long count);

  void TheymesInitializeWithToken(const char *token, const char *domain);

  void TheymesOpenSupport();
  void TheymesOpenSupportWithConfig(const char *config);
  void TheymesOpenResource(const char *resource);
  void TheymesOpenResourceWithConfig(const char *resource, const char *config);
  void TheymesClose();

  const char *TheymesGetSdkVersion();
  bool TheymesIsSupported();

  void TheymesRequestNotificationPermission();
  void TheymesEnableNotifications();
  void TheymesDisableNotifications();
  long TheymesGetUnreadMessageCount();
  long TheymesGetUnansweredMessageCount();

  void TheymesRecordRetention();

  void TheymesReset();

  const char *TheymesGetLanguage();
  void TheymesSetLanguage(const char *language);

  const char *TheymesGetSignedMetadataToken();
  void TheymesSetSignedMetadataToken(const char *token);

  const char *TheymesGetPlayer();
  void TheymesSetPlayer(const char *player);

  const char *TheymesGetTags();
  void TheymesSetTags(const char *tags);
  void TheymesAddTag(const char *tag);
  void TheymesAddTags(const char *tags);
  void TheymesRemoveTag(const char *tag);
  void TheymesRemoveTags(const char *tags);
  void TheymesRemoveAllTags();

  const char *TheymesGetFields();
  void TheymesSetFields(const char *fields);
  void TheymesAddField(const char *key, const char *value);
  void TheymesAddFields(const char *fields);
  void TheymesRemoveField(const char *field);
  void TheymesRemoveFields(const char *fields);
  void TheymesRemoveAllFields();

  void TheymesEnableLogging();
  void TheymesDisableLogging();

  bool TheymesIsYoungPlayer();
  void TheymesSetYoungPlayer(bool youngPlayer);
  bool TheymesIsPrivacyMode();
  void TheymesSetPrivacyMode(bool privacyMode);

  void TheymesRegisterPushToken(const char *token, const char *type);
  bool TheymesHandlePendingNotificationAction(const char *config);
  bool TheymesHasPendingNotificationAction();

  void TheymesOnOpen(OnOpenClose callback);
  void TheymesOnClose(OnOpenClose callback);
  void TheymesOnUnreadMessageCountUpdated(OnMessageCountUpdated callback);
  void TheymesOnUnansweredMessageCountUpdated(OnMessageCountUpdated callback);

#ifdef __cplusplus
}
#endif
