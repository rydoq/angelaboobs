using UnityEngine;

namespace angelaboobs;

public class AngelaBoobController : MonoBehaviour
{
    public float nippleInnie;
    public float nippleSize;
    public float boobSmall;
    public float boobLarge;
    public float nippleGone;
    public float boobSag;

    public SkinnedMeshRenderer boobs;
    public SkinnedMeshRenderer torso;
    
    public void ApplySettings()
    {
        nippleGone = AngelaBoobsBase.nippleGone.Value * 100.0f;
        nippleInnie = AngelaBoobsBase.nippleInnie.Value * 100.0f;
        boobSmall = AngelaBoobsBase.boobSmall.Value * 100.0f;
        boobLarge = AngelaBoobsBase.boobLarge.Value * 100.0f;
        nippleSize = AngelaBoobsBase.nippleSize.Value * 100.0f;
        boobSag = AngelaBoobsBase.topless.Value ? 100.0f - boobSmall + boobLarge/2f : 0.0f;
        
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleInnie"), nippleInnie);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSmall"), boobSmall);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSize"), boobLarge);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleSize"), nippleSize);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_nippleGone"), nippleGone);
        boobs.SetBlendShapeWeight(boobs.sharedMesh.GetBlendShapeIndex("key_boobSag"), boobSag);
        boobs.material.mainTexture = AngelaBoobsBase.topless.Value ? AngelaBoobsBase.nudeTexture : AngelaBoobsBase.clothedTexture;
        torso.material.mainTexture = AngelaBoobsBase.topless.Value ? AngelaBoobsBase.nudeTexture : AngelaBoobsBase.clothedTexture;
    }
}