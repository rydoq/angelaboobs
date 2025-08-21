using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using Nessie.ATLYSS.EasySettings;
using Nessie.ATLYSS.EasySettings.UIElements;
using BepInEx.Configuration;
using System.IO;
using System.Xml;

namespace angelaboobs;

public class EnokSliderBase : MonoBehaviour
{
  private const string NPC_NAME = nameof(NPCEnum.Enok);
  
  private static Mesh _newCloth;
  public static Mesh dick;
  public static Mesh dickSheath;
  public static AnimationCurve dickAnimationCurve;
  public static AnimationCurve clothAnimationCurve;
  
  public static Sprite facepic1Sprite;

  public ConfigFile config;
  public PluginInfo info;
  
  internal static ConfigEntry<int> textureSelector;
  internal static ConfigEntry<int> dickType;
  internal static ConfigEntry<KeyCode> dickErect;
  internal static ConfigEntry<bool> clothGone;
  private static AtlyssDropdown textureDropdown;
  private static AtlyssDropdown dickDropdown;

  public static EnokSliderController EnokSliderController;
  
  public static List<string> textureFolderNames = new List<string>();
  public static int selectedTexture;
  public static List<string> dickNames = new List<string>();
  public static int selectedDick;

  internal static List<Dialog> DialogList = new List<Dialog>();
  
  private void Start()
  {
    InitConfig();
    Settings.OnApplySettings.AddListener(() =>
    {
      if (textureSelector.Value != selectedTexture)
      {
        textureSelector.Value = selectedTexture;
        EnokSliderController.textureChanged = true;
      }

      if (dickType.Value != selectedDick) dickType.Value = selectedDick;
      config.Save();
      if (EnokSliderController != null)
        EnokSliderController.ApplySettings();
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
    
    try
    {
      _newCloth = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_cloth");
      dick = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_cock");
      dickSheath = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_sheathCock");
      
      dickNames.Add(dick.name);
      dickNames.Add(dickSheath.name);
    }
    catch (Exception e)
    {
      NPCPatcherBase.Log.LogError("Unable to find angelaboobs.unity3d in " + info.Location + ". Patcher will not run correctly.");
      NPCPatcherBase.Log.LogError(e.ToString());
    }
    AddSettings();
    
    Keyframe clothKeyframe1 = new Keyframe(0, 0);
    clothKeyframe1.inTangent = 2.25f;
    clothKeyframe1.outTangent = 2.25f;
    clothKeyframe1.outWeight = 0.015f;
    Keyframe clothKeyframe2 = new Keyframe(1, 1);

    dickAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    dickAnimationCurve.preWrapMode = WrapMode.Clamp;
    dickAnimationCurve.postWrapMode = WrapMode.Clamp;

    clothAnimationCurve = new AnimationCurve(clothKeyframe1, clothKeyframe2);
    clothAnimationCurve.preWrapMode = WrapMode.Clamp;
    clothAnimationCurve.postWrapMode = WrapMode.Clamp;
  }
  
  private void InitConfig()
  {
    var textureSelectionDefinition = new ConfigDefinition("EnokSlider", "TextureSelection");
    var textureSelectionDescription = new ConfigDescription("TextureSelection");
    textureSelector = config.Bind(textureSelectionDefinition, 0, textureSelectionDescription);
    selectedTexture = textureSelector.Value;
    
    var dickTypeDefinition = new ConfigDefinition("EnokSlider", "DickType");
    var dickTypeDescription = new ConfigDescription("DickType");
    dickType = config.Bind(dickTypeDefinition, 0, dickTypeDescription);
    selectedDick = dickType.Value;
    
    var clothlessDefinition = new ConfigDefinition("EnokSlider", "Clothless");
    var clothlessDescription = new ConfigDescription("Clothless");
    clothGone = config.Bind(clothlessDefinition, false, clothlessDescription);
    
    var dickErectDefinition = new ConfigDefinition("EnokSlider", "DickErect");
    var dickErectDescription = new ConfigDescription("Erection hotkey");
    dickErect = config.Bind(dickErectDefinition, KeyCode.Period, dickErectDescription);
  }
  
  private void AddSettings()
  {
    SettingsTab tab = Settings.ModTab;
    tab.AddHeader("Enok Appearance Mod");
    textureDropdown = tab.AddDropdown("Texture", textureFolderNames, selectedTexture);
    textureDropdown.OnValueChanged.AddListener((x) => selectedTexture = x);
    dickDropdown = tab.AddDropdown("Dick Type", dickNames, selectedDick);
    dickDropdown.OnValueChanged.AddListener((x) => selectedDick = x);
    tab.AddToggle("Bottomless", clothGone);
    tab.AddKeyButton("Erection shortcut", dickErect);
  }
  
  [HarmonyPatch(typeof(NetNPC), nameof(NetNPC.Start))]
  public static class AddNetNPCPatch {
    [HarmonyPostfix]
    public static void NetNPCAwakePatch(NetNPC __instance) {
      try
      {
        if (__instance.ToString().Contains("_npc_Enok"))
        {
          NPCPatcherBase.Log.LogInfo($"{NPC_NAME} located");
          EnokSliderController = __instance.gameObject.AddComponent<EnokSliderController>();
          
          var dialogController = __instance.transform.Find("_dialogTrigger_01").GetComponent<DialogTrigger>();
          foreach (DialogBranch branch in dialogController._scriptDialogData._dialogBranches)
          {
            foreach (Dialog dialog in branch.dialogs)
            {
              DialogList.Add(dialog);
              facepic1Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.FacePic1, textureFolderNames[selectedTexture]), dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
              dialog.facepic = facepic1Sprite;
            }
          }
          
          var mainMesh = __instance.transform.Find("_visual").Find("enok_fbx");
          var cloth = mainMesh.Find("_enok_cloth").GetComponent<SkinnedMeshRenderer>();
          
          cloth.sharedMesh = _newCloth;
          
          var torso = mainMesh.Find("_enok_base").GetComponent<SkinnedMeshRenderer>();
          var belt = mainMesh.Find("_enok_belt").GetComponent<SkinnedMeshRenderer>();
          var arms = mainMesh.Find("_enok_arms").GetComponent<SkinnedMeshRenderer>();
          var ears = mainMesh.Find("_enok_ears").GetComponent<SkinnedMeshRenderer>();
          var neckbrace =  mainMesh.Find("_enok_neckbrace").GetComponent<SkinnedMeshRenderer>();
          var feet =  mainMesh.Find("_enok_feet").GetComponent<SkinnedMeshRenderer>();
          var horns =  mainMesh.Find("_enok_horns").GetComponent<SkinnedMeshRenderer>();
          var tail =   mainMesh.Find("_enok_tail").GetComponent<SkinnedMeshRenderer>();
          var shadow = __instance.transform.Find("_visual").Find("_shadowProjection").GetChild(0).GetComponent<MeshRenderer>();
          var torsoBone = mainMesh.Find("Armature_character").GetChild(0).GetChild(0).GetChild(0)
            .Find("lowBody").GetChild(0).GetChild(0);
          var axe1 = torsoBone.Find("heavyAxe_01").GetComponent<MeshRenderer>();
          var axe2 = torsoBone.Find("heavyAxe_02").GetComponent<MeshRenderer>();
          var dickMaterial = torso.material;
          var dick = mainMesh.gameObject.AddComponent<SkinnedMeshRenderer>();
          dick.bones = cloth.bones;
          dick.material = dickMaterial;
          dick.sharedMesh = dickNames[selectedDick] == EnokSliderBase.dick.name ? EnokSliderBase.dick : dickSheath;
          
          EnokSliderController.cloth = cloth;
          EnokSliderController.torso = torso;
          EnokSliderController.belt = belt;
          EnokSliderController.arms = arms;
          EnokSliderController.ears = ears;
          EnokSliderController.feet = feet;
          EnokSliderController.neckbrace = neckbrace;
          EnokSliderController.horns = horns;
          EnokSliderController.tail = tail;
          EnokSliderController.axe1 = axe1;
          EnokSliderController.axe2 = axe2;
          EnokSliderController.shadow = shadow;
          EnokSliderController.dick = dick;
          EnokSliderController.ApplySettings();
          
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