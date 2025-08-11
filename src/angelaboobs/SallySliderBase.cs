using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using Nessie.ATLYSS.EasySettings;
using Nessie.ATLYSS.EasySettings.UIElements;
using BepInEx.Configuration;
using System.IO;

namespace angelaboobs;

public class SallySliderBase : MonoBehaviour
{
  private const string NPC_NAME = nameof(NPCEnum.Sally);
  
  private static Mesh _newBoobs;
  private static Mesh _newBody;
  private static Mesh _newArms;
  private static Mesh _newApron;
  private static Mesh _newSkirt;
  private static Mesh _newHood;
  public static Mesh hoodHair;
  public static Mesh longHair;
  
  public static Sprite facepic1Sprite;
  public static Sprite facepic2Sprite;
  public static Sprite facepic3Sprite;

  public ConfigFile config;
  public PluginInfo info;
  
  internal static ConfigEntry<int> textureSelector;
  internal static ConfigEntry<float> nippleInnie;
  internal static ConfigEntry<float> nippleSize;
  internal static ConfigEntry<float> boobSmall;
  internal static ConfigEntry<float> boobLarge;
  internal static ConfigEntry<float> nippleGone;
  internal static ConfigEntry<bool> topless;
  internal static ConfigEntry<bool> bottomless;
  internal static ConfigEntry<bool> apronless;
  internal static ConfigEntry<bool> hoodOn;
  internal static ConfigEntry<bool> hoodDown;
  internal static ConfigEntry<bool> nippleInnieWhenClothed;
  internal static ConfigEntry<bool> nippleGoneWhenClothed;
  internal static ConfigEntry<float> assLarge;
  internal static ConfigEntry<float> thighsThick;
  internal static ConfigEntry<float> bellySize;
  private static AtlyssDropdown textureDropdown;

  public static SallySliderController SallySliderController;
  public static TextureChangeAnimEvent eyeController;
  
  public static List<string> textureFolderNames = new List<string>();
  public static int selectedTexture;

  internal static List<Dialog> DialogList = new List<Dialog>();
  
  private void Start()
  {
    InitConfig();
    Settings.OnApplySettings.AddListener(() =>
    {
      textureSelector.Value = selectedTexture;
      config.Save();
      if (SallySliderController != null)
        SallySliderController.ApplySettings();
      if (eyeController != null)
        ApplyEyeTexturesToEyeController();
    });
    
    try
    {
      DirectoryInfo[] textureFolders = new DirectoryInfo(Path.Combine(NPCPatcherBase.texturePath, NPC_NAME)).GetDirectories();
      
      for (int i = 0; i < textureFolders.Length; i++)
      {
        textureFolderNames.Add(textureFolders[i].Name);
        if (NPCPatcherBase.VerboseLogging)
          NPCPatcherBase.Log.LogInfo($"Loaded {textureFolderNames[i]} texture group");
      }

      if (selectedTexture > textureFolders.Length-1)
      {
        selectedTexture = textureFolders.Length-1; // set if the player deletes a texture mod and it can't load it
      }
    }
    catch (Exception e)
    {
      NPCPatcherBase.Log.LogError("Unable to find textures in path " + NPCPatcherBase.texturePath + ". Continuing.");
      NPCPatcherBase.Log.LogError(e.ToString());
    }
    AddSettings();
    
    try
    {
      _newBoobs = NPCPatcherBase.bundle.LoadAsset<Mesh>("_sally_boobs");
      _newBody = NPCPatcherBase.bundle.LoadAsset<Mesh>("_sally_base");
      _newSkirt = NPCPatcherBase.bundle.LoadAsset<Mesh>("_sally_skirt");
      _newArms = NPCPatcherBase.bundle.LoadAsset<Mesh>("_sally_arms");
      _newApron = NPCPatcherBase.bundle.LoadAsset<Mesh>("_sally_apron");
      _newHood = NPCPatcherBase.bundle.LoadAsset<Mesh>("_sally_hood");
      hoodHair = NPCPatcherBase.bundle.LoadAsset<Mesh>("_sally_hair");
      longHair = NPCPatcherBase.bundle.LoadAsset<Mesh>("_sally_longHair");
    }
    catch (Exception e)
    {
      NPCPatcherBase.Log.LogError("Unable to find angelaboobs.unity3d in " + info.Location + ". Patcher will not run correctly.");
      NPCPatcherBase.Log.LogError(e.ToString());
    }
  }
  
  private static void ApplyEyeTexturesToEyeController()
  {
    eyeController._textureStructs[0]._texture =
      NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Eye, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[1]._texture = 
      NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Eye2, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[2]._texture = 
      NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Eye3, textureFolderNames[selectedTexture]);
  }
  
  private void InitConfig()
  {
    var textureSelectionDefinition = new ConfigDefinition("SallySlider", "TextureSelection");
    var textureSelectionDescription = new ConfigDescription("TextureSelection");
    textureSelector = config.Bind(textureSelectionDefinition, 0, textureSelectionDescription);
    selectedTexture = textureSelector.Value;
    
    var toplessDefinition = new ConfigDefinition("SallySlider", "Topless");
    var toplessDescription = new ConfigDescription("Topless");
    topless = config.Bind(toplessDefinition, false, toplessDescription);
    
    var apronlessDefinition = new ConfigDefinition("SallySlider", "Apronless");
    var apronlessDescription = new ConfigDescription("Apronless");
    apronless = config.Bind(apronlessDefinition, false, apronlessDescription);
    
    var hoodOnDefinition = new ConfigDefinition("SallySlider", "HoodOn");
    var hoodOnDescription = new ConfigDescription("HoodOn");
    hoodOn = config.Bind(hoodOnDefinition, true, hoodOnDescription);
    
    var hoodDownDefinition = new ConfigDefinition("SallySlider", "HoodDown");
    var hoodDownDescription = new ConfigDescription("HoodDown");
    hoodDown = config.Bind(hoodDownDefinition, false, hoodDownDescription);
    
    var bottomlessDefinition = new ConfigDefinition("SallySlider", "Bottomless");
    var bottomlessDescription = new ConfigDescription("Bottomless");
    bottomless = config.Bind(bottomlessDefinition, false, bottomlessDescription);
    
    var nippleInnieWhenClothedDefinition = new ConfigDefinition("SallySlider", "NippleInnieWhenClothed");
    var nippleInnieWhenClothedDescription = new ConfigDescription("Nipples flat when clothed");
    nippleInnieWhenClothed = config.Bind(nippleInnieWhenClothedDefinition, true, nippleInnieWhenClothedDescription);
    
    var nippleGoneWhenClothedDefinition = new ConfigDefinition("SallySlider", "NippleGoneWhenClothed");
    var nippleGoneWhenClothedDescription = new ConfigDescription("Nipples gone when clothed");
    nippleGoneWhenClothed = config.Bind(nippleGoneWhenClothedDefinition, false, nippleGoneWhenClothedDescription);
    
    var nippleInnieDefinition = new ConfigDefinition("SallySlider", "NippleInnie");
    var nippleInnieDescription = new ConfigDescription("Flat nipple tips");
    nippleInnie = config.Bind(nippleInnieDefinition, 0.0f, nippleInnieDescription);

    var nippleSizeDefinition = new ConfigDefinition("SallySlider", "NippleSize");
    var nippleSizeDescription = new ConfigDescription("Nipple size");
    nippleSize = config.Bind(nippleSizeDefinition, 0.0f, nippleSizeDescription);

    var breastSmallDefinition = new ConfigDefinition("SallySlider", "BreastSmall");
    var breastSmallDescription = new ConfigDescription("Smaller breasts");
    boobSmall = config.Bind(breastSmallDefinition, 0.0f, breastSmallDescription);
    
    var breastLargeDefinition = new ConfigDefinition("SallySlider", "BreastLarge");
    var breastLargeDescription = new ConfigDescription("Larger breasts");
    boobLarge = config.Bind(breastLargeDefinition, 0.0f, breastLargeDescription);

    var nippleGoneDefinition = new ConfigDefinition("SallySlider", "NippleGone");
    var nippleGoneDescription = new ConfigDescription("Make nipples go away");
    nippleGone = config.Bind(nippleGoneDefinition, 0.0f, nippleGoneDescription);
    
    var assLargeDefinition = new ConfigDefinition("SallySlider", "AssLarge");
    var assGoneDescription = new ConfigDescription("Make ass bigger");
    assLarge = config.Bind(assLargeDefinition, 0.0f, assGoneDescription);
    
    var thighsThickDefinition = new ConfigDefinition("SallySlider", "ThighsThick");
    var thighsThickDescription = new ConfigDescription("Make thighs thicker");
    thighsThick = config.Bind(thighsThickDefinition, 0.0f, thighsThickDescription);
    
    var bellyBigDefinition = new ConfigDefinition("SallySlider", "BellyBig");
    var bellyBigDescription = new ConfigDescription("Make belly bigger");
    bellySize = config.Bind(bellyBigDefinition, 0.0f, bellyBigDescription);
  }
  
  private void AddSettings()
  {
    SettingsTab tab = Settings.ModTab;
    tab.AddHeader("Sally Appearance Mod");
    textureDropdown = tab.AddDropdown("Texture", textureFolderNames, selectedTexture);
    textureDropdown.OnValueChanged.AddListener((x) => selectedTexture = x);
    tab.AddToggle("Topless", topless);
    tab.AddToggle("Bottomless", bottomless);
    tab.AddToggle("Apronless", apronless);
    tab.AddToggle("Hood On", hoodOn);
    tab.AddToggle("Hood Down",  hoodDown);
    tab.AddSlider("Larger Breasts", boobLarge);
    tab.AddSlider("Smaller Breasts", boobSmall);
    tab.AddSlider("Nipple Size", nippleSize);
    tab.AddSlider("Nipple Innie", nippleInnie);
    tab.AddSlider("Larger Ass", assLarge);
    tab.AddSlider("Thicker Thighs", thighsThick);
    tab.AddSlider("Bigger Belly", bellySize);
    tab.AddToggle("Nipples flat when clothed", nippleInnieWhenClothed);
    tab.AddToggle("Nipples gone when clothed", nippleGoneWhenClothed);
  }
  
  [HarmonyPatch(typeof(NetNPC), nameof(NetNPC.Start))]
  public static class AddNetNPCPatch {
    [HarmonyPostfix]
    public static void NetNPCAwakePatch(NetNPC __instance) {
      try
      {
        if (__instance.ToString().Contains("_npc_Sally"))
        {
          NPCPatcherBase.Log.LogInfo($"{NPC_NAME} located");
          SallySliderController = __instance.gameObject.AddComponent<SallySliderController>();
          eyeController = __instance.transform.Find("_visual").GetComponent<TextureChangeAnimEvent>();
          ApplyEyeTexturesToEyeController();

          /* trying to grab her animator to change her face texture
          var animController = __instance._animator.runtimeAnimatorController;
          foreach (AnimationClip clip in animController.animationClips)
          {
            if (clip.events.Length > 0)
              for (int i = 0; i < clip.events.Length; i++)
              {
                if (clip.events[i] != null && clip.events[i].functionName == "ChangeTexture")
                  clip.events[i].intParameter = 0;
              }
          }
          */
          
          var dialogController = __instance.transform.Find("_dialogTrigger_01").GetComponent<DialogTrigger>();
          foreach (DialogBranch branch in dialogController._scriptDialogData._dialogBranches)
          {
            foreach (Dialog dialog in branch.dialogs)
            {
              DialogList.Add(dialog);
              if (dialog.facepic.name == "facepic_sally01")
              {
                facepic1Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.FacePic1, textureFolderNames[selectedTexture]), dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = facepic1Sprite;
              }
              else if  (dialog.facepic.name == "facepic_sally02")
              {
                facepic2Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.FacePic2, textureFolderNames[selectedTexture]), dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = facepic2Sprite;
              }
              else if  (dialog.facepic.name == "facepic_sally03")
              {
                facepic3Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.FacePic3, textureFolderNames[selectedTexture]), dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = facepic3Sprite;
              }
            }
          }
          
          var mainMesh = __instance.transform.Find("_visual").Find("sally_FBX");
          var oldBoobs = mainMesh.Find("_sally_boobs").GetComponent<SkinnedMeshRenderer>();
          
          oldBoobs.sharedMesh = _newBoobs;
          
          var torso = mainMesh.Find("_sally_base").GetComponent<SkinnedMeshRenderer>();
          torso.sharedMesh = _newBody;
          
          var skirt = mainMesh.Find("_sally_skirt").GetComponent<SkinnedMeshRenderer>();
          skirt.sharedMesh = _newSkirt;
          
          var arms = mainMesh.Find("_sally_arms").GetComponent<SkinnedMeshRenderer>();
          arms.sharedMesh = _newArms;
          
          var ears = mainMesh.Find("_sally_ears").GetComponent<SkinnedMeshRenderer>();
          var eyes =  mainMesh.Find("_sally_eyes").GetComponent<SkinnedMeshRenderer>();
          var hair =  mainMesh.Find("_sally_hair").GetComponent<SkinnedMeshRenderer>();
          if (hoodOn.Value)
            hair.sharedMesh = hoodHair;
          else
            hair.sharedMesh = longHair;
          
          var apron =  mainMesh.Find("_sally_apron").GetComponent<SkinnedMeshRenderer>();
          apron.sharedMesh = _newApron;
          
          var hood =   mainMesh.Find("_sally_hood").GetComponent<SkinnedMeshRenderer>();
          hood.sharedMesh = _newHood;
          
          var tail =   mainMesh.Find("_sally_tail").GetComponent<SkinnedMeshRenderer>();
          
          var broom = mainMesh.Find("Armature_character").Find("masterBone").Find("hipCtrl").Find("hip")
            .Find("lowBody").Find("midBody").Find("torso").Find("shoulder.r").GetChild(0).GetChild(0).GetChild(0)
            .Find("_sallyBroomFBX").GetComponent<MeshRenderer>();
          var shadow = __instance.transform.Find("_visual").Find("_shadowProjection").GetChild(0).GetComponent<MeshRenderer>();

          var boobBoneL = __instance._jiggleBones[2];
          var boobBoneR = __instance._jiggleBones[3];
          
          SallySliderController.boobs = oldBoobs;
          SallySliderController.torso = torso;
          SallySliderController.skirt = skirt;
          SallySliderController.arms = arms;
          SallySliderController.ears = ears;
          SallySliderController.eyes = eyes;
          SallySliderController.hood = hood;
          SallySliderController.hair = hair;
          SallySliderController.apron = apron;
          SallySliderController.tail = tail;
          SallySliderController.broom = broom;
          SallySliderController.shadow = shadow;
          SallySliderController.boobBoneL = boobBoneL;
          SallySliderController.boobBoneR = boobBoneR;
          SallySliderController.boobBoneLOrig = boobBoneL;
          SallySliderController.boobBoneROrig = boobBoneR;
          SallySliderController.ApplySettings();
          
          NPCPatcherBase.Log.LogInfo($"{NPC_NAME} body patched!");
        }
      }
      catch (Exception e)
      {
        NPCPatcherBase.Log.LogError(e);
      }
      
    }
  }
}
