#if UNITY_IOS
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;

public class TheymesSdkIosPostProcessBuild
{
    [PostProcessBuild(900)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS)
        {
            return;
        }

        string projectPath = PBXProject.GetPBXProjectPath(path);
        PBXProject project = new PBXProject();
        project.ReadFromFile(projectPath);

        // Get the target GUIDs for Unity-iPhone (main app target) and UnityFramework
        string mainTargetGuid = project.GetUnityMainTargetGuid();
        string unityFrameworkTargetGuid = project.GetUnityFrameworkTargetGuid();

        string frameworksDir = Path.Combine(path, "Frameworks");
        if (!Directory.Exists(frameworksDir))
        {
            Debug.LogError("Could not find Frameworks directory in the iOS build folder.");
            return;
        }

        string[] foundFrameworks = Directory.GetDirectories(frameworksDir, "TheymesSdk.framework", SearchOption.AllDirectories);
        if (foundFrameworks.Length == 0)
        {
            Debug.LogError("Could not find TheymesSdk.framework in the iOS build folder.");
            return;
        }
        string frameworkPath = foundFrameworks[0];
        string relativeFrameworkPath = frameworkPath.Replace(path, "").TrimStart('/');
        string frameworkSearchPath = "$(PROJECT_DIR)/" + Path.GetDirectoryName(relativeFrameworkPath);
        string frameworkGuid = project.AddFile(relativeFrameworkPath, relativeFrameworkPath, PBXSourceTree.Source);

        if (frameworkGuid == null)
        {
            Debug.LogError("Could not add TheymesSdk.framework to the Xcode project.");
            return;
        }

        // Add the framework to the UnityFramework target
        project.AddFileToBuild(unityFrameworkTargetGuid, frameworkGuid);

        // Add the framework search path to the UnityFramework target
        string frameworkSearchPaths = project.GetBuildPropertyForAnyConfig(unityFrameworkTargetGuid, "FRAMEWORK_SEARCH_PATHS");
        if (frameworkSearchPaths == null || !frameworkSearchPaths.Contains(frameworkSearchPath))
        {
            project.AddBuildProperty(unityFrameworkTargetGuid, "FRAMEWORK_SEARCH_PATHS", frameworkSearchPath);
        }
        string unityFrameworkLdRunpathSearchPaths = project.GetBuildPropertyForAnyConfig(unityFrameworkTargetGuid, "LD_RUNPATH_SEARCH_PATHS");
        if (unityFrameworkLdRunpathSearchPaths == null || !unityFrameworkLdRunpathSearchPaths.Contains("@executable_path/Frameworks"))
        {
            project.AddBuildProperty(unityFrameworkTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
        }
        if (unityFrameworkLdRunpathSearchPaths == null || !unityFrameworkLdRunpathSearchPaths.Contains("@loader_path/Frameworks"))
        {
            project.AddBuildProperty(unityFrameworkTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "@loader_path/Frameworks");
        }

        // Embed the framework in the app bundle
        PBXProjectExtensions.AddFileToEmbedFrameworks(project, mainTargetGuid, frameworkGuid);

        // With the framework embedded, make sure it can be found at runtime
        string ldRunpathSearchPaths = project.GetBuildPropertyForAnyConfig(mainTargetGuid, "LD_RUNPATH_SEARCH_PATHS");
        if (ldRunpathSearchPaths == null || !ldRunpathSearchPaths.Contains("@executable_path/Frameworks"))
        {
            project.AddBuildProperty(mainTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
        }

        project.SetBuildProperty(mainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
        project.SetBuildProperty(unityFrameworkTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

        // Write changes to the Xcode project
        project.WriteToFile(projectPath);
    }
}
#endif
