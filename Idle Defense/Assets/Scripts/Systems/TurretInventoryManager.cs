using Assets.Scripts.SO;
using Assets.Scripts.Systems.Save;
using Assets.Scripts.Turrets;
using Assets.Scripts.WaveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class TurretInventoryManager : MonoBehaviour
    {
        public static TurretInventoryManager Instance { get; private set; }

        [SerializeField] private TurretUnlockTableSO unlockTable;
        public List<TurretStatsInstance> owned = new();
        public readonly HashSet<TurretType> unlockedTypes = new();
        [SerializeField] private TurretLibrarySO turretLibrary;   // assign the new asset

        public List<TurretStatsInstance> Owned => owned;

        private List<int> pendingEquipped;
        private List<bool> pendingPurchased;

        private readonly Dictionary<TurretStatsInstance, GameObject> instanceToGO = new();

        private List<TurretStatsInstance> pendingRuntimeStats;

        public List<EquippedTurretDTO> EquippedTurrets; 


        public int WaveRequirement(TurretType t) =>
            unlockTable.Entries.First(e => e.Type == t).WaveToUnlock;

        public event Action OnInventoryChanged;   // UI will subscribe

        public bool IsTurretTypeUnlocked(TurretType t) => unlockedTypes.Contains(t);

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            WaveManager.Instance.OnWaveStarted += HandleWaveStarted;
            EnsureStarterTurret();

            if (pendingEquipped != null)
            {
                TurretSlotManager.Instance.ImportEquipped(pendingEquipped);
                pendingEquipped = null;
            }
            if (pendingPurchased != null)
            {
                TurretSlotManager.Instance.ImportPurchasedFlags(pendingPurchased);
                pendingPurchased = null;
            }
            if (pendingRuntimeStats != null)
            {
                TurretSlotManager.Instance.ImportRuntimeStats(pendingRuntimeStats);
                pendingRuntimeStats = null;
            }

        }

        private void EnsureStarterTurret()
        {
            if (owned.Count > 0)
                return;                     // already got one (load)

            TryUnlockByWave(0);

            if (!unlockedTypes.Contains(TurretType.MachineGun))
                return;

            TurretInfoSO baseSO = turretLibrary.GetInfo(TurretType.MachineGun);
            TurretStatsInstance inst = new(baseSO)
            {
                TurretType = TurretType.MachineGun,
                IsUnlocked = true
            };
            owned.Add(inst);
            // Prevent auto-equipping to slot 0 by default
            pendingEquipped = new List<int> { -1, -1, -1, -1, -1 };

        }

        private void OnDestroy()
        {
            if (WaveManager.Instance != null)
                WaveManager.Instance.OnWaveStarted -= HandleWaveStarted;
        }

        private void HandleWaveStarted(object sender, WaveManager.OnWaveStartedEventArgs e)
        {
            TryUnlockByWave(e.WaveNumber);
        }

        public bool TryUnlockByWave(int currentWave)
        {
            bool changed = false;
            foreach (TurretUnlockTableSO.Entry e in unlockTable.Entries
                         .Where(e => currentWave >= e.WaveToUnlock && unlockedTypes
                             .Add(e.Type)))
                changed = true;

            OnInventoryChanged?.Invoke();
            return changed;
        }

        public bool TryPurchase(TurretType type)
        {
            //if (!unlockedTypes.Contains(type)) return false; Used only with the wave requirement

            int countOwned = owned.Count(t => t.TurretType == type);
            if (countOwned >= 5)
                return false;

            ulong cost = GetCost(type, countOwned);

            if (!GameManager.Instance.TrySpendCurrency(Currency.BlackSteel, cost))
                return false;


            // create runtime copy from the original SO
            TurretInfoSO baseSO = turretLibrary.GetInfo(type);
            TurretStatsInstance inst = new(baseSO)
            {
                TurretType = type,
                /* this flag MUST be true otherwise BaseTurret.Start()
               will clone the stats and you�ll lose all later upgrades */
                IsUnlocked = true
            };
            owned.Add(inst);

            OnInventoryChanged?.Invoke();
            SaveGameManager.Instance.SaveGame();

            return true;
        }

        public ulong GetCost(TurretType type, int currentOwned)
        {
            TurretUnlockTableSO.Entry entry = unlockTable.Entries.First(e => e.Type == type);
            return entry.FirstCopyCost * (ulong)Mathf.Pow(2, Mathf.Max(0, currentOwned - 1));
        }

        public GameObject GetPrefab(TurretType t) => turretLibrary.GetPrefab(t);

        public TurretInfoSO GetInfoSO(TurretType type) => turretLibrary.GetInfo(type);

        public (Currency currency, ulong cost) GetCostAndCurrency(TurretType type, int owned)
        {
            Currency currency = Currency.BlackSteel;
            ulong cost = GetCost(type, owned); 

            return (currency, cost);
        }


        // ---------- save / load ----------

        public TurretInventoryDTO ExportToDTO()
        {
            return new TurretInventoryDTO
            {
                Owned = owned,
                EquippedIds = TurretSlotManager.Instance.ExportEquipped(),
                EquippedRuntimeStats = TurretSlotManager.Instance.ExportRuntimeStats(),
                EquippedTurrets = TurretSlotManager.Instance.ExportEquippedTurrets(), 
                UnlockedTypes = unlockedTypes.ToList(),
                SlotPurchased = TurretSlotManager.Instance.GetPurchasedFlags()
            };

        }

        public void ImportFromDTO(TurretInventoryDTO dto)
        {
            owned.Clear();
            owned.AddRange(dto.Owned ?? new List<TurretStatsInstance>());

            unlockedTypes.Clear();
            unlockedTypes.UnionWith(dto.UnlockedTypes ?? new List<TurretType>());

            if (TurretSlotManager.Instance != null)
            {
                //  Skip this entirely if full turrets are present
                if (dto.EquippedTurrets == null || dto.EquippedTurrets.Count == 0)
                    TurretSlotManager.Instance.ImportEquipped(dto.EquippedIds);

                if (dto.EquippedTurrets != null && dto.EquippedTurrets.Count > 0)
                {
                    TurretSlotManager.Instance.ImportEquippedTurrets(dto.EquippedTurrets);
                }
                else
                {
                    TurretSlotManager.Instance.ImportRuntimeStats(dto.EquippedRuntimeStats);
                }

                TurretSlotManager.Instance.ImportPurchasedFlags(dto.SlotPurchased);
            }
        }


        // Used to deactivate and activate object logic to replace destroy and instantiate

        public GameObject GetGameObjectForInstance(TurretStatsInstance stats)
        {
            instanceToGO.TryGetValue(stats, out var go);
            return go;
        }

        public void RegisterTurretInstance(TurretStatsInstance stats, GameObject go)
        {
            if (stats == null || go == null)
                return;

            if (!instanceToGO.ContainsKey(stats))
                instanceToGO.Add(stats, go);
            else
                instanceToGO[stats] = go;
        }


        public void UnregisterTurretInstance(TurretStatsInstance stats)
        {
            if (instanceToGO.ContainsKey(stats))
                instanceToGO.Remove(stats);
        }

        public void ClearUnusedTurrets()
        {
            var equippedStats = TurretSlotManager.Instance.GetEquippedStats()
                .Where(s => s != null)
                .ToHashSet();

            var keysToRemove = instanceToGO
                .Where(pair => !equippedStats.Contains(pair.Key))
                .ToList();

            foreach (var pair in keysToRemove)
            {
                if (pair.Value != null)
                    Destroy(pair.Value);

                instanceToGO.Remove(pair.Key);
            }

            // Extra cleanup for any lingering scene turrets (fallback)
            foreach (var obj in FindObjectsByType<BaseTurret>(FindObjectsSortMode.None))
            {
                if (!instanceToGO.ContainsValue(obj.gameObject))
                    Destroy(obj.gameObject);
            }
        }

    }
}