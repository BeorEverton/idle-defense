using Assets.Scripts.Enums;
using Assets.Scripts.UI;
using Assets.Scripts.UpgradeSystem.TurretUpgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemButton : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private TMP_Text dpsLabel;     // e.g. "41.2 K DPS"
    [SerializeField] private TMP_Text levelLabel;   // e.g. "Lv 87"
    [SerializeField] private Image[] stars;        // 4 star images
    [SerializeField] private Button btn;
    [SerializeField] private Image iconImage;

    private TurretStatsInstance data;

    // Called by EquipPanelUI
    public void Init(TurretStatsInstance inst, System.Action onClick, Sprite icon)
    {
        data = inst;

        // ----- DPS --------------------------------------------------------
        double dps = ComputeDps(inst);
        dpsLabel.text = UIManager.AbbreviateNumber(dps) + " DPS";

        // ----- Level & stars ---------------------------------------------
        int totalLvl = TotalLevel(inst);
        levelLabel.text = "Lv " + totalLvl;

        int starCount = totalLvl >= 500 ? 4 :
                        totalLvl >= 250 ? 3 :
                        totalLvl >= 50 ? 2 : 1;

        for (int i = 0; i < stars.Length; i++)
            stars[i].enabled = i < starCount;

        iconImage.sprite = icon;

        // Adjust size to preserve sprite's native aspect ratio with max height
        if (icon != null)
        {
            float maxHeight = 40f;

            float width = icon.rect.width;
            float height = icon.rect.height;
            float pixelsPerUnit = icon.pixelsPerUnit;

            float nativeW = width / pixelsPerUnit;
            float nativeH = height / pixelsPerUnit;

            float scale = maxHeight / nativeH;
            float finalW = nativeW * scale;
            float finalH = maxHeight;

            iconImage.rectTransform.sizeDelta = new Vector2(finalW, finalH);
        }


        // ----- Click ------------------------------------------------------
        if (!btn)
            btn = GetComponent<Button>();

        btn.onClick.RemoveAllListeners();

        if (onClick != null)
        {
            btn.onClick.AddListener(() => onClick.Invoke());
            btn.interactable = true;            // enable
        }
        else
        {
            Debug.LogWarning($"{name}: onClick delegate was null.");
            btn.interactable = false;           // keep it disabled
        }

    }

    private static double ComputeDps(TurretStatsInstance s)
    {
        // Expected crit bonus
        double critBonus = (s.Stats[TurretStatType.CriticalChance].Value / 100.0) *
                           (s.Stats[TurretStatType.CriticalDamage].Value / 100.0 - 1.0);

        double dmgPerProj = s.Stats[TurretStatType.Damage].Value * (1.0 + critBonus);

        int pellets = Mathf.Max(1, s.PelletCount);

        // For lasers FireRate is 0, treat as 1 shot/sec (damage already per sec)
        double shotsPerSec = s.Stats[TurretStatType.FireRate].Value > 0.01f ? s.Stats[TurretStatType.FireRate].Value : 1.0;

        return dmgPerProj * pellets * shotsPerSec;
    }

    private static int TotalLevel(TurretStatsInstance s)
    {
        return Mathf.FloorToInt(
              s.Stats[TurretStatType.Damage].Level
            + s.Stats[TurretStatType.FireRate].Level
            + s.Stats[TurretStatType.CriticalChance].Level
            + s.Stats[TurretStatType.CriticalDamage].Level
            + s.ExplosionRadiusLevel
            + s.SplashDamageLevel
            + s.PierceChanceLevel
            + s.PierceDamageFalloffLevel
            + s.PelletCountLevel
            + s.DamageFalloffOverDistanceLevel
            + s.PercentBonusDamagePerSecLevel
            + s.SlowEffectLevel);
    }
}
