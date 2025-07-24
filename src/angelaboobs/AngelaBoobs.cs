using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Nessie.ATLYSS.EasySettings;
using BepInEx.Configuration;
using System.IO;


namespace angelaboobs;

[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class AngelaBoobsBase : BaseUnityPlugin
{
  internal static new ManualLogSource Log = null!;

  public static Mesh newBoobs;
  public static Texture clothedTexture;
  public static Texture nudeTexture;

  private string textureName = "angelaTex_01";
  
  internal static ConfigEntry<float> nippleInnie;
  internal static ConfigEntry<float> nippleSize;
  internal static ConfigEntry<float> boobSmall;
  internal static ConfigEntry<float> boobLarge;
  internal static ConfigEntry<float> nippleGone;
  internal static ConfigEntry<bool> topless;

  public static AngelaBoobController angelaBoobController;

  private void Awake()
  {
    new Harmony("ry.angelaboobs").PatchAll();
    
    InitConfig();
    Settings.OnInitialized.AddListener(AddSettings);
    Settings.OnApplySettings.AddListener(() =>
    {
      Config.Save();
      angelaBoobController.ApplySettings();
    });
    Log = Logger;
    
    DirectoryInfo texturePath = new DirectoryInfo(Path.Combine(Paths.ConfigPath, "NPCNSFWModFiles", "Angela"));
    if (!texturePath.Exists)
    {
      texturePath = new DirectoryInfo(Path.Combine(Info.Location, "Config", "NPCNSFWModFiles", "Angela"));
    }

    try
    {
      clothedTexture = LoadTexture(new FileInfo(Path.Combine(texturePath.FullName, textureName + ".png")), "angelaTex_01.png");
      nudeTexture = LoadTexture(new FileInfo(Path.Combine(texturePath.FullName, textureName + "_nude.png")), "angelaTex_01_nude.png");
    }
    catch (Exception e)
    {
      Log.LogError("Unable to find textures " + textureName + ".png and " + textureName + "_nude.png in path " + texturePath.FullName + ". Continuing.");
    }

    try
    {
      AssetBundle bundle = AssetBundle.LoadFromFile(Info.Location.Replace("angelaboobs.dll", "angelaboobs.unity3d"));
      newBoobs = bundle.LoadAsset<Mesh>("angela_boobs");
    }
    catch (Exception e)
    {
      Log.LogError("Unable to find angelaboobs.unity3d in " + Info.Location + ". Patcher will not run correctly.");
    }

    Log.LogInfo($"Plugin {LCMPluginInfo.PLUGIN_NAME} version {LCMPluginInfo.PLUGIN_VERSION} is loaded!");

  }

  private Texture LoadTexture(FileInfo FileI, string objname)
  {
    byte[] raw = File.ReadAllBytes(FileI.FullName);

    Texture2D Tex = new(128, 128, TextureFormat.ARGB32, false) {
      name = objname,
      anisoLevel = 16,
      filterMode = FilterMode.Point,
      mipMapBias = 0
    };

    Tex.LoadImage(raw, false);
    
    return Tex;
  }
  
  
  private void InitConfig()
    {
      var toplessDefinition = new ConfigDefinition("Boobs", "Topless");
      var toplessDescription = new ConfigDescription("Topless");
      topless = Config.Bind(toplessDefinition, false, toplessDescription);
      
        var nippleInnieDefinition = new ConfigDefinition("Boobs", "NippleInnie");
        var nippleInnieDescription = new ConfigDescription("Flat nipple tips");
        nippleInnie = Config.Bind(nippleInnieDefinition, 0.0f, nippleInnieDescription);

        var nippleSizeDefinition = new ConfigDefinition("Boobs", "NippleSize");
        var nippleSizeDescription = new ConfigDescription("Nipple size");
        nippleSize = Config.Bind(nippleSizeDefinition, 0.0f, nippleSizeDescription);

        var breastSmallDefinition = new ConfigDefinition("Boobs", "BreastSmall");
        var breastSmallDescription = new ConfigDescription("Smaller breasts");
        boobSmall = Config.Bind(breastSmallDefinition, 0.0f, breastSmallDescription);
        
        var breastLargeDefinition = new ConfigDefinition("Boobs", "BreastLarge");
        var breastLargeDescription = new ConfigDescription("Larger breasts");
        boobLarge = Config.Bind(breastLargeDefinition, 0.0f, breastLargeDescription);

        var nippleGoneDefinition = new ConfigDefinition("Boobs", "NippleGone");
        var nippleGoneDescription = new ConfigDescription("Make nipples go away");
        nippleGone = Config.Bind(nippleGoneDefinition, 0.0f, nippleGoneDescription);
    }
  
  private void AddSettings()
  {
    SettingsTab tab = Settings.ModTab;
    tab.AddHeader("Angela Breast Improvement");
    tab.AddToggle("Topless", topless);
    tab.AddSlider("Larger Breasts", boobLarge);
    tab.AddSlider("Smaller Breasts", boobSmall);
    tab.AddSlider("Nipple Size", nippleSize);
    tab.AddSlider("Nipple Innie", nippleInnie);
    tab.AddSlider("Nipple Gone", nippleGone);
  }

  [HarmonyPatch(typeof(NetNPC), nameof(NetNPC.Start))]
  public static class AddNetNPCPatch {
    [HarmonyPostfix]
    public static void NetNPCAwakePatch(NetNPC __instance) {
      try
      {
        if (__instance.ToString().Contains("_npc_Angela"))
        {
          Log.LogInfo("Angela located");
          angelaBoobController = __instance.gameObject.AddComponent<AngelaBoobController>();
          var oldBoobs = __instance.transform.Find("_visual").Find("angela_mesh").Find("angela_boobs").GetComponent<SkinnedMeshRenderer>();
          oldBoobs.sharedMesh = newBoobs;
          var torso = __instance.transform.Find("_visual").Find("angela_mesh").Find("angela_base").GetComponent<SkinnedMeshRenderer>();
          angelaBoobController.boobs = oldBoobs;
          angelaBoobController.torso = torso;
          angelaBoobController.ApplySettings();
          
          Log.LogInfo("Angela boobs patched!");
        }
      }
      catch
      {
      }
      
    }
  }

  [HarmonyPatch(typeof(VisualManager), nameof(VisualManager.Apply_FilteringMode))]
  public static class ChangeFilteringModePatch
  {
    [HarmonyPostfix]
    public static void FilteringModePatch(FilterMode _filterMode)
    {
      clothedTexture.filterMode = _filterMode;
      clothedTexture.anisoLevel = 16;
      nudeTexture.filterMode = _filterMode;
      nudeTexture.anisoLevel = 16;
    }
  }
  


}
