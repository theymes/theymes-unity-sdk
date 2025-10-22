package com.theymes.sdk.unity;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Map;
import java.util.List;
import java.util.ArrayList;
import java.util.Set;
import com.theymes.sdk.android.TheymesSdk;
import com.theymes.sdk.android.TheymesEventListener;
import com.theymes.sdk.android.TheymesPlayer;
import com.theymes.sdk.android.TheymesConfig;

public class TheymesAndroidBridge {

    public static void initialize(Context context, String token, String domain) {
        TheymesSdk.initialize(context, token, domain);
    }

    public static void setEventListener(TheymesEventListener eventListener) {
        TheymesSdk.setEventListener(eventListener);
    }

    public static void openSupport(Context context) {
        TheymesSdk.openSupport(context, null);
    }

    public static void openSupport(Context context, String configJson) {
        TheymesConfig config = jsonStrToTheymesConfig(configJson);
        TheymesSdk.openSupport(context, config);
    }

    public static void openResource(Context context, String resource) {
        TheymesSdk.openResource(context, resource, null);
    }

    public static void openResource(Context context, String resource, String configJson) {
        TheymesConfig config = jsonStrToTheymesConfig(configJson);
        TheymesSdk.openResource(context, resource, config);
    }

    public static void close() {
        TheymesSdk.close();
    }

    public static String getSdkVersion() {
        return TheymesSdk.getSdkVersion();
    }

    public static boolean isSupported() {
        return TheymesSdk.isSupported();
    }

    public static void requestNotificationPermission(Activity activity) {
        TheymesSdk.requestNotificationPermission(activity);
    }

    public static void enableNotifications() {
        TheymesSdk.enableNotifications();
    }

    public static void disableNotifications() {
        TheymesSdk.disableNotifications();
    }

    public static int getUnreadMessageCount() {
        return TheymesSdk.getUnreadMessageCount();
    }

    public static int getUnansweredMessageCount() {
        return TheymesSdk.getUnansweredMessageCount();
    }

    public static void recordRetention() {
        TheymesSdk.recordRetention();
    }

    public static void reset() {
        TheymesSdk.reset();
    }

    public static String getLanguage() {
        return TheymesSdk.getLanguage();
    }

    public static void setLanguage(String language) {
        TheymesSdk.setLanguage(language);
    }

    public static String getSignedMetadataToken() {
        return TheymesSdk.getSignedMetadataToken();
    }

    public static void setSignedMetadataToken(String token) {
        TheymesSdk.setSignedMetadataToken(token);
    }

    public static String getPlayer() {
        TheymesPlayer player = TheymesSdk.getPlayer();
        return playerToJson(player);
    }

    public static void setPlayer(String playerJson) {
        TheymesPlayer player = jsonStrToTheymesPlayer(playerJson);
        TheymesSdk.setPlayer(player);
    }

    public static String getTags() {
        Set<String> tags = TheymesSdk.getTags();
        JSONArray tagsArray = new JSONArray(tags);
        return tagsArray.toString();
    }

    public static void setTags(String tagsJson) {
        Set<String> tags = jsonStrToSet(tagsJson);
        TheymesSdk.setTags(tags);
    }

    public static void addTag(String tag) {
        TheymesSdk.addTag(tag);
    }

    public static void addTags(String tagsJson) {
        Set<String> tags = jsonStrToSet(tagsJson);
        TheymesSdk.addTags(tags);
    }

    public static void removeTag(String tag) {
        TheymesSdk.removeTag(tag);
    }

    public static void removeTags(String tagsJson) {
        Set<String> tags = jsonStrToSet(tagsJson);
        TheymesSdk.removeTags(tags);
    }

    public static void removeAllTags() {
        TheymesSdk.removeAllTags();
    }

    public static String getFields() {
        Map<String, Object> fields = TheymesSdk.getFields();
        JSONObject fieldsJson = new JSONObject(fields);
        return fieldsJson.toString();
    }

    public static void setFields(String fieldsJson) {
        Map<String, Object> fields = jsonStrToMap(fieldsJson);
        TheymesSdk.setFields(fields);
    }

    public static void addField(String key, Object value) {
        TheymesSdk.addField(key, value);
    }

    public static void addFields(String fieldsJson) {
        Map<String, Object> fields = jsonStrToMap(fieldsJson);
        TheymesSdk.addFields(fields);
    }

    public static void removeField(String key) {
        TheymesSdk.removeField(key);
    }

    public static void removeFields(String fieldsJson) {
        Set<String> keys = jsonStrToSet(fieldsJson);
        TheymesSdk.removeFields(keys);
    }

    public static void removeAllFields() {
        TheymesSdk.removeAllFields();
    }

    public static void enableLogging() {
        TheymesSdk.enableLogging();
    }

    public static void disableLogging() {
        TheymesSdk.disableLogging();
    }

    public static boolean isYoungPlayer() {
        return TheymesSdk.isYoungPlayer();
    }

    public static void setYoungPlayer(boolean youngPlayer) {
        TheymesSdk.setYoungPlayer(youngPlayer);
    }

    public static boolean isPrivacyMode() {
        return TheymesSdk.isPrivacyMode();
    }

    public static void setPrivacyMode(boolean privacyMode) {
        TheymesSdk.setPrivacyMode(privacyMode);
    }

    public static void registerPushToken(String pushToken, String type) {
        TheymesSdk.registerPushToken(pushToken, type);
    }

    public static void handleNotification(boolean opened, String dataJson) {
        Map<String, String> data = jsonStrToStrMap(dataJson);
        TheymesSdk.handleNotification(opened, data);
    }

    public static boolean handlePendingNotificationAction(Context context, String configJson) {
        TheymesConfig config = jsonStrToTheymesConfig(configJson);
        return TheymesSdk.handlePendingNotificationAction(context, config);
    }

    public static boolean getIsInForeground() {
        return TheymesSdk.getIsInForeground();
    }

    public static void setIsInForeground(boolean isInForeground) {
        TheymesSdk.setIsInForeground(isInForeground);
    }

    // Helper methods
    private static TheymesConfig jsonStrToTheymesConfig(String jsonStr) {
        if (jsonStr == null || jsonStr.isEmpty()) {
            return null;
        }

        try {
            JSONObject jsonObject = new JSONObject(jsonStr);
            String language = jsonObject.optString("language", null);
            List<String> tags = jsonArrayToList(jsonObject.optJSONArray("tags"));
            Map<String, Object> fields = jsonStrToMap(jsonObject.optString("fields", null));

            return new TheymesConfig()
                .setLanguage(language)
                .setTags(tags)
                .setFields(fields);

        } catch (JSONException e) {
            Log.e("TheymesAndroidBridge", "Failed to parse JSON into TheymesConfig: " + e.getMessage());
            return null;
        }
    }

    private static TheymesPlayer jsonObjectToTheymesPlayer(JSONObject jsonObject) {
        TheymesPlayer player = new TheymesPlayer();
        player.setId(jsonObject.optString("id", null));
        player.setName(jsonObject.optString("name", null));
        player.setEmail(jsonObject.optString("email", null));
        if (jsonObject.has("tier") && !jsonObject.isNull("tier")) {
            player.setTier(jsonObject.optInt("tier"));
        }
        return player;
    }

    private static TheymesPlayer jsonStrToTheymesPlayer(String jsonStr) {
        if (jsonStr == null) {
            return null;
        }
        try {
            JSONObject playerJson = new JSONObject(jsonStr);
            return jsonObjectToTheymesPlayer(playerJson);
        } catch (JSONException e) {
            Log.e("TheymesAndroidBridge", "Failed to parse JSON into TheymesPlayer: " + e.getMessage());
            return null;
        }
    }

    private static List<String> jsonArrayToList(JSONArray jsonArray) {
        List<String> list = new ArrayList<>();
        if (jsonArray != null) {
            for (int i = 0; i < jsonArray.length(); i++) {
                list.add(jsonArray.optString(i));
            }
        }
        return list;
    }

    private static Set<String> jsonStrToSet(String jsonStr) {
        Set<String> set = new HashSet<>();
        if (jsonStr != null) {
            try {
                JSONArray jsonArray = new JSONArray(jsonStr);
                for (int i = 0; i < jsonArray.length(); i++) {
                    set.add(jsonArray.getString(i));
                }
            } catch (JSONException e) {
                Log.e("TheymesAndroidBridge", "Failed to parse JSON into Set<String>: " + e.getMessage());
            }
        }
        return set;
    }

    private static Map<String, Object> jsonStrToMap(String jsonStr) {
        Map<String, Object> map = new HashMap<>();
        if (jsonStr != null) {
            try {
                JSONObject jsonObject = new JSONObject(jsonStr);
                Iterator<String> keys = jsonObject.keys();
                while (keys.hasNext()) {
                    String key = keys.next();
                    map.put(key, jsonObject.get(key));
                }
            } catch (JSONException e) {
                Log.e("TheymesAndroidBridge", "Failed to parse JSON into Map<String, Object>: " + e.getMessage());
            }
        }
        return map;
    }

    private static Map<String, String> jsonStrToStrMap(String jsonStr) {
        Map<String, String> map = new HashMap<>();
        if (jsonStr != null) {
            try {
                JSONObject jsonObject = new JSONObject(jsonStr);
                Iterator<String> keys = jsonObject.keys();
                while (keys.hasNext()) {
                    String key = keys.next();
                    map.put(key, jsonObject.getString(key));
                }
            } catch (JSONException e) {
                Log.e("TheymesAndroidBridge", "Failed to parse JSON into Map<String, String>: " + e.getMessage());
            }
        }
        return map;
    }

    private static String playerToJson(TheymesPlayer player) {
        if (player == null) {
            return null;
        }

        JSONObject playerJson = new JSONObject();
        try {
            playerJson.put("id", player.getId());
            playerJson.put("name", player.getName());
            playerJson.put("email", player.getEmail());
            playerJson.put("tier", player.getTier());
        } catch (JSONException e) {
            Log.e("TheymesAndroidBridge", "Failed to serialize TheymesPlayer to JSON: " + e.getMessage());
        }
        return playerJson.toString();
    }
}