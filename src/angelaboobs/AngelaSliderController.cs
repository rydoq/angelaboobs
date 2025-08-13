using UnityEngine;

namespace angelaboobs;

public class AngelaSliderController : MonoBehaviour
{
    public float nippleInnie;
    public float nippleSize;
    public float boobSmall;
    public float boobLarge;
    public float nippleGone;
    public float boobSag;
    public float assSize;
    public float thighsThick;
    public bool textureChanged = true;

    public SkinnedMeshRenderer boobs;
    public SkinnedMeshRenderer torso;
    public SkinnedMeshRenderer skirt;
    public SkinnedMeshRenderer eyes;
    public SkinnedMeshRenderer feet;
    public SkinnedMeshRenderer hairTuft;
    public SkinnedMeshRenderer horns;
    public SkinnedMeshRenderer necklace;
    public SkinnedMeshRenderer arms;
    public SkinnedMeshRenderer ears;
    public SkinnedMeshRenderer tail;
    public MeshRenderer glyph;
    public MeshRenderer bookL;
    public MeshRenderer bookR;
    public MeshRenderer shadow;
    public DynamicBone boobBoneL;
    public DynamicBone boobBoneR;
    public DynamicBone boobBoneLOrig;
    public DynamicBone boobBoneROrig;
    
    public void ApplySettings()
    {
        nippleGone = (AngelaSliderBase.nippleGoneWhenClothed.Value && !AngelaSliderBase.topless.Value) ? 100.0f : AngelaSliderBase.nippleGone.Value * 100.0f;
        nippleInnie = (AngelaSliderBase.nippleInnieWhenClothed.Value && !AngelaSliderBase.topless.Value) ? 100.0f : AngelaSliderBase.nippleInnie.Value * 100.0f;
        boobSmall = AngelaSliderBase.boobSmall.Value * 100.0f;
        boobLarge = AngelaSliderBase.boobLarge.Value * 100.0f;
        nippleSize = AngelaSliderBase.nippleSize.Value * 100.0f;
        boobSag = AngelaSliderBase.topless.Value ? 100.0f - boobSmall + boobLarge/2f : 0.0f;
        assSize = AngelaSliderBase.assLarge.Value * 100.0f;
        thighsThick = AngelaSliderBase.thighsThick.Value * 100.0f;
        
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleInnie"), nippleInnie);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSmall"), boobSmall);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSize"), boobLarge);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleSize"), nippleSize);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleGone"), nippleGone);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSag"), boobSag);
        
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_assSize"), assSize);
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_thighSize"), thighsThick);
        skirt.SetBlendShapeWeight(skirt.sharedMesh.GetBlendShapeIndex("key_thighSize"), thighsThick);

        if (AngelaSliderBase.topless.Value)
        {
            if (AngelaSliderBase.bottomless.Value)
            {
                NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Base] =
                    NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base,
                        AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_nude");
            }
            else
            {
                NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Base] =
                    NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base,
                        AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_topless");
            }
        }
        else if (AngelaSliderBase.bottomless.Value)
        {
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Base] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_bottomless");
        }
        else
        {
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Base] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_base");
        }

        boobs.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Base];
        torso.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Base];
        arms.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Base];
        hairTuft.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Base];
        
        if (textureChanged)
        {
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Face] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Face,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            torso.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Face];

            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Skirt] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Skirt,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blue3] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Angela, TexturesEnum.Blue3,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            horns.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blue3];
            arms.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blue3];
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Tail] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Angela, TexturesEnum.Tail,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            tail.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Tail];
            tail.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blue3];
            ears.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Tail];
            
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Bell] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Angela, TexturesEnum.Bell,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            ears.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Bell];
            
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye2] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye2,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye3] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye3,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye4] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye4,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye5] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye5,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.EyeDown] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeDown,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.EyeLeft] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeLeft,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.EyeRight] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeRight,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.EyePissed] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyePissed,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);

            switch (eyes.material.mainTexture.name)
            {
                case "angelaEyeTex.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye];
                    break;
                case "angelaEyeTex2.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye2];
                    break;
                case "angelaEyeTex3.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye3];
                    break;
                case "angelaEyeTex4.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye4];
                    break;
                case "angelaEyeTex_5.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Eye5];
                    break;
                case "angelaEyeTex_down.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.EyeDown];
                    break;
                case "angelaEyeTex_L.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.EyeLeft];
                    break;
                case "angelaEyeTex_R.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.EyeRight];
                    break;
                case "angelaEyeTex_pissed.png":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.EyePissed];
                    break;
            }

            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Red] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Angela, TexturesEnum.Red,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            eyes.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Red];

            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blui2] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Angela, TexturesEnum.Blui2,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            feet.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blui2];
            feet.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blui2];
            
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blu] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Angela, TexturesEnum.Blu,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);

            necklace.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Blue3];
            necklace.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Bell];

            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Glyph] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Angela, TexturesEnum.Glyph,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "tomeGlyph");
            glyph.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Glyph];

            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.BookSpine] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.BookSpine,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.BookCover] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.BookCover,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            bookL.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.BookSpine];
            bookL.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.BookCover];
            bookR.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.BookSpine];
            bookR.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.BookCover];

            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Shadow] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Angela, TexturesEnum.Shadow,
                AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            shadow.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Shadow];

            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.FacePic1] =
                NPCPatcherBase.LoadTextureOrDefault(
                    NPCEnum.Angela, TexturesEnum.FacePic1,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.FacePic2] =
                NPCPatcherBase.LoadTextureOrDefault(
                    NPCEnum.Angela, TexturesEnum.FacePic2,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.FacePic3] =
                NPCPatcherBase.LoadTextureOrDefault(
                    NPCEnum.Angela, TexturesEnum.FacePic3,
                    AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);

            AngelaSliderBase.facepic1Sprite = Sprite.Create(
                NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.FacePic1],
                AngelaSliderBase.facepic1Sprite.rect, AngelaSliderBase.facepic1Sprite.pivot,
                AngelaSliderBase.facepic1Sprite.pixelsPerUnit, 0);
            AngelaSliderBase.facepic2Sprite = Sprite.Create(
                NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.FacePic2],
                AngelaSliderBase.facepic2Sprite.rect, AngelaSliderBase.facepic2Sprite.pivot,
                AngelaSliderBase.facepic2Sprite.pixelsPerUnit, 0);
            AngelaSliderBase.facepic3Sprite = Sprite.Create(
                NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.FacePic3],
                AngelaSliderBase.facepic3Sprite.rect, AngelaSliderBase.facepic3Sprite.pivot,
                AngelaSliderBase.facepic3Sprite.pixelsPerUnit, 0);

            for (int i = 0; i < AngelaSliderBase.DialogList.Count; i++)
            {
                if (AngelaSliderBase.DialogList[i].facepic.texture.name == "facepic_angela01")
                {
                    AngelaSliderBase.DialogList[i].facepic = AngelaSliderBase.facepic1Sprite;
                }
                else if (AngelaSliderBase.DialogList[i].facepic.texture.name == "facepic_angela02")
                {
                    AngelaSliderBase.DialogList[i].facepic = AngelaSliderBase.facepic2Sprite;
                }
                else if (AngelaSliderBase.DialogList[i].facepic.texture.name == "facepic_angela03")
                {
                    AngelaSliderBase.DialogList[i].facepic = AngelaSliderBase.facepic3Sprite;
                }
            }
            textureChanged = false;
        }
        
        skirt.material.mainTexture = AngelaSliderBase.bottomless.Value
            ? Texture2D.blackTexture
            : NPCPatcherBase.textureDictionary[NPCEnum.Angela][TexturesEnum.Skirt];

        boobBoneL = boobBoneLOrig;
        boobBoneR = boobBoneROrig;
        if (boobSmall > 0.0f)
        {
            float stiffnessChange = Mathf.Min(1.0f, boobBoneL.m_Stiffness + boobSmall/100.0f); 
            foreach (DynamicBone.Particle particle in boobBoneL.m_Particles)
            {
                particle.m_Stiffness = stiffnessChange;
            }
            foreach (DynamicBone.Particle particle in boobBoneR.m_Particles)
            {
                particle.m_Stiffness = stiffnessChange;
            }

            boobBoneL.m_Stiffness = stiffnessChange;
            boobBoneR.m_Stiffness = stiffnessChange;
        }
    }
}