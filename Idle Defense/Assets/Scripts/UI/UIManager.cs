﻿using Assets.Scripts.Systems;
using Assets.Scripts.Systems.Save;
using Assets.Scripts.Turrets;
using Assets.Scripts.WaveSystem;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Panels and UI Elements")]
        [SerializeField] private GameObject[] rightPanels;
        [SerializeField] private TextMeshProUGUI _dmgBonus, _spdBonus, _wave, _enemies;
        [SerializeField] private GameObject permanentUpgradePanels;
        [SerializeField] private GameObject temporaryUpgradePanels;

        [Header("Currency UI")]
        [SerializeField] private TextMeshProUGUI _scraps;
        [SerializeField] private TextMeshProUGUI _blackSteelText;
        [SerializeField] private TextMeshProUGUI _crimsonCoreText;

        [Header("Speed click")]
        [SerializeField] private Slider _spdBonusSlider;
        [SerializeField] private Image _decreaseDelayFill;
        private Coroutine _delayFillRoutine;

        [Header("Equip Management")]
        [SerializeField] private GameObject equipPanel;   
        [SerializeField] private GameObject permanentEquipPanel;
        [SerializeField] private TextMeshProUGUI toast;   // 1-line overlay
        public GameObject wallUpgradePanel;

        [Header("Death Screen")]        
        [SerializeField] private GameObject deathCountdownPanel, startGamePanel;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private Button immediateRestartButton;
        private Coroutine deathRoutine;
        private int rollbackWaveIndex;
        public float timeSpeedOnDeath;

        private int activeSlot; // for the equipment

        private int _enemyCount;

        private float timeScaleOnPause;
        public bool gamePaused = false;

        private bool stopOnDeath = false; // used to pause the game on death and resume with standard speed

        private const string ICON_SCRAPS = "⚙";      // U+2699
        private const string ICON_BLACKSTEEL = "§";  // U+00A7
        private const string ICON_CRIMSONCORE = "Ø"; // U+00D8


        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            EnemySpawner.Instance.OnWaveCreated += OnWaveCreated;
            EnemySpawner.Instance.OnEnemyDeath += OnEnemyDeath;
            WaveManager.Instance.OnWaveStarted += OnWaveStarted;
            GameManager.Instance.OnCurrencyChanged += UpdateCurrency;
        }

        private void OnEnemyDeath(object sender, EventArgs _)
        {
            _enemyCount--;
            _enemies.text = $"Enemies\n{_enemyCount}";
        }

        private void OnWaveStarted(object sender, WaveManager.OnWaveStartedEventArgs args)
        {
            _wave.text = $"Zone\n{args.WaveNumber}";
        }

        private void OnWaveCreated(object sender, EnemySpawner.OnWaveCreatedEventArgs args)
        {
            _enemyCount = args.EnemyCount;
            _enemies.text = $"Enemies\n{_enemyCount}";
        }

        public void UpdateSpdBonus(float value)
        {
            _spdBonusSlider.value = value;
            _spdBonus.text = "Spd + " + value.ToString("F0") + "%";

            //UpdateBonusColor(_spdBonus, value);
        }

        public void StartDelayFill(float delayDuration)
        {
            if (_delayFillRoutine != null)
                StopCoroutine(_delayFillRoutine);

            _decreaseDelayFill.fillAmount = 1f;
            _decreaseDelayFill.gameObject.SetActive(true);

            _delayFillRoutine = StartCoroutine(DelayFillRoutine(delayDuration));
        }

        private IEnumerator DelayFillRoutine(float duration)
        {
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                _decreaseDelayFill.fillAmount = 1f - (t / duration);
                yield return null;
            }

            _decreaseDelayFill.fillAmount = 0f;
            _decreaseDelayFill.gameObject.SetActive(false);
            _delayFillRoutine = null;
        }

        public void UpdateDmgBonus(float value)
        {
            _dmgBonus.text = "Dmg +\n" + value.ToString("F0") + "%";
            UpdateBonusColor(_dmgBonus, value);
        }

        /// <summary>
        /// Colors the bonus text based on the value
        /// </summary>
        public void UpdateBonusColor(TextMeshProUGUI element, float value)
        {
            float t = Mathf.Clamp01(value / 200f);
            element.color = Color.Lerp(Color.black, Color.red, t);
        }

        public void UpdateCurrency(Currency type, ulong value)
        {
            switch (type)
            {
                case Currency.Scraps:
                    _scraps.SetText(FormatCurrency(ICON_SCRAPS, value));
                    break;
                case Currency.BlackSteel:
                    _blackSteelText.SetText(FormatCurrency(ICON_BLACKSTEEL, value));
                    break;
                case Currency.CrimsonCore:
                    _crimsonCoreText.SetText(FormatCurrency(ICON_CRIMSONCORE, value));
                    break;
            }
        }

        private string FormatCurrency(string icon, ulong value)
        {
            return icon + " " + AbbreviateNumber(value);
        }

        public static string GetCurrencyIcon(Currency type)
        {
            return type switch
            {
                Currency.Scraps => ICON_SCRAPS,
                Currency.BlackSteel => ICON_BLACKSTEEL,
                Currency.CrimsonCore => ICON_CRIMSONCORE,
                _ => string.Empty
            };
        }

        public static string AbbreviateNumber(double number, bool showPercent = false, bool showTwoDecimals = false)
        {
            const double Thousand = 1E3;
            const double Million = 1E6;
            const double Billion = 1E9;
            const double Trillion = 1E12;
            const double Quadrillion = 1E15;
            const double Quintillion = 1E18;
            const double Sextillion = 1E21;
            const double Septillion = 1E24;
            const double Octillion = 1E27;
            const double Nonillion = 1E30;
            const double Decillion = 1E33;
            const double Undecillion = 1E36;
            const double Duodecillion = 1E39;

            string format = showTwoDecimals ? "0.##" : "0.#";

            return number switch
            {
                >= Duodecillion => (number / Duodecillion).ToString(format) + "D",
                >= Undecillion => (number / Undecillion).ToString(format) + "U",
                >= Decillion => (number / Decillion).ToString(format) + "d",
                >= Nonillion => (number / Nonillion).ToString(format) + "N",
                >= Octillion => (number / Octillion).ToString(format) + "O",
                >= Septillion => (number / Septillion).ToString(format) + "S",
                >= Sextillion => (number / Sextillion).ToString(format) + "s",
                >= Quintillion => (number / Quintillion).ToString(format) + "Q",
                >= Quadrillion => (number / Quadrillion).ToString(format) + "q",
                >= Trillion => (number / Trillion).ToString(format) + "T",
                >= Billion => (number / Billion).ToString(format) + "B",
                >= Million => (number / Million).ToString(format) + "M",
                >= Thousand => (number / Thousand).ToString(format) + "K",
                _ => number.ToString(showPercent ? "F2" : "F0")
            };
        }


        public static string FormatTime(TimeSpan time)
        {
            if (time.TotalSeconds < 60)
            {
                return $"{(int)time.TotalSeconds} sec";
            }
            else if (time.TotalMinutes < 60)
            {
                return $"{time.Minutes:D2}:{time.Seconds:D2}";
            }
            else if (time.TotalHours < 24)
            {
                return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
            }
            else
            {
                return $"{(int)time.TotalDays}d:{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
            }
        }

        public void OpenEquipPanel(int slot)
        {
            DeactivateRightPanels();
            activeSlot = slot;

            bool isManagement = GameManager.Instance.CurrentGameState == GameState.Management;
            GameObject panel = isManagement ? permanentEquipPanel : equipPanel;

            panel.SetActive(true);
            panel.GetComponent<EquipPanelUI>().Open(slot);
        }


        public void ShowToast(string msg, float time = 1.5f)
        {
            if (toast == null)
            { Debug.Log(msg); return; }  // no text object assigned
            StopAllCoroutines();                            // stop any previous toast
            StartCoroutine(ToastRoutine(msg, time));
        }

        private IEnumerator ToastRoutine(string m, float t)
        {
            toast.text = m;                // set message
            toast.gameObject.SetActive(true);
            yield return new WaitForSeconds(t);
            toast.gameObject.SetActive(false); // hide after delay
        }

        public void DeactivateRightPanels()
        {
            foreach (GameObject panel in rightPanels)
            {
                panel.SetActive(false);
            }
        }

        #region Death Methods
        public void ShowDeathCountdown(float seconds = 5f)
        {
            if (deathRoutine != null)
                StopCoroutine(deathRoutine);

            timeSpeedOnDeath = Time.timeScale; // Store current time scale
            Time.timeScale = 0f; // Pause the game

            deathCountdownPanel.SetActive(true);

            // Calculate and store rollback wave
            int currentWave = WaveManager.Instance.GetCurrentWaveIndex();
            rollbackWaveIndex = Mathf.Max(1, currentWave - 2); // clamp to wave 1

            countdownText.text = $"Restarting from Zone {rollbackWaveIndex} in {Mathf.CeilToInt(seconds)}...";
            immediateRestartButton.onClick.RemoveAllListeners();
            immediateRestartButton.onClick.AddListener(SkipDeathCountdown);

            deathRoutine = StartCoroutine(DeathCountdownRoutine(seconds));
        }

        private IEnumerator DeathCountdownRoutine(float seconds)
        {
            float timeLeft = seconds;

            while (timeLeft > 0f)
            {
                countdownText.text = $"Restarting from Zone {rollbackWaveIndex} in {Mathf.CeilToInt(timeLeft)}...";
                yield return new WaitForSecondsRealtime(1f);
                timeLeft -= 1f;
            }

            countdownText.text = $"Restarting now...";
            yield return new WaitForSecondsRealtime(0.5f); // optional buffer
            TriggerGameReset();
        }

        public void SkipDeathCountdown()
        {
            if (deathRoutine != null)
                StopCoroutine(deathRoutine);
            TriggerGameReset();
        }

        private void TriggerGameReset()
        {
            deathCountdownPanel.SetActive(false);
            WaveManager.Instance.LoadWave(rollbackWaveIndex);
            WaveManager.Instance.ForceRestartWave();
            PlayerBaseManager.Instance.InitializeGame(true);
        }

        public void RestartCurrentWave()
        {
            if (deathRoutine != null)
                StopCoroutine(deathRoutine);

            deathCountdownPanel.SetActive(false);

            // Set to current wave minus one, since LoadWave will increment to it
            WaveManager.Instance.LoadWave(rollbackWaveIndex);
            WaveManager.Instance.ForceRestartWave();
            PlayerBaseManager.Instance.InitializeGame(true);
        }

        public void StopOnDeath()
        {
            GameSpeedManager.Instance.ResetGameSpeed(); // Reset game speed to default
            Time.timeScale = 0f; // Pause the game
            stopOnDeath = true; // Set flag to pause on death
            startGamePanel.SetActive(true);
            rollbackWaveIndex = 1; // Reset rollback wave index
            GameManager.Instance.ChangeGameState(GameState.Management); // Change game state to GameOver
        }

        public void ClickStart()
        {
            startGamePanel.SetActive(false);
            WaveManager.Instance.LoadWave(1); // Load wave 1
            WaveManager.Instance.ForceRestartWave();
            PlayerBaseManager.Instance.InitializeGame(true, true); // Reset player base stats
            if (!stopOnDeath)
                Time.timeScale = timeSpeedOnDeath; // Resume game at previous speed
            else
            {
                Time.timeScale = 1f; // Resume game at normal speed if stopOnDeath is true
            }
            GameManager.Instance.ChangeGameState(GameState.InGame); // Change game state to regular
        }
        #endregion

        public void PauseGame(bool pause)
        {
            if (pause)
            {
                // Save game state or perform any necessary actions on pause
                gamePaused = true;
                SaveGameManager.Instance.SaveGame();
                if (Time.timeScale == 0f)
                    return; // Already paused
                timeScaleOnPause = Time.timeScale; // Store current time scale
                Time.timeScale = 0f; // Pause the game                
            }
            else
            {
                Time.timeScale = timeScaleOnPause; // Resume the game
                gamePaused = false;
            }
        }

        public void ToggleUpgradePanels(GameState state)
        {
            bool isManagement = (state == GameState.Management);

            permanentUpgradePanels.SetActive(isManagement);
            temporaryUpgradePanels.SetActive(!isManagement);

        }


    }
}