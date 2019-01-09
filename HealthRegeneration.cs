
using Harmony;
using System.Xml;
using System.IO;
using System.Reflection;
using UnityEngine;


[HarmonyPatch(typeof(ExperienceMode), "Start")]
class HealthRegeneration
{
    static string configFileName = "HealthRegeneration.xml";
    static float sleep = 0;
    static float awake = 0;

    static public void OnLoad()
    {
        Debug.LogFormat("HealthRegeneration: init");

        string modsDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string configPath = Path.Combine(modsDir, configFileName);

        XmlDocument xml = new XmlDocument();
        xml.Load(configPath);

        if (!GetNodeFloat(xml.SelectSingleNode("/config/sleep"), out sleep))
        {
            Debug.LogFormat("HealthRegeneration: missing/invalid 'sleep' entry");
            sleep = 0;
        }
        if (!GetNodeFloat(xml.SelectSingleNode("/config/awake"), out awake))
        {
            Debug.LogFormat("HealthRegeneration: missing/invalid 'awake' entry");
            awake = 0;
        }

    }

    static private bool GetNodeFloat(XmlNode node, out float value)
    {
        if (node == null || node.Attributes["value"] == null || !float.TryParse(node.Attributes["value"].Value, out value))
        {
            value = 1f;
            return false;
        }

        return true;
    }

    public static void Prefix(ExperienceMode __instance)
    {
        __instance.m_ConditonRecoveryFromRestScale = sleep;
        __instance.m_ConditonRecoveryWhileAwakeScale = awake;
    }
}
