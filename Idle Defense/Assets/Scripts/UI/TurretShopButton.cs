using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Systems;
using Assets.Scripts.Turrets;
using Assets.Scripts.WaveSystem;
using Assets.Scripts.SO;

namespace Assets.Scripts.UI
{
    public class TurretShopButton : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private TurretType turretType;
        [SerializeField] private Image icon;          // UI Image
        [SerializeField] private TextMeshProUGUI countText;     // �x3�
        [SerializeField] private TextMeshProUGUI costText;      // �$10k�
        //[SerializeField] private TextMeshProUGUI lockText;      // �Wave 30�
        [SerializeField] private TextMeshProUGUI dpsText;      
        [SerializeField] private Image  lockIcon;      
        [SerializeField] private Button buyButton;

        private void Start()
        {
            buyButton.onClick.AddListener(TryBuy);
            GameManager.Instance.OnMoneyChanged += _ => Refresh();
            TurretInventoryManager.I.OnInventoryChanged += Refresh;
            WaveManager.Instance.OnWaveStarted += (_, __) => Refresh();
            Refresh();
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveListener(TryBuy);
            if (GameManager.Instance != null)
                GameManager.Instance.OnMoneyChanged -= _ => Refresh();
            if (TurretInventoryManager.I != null)
                TurretInventoryManager.I.OnInventoryChanged -= Refresh;
            if (WaveManager.Instance != null)
                WaveManager.Instance.OnWaveStarted -= (_, __) => Refresh();
        }

        private void Refresh()
        {
            var inv = TurretInventoryManager.I;
            int owned = inv.Owned.Count(t => t.TurretType == turretType);

            if (owned >= 5)
            {
                icon.color = Color.white;
                //lockText.gameObject.SetActive(false);
                lockIcon.gameObject.SetActive(false);

                countText.text = $"x{owned}";
                costText.text = "Max";
                costText.color = Color.black;
                buyButton.interactable = false;
                dpsText.gameObject.SetActive(false); // Hide DPS text when maxed out
                return;
            }

            int curWave = WaveManager.Instance.GetCurrentWaveIndex();
            bool ownsAny = owned > 0;

            if (ownsAny)
            {
                icon.color = Color.white;
                //lockText.gameObject.SetActive(false);
                lockIcon.gameObject.SetActive(false);
            }
            else
            {
                icon.color = Color.black;
                //lockText.gameObject.SetActive(true);
                lockIcon.gameObject.SetActive(true);
               // lockText.text = $"Not owned";
            }

            // Continue with normal pricing
            ulong cost = inv.GetCost(turretType, owned);
            countText.text = $"x{owned}";
            costText.text = $"${UIManager.AbbreviateNumber(cost)}";

            bool afford = GameManager.Instance.Money >= cost;
            buyButton.interactable = afford;
            costText.color = afford ? Color.black : Color.red;

            UpdateDPSDisplay();
            /* These controls unlocking the turrets based on wave number
            bool unlocked = inv.IsTurretTypeUnlocked(turretType);
            int curWave = WaveManager.Instance.GetCurrentWaveIndex();

            if (unlocked)
            {
                // unlocked state
                icon.color = Color.white;
                lockText.gameObject.SetActive(false);

                ulong cost = inv.GetCost(turretType, owned);
                countText.text = $"x{owned}";
                costText.text = $"${UIManager.AbbreviateNumber(cost)}";

                bool afford = GameManager.Instance.Money >= cost;
                buyButton.interactable = afford;
                costText.color = afford ? Color.black : Color.red;
            }
            else
            {
                // locked state
                icon.color = Color.black;
                countText.text = "";
                costText.text = "";

                int waveReq = inv.WaveRequirement(turretType);
                lockText.gameObject.SetActive(true);
                lockText.text = $"Wave {waveReq}";

                // only show price when wave reached
                if (curWave >= waveReq)
                    costText.text = $"${UIManager.AbbreviateNumber(inv.GetCost(turretType, 0))}";

                buyButton.interactable = false;
            }*/
        }

        private void UpdateDPSDisplay()
        {
            TurretInfoSO info = TurretInventoryManager.I.GetInfoSO(turretType);
            float dps = TurretStatsCalculator.CalculateDPS(info);
            if (info.TurretType == TurretType.Laser)
                dpsText.text = $">{dps:F1} DPS";
            else
                dpsText.text = $"{dps:F1} DPS";
        }


        private void TryBuy()
        {
            if (TurretInventoryManager.I.TryPurchase(turretType))
                UIManager.Instance.ShowToast("Turret bought!");
            else
                UIManager.Instance.ShowToast("Need more coins");
            Refresh();
        }
    }
}
