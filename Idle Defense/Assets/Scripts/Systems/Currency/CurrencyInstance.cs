using Assets.Scripts.Enums;
using Assets.Scripts.SO;
using UnityEngine;

namespace Assets.Scripts.Systems.Currency
{
    public class CurrencyInstance : MonoBehaviour
    {
        [Tooltip("Does the currency persist after player death?")]
        public bool IsPermanent;

        [Tooltip("Currency amount")]
        public ulong Currency;
    }
}