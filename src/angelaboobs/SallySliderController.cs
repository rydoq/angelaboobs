using UnityEngine;

namespace angelaboobs;

public class SallySliderController : MonoBehaviour
{
    public float nippleInnie;
    public float nippleSize;
    public float boobSmall;
    public float boobLarge;
    public float nippleGone;
    public float boobSag;
    public float assSize;
    public float thighsThick;
    public float bellySize;
    public bool textureChanged = true;

    public SkinnedMeshRenderer boobs;
    public SkinnedMeshRenderer torso;
    public SkinnedMeshRenderer skirt;
    public SkinnedMeshRenderer eyes;
    public SkinnedMeshRenderer hair;
    public SkinnedMeshRenderer apron;
    public SkinnedMeshRenderer hood;
    public SkinnedMeshRenderer arms;
    public SkinnedMeshRenderer ears;
    public SkinnedMeshRenderer tail;
    public MeshRenderer broom;
    public MeshRenderer shadow;
    public DynamicBone boobBoneL;
    public DynamicBone boobBoneR;
    public DynamicBone boobBoneLOrig;
    public DynamicBone boobBoneROrig;

    private int _longHairTexture;

    private void Awake()
    {
        _longHairTexture = NPCPatcherBase.customTextures.Count;
        NPCPatcherBase.customTextures.Add(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Base,
            SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture], "sallyTex_hairLong"));
    }
    
    public void ApplySettings()
    {
        nippleGone = (SallySliderBase.nippleGoneWhenClothed.Value && !SallySliderBase.topless.Value) ? 100.0f : SallySliderBase.nippleGone.Value * 100.0f;
        nippleInnie = (SallySliderBase.nippleInnieWhenClothed.Value && !SallySliderBase.topless.Value) ? 100.0f : SallySliderBase.nippleInnie.Value * 100.0f;
        boobSmall = SallySliderBase.boobSmall.Value * 100.0f;
        boobLarge = SallySliderBase.boobLarge.Value * 100.0f;
        nippleSize = SallySliderBase.nippleSize.Value * 100.0f;
        boobSag = SallySliderBase.topless.Value ? Mathf.Max(100.0f - boobSmall*2, 0.0f) + boobLarge/2f : 0.0f;
        assSize = SallySliderBase.assLarge.Value * 100.0f;
        thighsThick = SallySliderBase.thighsThick.Value * 100.0f;
        bellySize = SallySliderBase.bellySize.Value * 100.0f;
        
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleInnie"), nippleInnie);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSmall"), boobSmall);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSize"), boobLarge);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleSize"), nippleSize);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleGone"), nippleGone);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSag"), boobSag);
        
        apron.SetBlendShapeWeight(apron.sharedMesh.GetBlendShapeIndex("key_boobSize"), boobLarge);
        apron.SetBlendShapeWeight(apron.sharedMesh.GetBlendShapeIndex("key_boobSmall"), boobSmall);
        apron.SetBlendShapeWeight(apron.sharedMesh.GetBlendShapeIndex("key_boobSag"), boobSag);
        apron.SetBlendShapeWeight(apron.sharedMesh.GetBlendShapeIndex("key_bellySize"), bellySize);
        
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_assSize"), assSize);
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_thighThick"), thighsThick);
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_bellySize"), bellySize);
        
        skirt.SetBlendShapeWeight(skirt.sharedMesh.GetBlendShapeIndex("key_thighThick"), thighsThick);
        skirt.SetBlendShapeWeight(skirt.sharedMesh.GetBlendShapeIndex("key_assSize"), assSize);
        skirt.SetBlendShapeWeight(skirt.sharedMesh.GetBlendShapeIndex("key_bellySize"), bellySize);

        hood.SetBlendShapeWeight(hood.sharedMesh.GetBlendShapeIndex("key_hoodDown"),
            SallySliderBase.hoodDown.Value ? 100.0f : 0.0f);
        
        if (SallySliderBase.topless.Value)
        {
            if (SallySliderBase.bottomless.Value)
            {
                NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base] =
                    NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Base,
                        SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture], "sallyTex_nude");
            }
            else
            {
                NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base] =
                    NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Base,
                        SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture], "sallyTex_topless");
            }
            arms.SetBlendShapeWeight(arms.sharedMesh.GetBlendShapeIndex("key_armsBare"), 100.0f);
        }
        else
        {
            if (SallySliderBase.bottomless.Value)
            {
                NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base] =
                    NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Base,
                        SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture], "sallyTex_bottomless");
            }
            else
            {
                NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base] =
                    NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Base,
                        SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture], "sallyTex_base");
            }
            arms.SetBlendShapeWeight(arms.sharedMesh.GetBlendShapeIndex("key_armsBare"), 0.0f);
        }

        boobs.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base];
        torso.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base];
        arms.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base];
        
        
        if (textureChanged)
        {
            if (SallySliderBase.hoodOn.Value && !SallySliderBase.hoodDown.Value)
            {
                hair.sharedMesh = SallySliderBase.hoodHair;
                hair.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base];

                NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.HoodGem] =
                    NPCPatcherBase.LoadTextureOrDefault(
                        NPCEnum.Sally, TexturesEnum.HoodGem,
                        SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);
                hood.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.HoodGem];
            }
            else
            {
                hair.sharedMesh = SallySliderBase.longHair;
                NPCPatcherBase.customTextures[_longHairTexture] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Base,
                    SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture], "sallyTex_hairLong");
                hair.material.mainTexture = NPCPatcherBase.customTextures[_longHairTexture];
                hair.material.mainTexture.filterMode = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base].filterMode;
            }

            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Face] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Face, SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);
            torso.materials[1].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Face];
            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Skirt] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Skirt,
                SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);

            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Ears] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Ears, SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);
            ears.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Ears];

            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Eye] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Eye, SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Eye2] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Eye2, SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);
            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Eye3] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Eye3, SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);

            switch (eyes.material.mainTexture.name)
            {
                case "sally_eyeTex01":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Eye];
                    break;
                case "sally_eyeTex02":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Eye2];
                    break;
                case "sally_eyeTex03":
                    eyes.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Eye3];
                    break;
            }

            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Apron] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.Apron,
                    SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);

            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Tail] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Tail, SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);
            tail.materials[0].mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Tail];

            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Broom] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Broom, SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);
            broom.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Broom];

            NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Shadow] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Sally, TexturesEnum.Shadow,
                SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]);
            shadow.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Shadow];

            SallySliderBase.facepic1Sprite = Sprite.Create(
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.FacePic1,
                    SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]),
                SallySliderBase.facepic1Sprite.rect, SallySliderBase.facepic1Sprite.pivot,
                SallySliderBase.facepic1Sprite.pixelsPerUnit, 0);
            SallySliderBase.facepic2Sprite = Sprite.Create(
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.FacePic2,
                    SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]),
                SallySliderBase.facepic2Sprite.rect, SallySliderBase.facepic2Sprite.pivot,
                SallySliderBase.facepic2Sprite.pixelsPerUnit, 0);
            SallySliderBase.facepic3Sprite = Sprite.Create(
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Sally, TexturesEnum.FacePic3,
                    SallySliderBase.textureFolderNames[SallySliderBase.selectedTexture]),
                SallySliderBase.facepic3Sprite.rect, SallySliderBase.facepic3Sprite.pivot,
                SallySliderBase.facepic3Sprite.pixelsPerUnit, 0);

            for (int i = 0; i < SallySliderBase.DialogList.Count; i++)
            {
                if (SallySliderBase.DialogList[i].facepic.texture.name == "facepic_sally01")
                {
                    SallySliderBase.DialogList[i].facepic = SallySliderBase.facepic1Sprite;
                }
                else if (SallySliderBase.DialogList[i].facepic.texture.name == "facepic_sally02")
                {
                    SallySliderBase.DialogList[i].facepic = SallySliderBase.facepic2Sprite;
                }
                else if (SallySliderBase.DialogList[i].facepic.texture.name == "facepic_sally03")
                {
                    SallySliderBase.DialogList[i].facepic = SallySliderBase.facepic3Sprite;
                }
            }

            textureChanged = false;
        }

        skirt.material.mainTexture = SallySliderBase.bottomless.Value
            ? Texture2D.blackTexture
            : NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Skirt];
        hood.materials[0].mainTexture = SallySliderBase.hoodOn.Value
            ? NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base]
            : Texture2D.blackTexture;
        hood.materials[1].mainTexture = SallySliderBase.hoodOn.Value
            ? NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.HoodGem]
            : Texture2D.blackTexture;
        apron.materials[0].mainTexture = SallySliderBase.apronless.Value
            ? Texture2D.blackTexture
            : NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Apron];
        apron.materials[1].mainTexture = SallySliderBase.apronless.Value
            ? Texture2D.blackTexture
            : NPCPatcherBase.textureDictionary[NPCEnum.Sally][TexturesEnum.Base];

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