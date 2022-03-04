using System;
using UnityEngine;

namespace Scripts.Weapon
{
    public abstract class IWeapon : MonoBehaviour
    {
        public String WeaponName;
        public abstract void DoAttack();
    }
}