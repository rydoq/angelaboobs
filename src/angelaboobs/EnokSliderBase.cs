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

  public static GameObject CockObject;
  public static GameObject SheathCockObject;
  private static Mesh _newCloth;
  private static Mesh _newTorso;
  private static Mesh _newArms;
  private static Mesh _newFeet;
  private static Mesh _newBelt;
  private static Mesh _nipples;
  private static Mesh _beard;
  private static Mesh _eyes;
  public static AnimationCurve dickAnimationCurve;
  public static AnimationCurve clothAnimationCurve;
  
  public static Sprite facepic1Sprite;

  public ConfigFile config;
  public PluginInfo info;
  
  internal static ConfigEntry<int> textureSelector;
  internal static ConfigEntry<int> dickType;
  internal static ConfigEntry<float> dickSize;
  internal static ConfigEntry<float> ballsSize;
  internal static ConfigEntry<float> ballsDivot;
  internal static ConfigEntry<KeyCode> dickErect;
  internal static ConfigEntry<bool> clothGone;
  internal static ConfigEntry<bool> helmetOff;
  internal static ConfigEntry<bool> beardToggle;
  internal static ConfigEntry<float> bodyFemboy;
  internal static ConfigEntry<float> bodyMuscular;
  internal static ConfigEntry<float> bodyAssSize;
  internal static ConfigEntry<float> bodyBellySize;
  private static AtlyssDropdown textureDropdown;
  private static AtlyssDropdown dickDropdown;

  public static EnokSliderController EnokSliderController;
  
  public static List<string> textureFolderNames = new List<string>();
  public static int selectedTexture;
  public static List<string> dickNames = new List<string>();
  public static int selectedDick;
  private static bool _helmetOffPrevious;
  public static List<Mesh> dicksMeshList = new List<Mesh>();

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

      if (helmetOff.Value != _helmetOffPrevious)
      {
        EnokSliderController.textureChanged = true;
        _helmetOffPrevious = helmetOff.Value;
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
      _nipples = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_nipples");
      _beard = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_beard");
      _eyes = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_eyes");
      _newTorso = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_base");
      _newArms = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_arms");
      _newBelt = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_belt");
      _newFeet = NPCPatcherBase.bundle.LoadAsset<Mesh>("_enok_feet");
      
      CockObject = NPCPatcherBase.bundle.LoadAsset<GameObject>("enok_cock");
      SheathCockObject = NPCPatcherBase.bundle.LoadAsset<GameObject>("enok_sheathcock");
      
      dickNames.Add(CockObject.name);
      dickNames.Add(SheathCockObject.name);
      
      dicksMeshList.Add(CockObject.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh);
      dicksMeshList.Add(SheathCockObject.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh);
    }
    catch (Exception e)
    {
      NPCPatcherBase.Log.LogError("Unable to load angelaboobs.unity3d in " + info.Location + ". Patcher will not run correctly.");
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
    
    var helmetOffDefinition = new ConfigDefinition("EnokSlider", "HelmetOff");
    var helmetOffDescription = new ConfigDescription("HelmetOff");
    helmetOff = config.Bind(helmetOffDefinition, false, helmetOffDescription);
    _helmetOffPrevious = helmetOff.Value;
    
    var beardToggleDefinition = new ConfigDefinition("EnokSlider", "BeardToggle");
    var beardToggleDescription = new ConfigDescription("BeardToggle");
    beardToggle = config.Bind(beardToggleDefinition, true, beardToggleDescription);
    
    var dickSizeDefinition = new ConfigDefinition("EnokSlider", "DickSize");
    var dickSizeDescription = new ConfigDescription("Dick size");
    dickSize = config.Bind(dickSizeDefinition, 0.0f, dickSizeDescription);
    
    var ballsSizeDefinition = new ConfigDefinition("EnokSlider", "BallsSize");
    var ballsSizeDescription = new ConfigDescription("Balls size");
    ballsSize = config.Bind(ballsSizeDefinition, 0.0f, ballsSizeDescription);
    
    var ballsDivotDefinition = new ConfigDefinition("EnokSlider", "BallsDivot");
    var ballsDivotDescription = new ConfigDescription("Balls separation");
    ballsDivot = config.Bind(ballsDivotDefinition, 0.0f, ballsDivotDescription);
    
    var bodyMuscularDefinition = new ConfigDefinition("EnokSlider", "BodyMuscular");
    var bodyMuscularDescription = new ConfigDescription("Add muscles");
    bodyMuscular = config.Bind(bodyMuscularDefinition, 0.0f, bodyMuscularDescription);
    
    var bodyFemboyDefinition = new ConfigDefinition("EnokSlider", "BodyMuscular");
    var bodyFemboyDescription = new ConfigDescription("Femboy bodytype");
    bodyFemboy = config.Bind(bodyFemboyDefinition, 0.0f, bodyFemboyDescription);
    
    var bodyBellySizeDefinition = new ConfigDefinition("EnokSlider", "BodyBellySize");
    var bodyBellySizeDescription = new ConfigDescription("Belly Size");
    bodyBellySize = config.Bind(bodyBellySizeDefinition, 0.0f, bodyBellySizeDescription);
    
    var bodyAssSizeDefinition = new ConfigDefinition("EnokSlider", "BodyAssSize");
    var bodyAssSizeDescription = new ConfigDescription("Ass Size");
    bodyAssSize = config.Bind(bodyAssSizeDefinition, 0.0f, bodyAssSizeDescription);
    
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
    tab.AddToggle("Helmet off (unfinished)", helmetOff);
    tab.AddToggle("Beard", beardToggle);
    tab.AddSlider("Dick size", dickSize);
    tab.AddSlider("Balls size", ballsSize);
    tab.AddSlider("Balls separation", ballsDivot);
    tab.AddSlider("More muscular", bodyMuscular);
    tab.AddSlider("Bigger ass", bodyAssSize);
    tab.AddSlider("Femboy body", bodyFemboy);
    tab.AddSlider("Belly size", bodyBellySize);
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
          torso.sharedMesh = _newTorso;
          
          var belt = mainMesh.Find("_enok_belt").GetComponent<SkinnedMeshRenderer>();
          belt.sharedMesh = _newBelt;
          
          var arms = mainMesh.Find("_enok_arms").GetComponent<SkinnedMeshRenderer>();
          arms.sharedMesh = _newArms;
          
          var ears = mainMesh.Find("_enok_ears").GetComponent<SkinnedMeshRenderer>();
          var neckbrace =  mainMesh.Find("_enok_neckbrace").GetComponent<SkinnedMeshRenderer>();
          var feet =  mainMesh.Find("_enok_feet").GetComponent<SkinnedMeshRenderer>();
          feet.sharedMesh = _newFeet;
          
          var horns =  mainMesh.Find("_enok_horns").GetComponent<SkinnedMeshRenderer>();
          var tail =   mainMesh.Find("_enok_tail").GetComponent<SkinnedMeshRenderer>();
          var shadow = __instance.transform.Find("_visual").Find("_shadowProjection").GetChild(0).GetComponent<MeshRenderer>();
          var torsoBone = mainMesh.Find("Armature_character").GetChild(0).GetChild(0).GetChild(0)
            .Find("lowBody").GetChild(0).GetChild(0);
          var axe1 = torsoBone.Find("heavyAxe_01").GetComponent<MeshRenderer>();
          var axe2 = torsoBone.Find("heavyAxe_02").GetComponent<MeshRenderer>();
          //var dickMaterial = torso.material;
          //CockObject.transform.SetParent(mainMesh);
          //CockObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = dickMaterial;
          // var dick = new GameObject("_enok_dick");
          // dick.transform.SetParent(mainMesh);
          //var dickMeshRenderer = CockObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
          //dickMeshRenderer.bones = cloth.bones;
          //dickMeshRenderer.material = dickMaterial;
          //NPCPatcherBase.Log.LogInfo(dickNames[selectedDick] + " " + dick.name + " or " + dickSheath.name);
          //dickMeshRenderer.sharedMesh = dickNames[selectedDick] == CockObject.name ? CockObject.transform.GetChild(0).GetComponent : dickSheath;

          var nipples = new GameObject("_enok_nipples");
          nipples.transform.SetParent(mainMesh);
          var nipplesMeshRenderer = nipples.gameObject.AddComponent<SkinnedMeshRenderer>();
          nipplesMeshRenderer.bones = torso.bones;
          nipplesMeshRenderer.material = torso.material;
          nipplesMeshRenderer.sharedMesh = _nipples;
          
          var beard = new GameObject("_enok_beard");
          beard.transform.SetParent(mainMesh);
          var beardMeshRenderer = beard.gameObject.AddComponent<SkinnedMeshRenderer>();
          beardMeshRenderer.bones = torso.bones;
          beardMeshRenderer.material = torso.material;
          beardMeshRenderer.sharedMesh = _beard;
          
          var eyes = new GameObject("_enok_eyes");
          eyes.transform.SetParent(mainMesh);
          var eyesMeshRenderer = eyes.gameObject.AddComponent<SkinnedMeshRenderer>();
          eyesMeshRenderer.bones = torso.bones;
          eyesMeshRenderer.material = torso.material;
          eyesMeshRenderer.sharedMesh = _eyes;
          
          var dickObject = Instantiate(CockObject, mainMesh);
          var dickMeshRenderer = dickObject.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
          dickMeshRenderer.sharedMesh = dicksMeshList[selectedDick];
          dickMeshRenderer.material = torso.material;
          Transform dickBaseBone = dickMeshRenderer.rootBone;
          dickBaseBone.SetParent(mainMesh.Find("Armature_character").GetChild(0).GetChild(0)); //hipCtrl
          
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
          EnokSliderController.dickObject = CockObject;
          EnokSliderController.dick = dickMeshRenderer;
          EnokSliderController.nipples = nipplesMeshRenderer;
          EnokSliderController.beard = beardMeshRenderer;
          EnokSliderController.eyes = eyesMeshRenderer;
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