using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NipaPrefs;

namespace Nipa.Demo
{
    public class Building : MonoBehaviour
    {
        NipaString signTitle = new NipaString(DemoParam.ENVIROMENT_PREFS, "signTitle", "NIPA Inc.");
        NipaVector scale = new NipaVector(DemoParam.ENVIROMENT_PREFS, "buildingScale", new Vector3(2f, 7f, 2f), "scale of building xyz");
        NipaVector position = new NipaVector(DemoParam.ENVIROMENT_PREFS, "buildingPosition", Vector3.zero, "position of building xyz");
        NipaColor color = new NipaColor(DemoParam.ENVIROMENT_PREFS, "buildingColor", Color.gray, "color of building");
        TextMesh sign;
        MaterialPropertyBlock mpb;
        // Start is called before the first frame update
        void Start()
        {
            sign = GetComponentInChildren<TextMesh>();
            sign.transform.SetParent(transform.parent);
            mpb = new MaterialPropertyBlock();

            UpdateBuilding();
            NipaPrefsManagerInterface.GetManager(DemoParam.ENVIROMENT_PREFS).OnPrefsUpdated += () => UpdateBuilding();
        }

        private void Update()
        {
            sign.transform.Rotate(0f, 30f * Time.deltaTime, 0f);
        }

        void UpdateBuilding()
        {
            transform.localScale = scale;
            transform.position = position + Vector3.up * scale.y * 0.5f;
            sign.transform.position = position + Vector3.up * (scale.y + 1f);
            sign.color = color;
            sign.text = signTitle;
            var renderer = GetComponent<MeshRenderer>();

            mpb.SetColor("_Color", color);
            renderer.SetPropertyBlock(mpb);
        }


        Rect dubugWindow = new Rect(0, 600, 350, 300);

        void OnGUI()
        {
            dubugWindow = GUILayout.Window(-3, dubugWindow, DebugWindow, "Building", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
        }

        void DebugWindow(int id)
        {
            GUILayout.BeginVertical();

            position.OnGUI();
            scale.OnGUISlider(1f, 20f);
            color.OnGUI();
            signTitle.OnGUI("sign title");

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}