using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/Weapon/NewWeaponData", fileName = "NewWeaponData")]
public class  WeaponData : ScriptableObject
{
    #region Serialized

    [Header("Settings")] 
    [Tooltip("武器ID")]
    [SerializeField] public int ID;
    [Tooltip("Weapon Name. Currently not used for anything, but in the future, we will use this for pickups!")]
    [SerializeField]
    public string weaponName;

    [SerializeField] public string weaponShowName;

    [Tooltip("How much the character's movement speed is multiplied by when wielding this weapon.")] [SerializeField]
    public float multiplierMovementSpeed = 1.0f;

    [Header("Firing")]
    [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
    [SerializeField]
    public bool automatic;

    [Tooltip("Is this weapon bolt-action? If yes, then a bolt-action animation will play after every shot.")]
    [SerializeField]
    public bool boltAction;

    [Tooltip(
        "Amount of shots fired at once. Helpful for things like shotguns, where there are multiple projectiles fired at once.")]
    [SerializeField]
    public int shotCount = 1;

    [Tooltip("散射")] [SerializeField]
    public float spread = 0.25f;

    [Tooltip("散布的移动惩罚系数")]
    public float spreadSpeedTimer;

    [Tooltip("首颗子弹是否有散射保护")]
    public bool firstBulletAcc;

    [SerializeField]
    [Tooltip("后坐力重置时间")]
    public float recoilReturnTime = 0.3f;

    [Header("枪械属性")] [Tooltip("How fast the projectiles are.")] [SerializeField]
    public float projectileImpulse = 400.0f;

    [Tooltip("枪械伤害")] [SerializeField] public float DMG = 10f;

    [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
    [SerializeField]
    public int roundsPerMinutes = 200;
    

    [Tooltip(
        "Maximum distance at which this weapon can fire accurately. Shots beyond this distance will not use linetracing for accuracy.")]
    [SerializeField]
    public float maximumDistance = 500.0f;

    [Header("Reloading")]
    [Tooltip("Determines if this weapon reloads in cycles, meaning that it inserts one bullet at a time, or not.")]
    [SerializeField]
    public bool cycledReload;

    [Tooltip("Determines if the player can reload this weapon when it is full of ammunition.")] [SerializeField]
    public bool canReloadWhenFull = true;

    [Tooltip("Should this weapon be reloaded automatically after firing its last shot?")] [SerializeField]
    public bool automaticReloadOnEmpty;

    [Tooltip("Time after the last shot at which a reload will automatically start.")] [SerializeField]
    public float automaticReloadOnEmptyDelay = 0.25f;

    [Header("Animation")]

    [Tooltip("Weapon Bone Offsets.")] [SerializeField]
    public Offsets weaponOffsets;

    [Tooltip("Sway smoothing value. Makes the weapon sway smoother.")] [SerializeField]
    public float swaySmoothValue = 10.0f;

    [Tooltip("Character arms sway when wielding this weapon.")] [SerializeField]
    public Sway sway;

    [Header("Resources")] [Tooltip("Casing Prefab.")] [SerializeField]
    public GameObject prefabCasing;

    [Tooltip("Projectile Prefab. This is the prefab spawned when the weapon shoots.")] [SerializeField]
    public GameObject prefabProjectile;

    [Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")] [SerializeField]
    public RuntimeAnimatorController controller;

    [Tooltip("Weapon Body Texture.")] [SerializeField]
    public Sprite spriteBody;

    [Header("Audio Clips Holster")] [Tooltip("Holster Audio Clip.")] [SerializeField]
    public AudioClip audioClipHolster;

    [Tooltip("Unholster Audio Clip.")] [SerializeField]
    public AudioClip audioClipUnholster;

    [Header("Audio Clips Reloads")] [Tooltip("Reload Audio Clip.")] [SerializeField]
    public AudioClip audioClipReload;

    [Tooltip("Reload Empty Audio Clip.")] [SerializeField]
    public AudioClip audioClipReloadEmpty;

    [Header("Audio Clips Reloads Cycled")] [Tooltip("Reload Open Audio Clip.")] [SerializeField]
    public AudioClip audioClipReloadOpen;

    [Tooltip("Reload Insert Audio Clip.")] [SerializeField]
    public AudioClip audioClipReloadInsert;

    [Tooltip("Reload Close Audio Clip.")] [SerializeField]
    public AudioClip audioClipReloadClose;

    [Header("Audio Clips Other")]
    [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
    [SerializeField]
    public AudioClip audioClipFireEmpty;
    [SerializeField] public AudioClip audioClipBoltAction;

    [Header("武器图片素材")] 
    [SerializeField] public Sprite DeployB;
    [SerializeField] public Sprite DeployD;
    [SerializeField] public Sprite KillPannel;
    
    
    
    //TODO 配件集合


    #endregion
}