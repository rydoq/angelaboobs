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
    
    public void ApplySettings()
    {
        nippleGone = (AngelaBoobsBase.nippleGoneWhenClothed.Value && !AngelaBoobsBase.topless.Value) ? 100.0f : AngelaBoobsBase.nippleGone.Value * 100.0f;
        nippleInnie = (AngelaBoobsBase.nippleInnieWhenClothed.Value && !AngelaBoobsBase.topless.Value) ? 100.0f : AngelaBoobsBase.nippleInnie.Value * 100.0f;
        boobSmall = AngelaBoobsBase.boobSmall.Value * 100.0f;
        boobLarge = AngelaBoobsBase.boobLarge.Value * 100.0f;
        nippleSize = AngelaBoobsBase.nippleSize.Value * 100.0f;
        boobSag = AngelaBoobsBase.topless.Value ? 100.0f - boobSmall + boobLarge/2f : 0.0f;
        assSize = AngelaBoobsBase.assLarge.Value * 100.0f;
        thighsThick = AngelaBoobsBase.thighsThick.Value * 100.0f;
        
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleInnie"), nippleInnie);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSmall"), boobSmall);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSize"), boobLarge);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleSize"), nippleSize);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleGone"), nippleGone);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSag"), boobSag);
        
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_assSize"), assSize);
        torso.SetBlendShapeWeight(torso.sharedMesh.GetBlendShapeIndex("key_thighSize"), thighsThick);
        skirt.SetBlendShapeWeight(skirt.sharedMesh.GetBlendShapeIndex("key_thighSize"), thighsThick);
        
        if (AngelaBoobsBase.topless.Value)
        {
            if (AngelaBoobsBase.bottomless.Value)
            {
                boobs.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Nude];
                torso.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Nude];
                arms.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Nude];
                hairTuft.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Nude];
            }
            else
            {
                boobs.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Topless];
                torso.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Topless];
                arms.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Topless];
                hairTuft.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Topless];
            }
        }
        else if (AngelaBoobsBase.bottomless.Value)
        {
            boobs.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Bottomless];
            torso.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Bottomless];
            arms.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Bottomless];
            hairTuft.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Bottomless];
        }
        else
        {
            boobs.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Base];
            torso.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Base];
            arms.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Base];
            hairTuft.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Base];
        }
        
        torso.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Face];
        skirt.material.mainTexture = AngelaBoobsBase.bottomless.Value ? Texture2D.blackTexture : AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Skirt];
        arms.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Blue3];
        ears.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Tail];
        ears.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Bell];
        
        switch (eyes.material.mainTexture.name)
        {
            case "angelaEyeTex.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Eye];
                break;
            case "angelaEyeTex2.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Eye2];
                break;
            case "angelaEyeTex3.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Eye3];
                break;
            case "angelaEyeTex4.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Eye4];
                break;
            case "angelaEyeTex_5.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Eye5];
                break;
            case "angelaEyeTex_down.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.EyeDown];
                break;
            case "angelaEyeTex_L.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.EyeLeft];
                break;
            case "angelaEyeTex_R.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.EyeRight];
                break;
            case "angelaEyeTex_pissed.png":
                eyes.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.EyePissed];
                break;
        }
        
        eyes.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Red];
        feet.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Blui2];
        feet.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Blui2];
        horns.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Blue3];
        necklace.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Blu];
        necklace.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Bell];
        tail.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Tail];
        tail.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Blue3];
        glyph.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Glyph];
        bookL.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.BookSpine];
        bookL.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.BookCover];
        bookR.materials[0].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.BookSpine];
        bookR.materials[1].mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.BookCover];
        shadow.material.mainTexture = AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.Shadow];

        AngelaBoobsBase.facepic1Sprite = Sprite.Create(AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.FacePic1], AngelaBoobsBase.facepic1Sprite.rect, AngelaBoobsBase.facepic1Sprite.pivot, AngelaBoobsBase.facepic1Sprite.pixelsPerUnit, 0);
        AngelaBoobsBase.facepic2Sprite = Sprite.Create(AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.FacePic2], AngelaBoobsBase.facepic2Sprite.rect, AngelaBoobsBase.facepic2Sprite.pivot, AngelaBoobsBase.facepic2Sprite.pixelsPerUnit, 0);
        AngelaBoobsBase.facepic3Sprite = Sprite.Create(AngelaBoobsBase.Textures[(int)AngelaBoobsBase.TexturesEnum.FacePic3], AngelaBoobsBase.facepic3Sprite.rect, AngelaBoobsBase.facepic3Sprite.pivot, AngelaBoobsBase.facepic3Sprite.pixelsPerUnit, 0);

        for (int i = 0; i < AngelaBoobsBase.DialogList.Count; i++)
        {
            if (AngelaBoobsBase.DialogList[i].facepic.texture.name == "facepic_angela01")
            {
                AngelaBoobsBase.DialogList[i].facepic = AngelaBoobsBase.facepic1Sprite;
            }
            else if  (AngelaBoobsBase.DialogList[i].facepic.texture.name == "facepic_angela02")
            {
                AngelaBoobsBase.DialogList[i].facepic = AngelaBoobsBase.facepic2Sprite;
            }
            else if  (AngelaBoobsBase.DialogList[i].facepic.texture.name == "facepic_angela03")
            {
                AngelaBoobsBase.DialogList[i].facepic = AngelaBoobsBase.facepic3Sprite;
            }
        }

        if (boobSmall > 0.0f)
        {
            if (boobSmall > 50.0f)
            {
                if (boobSmall > 80.0f)
                {
                    
                }
            }
        }
    }
}