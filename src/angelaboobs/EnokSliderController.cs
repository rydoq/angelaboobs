using System;
using UnityEngine;

namespace angelaboobs;

public class EnokSliderController : MonoBehaviour
{
    private float _arousalTarget = 0.0f;
    private float _arousalCurrent;
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
    public MeshRenderer axe1;
    public MeshRenderer axe2;
    public MeshRenderer shadow;
    
    private int _dickTexture;
    
    private void Awake()
    {
        _dickTexture = NPCPatcherBase.customTextures.Count;
        NPCPatcherBase.customTextures.Add(NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
            EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enokTex_dick"));
    }
    public void ApplySettings()
    {
        cloth.material.mainTexture = (EnokSliderBase.clothGone.Value)
            ? Texture2D.blackTexture
            : NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3];
        
        dick.sharedMesh = EnokSliderBase.dickNames[EnokSliderBase.selectedDick] == EnokSliderBase.dick.name ? EnokSliderBase.dick : EnokSliderBase.dickSheath;
        
        if (textureChanged)
        {
            NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex1] =
                NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex1,
                    EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture]);
            torso.material.mainTexture = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex1];
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
            neckbrace.material.mainTexture =  NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3];
            cloth.material.mainTexture =  NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3];
            
            NPCPatcherBase.customTextures[_dickTexture] = NPCPatcherBase.LoadTextureOrDefault(NPCEnum.Enok, TexturesEnum.Tex3,
                EnokSliderBase.textureFolderNames[EnokSliderBase.selectedTexture], "enokTex_dick");
            dick.material.mainTexture = NPCPatcherBase.customTextures[_dickTexture];
            dick.material.mainTexture.filterMode = NPCPatcherBase.textureDictionary[NPCEnum.Enok][TexturesEnum.Tex3].filterMode;
            
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
            dick.SetBlendShapeWeight(0, EnokSliderBase.dickAnimationCurve.Evaluate(_arousalCurrent) * 100);
            cloth.SetBlendShapeWeight(0, EnokSliderBase.clothAnimationCurve.Evaluate(_arousalCurrent) * 100);
        }
        
        if (Input.GetKeyDown(EnokSliderBase.dickErect.Value))
        {
            _arousalTarget = _arousalTarget == 0.0f ? 1.0f : 0.0f;
        }
    }
}