using Assets.Scripts.Enums;
using Assets.Scripts.SO;
using Assets.Scripts.Structs;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.PlayerBase
{
    [Serializable]
    public class PlayerBaseStatsInstance
    {
        public int MaxHealth;
        public float RegenAmount;
        public float RegenInterval;

        public Dictionary<PlayerBaseStatType, PlayerBaseStat> Stats = new();

        public PlayerBaseStatsInstance(PlayerBaseSO source)
        {
            MaxHealth = source.MaxHealth;
            RegenAmount = source.RegenAmount;
            RegenInterval = source.RegenInterval;

            Stats[PlayerBaseStatType.MaxHealth] = new PlayerBaseStat
            {
                UpgradeAmount = source.MaxHealthStat.UpgradeAmount,
                BaseCost = source.MaxHealthStat.BaseCost,
                Level = source.MaxHealthStat.Level
            };
            Stats[PlayerBaseStatType.RegenAmount] = new PlayerBaseStat
            {
                UpgradeAmount = source.RegenAmountStat.UpgradeAmount,
                BaseCost = source.RegenAmountStat.BaseCost,
                Level = source.RegenAmountStat.Level
            };
            Stats[PlayerBaseStatType.RegenInterval] = new PlayerBaseStat
            {
                UpgradeAmount = source.RegenIntervalStat.UpgradeAmount,
                BaseCost = source.RegenIntervalStat.BaseCost,
                Level = source.RegenIntervalStat.Level
            };
        }

        public PlayerBaseStatsInstance() { } //Used to load from DTO
    }
}