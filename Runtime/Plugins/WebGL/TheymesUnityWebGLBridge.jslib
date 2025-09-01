mergeInto(LibraryManager.library, {
  TheymesInitialize: function(token, domain) {
    try {
      var element = document.querySelector("#unity-canvas") || document.querySelector("#unityCanvas") || document.querySelector("canvas");
      window.theymes.initialize(UTF8ToString(token), UTF8ToString(domain), { mode: 'game', element });
    } catch (error) {
      console.error("TheymesSdk.Initialize() failed", error);
    }
  },

  TheymesOpenSupport: function() {
    try {
      window.theymes.openSupport();
    } catch (error) {
      console.error("TheymesSdk.OpenSupport() failed", error);
    }
  },

  TheymesOpenSupportWithConfig: function(config) {
    try {
      window.theymes.openSupport(JSON.parse(UTF8ToString(config)));
    } catch (error) {
      console.error("TheymesSdk.OpenSupport() failed", error);
    }
  },

  TheymesOpenResource: function(resource) {
    try {
      window.theymes.openResource(UTF8ToString(resource));
    } catch (error) {
      console.error("TheymesSdk.OpenResource() failed", error);
    }
  },

  TheymesOpenResourceWithConfig: function(resource, config) {
    try {
      window.theymes.openResource(UTF8ToString(resource), JSON.parse(UTF8ToString(config)));
    } catch (error) {
      console.error("TheymesSdk.OpenResource() failed", error);
    }
  },

  TheymesClose: function() {
    try {
      window.theymes.close();
    } catch (error) {
      console.error("TheymesSdk.Close() failed", error);
    }
  },

  TheymesGetSdkVersion: function() {
    try {
      var result = window.theymes.getSdkVersion();
      var bufferSize = lengthBytesUTF8(result) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(result, buffer, bufferSize);
      return buffer;
    } catch (error) {
      console.error("TheymesSdk.GetSdkVersion() failed", error);
      return "";
    }
  },

  TheymesIsSupported: function() {
    try {
      return window.theymes.isSupported();
    } catch (error) {
      console.error("TheymesSdk.IsSupported() failed", error);
      return false;
    }
  },

  TheymesRequestNotificationPermission: function() {
    try {
      window.theymes.requestNotificationPermission();
    } catch (error) {
      console.error("TheymesSdk.RequestNotificationPermission() failed", error);
    }
  },

  TheymesEnableNotifications: function() {
    try {
      window.theymes.enableNotifications();
    } catch (error) {
      console.error("TheymesSdk.EnableNotifications() failed", error);
    }
  },

  TheymesDisableNotifications: function() {
    try {
      window.theymes.disableNotifications();
    } catch (error) {
      console.error("TheymesSdk.DisableNotifications() failed", error);
    }
  },

  TheymesGetUnreadMessageCount: function() {
    try {
      return window.theymes.getUnreadMessageCount();
    } catch (error) {
      console.error("TheymesSdk.GetUnreadMessageCount() failed", error);
      return 0;
    }
  },

  TheymesGetUnansweredMessageCount: function() {
    try {
      return window.theymes.getUnansweredMessageCount();
    } catch (error) {
      console.error("TheymesSdk.GetUnansweredMessageCount() failed", error);
      return 0;
    }
  },

  TheymesReset: function() {
    try {
      window.theymes.reset();
    } catch (error) {
      console.error("TheymesSdk.Reset() failed", error);
    }
  },

  TheymesGetLanguage: function() {
    try {
      var result = window.theymes.getLanguage();
      if (result === null) {
        return null;
      }

      var bufferSize = lengthBytesUTF8(result) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(result, buffer, bufferSize);
      return buffer;
    } catch (error) {
      console.error("TheymesSdk.GetLanguage() failed", error);
      return null;
    }
  },

  TheymesSetLanguage: function(language) {
    try {
      window.theymes.setLanguage(UTF8ToString(language));
    } catch (error) {
      console.error("TheymesSdk.SetLanguage() failed", error);
    }
  },

  TheymesGetSignedMetadataToken: function() {
    try {
      var result = window.theymes.getSignedMetadataToken();
      if (result === null) {
        return null;
      }

      var bufferSize = lengthBytesUTF8(result) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(result, buffer, bufferSize);
      return buffer;
    } catch (error) {
      console.error("TheymesSdk.GetSignedMetadataToken() failed", error);
      return null;
    }
  },

  TheymesSetSignedMetadataToken: function(token) {
    try {
      window.theymes.setSignedMetadataToken(UTF8ToString(token));
    } catch (error) {
      console.error("TheymesSdk.SetSignedMetadataToken() failed", error);
    }
  },

  TheymesGetPlayer: function() {
    try {
      var player = window.theymes.getPlayer();
      if (result === null) {
        return null;
      }

      var result = JSON.stringify(player);
      var bufferSize = lengthBytesUTF8(result) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(result, buffer, bufferSize);
      return buffer;
    } catch (error) {
      console.error("TheymesSdk.GetPlayer() failed", error);
      return null;
    }
  },

  TheymesSetPlayer: function(player) {
    try {
      window.theymes.setPlayer(JSON.parse(UTF8ToString(player)));
    } catch (error) {
      console.error("TheymesSdk.SetPlayer() failed", error);
    }
  },

  TheymesGetTags: function() {
    try {
      var tags = window.theymes.getTags();
      if (tags === null) {
        return null;
      }

      var result = JSON.stringify(tags);
      var bufferSize = lengthBytesUTF8(result) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(result, buffer, bufferSize);
      return buffer;
    } catch (error) {
      console.error("TheymesSdk.GetTags() failed", error);
      return null;
    }
  },

  TheymesSetTags: function(tags) {
    try {
      window.theymes.setTags(JSON.parse(UTF8ToString(tags)));
    } catch (error) {
      console.error("TheymesSdk.SetTags() failed", error);
    }
  },

  TheymesAddTag: function(tag) {
    try {
      window.theymes.addTag(UTF8ToString(tag));
    } catch (error) {
      console.error("TheymesSdk.AddTag() failed", error);
    }
  },

  TheymesAddTags: function(tags) {
    try {
      window.theymes.addTags(JSON.parse(UTF8ToString(tags)));
    } catch (error) {
      console.error("TheymesSdk.AddTags() failed", error);
    }
  },

  TheymesRemoveTag: function(tag) {
    try {
      window.theymes.removeTag(UTF8ToString(tag));
    } catch (error) {
      console.error("TheymesSdk.RemoveTag() failed", error);
    }
  },

  TheymesRemoveTags: function(tags) {
    try {
      window.theymes.removeTags(JSON.parse(UTF8ToString(tags)));
    } catch (error) {
      console.error("TheymesSdk.RemoveTags() failed", error);
    }
  },

  TheymesRemoveAllTags: function() {
    try {
      window.theymes.removeAllTags();
    } catch (error) {
      console.error("TheymesSdk.RemoveAllTags() failed", error);
    }
  },

  TheymesGetFields: function() {
    try {
      var fields = window.theymes.getFields();
      if (fields === null) {
        return null;
      }

      var result = JSON.stringify(fields);
      var bufferSize = lengthBytesUTF8(result) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(result, buffer, bufferSize);
      return buffer;
    } catch (error) {
      console.error("TheymesSdk.GetFields() failed", error);
      return null;
    }
  },

  TheymesSetFields: function(fields) {
    try {
      window.theymes.setFields(JSON.parse(UTF8ToString(fields)));
    } catch (error) {
      console.error("TheymesSdk.SetFields() failed", error);
    }
  },

  TheymesAddField: function(key, value) {
    try {
      window.theymes.addField(UTF8ToString(key), UTF8ToString(value));
    } catch (error) {
      console.error("TheymesSdk.AddField() failed", error);
    }
  },

  TheymesAddFields: function(fields) {
    try {
      window.theymes.addFields(JSON.parse(UTF8ToString(fields)));
    } catch (error) {
      console.error("TheymesSdk.AddFields() failed", error);
    }
  },

  TheymesRemoveField: function(field) {
    try {
      window.theymes.removeField(UTF8ToString(field));
    } catch (error) {
      console.error("TheymesSdk.RemoveField() failed", error);
    }
  },

  TheymesRemoveFields: function(fields) {
    try {
      window.theymes.removeFields(JSON.parse(UTF8ToString(fields)));
    } catch (error) {
      console.error("TheymesSdk.RemoveFields() failed", error);
    }
  },

  TheymesRemoveAllFields: function() {
    try {
      window.theymes.removeAllFields();
    } catch (error) {
      console.error("TheymesSdk.RemoveAllFields() failed", error);
    }
  },

  TheymesEnableLogging: function() {
    try {
      window.theymes.enableLogging();
    } catch (error) {
      console.error("TheymesSdk.EnableLogging() failed", error);
    }
  },

  TheymesDisableLogging: function() {
    try {
      window.theymes.disableLogging();
    } catch (error) {
      console.error("TheymesSdk.DisableLogging() failed", error);
    }
  },

  TheymesIsYoungPlayer: function() {
    try {
      return window.theymes.isYoungPlayer();
    } catch (error) {
      console.error("TheymesSdk.IsYoungPlayer() failed", error);
      return false;
    }
  },

  TheymesSetYoungPlayer: function(youngPlayer) {
    try {
      window.theymes.setYoungPlayer(youngPlayer);
    } catch (error) {
      console.error("TheymesSdk.SetYoungPlayer() failed", error);
    }
  },

  TheymesIsPrivacyMode: function() {
    try {
      return window.theymes.isPrivacyMode();
    } catch (error) {
      console.error("TheymesSdk.IsPrivacyMode() failed", error);
      return false;
    }
  },

  TheymesSetPrivacyMode: function(privacyMode) {
    try {
      window.theymes.setPrivacyMode(privacyMode);
    } catch (error) {
      console.error("TheymesSdk.SetPrivacyMode() failed", error);
    }
  },

  TheymesOnUnreadMessageCountUpdated: function(callback) {
    window.theymes.addEventListener("unreadMessageCountUpdate", function(count) {
      {{{ makeDynCall('vi', 'callback') }}} (count);
    });
  },

  TheymesOnUnansweredMessageCountUpdated: function(callback) {
    window.theymes.addEventListener("unansweredMessageCountUpdate", function(count) {
      {{{ makeDynCall('vi', 'callback') }}} (count);
    });
  },

  TheymesOnOpen: function(callback) {
    window.theymes.addEventListener("open", function() {
      {{{ makeDynCall('v', 'callback') }}} ();
    });
  },

  TheymesOnClose: function(callback) {
    window.theymes.addEventListener("close", function() {
      {{{ makeDynCall('v', 'callback') }}} ();
    });
  },

  TheymesRecordRetention: function() {
    try {
      window.theymes.recordRetention();
    } catch (error) {
      console.error("TheymesSdk.RecordRetention() failed", error);
    }
  },
});

