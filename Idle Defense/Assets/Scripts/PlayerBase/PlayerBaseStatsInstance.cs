using Assets.Scripts.SO;
using System;

namespace Assets.Scripts.PlayerBase
{
    [Serializable]
    public class PlayerBaseStatsInstance
    {
        public float MaxHealth;
        public float RegenAmount;
        public float RegenDelay;
        public float RegenInterval;

        public float MaxHealthUpgradeAmount;
        public float MaxHealthUpgradeBaseCost;
        public int MaxHealthLevel;

        public float RegenAmountUpgradeAmount;
        public float RegenAmountUpgradeBaseCost;
        public int RegenAmountLevel;

        public float RegenIntervalUpgradeAmount;
        public float RegenIntervalUpgradeBaseCost;
        public int RegenIntervalLevel;
        public float MinRegenInterval;


        public PlayerBaseStatsInstance(PlayerBaseSO source)
        {
            MaxHealth = source.MaxHealth;
            RegenAmount = source.RegenAmount;
            RegenInterval = source.RegenInterval;

            MaxHealthUpgradeAmount = source.MaxHealthUpgradeAmount;
            MaxHealthUpgradeBaseCost = source.MaxHealthUpgradeBaseCost;
            MaxHealthLevel = source.MaxHealthLevel;

            RegenAmountUpgradeAmount = source.RegenAmountUpgradeAmount;
            RegenAmountUpgradeBaseCost = source.RegenAmountUpgradeBaseCost;
            RegenAmountLevel = source.RegenAmountLevel;

            RegenIntervalUpgradeAmount = source.RegenIntervalUpgradeAmount;
            RegenIntervalUpgradeBaseCost = source.RegenIntervalUpgradeBaseCost;
            RegenIntervalLevel = source.RegenIntervalLevel;
            MinRegenInterval = source.MinRegenInterval;
        }

        public PlayerBaseStatsInstance() { } //Used to load from DTO
    }
}