# Changelog

All notable changes to this project will be documented in this file.

## 1.4.0

- Add breadcrumb support for passing recent game context to support sessions.
  - New Unity APIs: `TheymesSdk.AddBreadcrumb()`, `TheymesSdk.AddBreadcrumbs()`, and `TheymesSdk.ClearBreadcrumbs()`.
  - Breadcrumbs are timestamped automatically, bounded by SDK settings, cleared by `TheymesSdk.Reset()`, and sent when opening support or resources.
- Add Windows, macOS, and Linux support.
- Allow SDK integrations to be tested while running games in the Unity Editor.
- Send the Unity engine version as metadata.
- Fix a bug on Android where restoring the Theymes support view after app was terminated crashes on Android 13 on certain Xiaomis devices.
- Avoid running Android SDK foreground/background lifecycle work inline during Unity pause/resume callbacks, reducing the risk of pause-time ANRs.
- Include the target player in notification actions so pending notification actions survive reset/logout and only open for the targeted player.
- Add `TheymesSdk.onSignedMetadataTokenExpirationUpdated` for tracking signed metadata token expiration time while the game is in the foreground.

## 1.3.0

- Add optional `InitializeOptions` parameter to `TheymesSdk.Initialize()` for configuring various options.
  - `InitializeOptions.apiDomain`: Allow overriding the API domain the SDK uses. Normally you do not need to touch this.
  - `InitializeOptions.android.orientation`: Allow overriding the orientation of the support center for Android. Use the `Orientation` constants (`Orientation.Portrait`, `Orientation.Landscape`, `Orientation.Unspecified`).
  - `InitializeOptions.web.canvasSelector`: Custom CSS selector for the canvas element (e.g. `#gameCanvas`). Use when you need to specify a custom selector for your game canvas.
  - `InitializeOptions.web.nonce`: Nonce string for Content Security Policy (CSP) compatibility. Pass the same nonce used in your CSP `script-src` and `style-src` directives.
- Reduce the number of API calls the SDK makes in the background, making it less chatty.
- Improve push token handling for devices that are used by multiple users.
- Fix a bug for Android on certain devices when opening the support center while in landscape, the support center renders in incorrect size if forced to be in portrait mode.
- Fix a bug on iOS that automatically opens YouTube videos in browser or YouTube app and won't allow playback inside the SDK.

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
