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
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Mime;
using Object = System.Object;

namespace angelaboobs;

public enum NPCEnum // named the same as the folders inside the /config/NPCNSFWModTextures
{
    Angela,
    Sally
}

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
    Ears,
    HoodGem,
    Broom,
    Apron
}

[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class NPCPatcherBase : BaseUnityPlugin
{
    internal static new ManualLogSource Log = null!;
    internal static bool VerboseLogging = false;
    
    public static AssetBundle bundle;
    
    internal AngelaSliderBase AngelaSlider;
    internal SallySliderBase SallySlider;
    internal static GameObject NPCPatcherObject = new("NPCPatcher-GameObject")
    {
        hideFlags = HideFlags.HideAndDontSave,
    };

    private const string DEFAULT_TEXTURE_FOLDER_NAME = "Default";
    private const string NPC_TEXTURE_ASSET_LOCATION = "_graphic/_mesh/02_friendlynpc"; //inside resources.assets
    
    public static string texturePath { get; set; }

    //public TexturesEnum TexturesEnum { get; set; }
    //public NPCEnum NPCEnum { get; set; }

    public static Dictionary<NPCEnum, Dictionary<TexturesEnum, Texture2D>> textureDictionary = new () {
        { NPCEnum.Angela, new () {
            { TexturesEnum.Bell, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaBellTex")) },
            { TexturesEnum.Eye, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex" )) },
            { TexturesEnum.Eye2, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex2")) },
            { TexturesEnum.Eye3, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex3")) },
            { TexturesEnum.Eye4, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex4")) },
            { TexturesEnum.Eye5, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex_5")) },
            { TexturesEnum.EyeDown, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex_down")) },
            { TexturesEnum.EyeLeft, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex_L")) },
            { TexturesEnum.EyeRight, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex_R")) },
            { TexturesEnum.EyePissed, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaEyeTex_pissed")) },
            { TexturesEnum.Face, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaFaceTex")) },
            { TexturesEnum.FaceOpenMouth, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaFaceTex_openMouth")) },
            { TexturesEnum.Red, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaRed")) },
            { TexturesEnum.Skirt, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaTex03")) },
            { TexturesEnum.Base, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaTex_01")) },
            { TexturesEnum.Tail, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaTex_02")) },
            { TexturesEnum.Blu, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "angelaTex_blu")) },
            { TexturesEnum.Blue3, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "blue3")) },
            { TexturesEnum.Blui2, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "angela", "blui2")) },
            { TexturesEnum.BookSpine, Resources.Load<Texture2D>(Path.Combine("_graphic/_mesh/_prop/_book", "book_spineTex")) },
            { TexturesEnum.BookCover, Resources.Load<Texture2D>(Path.Combine("_graphic/_mesh/_prop/_book", "bookCoverTex1")) },
            { TexturesEnum.Glyph, Resources.Load<Texture2D>("_graphic/_particle/_particle_tomeGlyph") },
            { TexturesEnum.Shadow, Resources.Load<Texture2D>("_graphic/_particle/shadow") },
            { TexturesEnum.FacePic1, Resources.Load<Texture2D>(Path.Combine("_graphic/_ui/_facepics", "facepic_angela01")) },
            { TexturesEnum.FacePic2, Resources.Load<Texture2D>(Path.Combine("_graphic/_ui/_facepics", "facepic_angela02")) },
            { TexturesEnum.FacePic3, Resources.Load<Texture2D>(Path.Combine("_graphic/_ui/_facepics", "facepic_angela03")) },
        }},
        
        { NPCEnum.Sally, new () {
            { TexturesEnum.Base, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sallyTex_base")) },
            { TexturesEnum.Tail, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sallyTail_tex")) },
            { TexturesEnum.Ears, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sallyTex_ears")) },
            { TexturesEnum.Skirt, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sallySkirtTex")) },
            { TexturesEnum.Apron, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sally_apronTex")) },
            { TexturesEnum.Broom, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sallyBroomTex")) },
            { TexturesEnum.HoodGem, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sallyHoodGem_tex")) },
            { TexturesEnum.Face, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sallyFace_text")) },
            { TexturesEnum.Eye, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sally_eyeTex01")) },
            { TexturesEnum.Eye2, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sally_eyeTex02")) },
            { TexturesEnum.Eye3, Resources.Load<Texture2D>(Path.Combine(NPC_TEXTURE_ASSET_LOCATION, "sally", "sally_eyeTex03")) },
            { TexturesEnum.Shadow, Resources.Load<Texture2D>(Path.Combine("_graphic/_particle/shadow")) },
            { TexturesEnum.FacePic1, Resources.Load<Texture2D>(Path.Combine("_graphic/_ui/_facepics", "facepic_sally01")) },
            { TexturesEnum.FacePic2, Resources.Load<Texture2D>(Path.Combine("_graphic/_ui/_facepics", "facepic_sally02")) },
            { TexturesEnum.FacePic3, Resources.Load<Texture2D>(Path.Combine("_graphic/_ui/_facepics", "facepic_sally03")) },
        }}
    };

    public static List<Texture2D> customTextures = new();
    //public static Texture2D[] Textures { get; set; }
    
    
    private void Awake()
    {
        Log = Logger;
        
        string path = Path.Combine(Paths.ConfigPath, "NPCNSFWModFiles");
        
        texturePath = Directory.Exists(path) ? path : Path.Combine(Info.Location.Replace("angelaboobs.dll", ""), "config", "NPCNSFWModFiles");
        Log.LogInfo(texturePath);
        new Harmony("ry.npcpatcher").PatchAll();
        
        if (VerboseLogging)
            foreach (KeyValuePair<TexturesEnum, Texture2D> kvp in textureDictionary[NPCEnum.Angela])
                Log.LogInfo("Loaded Texture2D " + textureDictionary[NPCEnum.Angela][kvp.Key]);

        bundle = AssetBundle.LoadFromFile(Info.Location.Replace("angelaboobs.dll", "angelaboobs.unity3d"));
        
        Settings.OnInitialized.AddListener(() => { 
            AngelaSlider = NPCPatcherObject.AddComponent<AngelaSliderBase>();
            AngelaSlider.info = Info;
            AngelaSlider.config = Config;
            SallySlider = NPCPatcherObject.AddComponent<SallySliderBase>();
            SallySlider.info = Info;
            SallySlider.config = Config;
        });
        
        
        Config.Save();
        Log.LogInfo($"Plugin {LCMPluginInfo.PLUGIN_NAME} version {LCMPluginInfo.PLUGIN_VERSION} is loaded!");
    }
    
    private static Texture2D LoadTexture(string fullPath, string objname)
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
    
    /// <summary>
    /// Loads the default texture  from currentTexturefolder and replaces a texture in the textureDictionary with it
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="textureType"></param>
    /// <param name="currentTextureFolder"></param>
    /// <returns></returns>
    public static Texture2D LoadTextureOrDefault(NPCEnum npc, TexturesEnum textureType, string currentTextureFolder)
    {
        if (VerboseLogging)
            Log.LogInfo($"Loading texture {textureDictionary[npc][textureType]}");
        
        string npcName = npc.ToString();
    
        string path = Path.Combine(texturePath, npcName, currentTextureFolder, textureDictionary[npc][textureType].name + ".png");
        FileInfo f = new FileInfo(path);
        
        var newTex = f.Exists
            ? LoadTexture(path,
                textureDictionary[npc][textureType].name)
            : LoadTexture(Path.Combine(texturePath, npcName, DEFAULT_TEXTURE_FOLDER_NAME, textureDictionary[npc][textureType].name + ".png"),
                textureDictionary[npc][textureType].name);
    
        return newTex;
    }
    
    /// <summary>
    /// Loads a texture, overrideTexture, from currentTexturefolder and replaces a texture in the textureDictionary with it
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="textureType"></param>
    /// <param name="currentTextureFolder"></param>
    /// <param name="overrideTexture"></param>
    /// <returns></returns>
    public static Texture2D LoadTextureOrDefault(NPCEnum npc, TexturesEnum textureType, string currentTextureFolder, string overrideTexture)
    {
        if (VerboseLogging)
            Log.LogInfo($"Loading texture {textureDictionary[npc][textureType]}");
        
        string npcName = npc.ToString();

        string path = Path.Combine(texturePath, npcName, currentTextureFolder, overrideTexture + ".png");
        FileInfo f = new FileInfo(path);
        
        var newTex = f.Exists
            ? LoadTexture(path,
                textureDictionary[npc][textureType].name)
            : LoadTexture(Path.Combine(texturePath, npcName, DEFAULT_TEXTURE_FOLDER_NAME, overrideTexture + ".png"),
                textureDictionary[npc][textureType].name);
    
        return newTex;
    }

    /*
    public static void LoadTexturesIntoVariables(NPCEnum npc, List<string> textureFolderNames, int selectedTexture)
    {
        Log.LogInfo("Loading textures...");

        for (int i = 0; i < textureDictionary[npc].Count; i++)
        {
            textureDictionary[npc][i]
        }
        
        foreach (KeyValuePair<NPCEnum, Dictionary<TexturesEnum, Texture2D>> textureGroup in textureDictionary)
        {
            foreach (KeyValuePair<TexturesEnum, Texture2D> texture in textureGroup.Value)
            {
                texture.Value = LoadTextureOrDefault(npc, texture.Key, textureGroup.Key, selectedTexture);
            }
        }
    }
    
    /*
    public void LoadTexturesIntoVariables(Texture2D[] textures)
    {
        Log.LogInfo("Loading textures...");
        for (int i = 0; i < textures.Length; i++)
        {
            var texEnum = (TexturesEnum)i;
            textures[i] = LoadTextureOrDefault(texEnum);
        }
        Log.LogInfo("Loaded textures");
    }
    */
    
    [HarmonyPatch(typeof(VisualManager), nameof(VisualManager.Apply_FilteringMode))]
    public static class ChangeFilteringModePatch
    {
        [HarmonyPostfix]
        public static void FilteringModePatch(FilterMode _filterMode)
        {
            foreach (Dictionary<TexturesEnum, Texture2D> textureGroup in textureDictionary.Values)
            {
                foreach (Texture2D texture in textureGroup.Values)
                {
                    texture.filterMode = _filterMode;
                    texture.anisoLevel = 16;
                }
            }

            if (customTextures.Any() == true)
            {
                foreach (Texture2D texture in customTextures)
                {
                    texture.filterMode = _filterMode;
                    texture.anisoLevel = 16;
                }
            }
        }
    }
}