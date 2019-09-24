using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NipaPrefs;

namespace Nipa.Demo
{
    public class Jeep : MonoBehaviour
    {

        Vector3 goal;


        public void UpdateGoal()
        {
            goal = new Vector3(Random.Range(DemoParam.fieldMin.x, DemoParam.fieldMax.x), 0f, Random.Range(DemoParam.fieldMin.y, DemoParam.fieldMax.y));
        }

        // Update is called once per frame
        void Update()
        {
            if (!JeepManager. isActive)
                return;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goal - transform.position), Time.deltaTime * JeepManager.rotSpeed);
            transform.Translate(Vector3.forward * JeepManager.speed * Time.deltaTime);
        }
    }
}