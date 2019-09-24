using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;

namespace NipaPrefs
{
    public static class NipaPrefsManagerInterface
    {
        static Dictionary<string, NipaPrefsManager> managers = new Dictionary<string, NipaPrefsManager>();

        public static NipaPrefsManager GetManager(string managerId)
        {
            if (!managers.ContainsKey(managerId))
            { 
                var allDatabases = Resources.FindObjectsOfTypeAll<MonoBehaviour>()
                        .Select(v => v.GetComponents<NipaPrefsManager>())
                        .SelectMany(v=>v)
                        .Where(o => o != null);
                foreach (var item in allDatabases)
                {
                    if (!managers.ContainsKey(item.id))
                    {
                        managers.Add(item.id, item);
                        Debug.Log("[NipaPrefs] added manager " + item.id);
                    }
                }
            }

            if (managers.ContainsKey(managerId))
                return managers[managerId];
            else
                return null;
        }
    }
}