using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace NipaPrefs
{
    public static class PathProvider
    {
        #region ===============================================  Define

        public enum EditorRootDirectroy
        {
            Assets,
            StreamingAssets
        }

        public enum StandaloneRootDirectroy
        {
            DataDir,
            StreamingAssets,
            PersistentData,
            DirectoryExeFileIn,
            ParentDirOfDirExeFileIn,
            MyDocuments,
        }

        #endregion

        public static string GetPath(EditorRootDirectroy rootDir, string filePathFromRootDir)
        {
            var path = "";
            switch (rootDir)
            {
                case EditorRootDirectroy.Assets:
                    path = Path.Combine(Application.dataPath, filePathFromRootDir);
                    break;
                case EditorRootDirectroy.StreamingAssets:
                    path = Path.Combine(Application.streamingAssetsPath, filePathFromRootDir);
                    break;
                default:
                    break;
            }
            return path;
        }

        public static string GetPath(StandaloneRootDirectroy rootDir, string filePathFromRootDir)
        {
            var path = "";
            switch (rootDir)
            {
                case StandaloneRootDirectroy.DataDir:
                    path = Path.Combine(Application.dataPath, filePathFromRootDir);
                    break;
                case StandaloneRootDirectroy.DirectoryExeFileIn:
                    path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, filePathFromRootDir);
                    break;
                case StandaloneRootDirectroy.ParentDirOfDirExeFileIn:
                    path = Path.Combine(Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName, filePathFromRootDir);
                    break;
                case StandaloneRootDirectroy.MyDocuments:
                    path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), filePathFromRootDir);
                    break;
                case StandaloneRootDirectroy.StreamingAssets:
                    path = Path.Combine(Application.streamingAssetsPath, filePathFromRootDir);
                    break;
                case StandaloneRootDirectroy.PersistentData:
                    path = Path.Combine(Application.persistentDataPath, filePathFromRootDir);
                    break;
                default:
                    break;
            }
            return path;
        }
    }
}