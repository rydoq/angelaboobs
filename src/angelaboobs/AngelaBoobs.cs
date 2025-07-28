using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Nessie.ATLYSS.EasySettings;
using Nessie.ATLYSS.EasySettings.UIElements;
using BepInEx.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.TextCore.Text;


namespace angelaboobs;

[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class AngelaBoobsBase : BaseUnityPlugin
{
  internal static new ManualLogSource Log = null!;

  private static Mesh _newBoobs;
  private static Mesh _newBody;
  private static Mesh _newSkirt;
  
  public static Sprite facepic1Sprite;
  public static Sprite facepic2Sprite;
  public static Sprite facepic3Sprite;

  private string texturePath;
  
  public static Dictionary<string, string> textureDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
  {
    {"bell", "angelaBellTex.png"},
    {"eye", "angelaEyeTex.png"},
    {"eye2", "angelaEyeTex2.png"},
    {"eye3", "angelaEyeTex3.png"},
    {"eye4", "angelaEyeTex4.png"},
    {"eye5", "angelaEyeTex_5.png"},
    {"eyedown", "angelaEyeTex_down.png"},
    {"eyeleft", "angelaEyeTex_L.png"},
    {"eyeright", "angelaEyeTex_R.png"},
    {"eyepissed", "angelaEyeTex_pissed.png"},
    {"face", "angelaFaceTex.png"},
    {"faceopenmouth", "angelaFaceTex_openMouth.png"},
    {"red", "angelaRed.png"},
    {"skirt", "angelaTex03.png"},
    {"base", "angelaTex_01_base.png"},
    {"bottomless", "angelaTex_01_bottomless.png"},
    {"nude", "angelaTex_01_nude.png"},
    {"topless", "angelaTex_01_topless.png"},
    {"tail", "angelaTex_02.png"},
    {"blu", "angelaTex_blu.png"},
    {"blue3", "blue3.png"},
    {"blui2", "blui2.png"},
    {"bookspine", "book_spineTex.png"},
    {"bookcover", "bookCoverTex1.png"},
    {"glyph", "tomeGlyph.png"},
    {"shadow", "shadow.png"},
    {"facepic1", "facepic_angela01.png"},
    {"facepic2", "facepic_angela02.png"},
    {"facepic3", "facepic_angela03.png"},
  };
  
  public static readonly Texture2D[] Textures = new Texture2D[textureDictionary.Count];
  
  public enum TexturesEnum {
    Bell,
    Eye,
    Eye2,
    Eye3,
    Eye4,
    Eye5,
    EyeDown,
    EyeLeft,
    EyeRight,
    EyePissed,
    Face,
    FaceOpenMouth,
    Red,
    Skirt,
    Base,
    Bottomless,
    Nude,
    Topless,
    Tail,
    Blu,
    Blue3,
    Blui2,
    BookSpine,
    BookCover,
    Glyph,
    Shadow,
    FacePic1,
    FacePic2,
    FacePic3,
  }

  public List<string> textureFolderNames = new List<string>();
  public int selectedTexture;
  
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

  internal static List<Dialog> DialogList = new List<Dialog>();

  private void Awake()
  {
    new Harmony("ry.angelaboobs").PatchAll();
    Log = Logger;
    
    texturePath = Path.Combine(Paths.ConfigPath, "NPCNSFWModFiles", "Angela");
    if (texturePath.IsNullOrWhiteSpace())
    {
      texturePath = Path.Combine(Info.Location, "config", "NPCNSFWModFiles", "Angela");
    }
    
    InitConfig();
    Settings.OnInitialized.AddListener(AddSettings);
    Settings.OnApplySettings.AddListener(() =>
    {
      textureSelector.Value = selectedTexture;
      Config.Save();
      LoadTexturesIntoVariables();
      if (AngelaSliderController != null)
        AngelaSliderController.ApplySettings();
      if (eyeController != null)
        ApplyEyeTexturesToEyeController();
    });
    
    try
    {
      DirectoryInfo[] textureFolders = new DirectoryInfo(texturePath).GetDirectories();
      
      for (int i = 0; i < textureFolders.Length; i++)
      {
        Log.LogInfo($"Loaded {textureFolders[i].Name} texture group");
        textureFolderNames.Add(textureFolders[i].Name);
      }

      if (selectedTexture > textureFolders.Length-1)
      {
        selectedTexture = textureFolders.Length-1; // set if the player deletes a texture mod and it can't load it
      }

      LoadTexturesIntoVariables();
    }
    catch (Exception e)
    {
      Log.LogError("Unable to find textures in path " + texturePath + ". Continuing.");
      Log.LogError(e.ToString());
    }
    
    try
    {
      AssetBundle bundle = AssetBundle.LoadFromFile(Info.Location.Replace("angelaboobs.dll", "angelaboobs.unity3d"));
      _newBoobs = bundle.LoadAsset<Mesh>("angela_boobs");
      _newBody = bundle.LoadAsset<Mesh>("angela_base");
      _newSkirt = bundle.LoadAsset<Mesh>("angela_robeSkirt");
    }
    catch (Exception e)
    {
      Log.LogError("Unable to find angelaboobs.unity3d in " + Info.Location + ". Patcher will not run correctly.");
      Log.LogError(e.ToString());
    }

    Log.LogInfo($"Plugin {LCMPluginInfo.PLUGIN_NAME} version {LCMPluginInfo.PLUGIN_VERSION} is loaded!");
  }

  private Texture2D LoadTexture(string fullPath, string objname)
  {
    byte[] raw = File.ReadAllBytes(fullPath);

    Texture2D Tex = new(128, 128, TextureFormat.ARGB32, false) {
      name = objname,
      anisoLevel = 16,
      filterMode = FilterMode.Point,
      mipMapBias = 0
    };

    Tex.LoadImage(raw, false);
    
    return Tex;
  }

  private static void ApplyEyeTexturesToEyeController()
  {
    eyeController._textureStructs[0]._texture = Textures[(int)TexturesEnum.Eye];
    eyeController._textureStructs[1]._texture = Textures[(int)TexturesEnum.EyeLeft];
    eyeController._textureStructs[2]._texture = Textures[(int)TexturesEnum.EyeRight];
    eyeController._textureStructs[3]._texture = Textures[(int)TexturesEnum.EyeDown];
    eyeController._textureStructs[4]._texture = Textures[(int)TexturesEnum.Eye3];
    eyeController._textureStructs[5]._texture = Textures[(int)TexturesEnum.Eye2];
    eyeController._textureStructs[6]._texture = Textures[(int)TexturesEnum.EyePissed];
    eyeController._textureStructs[7]._texture = Textures[(int)TexturesEnum.Eye4];
    eyeController._textureStructs[8]._texture = Textures[(int)TexturesEnum.Eye5];
  }
  
  private void LoadTexturesIntoVariables()
  {
    Log.LogInfo("Loading textures...");
    for (int i = 0; i < Textures.Length; i++)
    {
      var texEnum = (TexturesEnum)i;
      Textures[i] = LoadTextureOrDefault(texEnum.ToString());
    }
    Log.LogInfo("Loaded textures");
  }

  private Texture2D LoadTextureOrDefault(string textureType)
  {
    Texture2D tex;
    Log.LogInfo($"Loading texture {textureDictionary[textureType]}");
    
    string path = Path.Combine(texturePath, textureFolderNames[selectedTexture], textureDictionary[textureType]);
    FileInfo f = new FileInfo(path);
    tex = f.Exists
      ? LoadTexture(path,
        textureDictionary[textureType])
      : LoadTexture(Path.Combine(texturePath, "Default", textureDictionary[textureType]),
        textureDictionary[textureType]);
    
    return tex;
  }
  
  private void InitConfig()
  {
    var textureSelectionDefinition = new ConfigDefinition("AngelaBoobs", "TextureSelection");
    var textureSelectionDescription = new ConfigDescription("TextureSelection");
    textureSelector = Config.Bind(textureSelectionDefinition, 0, textureSelectionDescription);
    selectedTexture = textureSelector.Value;
    
    var toplessDefinition = new ConfigDefinition("AngelaBoobs", "Topless");
    var toplessDescription = new ConfigDescription("Topless");
    topless = Config.Bind(toplessDefinition, false, toplessDescription);
    
    var bottomlessDefinition = new ConfigDefinition("AngelaBoobs", "Bottomless");
    var bottomlessDescription = new ConfigDescription("Bottomless");
    bottomless = Config.Bind(bottomlessDefinition, false, bottomlessDescription);
    
    var nippleInnieWhenClothedDefinition = new ConfigDefinition("AngelaBoobs", "NippleInnieWhenClothed");
    var nippleInnieWhenClothedDescription = new ConfigDescription("Nipples flat when clothed");
    nippleInnieWhenClothed = Config.Bind(nippleInnieWhenClothedDefinition, true, nippleInnieWhenClothedDescription);
    
    var nippleGoneWhenClothedDefinition = new ConfigDefinition("AngelaBoobs", "NippleGoneWhenClothed");
    var nippleGoneWhenClothedDescription = new ConfigDescription("Nipples gone when clothed");
    nippleGoneWhenClothed = Config.Bind(nippleGoneWhenClothedDefinition, false, nippleGoneWhenClothedDescription);
    
    var nippleInnieDefinition = new ConfigDefinition("AngelaBoobs", "NippleInnie");
    var nippleInnieDescription = new ConfigDescription("Flat nipple tips");
    nippleInnie = Config.Bind(nippleInnieDefinition, 0.0f, nippleInnieDescription);

    var nippleSizeDefinition = new ConfigDefinition("AngelaBoobs", "NippleSize");
    var nippleSizeDescription = new ConfigDescription("Nipple size");
    nippleSize = Config.Bind(nippleSizeDefinition, 0.0f, nippleSizeDescription);

    var breastSmallDefinition = new ConfigDefinition("AngelaBoobs", "BreastSmall");
    var breastSmallDescription = new ConfigDescription("Smaller breasts");
    boobSmall = Config.Bind(breastSmallDefinition, 0.0f, breastSmallDescription);
    
    var breastLargeDefinition = new ConfigDefinition("AngelaBoobs", "BreastLarge");
    var breastLargeDescription = new ConfigDescription("Larger breasts");
    boobLarge = Config.Bind(breastLargeDefinition, 0.0f, breastLargeDescription);

    var nippleGoneDefinition = new ConfigDefinition("AngelaBoobs", "NippleGone");
    var nippleGoneDescription = new ConfigDescription("Make nipples go away");
    nippleGone = Config.Bind(nippleGoneDefinition, 0.0f, nippleGoneDescription);
    
    var assLargeDefinition = new ConfigDefinition("AngelaBoobs", "AssLarge");
    var assGoneDescription = new ConfigDescription("Make ass bigger");
    assLarge = Config.Bind(assLargeDefinition, 0.0f, assGoneDescription);
    
    var thighsThickDefinition = new ConfigDefinition("AngelaBoobs", "ThighsThick");
    var thighsThickDescription = new ConfigDescription("Make thighs thicker");
    thighsThick = Config.Bind(thighsThickDefinition, 0.0f, thighsThickDescription);
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
          Log.LogInfo("Angela located");
          AngelaSliderController = __instance.gameObject.AddComponent<AngelaSliderController>();
          eyeController = __instance.transform.Find("_visual").GetComponent<TextureChangeAnimEvent>();
          ApplyEyeTexturesToEyeController();

          /*
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
                AngelaBoobsBase.facepic1Sprite = Sprite.Create(AngelaBoobsBase.Textures[(int)TexturesEnum.FacePic1], dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = AngelaBoobsBase.facepic1Sprite;
              }
              else if  (dialog.facepic.name == "facepic_angela02")
              {
                AngelaBoobsBase.facepic2Sprite = Sprite.Create(AngelaBoobsBase.Textures[(int)TexturesEnum.FacePic2], dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = AngelaBoobsBase.facepic2Sprite;
              }
              else if  (dialog.facepic.name == "facepic_angela03")
              {
                AngelaBoobsBase.facepic3Sprite = Sprite.Create(AngelaBoobsBase.Textures[(int)TexturesEnum.FacePic3], dialog.facepic.rect, dialog.facepic.pivot, dialog.facepic.pixelsPerUnit, 0);
                dialog.facepic = AngelaBoobsBase.facepic3Sprite;
              }
            }
          }
          
          var mainAngela = __instance.transform.Find("_visual").Find("angela_mesh");
          var oldBoobs = mainAngela.Find("angela_boobs").GetComponent<SkinnedMeshRenderer>();
          
          oldBoobs.sharedMesh = _newBoobs;
          
          var torso = mainAngela.Find("angela_base").GetComponent<SkinnedMeshRenderer>();
          torso.sharedMesh = _newBody;
          
          var skirt = mainAngela.Find("angela_robeSkirt").GetComponent<SkinnedMeshRenderer>();
          skirt.sharedMesh = _newSkirt;
          
          var arms = mainAngela.Find("angela_arms").GetComponent<SkinnedMeshRenderer>();
          var ears = mainAngela.Find("angela_ears").GetComponent<SkinnedMeshRenderer>();
          var eyes =  mainAngela.Find("angela_eyes").GetComponent<SkinnedMeshRenderer>();
          var feet =  mainAngela.Find("angela_feet").GetComponent<SkinnedMeshRenderer>();
          var hairTuft =  mainAngela.Find("angela_hairTuft").GetComponent<SkinnedMeshRenderer>();
          var horns =  mainAngela.Find("angela_horns").GetComponent<SkinnedMeshRenderer>();
          var necklace =   mainAngela.Find("angela_necklace").GetComponent<SkinnedMeshRenderer>();
          var tail =   mainAngela.Find("angela_tail").GetComponent<SkinnedMeshRenderer>();
          var glyph = mainAngela.Find("Armature_character").Find("masterBone").Find("Quad").Find("Quad_01")
            .GetComponent<MeshRenderer>();
          var bookMain = mainAngela.Find("Armature_character").Find("masterBone").Find("hipCtrl").Find("hip")
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
          AngelaSliderController.ApplySettings();
          
          Log.LogInfo("Angela body patched!");
        }
      }
      catch (Exception e)
      {
        Log.LogError(e);
      }
      
    }
  }

  [HarmonyPatch(typeof(VisualManager), nameof(VisualManager.Apply_FilteringMode))]
  public static class ChangeFilteringModePatch
  {
    [HarmonyPostfix]
    public static void FilteringModePatch(FilterMode _filterMode)
    {
      foreach (Texture2D texture in Textures)
      {
        texture.filterMode = _filterMode;
        texture.anisoLevel = 16;
      }
    }
  }
}
