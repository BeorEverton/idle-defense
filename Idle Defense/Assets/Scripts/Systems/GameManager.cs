using Assets.Scripts.Systems.Audio;
using Assets.Scripts.UI;
using Assets.Scripts.WaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts.Systems
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public PlayerInput Input { get; private set; }

        // Haven't comented this one, because it might be helpful in the future.
        public float spdBonus { get; private set; }

        private const float maxSpdBonus = 100f;
        private const float holdIncreaseRate = 5f;
        private const float initialBoost = 5f;

        private bool isHolding;
        private float decreaseDelay = 1f;
        private float decreaseTimer = 0f;

        public static event Action<float> OnSpdBonusChanged; // Used for the tutorial

        [SerializeField] GraphicRaycaster uiRaycaster;
        PointerEventData _ped;
        List<RaycastResult> _results = new();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _ped = new PointerEventData(EventSystem.current);

        }

        private void OnEnable()
        {
            Input = new PlayerInput();
            Input.Player.Click.performed += OnClickStarted;
            Input.Player.Click.canceled += OnClickReleased;
            Input.Player.Enable();
        }

        private void OnDisable()
        {
            Input.Player.Click.performed -= OnClickStarted;
            Input.Player.Click.canceled -= OnClickReleased;
            Input.Player.Disable();
            Input = null;
        }

        private void OnDestroy()
        {
            if (Input == null)
                return;
            Input.Player.Click.performed -= OnClickStarted;
            Input.Player.Click.canceled -= OnClickReleased;
            Input.Player.Disable();
            Input = null;
        }

        private void OnClickStarted(InputAction.CallbackContext ctx)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();

            _ped.position = screenPos;
            _results.Clear();
            uiRaycaster.Raycast(_ped, _results);

            if (_results.Count > 0) //If any UI was hit, the count is greater than 0
                return;

            spdBonus += initialBoost;
            isHolding = true;
            decreaseTimer = 0f;
            StatsManager.Instance.SpeedBoostClicks++;
            OnSpdBonusChanged?.Invoke(spdBonus);
        }

        private void OnClickReleased(InputAction.CallbackContext ctx)
        {
            isHolding = false;
            decreaseTimer = 0f;
        }

        private void Update()
        {
            if (isHolding)
            {
                spdBonus += holdIncreaseRate * Time.deltaTime;
            }
            else
            {
                if (decreaseTimer >= decreaseDelay)
                {
                    spdBonus -= holdIncreaseRate * 0.8f * Time.deltaTime;
                }
                else
                {
                    decreaseTimer += Time.deltaTime;
                }
            }

            spdBonus = Mathf.Clamp(spdBonus, 0f, maxSpdBonus);

            UIManager.Instance.UpdateSpdBonus(spdBonus);
        }

        public void ResetGame()
        {
            spdBonus = 0;
            UIManager.Instance.UpdateSpdBonus(spdBonus);
        }
    }
}