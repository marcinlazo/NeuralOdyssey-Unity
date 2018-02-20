using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static string name { get; private set; }
    public static string nickname { get; private set; }
    public static bool toldName = false;
    public static bool madeNozomuWait = false;
    public static bool refusedToTellAnything = false;
    public static bool wasASmartass = false;
    public static bool solvedPuzzle = false;
    public static bool tookBatteryToExit = false;
    public static bool consideredHostile = false;

    public static void SetName(string newName)
    {
        name = newName;
    }
    public static void SetNickname(string newName)
    {
        nickname = newName;
    }
    public static void Reset()
    {
        madeNozomuWait = refusedToTellAnything = wasASmartass = solvedPuzzle = tookBatteryToExit = toldName = consideredHostile = false;
        nickname = "";
    }
}
