using Assets.Scripts.SO;
using System;

namespace Assets.Scripts.PlayerBase
{
    [Serializable]
    public class PlayerBaseStatsInstance
    {
        public int MaxHealth;
        public float RegenAmount;
        public float RegenInterval;

        public PlayerBaseStat MaxHealthStat;
        public PlayerBaseStat RegenAmountStat;
        public PlayerBaseStat RegenIntervalStat;

        public PlayerBaseStatsInstance(PlayerBaseSO source)
        {
            MaxHealth = source.MaxHealth;
            RegenAmount = source.RegenAmount;
            RegenInterval = source.RegenInterval;

            MaxHealthStat = new PlayerBaseStat
            {
                Amount = source.MaxHealthStat.Amount,
                BaseCost = source.MaxHealthStat.BaseCost,
                Level = source.MaxHealthStat.Level
            };
            RegenAmountStat = new PlayerBaseStat
            {
                Amount = source.RegenAmountStat.Amount,
                BaseCost = source.RegenAmountStat.BaseCost,
                Level = source.RegenAmountStat.Level
            };
            RegenIntervalStat = new PlayerBaseStat
            {
                Amount = source.RegenIntervalStat.Amount,
                BaseCost = source.RegenIntervalStat.BaseCost,
                Level = source.RegenIntervalStat.Level
            };
        }

        public PlayerBaseStatsInstance() { } //Used to load from DTO
    }

    public struct PlayerBaseStat
    {
        public float Amount;
        public ulong BaseCost;
        public int Level;
    }
}