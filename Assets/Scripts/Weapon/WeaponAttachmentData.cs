// ******************************************************************
//       /\ /|       @file       FILENAME
//       \ V/        @brief      
//       | "")       @author     topkang
//       /  |                    
//      /  \\        @Modified   DATE
//    *(__\_\        @Copyright  Copyright (c) YEAR, TOPGAMING
// ******************************************************************

using UnityEngine;

namespace UnityTemplateProjects.Weapon
{
    [CreateAssetMenu(menuName = "FPS/Weapon/NewWeaponAttachmentData", fileName = "NewWeaponAttachmentData")]
    public class WeaponAttachmentData : ScriptableObject
    {
        #region Serialezed

        [Header("配件ID")] 
        public int AttachmentID;
        
        [Header("配件名称")] 
        public string AttachmentName;
        
        [Header("优势描述")] 
        public string AdvantageDiscriptions;
        
        [Header("缺点描述")] 
        public string DisadvantageDiscriptions;
        
        [Header("伤害系数")]
        public float DamageAlpha = 1;

        [Header("射速系数")]
        public float ShootSpeedAlpha = 1;

        [Header("子弹飞行速度系数")]
        public float FlySpeedAlpha = 1;

        [Header("垂直后坐力系数")]
        public float VerticalRecoilAlpha = 1;

        [Header("水平后坐力系数")] 
        public float HorizentalRecoilAlpha = 1;

        [Header("瞄准散射系数")] 
        public float ADSSpreadAlpha = 1;
        
        [Header("腰射散射系数")] 
        public float HipShotSpreadAlpha = 1;
        
        [Header("移动散射惩罚系数")]
        public float MovSpreadAlpha = 1;

        [Header("开镜时间系数")] 
        public float T_ADSTimeAlpha = 1;
        
        [Header("切枪时间系数")] 
        public float T_SwitchGunAlpha = 1;

        [Header("换弹时间系数")] 
        public float T_ReloadAlpha = 1;
        
        [Header("装备栏图片")] 
        public Sprite sprite;
        
        [Header("配件更换系统选中图片")] 
        public Sprite BTNspriteB;
        
        [Header("配件更换系统未选中图片")] 
        public Sprite BTNspriteD;

        
        #endregion
    }
}