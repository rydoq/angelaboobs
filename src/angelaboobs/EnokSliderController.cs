using System;
using UnityEngine;

namespace angelaboobs;

public class EnokSliderController : MonoBehaviour
{
    private float _arousalTarget = 0.0f;
    private float _arousalCurrent;

    public float dickSize;
    public float ballsSize;
    public float bodyFemboy;
    public float bodyMuscular;
    public float bodyBellySize;
    public float bodyAssSize;
    public float ballsDivot;
    public float helmetOff;
    private float _dickSizeSheathForCloth;
    
    public bool textureChanged = true;

    public SkinnedMeshRenderer cloth;
    public SkinnedMeshRenderer torso;
    public SkinnedMeshRenderer belt;
    public SkinnedMeshRenderer feet;
    public SkinnedMeshRenderer horns;
    public SkinnedMeshRenderer neckbrace;
    public SkinnedMeshRenderer arms;
    public SkinnedMeshRenderer ears;
    public SkinnedMeshRenderer tail;
    public SkinnedMeshRenderer dick;
    public GameObject dickObject;
    public SkinnedMeshRenderer nipples;
    public SkinnedMeshRenderer beard;
    public SkinnedMeshRenderer eyes;
    public MeshRenderer axe1;
    public MeshRenderer axe2;
    public MeshRenderer shadow;
    
    private int _dickTexture;
    private int _beardTexture;
    private int _eyesTexture;
    
    private void Awake()
    {
        _dickTexture = NPCPatcherBase.customTextures.Count;
        NPCPatcherBase.customTextures.Add(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
            EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enokTex_dick"));
        
        _beardTexture = NPCPatcherBase.customTextures.Count;
        NPCPatcherBase.customTextures.Add(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
            EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enok_beardTex"));
        
        _eyesTexture = NPCPatcherBase.customTextures.Count;
        NPCPatcherBase.customTextures.Add(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
            EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enokTex_eyes"));
    }

    public void ApplyBones(GameObject go)
    {
        if (go.name.Contains("masterBone"))
            return;
        DynamicBone dynamicBone = go.AddComponent<DynamicBone>();
        dynamicBone.m_Root = go.transform;
        dynamicBone.m_Stiffness = 0.85f;
        dynamicBone.m_Damping = 0.33f;
        dynamicBone.m_Inert = 0.37f;
        if (go.name.Contains("Balls"))
        {
            //this.BallBone = child.gameObject;
            dynamicBone.m_Stiffness = 0.4f;
            dynamicBone.m_Damping = 0.2f;
            dynamicBone.m_Inert = 0.15f;
        }
    }

   public void ApplySettings()
    {
        dick.sharedMesh = EnokSliderBase.dicksMeshList[EnokSliderBase.selectedDick];
        
        float evaluatedDickArousal = EnokSliderBase.dickAnimationCurve.Evaluate(_arousalCurrent) * 100.0f;
        float evaluatedClothArousal = EnokSliderBase.clothAnimationCurve.Evaluate(_arousalCurrent) * 100.0f;
        
        dickSize = EnokSliderBase.dickSize.Value * 100.0f;
        ballsSize = EnokSliderBase.ballsSize.Value * 100.0f;
        ballsDivot = EnokSliderBase.ballsDivot.Value * 100.0f;
        _dickSizeSheathForCloth =
            dick.sharedMesh.name == "_" + EnokSliderBase.SheathCockObject.name
                ? MathF.Max(EnokSliderBase.dickSize.Value - (evaluatedClothArousal / 100.0f), 0) * 100.0f 
                : 0.0f;
        bodyFemboy = EnokSliderBase.bodyFemboy.Value * 100.0f;
        bodyMuscular = EnokSliderBase.bodyMuscular.Value * 100.0f;
        bodyBellySize = EnokSliderBase.bodyBellySize.Value * 100.0f;
        bodyAssSize = EnokSliderBase.bodyAssSize.Value * 100.0f;
        helmetOff = EnokSliderBase.helmetOff.Value ? 100.0f : 0.0f;
        
        dick.SetBlendShapeWeight(dick.sharedMesh.GetBlendShapeIndex("key_erect"), evaluatedDickArousal);
        dick.SetBlendShapeWeight(dick.sharedMesh.GetBlendShapeIndex("key_dickSizeErect"), evaluatedDickArousal * (dickSize/100));
        cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_erect"), evaluatedClothArousal);
        cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_dickSizeErect"), evaluatedClothArousal * (dickSize/100));
        
        dick.SetBlendShapeWeight(dick.sharedMesh.GetBlendShapeIndex("key_dickSize"), dickSize);
        dick.SetBlendShapeWeight(dick.sharedMesh.GetBlendShapeIndex("key_ballSize"), ballsSize);
        dick.SetBlendShapeWeight(dick.sharedMesh.GetBlendShapeIndex("key_ballDivot"), ballsDivot);
        
        cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_dickSize"), Mathf.Min(dickSize * 1.5f, 100));
        cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_dickSizeSheath"), _dickSizeSheathForCloth);
        cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_bellySize"), bodyBellySize);
        cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_muscles"), bodyMuscular);
        
        belt.SetBlendShapeWeight(belt.sharedMesh.GetBlendShapeIndex("key_muscles"), bodyMuscular);
        belt.SetBlendShapeWeight(belt.sharedMesh.GetBlendShapeIndex("key_bellySize"), bodyBellySize);
        belt.SetBlendShapeWeight(belt.sharedMesh.GetBlendShapeIndex("key_femboy"), bodyFemboy);
        
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_muscles"), bodyMuscular);
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_bellySize"), bodyBellySize);
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_femboy"), bodyFemboy);
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_assSize"), bodyAssSize);
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_helmOff"), helmetOff);
        
        nipples.SetBlendShapeWeight(nipples.sharedMesh.GetBlendShapeIndex("key_muscles"), bodyMuscular);
        nipples.SetBlendShapeWeight(nipples.sharedMesh.GetBlendShapeIndex("key_femboy"), bodyFemboy);
        
        arms.SetBlendShapeWeight(arms.sharedMesh.GetBlendShapeIndex("key_muscles"), bodyMuscular);
        arms.SetBlendShapeWeight(arms.sharedMesh.GetBlendShapeIndex("key_femboy"), bodyFemboy);
        
        if (textureChanged)
        {
            NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex1] = EnokSliderBase.helmetOff.Value ? 
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex1,
                    EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enokTex_01_helmetOff") :
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex1,
                    EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture]) ;
            torso.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex1];
            nipples.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex1];
            feet.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex1];
            
            NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Red] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Enok, TexturesEnum.Red,
                EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture]);
            horns.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Red];
            
            NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex2] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex2,
                    EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture]);
            tail.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex2];
            arms.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex2];
            ears.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex2];
            
            NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
                    EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture]);
            belt.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3];
            
            NPCPatcherBase.customTextures[_dickTexture] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
                EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enokTex_dick");
            dick.material.mainTexture = NPCPatcherBase.customTextures[_dickTexture];
            dick.material.mainTexture.filterMode = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3].filterMode;
            
            NPCPatcherBase.customTextures[_beardTexture] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
                    EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enok_beardTex");
            beard.material.mainTexture.filterMode = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3].filterMode;
            
            NPCPatcherBase.customTextures[_eyesTexture] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
                EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enokTex_eyes");
            eyes.material.mainTexture.filterMode = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3].filterMode;
            
            NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Shadow] = NPCPatcherBase.LoadTextureOrDefault(
                NPCEnum.Enok, TexturesEnum.Shadow,
                EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture]);
            shadow.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Shadow];
            
            NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Axe] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Axe,
                    EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture]);
            axe1.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Axe];
            axe2.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Axe];

            NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.FacePic1] =
                NPCPatcherBase.LoadTextureOrDefault(
                    NPCEnum.Enok, TexturesEnum.FacePic1,
                    EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture]);

            EnokSliderBase.facepic1Sprite = Sprite.Create(
                NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.FacePic1],
                EnokSliderBase.facepic1Sprite.rect, EnokSliderBase.facepic1Sprite.pivot,
                EnokSliderBase.facepic1Sprite.pixelsPerUnit, 0);

            for (int i = 0; i < EnokSliderBase.DialogList.Count; i++)
            {
                if (EnokSliderBase.DialogList[i].facepic.texture.name == "facepic_angela01")
                {
                    EnokSliderBase.DialogList[i].facepic = EnokSliderBase.facepic1Sprite;
                }
            }
            textureChanged = false;
        }
        
        cloth.material.mainTexture = (EnokSliderBase.clothGone.Value)
            ? Texture2D.blackTexture
            : NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3];
        
        beard.material.mainTexture = (EnokSliderBase.beardToggle.Value && EnokSliderBase.helmetOff.Value)
            ? NPCPatcherBase.customTextures[_beardTexture]
            : Texture2D.blackTexture;
        
        eyes.material.mainTexture = (EnokSliderBase.helmetOff.Value)
            ? NPCPatcherBase.customTextures[_eyesTexture]
            : Texture2D.blackTexture;
        
        neckbrace.material.mainTexture = (EnokSliderBase.helmetOff.Value)
            ? Texture2D.blackTexture
            : NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3];
    }

    private void Update()
    {
        if (Mathf.Approximately(_arousalTarget, 1.0f))
        {
            if (_arousalCurrent < _arousalTarget) { _arousalCurrent += 0.1f * Time.deltaTime; }
            else { _arousalCurrent = _arousalTarget; }
        }
        else
        {
            if (_arousalCurrent > _arousalTarget) { _arousalCurrent -= 0.1f * Time.deltaTime; }
            else { _arousalCurrent = _arousalTarget; }
        }

        if (!Mathf.Approximately(_arousalCurrent, _arousalTarget))
        {
            float evaluatedDickArousal = EnokSliderBase.dickAnimationCurve.Evaluate(_arousalCurrent) * 100.0f;
            float evaluatedClothArousal = EnokSliderBase.clothAnimationCurve.Evaluate(_arousalCurrent) * 100.0f;
            dick.SetBlendShapeWeight(dick.sharedMesh.GetBlendShapeIndex("key_erect"), evaluatedDickArousal);
            dick.SetBlendShapeWeight(dick.sharedMesh.GetBlendShapeIndex("key_dickSizeErect"), evaluatedDickArousal * (dickSize/100.0f));
            cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_erect"), evaluatedClothArousal);
            cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_dickSizeErect"), evaluatedClothArousal * (dickSize/100.0f));
            _dickSizeSheathForCloth =
                dick.sharedMesh.name == "_" + EnokSliderBase.SheathCockObject.name
                    ? MathF.Max(EnokSliderBase.dickSize.Value - (evaluatedClothArousal / 100.0f), 0) * 100.0f 
                    : 0.0f;
            cloth.SetBlendShapeWeight(cloth.sharedMesh.GetBlendShapeIndex("key_dickSizeSheath"), _dickSizeSheathForCloth);
        }
        
        if (Input.GetKeyDown(EnokSliderBase.dickErect.Value))
        {
            _arousalTarget = _arousalTarget == 0.0f ? 1.0f : 0.0f;
        }
    }
}