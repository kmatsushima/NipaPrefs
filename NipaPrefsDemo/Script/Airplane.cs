using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NipaPrefs;

namespace Nipa.Demo
{
    public class Airplane : MonoBehaviour
    {
        NipaFloat altitude = new NipaFloat(DemoParam.TRANSPORT_PREFS, "altitude", 10f);
        NipaColor smokeColor = new NipaColor(DemoParam.TRANSPORT_PREFS, "smokeColor", Color.yellow);
        ParticleSystem particle;
        Vector3 goal;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            particle = GetComponent<ParticleSystem>();
            goal = new Vector3(Random.Range(DemoParam.fieldMin.x, DemoParam.fieldMax.x), altitude, Random.Range(DemoParam.fieldMin.y, DemoParam.fieldMax.y));

            while (true)
            {
                yield return new WaitForSeconds(5f);
                goal = new Vector3(Random.Range(DemoParam.fieldMin.x, DemoParam.fieldMax.x), altitude, Random.Range(DemoParam.fieldMin.y, DemoParam.fieldMax.y));
            }
        }

        // Update is called once per frame
        void Update()
        {
            var main = particle.main;
            main.startColor = (Color)smokeColor;

            goal.y = altitude;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goal - transform.position), Time.deltaTime * 1f);
            transform.Translate(Vector3.forward *10f * Time.deltaTime);
        }


        Rect dubugWindow = new Rect(0, 250, 350, 300);

        void OnGUI()
        {
            dubugWindow = GUILayout.Window(2, dubugWindow, DebugWindow, "Airplane", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
        }

        void DebugWindow(int id)
        {
            GUILayout.BeginVertical();

            altitude.OnGUI();
            smokeColor.OnGUI("smoke");

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}