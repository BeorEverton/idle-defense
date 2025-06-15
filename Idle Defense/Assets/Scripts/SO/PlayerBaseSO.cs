using Assets.Scripts.Structs;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "PlayerBaseStats", menuName = "ScriptableObjects/PlayerBase", order = 3)]
    public class PlayerBaseSO : ScriptableObject
    {
        [Header("Health")]
        [Tooltip("Max health of player base")]
        public int MaxHealth;

        [Tooltip("Amount of health regenerated per tick")]
        public float RegenAmount;

        [Tooltip("Time interval between regen ticks")]
        public float RegenInterval = 0.5f;

        [Tooltip("Upgrade amount per level for MaxHealth")]
        public PlayerBaseStat MaxHealthStat;

        [Tooltip("Upgrade amount per level for RegenAmount")]
        public PlayerBaseStat RegenAmountStat;

        [Tooltip("Upgrade amount per level for RegenInterval (lower is faster)")]
        public PlayerBaseStat RegenIntervalStat;
    }
}