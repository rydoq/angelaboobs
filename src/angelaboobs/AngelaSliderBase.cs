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

public class AngelaSliderBase : MonoBehaviour
{
  private const string NPC_NAME = nameof(NPCEnum.Angela);
  
  private static Mesh _newBoobs;
  private static Mesh _newBody;
  private static Mesh _newSkirt;
  
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
  internal static ConfigEntry<bool> nippleInnieWhenClothed;
  internal static ConfigEntry<bool> nippleGoneWhenClothed;
  internal static ConfigEntry<float> assLarge;
  internal static ConfigEntry<float> thighsThick;
  private static AtlyssDropdown textureDropdown;

  public static AngelaSliderController AngelaSliderController;
  public static TextureChangeAnimEvent eyeController;
  
  public static List<string> textureFolderNames = new List<string>();
  public static int selectedTexture;

  internal static List<Dialog> DialogList = new List<Dialog>();
  
  private void Start()
  {
    InitConfig();
    Settings.OnApplySettings.AddListener(() =>
    {
      if (textureSelector.Value != selectedTexture)
      {
        textureSelector.Value = selectedTexture;
        AngelaSliderController.textureChanged = true;
      }
      config.Save();
      if (AngelaSliderController != null)
        AngelaSliderController.ApplySettings();
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
      _newBoobs = NPCPatcherBase.bundle.LoadAsset<Mesh>("angela_boobs");
      _newBody = NPCPatcherBase.bundle.LoadAsset<Mesh>("angela_base");
      _newSkirt = NPCPatcherBase.bundle.LoadAsset<Mesh>("angela_robeSkirt");
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
      NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[1]._texture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeLeft, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[2]._texture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeRight, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[3]._texture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeDown, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[4]._texture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye3, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[5]._texture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye2, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[6]._texture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyePissed, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[7]._texture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye4, textureFolderNames[selectedTexture]);
    eyeController._textureStructs[8]._texture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye5, textureFolderNames[selectedTexture]);
  }
  
  private void InitConfig()
  {
    var textureSelectionDefinition = new ConfigDefinition("AngelaBoobs", "TextureSelection");
    var textureSelectionDescription = new ConfigDescription("TextureSelection");
    textureSelector = config.Bind(textureSelectionDefinition, 0, textureSelectionDescription);
    selectedTexture = textureSelector.Value;
    
    var toplessDefinition = new ConfigDefinition("AngelaBoobs", "Topless");
    var toplessDescription = new ConfigDescription("Topless");
    topless = config.Bind(toplessDefinition, false, toplessDescription);
    
    var bottomlessDefinition = new ConfigDefinition("AngelaBoobs", "Bottomless");
    var bottomlessDescription = new ConfigDescription("Bottomless");
    bottomless = config.Bind(bottomlessDefinition, false, bottomlessDescription);
    
    var nippleInnieWhenClothedDefinition = new ConfigDefinition("AngelaBoobs", "NippleInnieWhenClothed");
    var nippleInnieWhenClothedDescription = new ConfigDescription("Nipples flat when clothed");
    nippleInnieWhenClothed = config.Bind(nippleInnieWhenClothedDefinition, true, nippleInnieWhenClothedDescription);
    
    var nippleGoneWhenClothedDefinition = new ConfigDefinition("AngelaBoobs", "NippleGoneWhenClothed");
    var nippleGoneWhenClothedDescription = new ConfigDescription("Nipples gone when clothed");
    nippleGoneWhenClothed = config.Bind(nippleGoneWhenClothedDefinition, false, nippleGoneWhenClothedDescription);
    
    var nippleInnieDefinition = new ConfigDefinition("AngelaBoobs", "NippleInnie");
    var nippleInnieDescription = new ConfigDescription("Flat nipple tips");
    nippleInnie = config.Bind(nippleInnieDefinition, 0.0f, nippleInnieDescription);

    var nippleSizeDefinition = new ConfigDefinition("AngelaBoobs", "NippleSize");
    var nippleSizeDescription = new ConfigDescription("Nipple size");
    nippleSize = config.Bind(nippleSizeDefinition, 0.0f, nippleSizeDescription);

    var breastSmallDefinition = new ConfigDefinition("AngelaBoobs", "BreastSmall");
    var breastSmallDescription = new ConfigDescription("Smaller breasts");
    boobSmall = config.Bind(breastSmallDefinition, 0.0f, breastSmallDescription);
    
    var breastLargeDefinition = new ConfigDefinition("AngelaBoobs", "BreastLarge");
    var breastLargeDescription = new ConfigDescription("Larger breasts");
    boobLarge = config.Bind(breastLargeDefinition, 0.0f, breastLargeDescription);

    var nippleGoneDefinition = new ConfigDefinition("AngelaBoobs", "NippleGone");
    var nippleGoneDescription = new ConfigDescription("Make nipples go away");
    nippleGone = config.Bind(nippleGoneDefinition, 0.0f, nippleGoneDescription);
    
    var assLargeDefinition = new ConfigDefinition("AngelaBoobs", "AssLarge");
    var assGoneDescription = new ConfigDescription("Make ass bigger");
    assLarge = config.Bind(assLargeDefinition, 0.0f, assGoneDescription);
    
    var thighsThickDefinition = new ConfigDefinition("AngelaBoobs", "ThighsThick");
    var thighsThickDescription = new ConfigDescription("Make thighs thicker");
    thighsThick = config.Bind(thighsThickDefinition, 0.0f, thighsThickDescription);
  }
  
  private void AddSettings()
  {
    SettingsTab tab = Settings.ModTab;
    tab.AddHeader("Angela Appearance Mod");
    textureDropdown = tab.AddDropdown("Texture", textureFolderNames, selectedTexture);
    textureDropdown.OnValueChanged.AddListener((x) => selectedTexture = x);
    tab.AddToggle("Topless", topless);
    tab.AddToggle("Bottomless", bottomless);
    tab.AddSlider("Larger Breasts", boobLarge);
    tab.AddSlider("Smaller Breasts", boobSmall);
    tab.AddSlider("Nipple Size", nippleSize);
    tab.AddSlider("Nipple Innie", nippleInnie);
    tab.AddSlider("Larger Ass", assLarge);
    tab.AddSlider("Thicker Thighs", thighsThick);
    tab.AddToggle("Nipples flat when clothed", nippleInnieWhenClothed);
    tab.AddToggle("Nipples gone when clothed", nippleGoneWhenClothed);
  }
  
  [HarmonyPatch(typeof(NetNPC), nameof(NetNPC.Start))]
  public static class AddNetNPCPatch {
    [HarmonyPostfix]
    public static void NetNPCAwakePatch(NetNPC __instance) {
      try
      {
        if (__instance.ToString().Contains("_npc_Angela"))
        {
          NPCPatcherBase.Log.LogInfo($"{NPC_NAME} located");
          AngelaSliderController = __instance.gameObject.AddComponent<AngelaSliderController>();
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
          
          var dialogController = __instance.transform.Find("_dialogTrigger").GetComponent<DialogTrigger>();
          foreach (DialogBranch branch in dialogController._scriptDialogData._dialogBranches)
          {
            foreach (Dialog dialog in branch.dialogs)
            {
              DialogList.Add(dialog);
              if (dialog.facepic.name == "facepic_angela01")
              {
                facepic1Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.FacePic1, textureFolderNames[selectedTexture]), dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = facepic1Sprite;
              }
              else if  (dialog.facepic.name == "facepic_angela02")
              {
                facepic2Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.FacePic2, textureFolderNames[selectedTexture]), dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = facepic2Sprite;
              }
              else if  (dialog.facepic.name == "facepic_angela03")
              {
                facepic3Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.FacePic3, textureFolderNames[selectedTexture]), dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = facepic3Sprite;
              }
            }
          }
          
          var mainMesh = __instance.transform.Find("_visual").Find("angela_mesh");
          var oldBoobs = mainMesh.Find("angela_boobs").GetComponent<SkinnedMeshRenderer>();
          
          oldBoobs.sharedMesh = _newBoobs;
          
          var torso = mainMesh.Find("angela_base").GetComponent<SkinnedMeshRenderer>();
          torso.sharedMesh = _newBody;
          
          var skirt = mainMesh.Find("angela_robeSkirt").GetComponent<SkinnedMeshRenderer>();
          skirt.sharedMesh = _newSkirt;
          
          var arms = mainMesh.Find("angela_arms").GetComponent<SkinnedMeshRenderer>();
          var ears = mainMesh.Find("angela_ears").GetComponent<SkinnedMeshRenderer>();
          var eyes =  mainMesh.Find("angela_eyes").GetComponent<SkinnedMeshRenderer>();
          var feet =  mainMesh.Find("angela_feet").GetComponent<SkinnedMeshRenderer>();
          var hairTuft =  mainMesh.Find("angela_hairTuft").GetComponent<SkinnedMeshRenderer>();
          var horns =  mainMesh.Find("angela_horns").GetComponent<SkinnedMeshRenderer>();
          var necklace =   mainMesh.Find("angela_necklace").GetComponent<SkinnedMeshRenderer>();
          var tail =   mainMesh.Find("angela_tail").GetComponent<SkinnedMeshRenderer>();
          var glyph = mainMesh.Find("Armature_character").Find("masterBone").Find("Quad").Find("Quad_01")
            .GetComponent<MeshRenderer>();
          var bookMain = mainMesh.Find("Armature_character").Find("masterBone").Find("hipCtrl").Find("hip")
            .Find("lowBody").Find("midBody").Find("torso").Find("shoulder.l").GetChild(0).GetChild(0).GetChild(0)
            .GetChild(0).Find("bookProp_01");
          var bookL = bookMain.Find("prop_book_L").GetComponent<MeshRenderer>(); 
          var bookR = bookMain.Find("prop_book_R").GetComponent<MeshRenderer>();
          var shadow = __instance.transform.Find("_visual").Find("_shadowProjection").GetChild(0).GetComponent<MeshRenderer>();

          var boobBoneL = __instance._jiggleBones[2];
          var boobBoneR = __instance._jiggleBones[3];
          
          AngelaSliderController.boobs = oldBoobs;
          AngelaSliderController.torso = torso;
          AngelaSliderController.skirt = skirt;
          AngelaSliderController.arms = arms;
          AngelaSliderController.ears = ears;
          AngelaSliderController.eyes = eyes;
          AngelaSliderController.feet = feet;
          AngelaSliderController.hairTuft = hairTuft;
          AngelaSliderController.horns = horns;
          AngelaSliderController.necklace = necklace;
          AngelaSliderController.tail = tail;
          AngelaSliderController.glyph = glyph;
          AngelaSliderController.bookL = bookL;
          AngelaSliderController.bookR = bookR;
          AngelaSliderController.shadow = shadow;
          AngelaSliderController.boobBoneL = boobBoneL;
          AngelaSliderController.boobBoneR = boobBoneR;
          AngelaSliderController.boobBoneLOrig = boobBoneL;
          AngelaSliderController.boobBoneROrig = boobBoneR;
          AngelaSliderController.ApplySettings();
          
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
