using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "CurrencySO", menuName = "ScriptableObjects/CurrencySO")]
    public class CurrencySO : ScriptableObject
    {
        [Tooltip("Does the currency persist after player death?")]
        public bool IsPermanent;

        [Tooltip("Currency amount")]
        public ulong Currency;

        [Tooltip("Currency type")]
        public CurrencyEnum CurrencyType;
    }
}