using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.FantasyMonsters.Monsters.Scripts.Editor
{
    [InitializeOnLoad]
    internal static class PackageUpdater
    {
        private const string PackageId = "159572";
        private const string CurrentVersion = "3.1";

        private static readonly string PrefsKeyTime = $"PackageUpdater.Time.{PackageId}";
        private static readonly string PrefsKeySkip = $"PackageUpdater.Skip.{PackageId}";
        
        static PackageUpdater()
        {
            if (EditorPrefs.HasKey(PrefsKeyTime))
            {
                var time = DateTime.Parse(EditorPrefs.GetString(PrefsKeyTime), CultureInfo.InvariantCulture);

                if ((DateTime.UtcNow - time).TotalHours < 24) return;
            }

            var request = UnityWebRequest.Get($"https://api.assetstore.unity3d.com/package/latest-version/{PackageId}");

            request.SendWebRequest().completed += _ =>
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var packageInfo = JsonUtility.FromJson<PackageInfo>(request.downloadHandler.text);
                    
                    if (new Version(packageInfo.version) > new Version(CurrentVersion))
                    {
                        if (EditorPrefs.HasKey(PrefsKeySkip) && EditorPrefs.GetString(PrefsKeySkip) == packageInfo.version) return;
                        
                        var confirm = EditorUtility.DisplayDialog("Package Updater", $"{packageInfo.name} ({CurrentVersion}) is outdated. Do you want to update it to the latest version ({packageInfo.version}) via Package Manager?", "Yes", "No");

                        if (confirm)
                        {
                            //UnityEditor.PackageManager.UI.Window.Open(PackageId);
                            Application.OpenURL($"https://assetstore.unity.com/packages/slug/{PackageId}");
                        }
                        else
                        {
                            EditorPrefs.SetString(PrefsKeySkip, packageInfo.version);
                            Debug.LogWarning($"{packageInfo.name} ({CurrentVersion}) is outdated. Please update to the latest version ({packageInfo.version}).");
                        }

                        EditorPrefs.SetString(PrefsKeyTime, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
                        Event($"PackageUpdater.{(confirm ? "Yes" : "No")}", "PackageId", PackageId);
                    }
                    else
                    {
                        Debug.Log($"{packageInfo.name} is up to date.");
                    }
                }
                else
                {
                    Debug.LogError(request.error);
                }

                request.Dispose();
            };
        }

        private static void Event(string eventName, string paramName, string paramValue)
        {
            var request = UnityWebRequest.Post("https://hippogames.dev/api/analytics/event", new Dictionary<string, string> { { "eventName", eventName }, { "paramName", paramName }, { "paramValue", paramValue } });

            request.SendWebRequest().completed += _ => request.Dispose();
        }

        private class PackageInfo
        {
            public string version;
            public string name;
            public string category;
            public string id;
            public string publisher;
        }
    }
}