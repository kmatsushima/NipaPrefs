using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System;
using NipaPrefs.Hidden;

namespace NipaPrefs
{

    public class NipaPrefsManager : MonoBehaviour
    {


        public event Action OnSaveAll;
        public event Action OnWriteTips;
        public event Action OnPrefsUpdated;
        public event Action OnLoad;

        public string id;


        [SerializeField, HeaderAttribute("Editor Path")]
        PathProvider.EditorRootDirectroy rootDirEditor;
        ///<summary> e.g Settings/MySetting.xml </summary>
        [SerializeField] string filePathFromRootDirEditor;
        [SerializeField, HeaderAttribute("Standalone Path")]
        PathProvider.StandaloneRootDirectroy rootDirStandalone;
        [SerializeField] string filePathFromRootDirStandalone;
        ///<summary> enable invoking event OnPrefsUpdated, so  listeners update its nornaml value according to NipaValue </summary>
        public bool enableManualUpdate = false;
        ///<summary> enable load values from setting file manualy. otherwise, value is loaded when it is accessed </summary>
        public bool enableManualLoad = false;
        public int guiLayoutWindowId;
        public Rect windowRect = new Rect(0, 0, 500, 500);
        public float valueEditorHeight = 300f;

        string path = "";
        string dirPath;
        bool firstLoadDone;
        bool rootExist;
        XDocument xml;
        XElement root;
        bool isMenuActive;
        bool isEditMode;
        List<Action> guiOfValues = new List<Action>();

        public void Load()
        {
            var content = System.IO.File.ReadAllText(path);

            rootExist = content.Contains(this.id);
            if (rootExist)
            {
                xml = XDocument.Parse(content);
                root = xml.Element(this.id);
            }
        }

        public void RegisterEditGui(Action gui)
        {
            guiOfValues.Add(gui);
        }

        public bool GetValue(string id, out string rawValue)
        {
            if (!firstLoadDone)
            {
                if (path == "")
                {
#if UNITY_EDITOR
                    path = PathProvider.GetPath(rootDirEditor, filePathFromRootDirEditor);
#else
                   path = PathProvider.GetPath(rootDirStandalone, filePathFromRootDirStandalone);
#endif
                    CheckPath();
                }

                Load();
                firstLoadDone = true;
            }
            if (rootExist && root.HasElements && root.Elements().Any(v => v.Name == id))
            {
                rawValue = root.Element(id).Value;
                return true;
            }
            rawValue = "";
            rawValue = "";
            return false;
        }

        public void SaveRawValue(string id, string rawValue)
        {
            if (!firstLoadDone)
            {
                CheckPath();
                Load();
            }
            if (!rootExist)
            {
                xml = new XDocument();
                xml.Add(new XElement(this.id, new XElement(id)));
                root = xml.Element(this.id);
                rootExist = true;
            }
            else if (!root.Elements().Any(v => v.Name == id))
                root.Add(new XElement(id));

            root.Element(id).Value = rawValue;
            xml.Save(path);
        }

        public void WriteTip(string id, string tip)
        {
            TipWriter.WriteTipOnFile(path, id, tip);
        }

        public void SetFilePath(string path)
        {
            this.path = path;
            CheckPath();
        }

        public void ToggleMenu(bool active)
        {
            isMenuActive = active;
        }

        public void ManualUpdate()
        {
            OnPrefsUpdated?.Invoke();
        }

        void CheckPath()
        {
            dirPath = Path.GetDirectoryName(path); Debug.Log(dirPath);
            if (!System.IO.File.Exists(path))
            {
                System.IO.Directory.CreateDirectory(dirPath);
                var stream = System.IO.File.Create(path);
                stream.Dispose();
            }
        }

        Vector2 scroll;

        void OnGUI()
        {
            if (isMenuActive)
                windowRect = GUILayout.Window(guiLayoutWindowId, windowRect, DebugWindow, string.Format("NipaPrefsMgr : {0}", id), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
        }

        public void SaveAll()
        {
            OnSaveAll?.Invoke();
        }

        public void WriteTip()
        {
            OnWriteTips?.Invoke();
            xml = XDocument.Load(path);
            root = xml.Element(this.id);
        }

        public void DebugWindow(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("close"))
                isMenuActive = false;
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("file path : " + path);

            var fileExist = File.Exists(path);

            if (fileExist && GUILayout.Button("Open XML file"))
                System.Diagnostics.Process.Start(path);
            if (fileExist && GUILayout.Button("Open directory"))
                System.Diagnostics.Process.Start(dirPath);
            if (enableManualUpdate && GUILayout.Button("Apply all values"))
                ManualUpdate();

            GUILayout.BeginHorizontal();
            isEditMode = GUILayout.Toggle(isEditMode, "Show all values");
            GUILayout.FlexibleSpace();
            if (enableManualLoad && GUILayout.Button("Load"))
                OnLoad?.Invoke();
            if (GUILayout.Button("Save all"))
                SaveAll();
            if (fileExist && rootExist && GUILayout.Button("Write tips"))
                WriteTip();
            if (fileExist && GUILayout.Button("Delete file"))
            {
                File.Delete(path);
                firstLoadDone = false;
            }
            GUILayout.EndHorizontal();


            if (isEditMode)
            {
                scroll = GUILayout.BeginScrollView(scroll, GUILayout.MinHeight(valueEditorHeight));
                foreach (var item in guiOfValues)
                {
                    item();
                }
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}

//else
//{
//    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.dataPath + "/test.xml"))
//    {
//        file.WriteLine(string.Format("<{0}>{1}</{0}>", PARENT_ELEMENT, System.Environment.NewLine));
//    }
//    xml = XDocument.Load(Application.dataPath + "/test.xml");
//    xmlElements = xml.Element(PARENT_ELEMENT);
//}