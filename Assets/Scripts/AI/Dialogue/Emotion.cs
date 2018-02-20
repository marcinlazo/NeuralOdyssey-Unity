using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    normal,
    happy,
    happyOpenMouth,
    confident,
    surprized3,
    annoyed,
    annoyedSmile,
    annoyedClosedEyes,
    sad,
    facingWall,
    facingWallHappy,
    surprized1,
    surprized2,
    noCharacter
}

[System.Serializable]
public struct EmotionPack
{
    public Emotion emotion;
    public Texture[] textures;
    public int finalEmotion;
}
