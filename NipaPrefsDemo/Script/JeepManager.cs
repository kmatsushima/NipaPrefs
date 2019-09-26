using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NipaPrefs;

namespace Nipa.Demo
{
    public class JeepManager : MonoBehaviour
    {

        public static NipaBool isActive = new NipaBool(DemoParam.TRANSPORT_PREFS, "jeepIsActive", true, "ジープが動くか");
        public static NipaFloat speed = new NipaFloat(DemoParam.TRANSPORT_PREFS, "jeepSpeed", 5f, "ジープのスピード");
        public static NipaFloat rotSpeed = new NipaFloat(DemoParam.TRANSPORT_PREFS, "jeepRotSpeed", 2f, "ジープの回転の速さ");
        public static NipaVector test = new NipaVector(DemoParam.TRANSPORT_PREFS, "test", Vector3.one, "ジープの回転の速さ");
        NipaInt count = new NipaInt(DemoParam.TRANSPORT_PREFS, "jeepCount", 5);
        [SerializeField] Jeep jeepPrefab;
        List<Jeep> jeeps = new List<Jeep>();

        IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);
                foreach (var item in jeeps)
                {
                    item.UpdateGoal();
                }
            }
        }

        private void Update()
        {
            while (jeeps.Count < count)
            {
                var j = Instantiate(jeepPrefab) as Jeep;
                j.transform.SetParent(transform);
                j.transform.position = new Vector3((DemoParam.fieldMin.x + DemoParam.fieldMax.x) * 0.5f, 0f, (DemoParam.fieldMin.y + DemoParam.fieldMax.y) * 0.5f);
                j.UpdateGoal();
                jeeps.Add(j);
            }
            while (jeeps.Count > count)
            {
                var j = jeeps[0];
                Destroy(j.gameObject);
                jeeps.RemoveAt(0);
            }
        }


        Rect dubugWindow = new Rect(0, 0, 350, 200);
        Vector2 scroll;

        void OnGUI()
        {
            dubugWindow = GUILayout.Window(0, dubugWindow, DebugWindow, "Jeep Mgr", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
        }

        void DebugWindow(int id)
        {
            GUILayout.BeginVertical();

            scroll = GUILayout.BeginScrollView(scroll, GUILayout.MinHeight(200));
            count.OnGUI("jeep count");
            isActive.OnGUI("active");
            speed.OnGUISlider(1f, 10f, "speed");
            rotSpeed.OnGUISlider(0.1f, 5f, "rotate speed");
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}