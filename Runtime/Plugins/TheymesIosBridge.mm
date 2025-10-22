#import "TheymesIosBridge.h"
#import <UserNotifications/UserNotifications.h>
#import <TheymesSdk/TheymesSdk-Swift.h>

@interface TheymesIosBridgeDelegate : NSObject <TheymesDelegate>
@end

static TheymesIosBridgeDelegate *delegate;
static OnOpenClose openCallback;
static OnOpenClose closeCallback;
static OnMessageCountUpdated unreadMessageCallback;
static OnMessageCountUpdated unansweredMessageCallback;

@implementation TheymesIosBridgeDelegate
- (void)didOpen {
    if (openCallback) {
        openCallback();
    }
}
- (void)didClose {
    if (closeCallback) {
        closeCallback();
    }
}
- (void)didUpdateUnreadMessageCount:(NSInteger)count { 
    if (unreadMessageCallback) {
        unreadMessageCallback(count);
    }
}
- (void)didUpdateUnansweredMessageCount:(NSInteger)count { 
    if (unansweredMessageCallback) {
        unansweredMessageCallback(count);
    }
}
@end

NSString *cStringToNSString(const char *str) {
    return str ? [NSString stringWithUTF8String:str] : nil;
}

const char *nsStringToCString(NSString *str) {
    return str ? [str UTF8String] : NULL;
}

NSDictionary *jsonStrPointerToNSDictionary(const char *jsonStr) {
    if (jsonStr == NULL) {
        return nil;
    }

    NSString *jsonString = [NSString stringWithUTF8String:jsonStr];
    NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError *error = nil;
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:jsonData options:0 error:&error];
    
    if (error) {
        NSLog(@"Failed to parse JSON: %@", error.localizedDescription);
        return nil;
    }
    
    return dict;
}

NSArray *jsonStrPointerToNSArray(const char *jsonStr) {
    if (jsonStr == NULL) {
        return nil;
    }
    
    NSString *jsonString = [NSString stringWithUTF8String:jsonStr];
    NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError *error = nil;
    NSArray *array = [NSJSONSerialization JSONObjectWithData:jsonData options:0 error:&error];
    
    if (error) {
        NSLog(@"Failed to parse JSON array: %@", error.localizedDescription);
        return nil;
    }
    
    return array;
}

const char *nsDictionaryToJsonCString(NSDictionary *dict) {
    if (!dict) return NULL;
    
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&error];
    
    if (error) {
        NSLog(@"Failed to serialize dictionary: %@", error.localizedDescription);
        return NULL;
    }
    
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return [jsonString UTF8String];
}

const char *nsArrayToJsonCString(NSArray *array) {
    if (!array) return NULL;
    
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:array options:0 error:&error];
    
    if (error) {
        NSLog(@"Failed to serialize array: %@", error.localizedDescription);
        return NULL;
    }
    
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return [jsonString UTF8String];
}

void TheymesInitializeWithToken(const char *token, const char *domain) {
    NSString *tokenString = [NSString stringWithUTF8String:token];
    NSString *domainString = [NSString stringWithUTF8String:domain];

    if (!delegate) {
        delegate = [[TheymesIosBridgeDelegate alloc] init];
        [Theymes setDelegate:delegate];
    }

    [Theymes initializeWithToken:tokenString domain:domainString];
}

void TheymesOpenSupport() {
    [Theymes openSupport];
}

void TheymesOpenSupportWithConfig(const char *config) {
    NSDictionary *configDict = jsonStrPointerToNSDictionary(config);
    if (configDict != nil) {
        [Theymes openSupportWithConfig:configDict];
    }
}

void TheymesClose() {
    [Theymes close];
}

void TheymesOpenResource(const char *resource) {
    NSString *resourceString = cStringToNSString(resource);
    [Theymes openResource:resourceString];
}

void TheymesOpenResourceWithConfig(const char *resource, const char *config) {
    NSString *resourceString = cStringToNSString(resource);
    NSDictionary *configDict = jsonStrPointerToNSDictionary(config);
    [Theymes openResource:resourceString config:configDict];
}

const char* TheymesGetSdkVersion() {
    NSString *version = [Theymes getSdkVersion];
    return [version UTF8String]; // Convert NSString to const char*
}

bool TheymesIsSupported() {
    return [Theymes isSupported];
}

void TheymesRequestNotificationPermission() {
    [Theymes requestNotificationPermission];
}

void TheymesEnableNotifications() {
    [Theymes enableNotifications];
}

void TheymesDisableNotifications() {
    [Theymes disableNotifications];
}

long TheymesGetUnreadMessageCount() {
    return [Theymes getUnreadMessageCount];
}

long TheymesGetUnansweredMessageCount() {
    return [Theymes getUnansweredMessageCount];
}

void TheymesRecordRetention() {
    [Theymes recordRetention];
}

void TheymesReset() {
    [Theymes reset];
}

const char *TheymesGetLanguage() {
    return nsStringToCString([Theymes getLanguage]);
}

void TheymesSetLanguage(const char *language) {
    [Theymes setLanguage:cStringToNSString(language)];
}

const char *TheymesGetSignedMetadataToken() {
    return nsStringToCString([Theymes getSignedMetadataToken]);
}

void TheymesSetSignedMetadataToken(const char *token) {
    [Theymes setSignedMetadataToken:cStringToNSString(token)];
}

const char *TheymesGetPlayer() {
    return nsDictionaryToJsonCString([Theymes getPlayer]);
}

void TheymesSetPlayer(const char *player) {
    [Theymes setPlayer:jsonStrPointerToNSDictionary(player)];
}

const char *TheymesGetTags() {
    return nsArrayToJsonCString([Theymes getTags]);
}

void TheymesSetTags(const char *tags) {
    NSArray *tagsArray = jsonStrPointerToNSArray(tags);
    [Theymes setTags:tagsArray];
}

void TheymesAddTag(const char *tag) {
    [Theymes addTag:cStringToNSString(tag)];
}

void TheymesAddTags(const char *tags) {
    NSArray *tagsArray = jsonStrPointerToNSArray(tags);
    [Theymes addTags:tagsArray];
}

void TheymesRemoveTag(const char *tag) {
    [Theymes removeTag:cStringToNSString(tag)];
}

void TheymesRemoveTags(const char *tags) {
    NSArray *tagsArray = jsonStrPointerToNSArray(tags);
    [Theymes removeTags:tagsArray];
}

void TheymesRemoveAllTags() {
    [Theymes removeAllTags];
}

const char *TheymesGetFields() {
    return nsDictionaryToJsonCString([Theymes getFields]);
}

void TheymesSetFields(const char *fields) {
    [Theymes setFields:jsonStrPointerToNSDictionary(fields)];
}

void TheymesAddField(const char *key, const char *value) {
    NSString *keyString = cStringToNSString(key);
    NSString *valueString = cStringToNSString(value);
    NSData *jsonData = [valueString dataUsingEncoding:NSUTF8StringEncoding];
    
    // parse json
    NSError *jsonError = nil;
    id valueObj = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingAllowFragments error:&jsonError];
    
    // Step 3: Handle any errors in JSON parsing
    if (jsonError) {
        NSLog(@"Failed to parse JSON value: %@", jsonError.localizedDescription);
        return;
    }
    
    // Step 4: Call the Swift method with the parsed JSON value
    [Theymes addField:keyString value:valueObj];
}

void TheymesAddFields(const char *fields) {
    [Theymes addFields:jsonStrPointerToNSDictionary(fields)];
}

void TheymesRemoveField(const char *field) {
    [Theymes removeField:cStringToNSString(field)];
}

void TheymesRemoveFields(const char *fields) {
    NSArray *fieldsArray = jsonStrPointerToNSArray(fields);
    [Theymes removeFields:fieldsArray];
}

void TheymesRemoveAllFields() {
    [Theymes removeAllFields];
}

void TheymesEnableLogging() {
    [Theymes enableLogging];
}

void TheymesDisableLogging() {
    [Theymes disableLogging];
}

bool TheymesIsYoungPlayer() {
    return [Theymes isYoungPlayer];
}

void TheymesSetYoungPlayer(bool youngPlayer) {
    [Theymes setYoungPlayer:youngPlayer];
}

bool TheymesIsPrivacyMode() {
    return [Theymes isPrivacyMode];
}

void TheymesSetPrivacyMode(bool privacyMode) {
    [Theymes setPrivacyMode:privacyMode];
}

void TheymesRegisterPushToken(const char *token, const char *type) {
    [Theymes registerPushToken:cStringToNSString(token) type:cStringToNSString(type)];
}

bool TheymesHandlePendingNotificationAction(const char *config) {
    NSDictionary *configDict = jsonStrPointerToNSDictionary(config);
    return [Theymes handlePendingNotificationAction:configDict];
}

void TheymesOnOpen(OnOpenClose callback) {
    openCallback = callback;
}

void TheymesOnClose(OnOpenClose callback) {
    closeCallback = callback;
}

void TheymesOnUnreadMessageCountUpdated(OnMessageCountUpdated callback) {
    unreadMessageCallback = callback;
}

void TheymesOnUnansweredMessageCountUpdated(OnMessageCountUpdated callback) {
    unansweredMessageCallback = callback;
}
