using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnumTools
{
    public enum WeaponKind
    {
        SMG,
        ASSAULT,
        LMG,
        MARKSMAN,
        SNIPER,
        UTILITY,
        HANDGUN,
        GRENADE,
        ITEM,
        
        
        
        
        NONE
    }
    
    public enum InventoryKind
    {
        Main,
        Sec,
        Grenade,
        Item,
        NONE
    }
    
    public enum HitKinds
    {
        normal,
        headShot,
        killShot
    }
    
    public enum KillKind
    {
        player,
        playerHeadshot,
        vehicle
    }
    
    public enum PlayerProperties
    {
        IsDeath,
        IsDetected,
        Data_kill,
        Data_Death,
        Data_Ping,
        IsMuzzle,
        CurrentWeaponID,
        
    }
    
    public enum Teams
    {
        None,
        Blue,
        Red
    }

    public enum TeamColor
    {
        None,
        Mine,
        Enemy
    }

    public enum ConquestPoints
    {
        None,
        BlueBirth,
        RedBirth,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J
    }
    
    public enum PostProcessing
    {
        Globle,
        Weapon,
        Scope,
        UI,
        DeathZone
    }
    
    public enum DetectTargetKind
    {
        Around,
        Gaze
    }

    public static Color GetTeamColor(TeamColor team)
    {
        return team switch
        {
            TeamColor.None => new Color(0.7f,0.7f,0.7f,1),
            TeamColor.Enemy => new Color(1,0,0.28f,1),
            TeamColor.Mine => new Color(0,1,1,1)
            
        };
    }
}
