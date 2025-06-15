using System;
using UnityEngine;

namespace Assets.Scripts.UpgradeSystem.PermanentUpgrades
{
    [Serializable]
    public class PermanentStatsInstance : MonoBehaviour
    {
        public PermanentStat DamageMultiplier;

        public PermanentStatsInstance() { } // Used to load from DTO
    }

    public struct PermanentStat
    {
        public float Value;
        public int Cost;
    }
}