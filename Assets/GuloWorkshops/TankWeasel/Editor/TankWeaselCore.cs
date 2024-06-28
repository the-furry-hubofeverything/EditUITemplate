#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

namespace GuloWorkshops.TankWeasel
{
    [InitializeOnLoad]
    public class TankWeaselCore : MonoBehaviour
    {
        public static Version Version = new Version(1, 0, 0);

        /// <summary>
        /// Static Constructor to register events and perform OOBE
        /// </summary>
        static TankWeaselCore()
        {
            LoadAutoState();
            Debug.Log(GuloWorkshops.TankWeasel.TankWeaselCore.Version);
            if (AlwaysShow && !EditorApplication.isPlaying)
            {
                EditorApplication.update -= ShowAboutWindow;
                EditorApplication.update += ShowAboutWindow;
            }
        }

        // [MenuItem("Gulo Workshops/Tank Weasel/Perform Action", false, 100)]
        // public static void DoAction()
        // {
        //     EditorUtility.DisplayDialog(
        //         "Tank Weasel", 
        //         "You just clicked a menu item. This could be used to perform some action.",
        //         "OK"
        //         );
        // }

        public static void OpenSupportFile(string targetGUID)
        {
            // if ()
            // {
            var assetPath = "https://avatardocs.gulo.dev";
            Debug.Log("Opening " + assetPath + "...");

            Application.OpenURL(assetPath);
        //     }
        //     else
        //     {
        //         EditorUtility.DisplayDialog(
        //             "Tank Weasel",
        //             "Sorry but Unity was unable to find the document! You may need to reinitialize the project.",
        //             "OK"
        //         );
        //     }
        }

        /// <summary>
        /// Displays About box
        /// </summary>
        [MenuItem("Gulo Workshops/Tank Weasel/About", false, 112)]
        static void ShowAboutWindow()
        {
            EditorApplication.update -= ShowAboutWindow;

            var window = (InfoWindow)EditorWindow.GetWindow(typeof(InfoWindow), true, "Tank Weasel - About");

            var width = UIHelpers.AboutWindowWidth + 20;
            var height = 850;
            int x = 300;
            int y = 100;
            window.position = new Rect(x, y, width, height);

            window.VRCFuryInstalled = PackageHunter.IsPackageInstalled(PackageHunter.VRCFuryPackageName);

            // window.ReadMeAssetReady = PackageHunter.IsFileGuidPresent(PackageHunter.ReadmeGUID);

            // window.ShaderReady = PackageHunter.IsShaderPresent(PackageHunter.PoiyomiShaderName);

            window.Show();
            window.Focus();
        }

        private static bool m_AlwaysShow = true;

        public static bool AlwaysShow
        {
            get { return m_AlwaysShow; }
            set
            {
                m_AlwaysShow = value;
                SaveAutoState();
            }
        }

        private static string GetSettingsPath()
        {
            return Path.Combine(new DirectoryInfo(Application.dataPath).Parent?.FullName, "ProjectSettings", "GuloWorkshops-TankWeasel");
        }

        private static void EnsurePathExists()
        {
            if (!Directory.Exists(GetSettingsPath()))
            {
                Directory.CreateDirectory(GetSettingsPath());
            }
        }

        const string settingsFilename = "settings.json";

        private static void SaveAutoState()
        {
            EnsurePathExists();
            var settings = new TankWeaselSettings() {AlwaysShow = m_AlwaysShow};
            var json = JsonUtility.ToJson(settings);
            File.WriteAllText(Path.Combine(GetSettingsPath(), settingsFilename), json);
        }

        private static void LoadAutoState()
        {
            EnsurePathExists();
            var path = Path.Combine(GetSettingsPath(), settingsFilename);
            if (File.Exists(path))
            {
                var settings = new TankWeaselSettings();
                EditorJsonUtility.FromJsonOverwrite(File.ReadAllText(path), settings);
                if (settings.LastVersion != GuloWorkshops.TankWeasel.TankWeaselCore.Version.ToString())
                {
                    m_AlwaysShow = true;
                    settings.AlwaysShow = true;
                    settings.LastVersion = GuloWorkshops.TankWeasel.TankWeaselCore.Version.ToString();
                    var json = JsonUtility.ToJson(settings);
                    File.WriteAllText(path, json);
                } else {
                    m_AlwaysShow = settings.AlwaysShow;
                }
            }
        }
    }

    public class TankWeaselSettings
    {
        public bool AlwaysShow = true;
        public string LastVersion = GuloWorkshops.TankWeasel.TankWeaselCore.Version.ToString();
    }
}
#endif