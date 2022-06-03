using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using InfimaGames.LowPolyShooterPack;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityTemplateProjects.Tools;
using AudioSettings = InfimaGames.LowPolyShooterPack.AudioSettings;
using EventCode = Scripts.Weapon.EventCode;

public class EventManager : MonoBehaviourPun,IOnEventCallback
{
    [SerializeField]
    private AudioClip ac_normalKill;
    [SerializeField]
    private AudioClip ac_headShotKill;
    private Battle Battle;

    private bool hitHint;



    private EnumTools.HitKinds HitKind;
    
    protected IGameModeService gameModeService;
    private IAudioManagerService audioManagerService;

    #region Unity

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Awake()
    {
        Battle = GetComponent<Battle>();
        audioManagerService ??= ServiceLocator.Current.Get<IAudioManagerService>();
    }

    private void Start()
    {
        gameModeService = ServiceLocator.Current.Get<IGameModeService>();
    }

    #endregion

    
    public void OnEvent(EventData photonEvent)
    {
        switch ((EventCode) photonEvent.Code)
        {
            case EventCode.HitPlayer:
                hitPlayer(photonEvent);
                break;
            case EventCode.KillPlayer:
                killPlayer(photonEvent);
                break;
        }
    }

    private void hitPlayer(EventData eventData)
    {
        Dictionary<byte, object> tmp_HitData = (Dictionary<byte, object>)eventData.CustomData;
        Player tmp_hitPlayer = (Player)tmp_HitData[0];
        Player tmp_DMGFrom = (Player)tmp_HitData[1];
        String tmp_DMGWeapon = (string)tmp_HitData[2];
        float tmp_DMG = (float)tmp_HitData[3];
        bool tmp_headShot = (bool)tmp_HitData[4];
        Vector3 tmp_contactPoint = (Vector3)tmp_HitData[5];
        long tmp_time = (long)tmp_HitData[6];

        bool tmp_IsSuicide = tmp_hitPlayer.Equals(tmp_DMGFrom);
        
        //死亡后不造成伤害和显示击中UI
        if (gameModeService.GetPlayerGameObject(tmp_hitPlayer).TryGetComponent(out Battle battle))
        {
            if (battle.GetIsDeath())
            {
                return;
            }
        }

        Debug.Log(tmp_DMGFrom.NickName+"用"+tmp_DMGWeapon+"击中了"+tmp_hitPlayer.NickName+"造成了"+tmp_DMG+"伤害——————是否爆头？："+tmp_headShot);
        
        //仅在伤害来源方执行
        //造成伤害UI提示
        if (tmp_DMGFrom.Equals(PhotonNetwork.LocalPlayer))
        {
            Debug.Log("准备播放击中");
            hitHint = true;
            HitKind = tmp_headShot ? EnumTools.HitKinds.headShot : EnumTools.HitKinds.normal;
        }
        
        //仅在被击中方执行
        //扣血
        if (tmp_hitPlayer.Equals(PhotonNetwork.LocalPlayer))
        {
            UIDamageIndecatorManager.CreateIndicator(gameModeService.GetPlayerGameObject(tmp_DMGFrom).transform);
            Debug.Log("显示伤害来源！");
            //扣血
            if (Battle.Damage(tmp_DMG))
            {
                Debug.Log("death！");
                //致死伤害准星提示
                HitKind = EnumTools.HitKinds.killShot;
                //如果造成伤害致死的情况下且击中人为自己
                //发送死亡event
                Dictionary<byte, object> tmp_DeathData = new Dictionary<byte, object>();
                //被击杀着
                tmp_DeathData.Add(0,tmp_hitPlayer);
                //击杀者
                tmp_DeathData.Add(1,tmp_DMGFrom);
                //击杀者使用武器
                tmp_DeathData.Add(2,tmp_DMGWeapon);
                //是否爆头
                tmp_DeathData.Add(3,tmp_headShot);
                //伤害时间戳
                tmp_DeathData.Add(4,tmp_time);
                
                RaiseEventOptions tmp_RaiseEventOptions = new RaiseEventOptions() {Receivers = ReceiverGroup.All};
                SendOptions tmp_SendOptions = SendOptions.SendReliable;
                PhotonNetwork.RaiseEvent(
                    (byte)EventCode.KillPlayer,
                    tmp_DeathData,
                    tmp_RaiseEventOptions,
                    tmp_SendOptions);
                Debug.Log("发送死亡事件！");
            }
        }
    }

    private void killPlayer(EventData eventData)
    {
        Dictionary<byte, object> tmp_KillData = (Dictionary<byte, object>)eventData.CustomData;
        Player tmp_deathPlayer =(Player)tmp_KillData[0];
        Player tmp_KillFrom =(Player)tmp_KillData[1];
        String tmp_KillWeapon = (String)tmp_KillData[2];
        bool tmp_headShot = (bool)tmp_KillData[3];
        long tmp_time = (long)tmp_KillData[4];
        
        //自杀不执行
        if(tmp_KillFrom.Equals(tmp_deathPlayer))
            return;
        
        //如果是本地玩家造成了角色死亡
        if(tmp_KillFrom.Equals(PhotonNetwork.LocalPlayer))
        {
            EnumTools.KillKind tmp_killkind;
            if (tmp_headShot)
            {
                tmp_killkind = EnumTools.KillKind.playerHeadshot;
            }
            else
            {
                tmp_killkind = EnumTools.KillKind.player;
            }
            //击杀提示
            UIKillFeedBackIconManager.CreateKillFeedbackIcon(tmp_killkind);
            //显示击杀提示
            //TODO score
            UIKillFeedBackTextManager.CreateKillFeedbackText(tmp_KillWeapon, tmp_deathPlayer.NickName, 100);
            //击杀音效
            AudioSettings settings = new AudioSettings(1.0f, 0.0f, true,false,Vector3.zero,null,100);
            audioManagerService?.PlayOneShot(tmp_headShot? ac_headShotKill: ac_normalKill, settings);
        }
    }
    #region Getter

    public bool GethitHint => hitHint;

    public EnumTools.HitKinds GetHitKind => HitKind;

    #endregion

    #region Setter

    public void SethitHint(bool set)
    {
        hitHint = set;
    }

    #endregion

}
