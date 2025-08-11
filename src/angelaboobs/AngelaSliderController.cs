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
                boobs.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_nude");
                torso.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_nude");
                arms.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_nude");
                hairTuft.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_nude");
            }
            else
            {
                boobs.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_topless");
                torso.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_topless");
                arms.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_topless");
                hairTuft.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_topless");
            }
        }
        else if (AngelaSliderBase.bottomless.Value)
        {
            boobs.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_bottomless");
            torso.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_bottomless");
            arms.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_bottomless");
            hairTuft.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_bottomless");
        }
        else
        {
            boobs.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_base");
            torso.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_base");
            arms.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_base");
            hairTuft.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Base, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "angelaTex_01_base");
        }
        
        torso.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Face, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        skirt.material.mainTexture = AngelaSliderBase.bottomless.Value ? Texture2D.blackTexture : NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Skirt, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        arms.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Blue3, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        ears.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Tail, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        ears.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Bell, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        
        switch (eyes.material.mainTexture.name)
        {
            case "angelaEyeTex.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
            case "angelaEyeTex2.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye2, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
            case "angelaEyeTex3.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye3, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
            case "angelaEyeTex4.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye4, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
            case "angelaEyeTex_5.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Eye5, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
            case "angelaEyeTex_down.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeDown, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
            case "angelaEyeTex_L.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeLeft, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
            case "angelaEyeTex_R.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyeRight, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
            case "angelaEyeTex_pissed.png":
                eyes.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.EyePissed, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
                break;
        }
        
        eyes.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Red, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        feet.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Blui2, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        feet.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Blui2, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        horns.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Blue3, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        necklace.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Blu, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        necklace.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Bell, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        tail.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Tail, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        tail.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Blue3, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        glyph.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Glyph, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture], "tomeGlyph");
        bookL.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.BookSpine, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        bookL.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.BookCover, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        bookR.materials[0].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.BookSpine, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        bookR.materials[1].mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.BookCover, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);
        shadow.material.mainTexture = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.Shadow, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]);

        AngelaSliderBase.facepic1Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.FacePic1, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]), AngelaSliderBase.facepic1Sprite.rect, AngelaSliderBase.facepic1Sprite.pivot, AngelaSliderBase.facepic1Sprite.pixelsPerUnit, 0);
        AngelaSliderBase.facepic2Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.FacePic2, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]), AngelaSliderBase.facepic2Sprite.rect, AngelaSliderBase.facepic2Sprite.pivot, AngelaSliderBase.facepic2Sprite.pixelsPerUnit, 0);
        AngelaSliderBase.facepic3Sprite = Sprite.Create(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Angela, TexturesEnum.FacePic3, AngelaSliderBase.textureFolderNames[AngelaSliderBase.selectedTexture]), AngelaSliderBase.facepic3Sprite.rect, AngelaSliderBase.facepic3Sprite.pivot, AngelaSliderBase.facepic3Sprite.pixelsPerUnit, 0);

        for (int i = 0; i < AngelaSliderBase.DialogList.Count; i++)
        {
            if (AngelaSliderBase.DialogList[i].facepic.texture.name == "facepic_angela01")
            {
                AngelaSliderBase.DialogList[i].facepic = AngelaSliderBase.facepic1Sprite;
            }
            else if  (AngelaSliderBase.DialogList[i].facepic.texture.name == "facepic_angela02")
            {
                AngelaSliderBase.DialogList[i].facepic = AngelaSliderBase.facepic2Sprite;
            }
            else if  (AngelaSliderBase.DialogList[i].facepic.texture.name == "facepic_angela03")
            {
                AngelaSliderBase.DialogList[i].facepic = AngelaSliderBase.facepic3Sprite;
            }
        }

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