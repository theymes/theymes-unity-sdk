# Changelog

All notable changes to this project will be documented in this file.

## 1.2.4

- Fix a bug where the iOS SDK would show push notifications while game is on foreground if there are no other delegate to handle foreground push notifications installed.

## 1.2.3

- Fix a bug where long and ulong custom fields were dropped.

## 1.2.2

- Add support for Android devices down to Android 6.0 (API level 23).

## 1.2.1

- Add support for checking if there is a pending notification action waiting: `TheymesSdk.HasPendingNotificationAction()`.

## 1.2.0

- Add support for push notifications, new functions: `TheymesSdk.RegisterPushToken()`, `TheymesSdk.IsTheymesNotification()`, `TheymesSdk.HandleNotification()`, `TheymesSdk.HandlePendingNotificationAction()`.
- Send new metadata from SDK: System memory, graphics memory, battery state, battery level, total storage, free storage, network type & carrier (omitted if privacy mode is enabled).
- Embed consumer proguard rules in the Android SDK bundle. You no longer need to include the rules manually.
- Fix a bug where in-game notifications would not be shown sometimes if used together with Unity Mobile Notifications.
- Fix a bug where onOpen / onClose event handlers were not called in the main thread.
- Fix a bug when closing the SDK didn't always refresh the unread and unanswered message counts.

## 1.1.5

- Support picking multiple files when attaching files to tickets or collectors in Android.

## 1.1.4

- Allow more screen orientations on bigger Android tablets.
