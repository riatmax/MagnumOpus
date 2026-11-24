using UnityEngine;
using UnityEngine.InputSystem;
using System;

[DefaultExecutionOrder(-100)]
public class InputManager : MonoBehaviour
{
    // Input Manager Singleton
    public static InputManager Instance { get; private set; }

    // Input Map state
    public enum InputMap
    {
        Gameplay,
        UI
    }
    public InputMap CurrMap { get; private set; }
    [SerializeField] private InputMap defaultMap = InputMap.Gameplay;

    // Stick/WASD/Mouse input values, read these in other scripts
    public Vector2 PlayerMoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Vector2 UIMoveInput { get; private set; }

    // Sprinting?
    public bool SprintHeld { get; private set; }

    // Using a controller?
    public bool LookIsGamepad { get; private set; }

    // Events for Gameplay binds
    public event Action OnDodgePressed;
    public event Action OnLockOnPressed;
    public event Action OnLightAttackPressed;
    public event Action OnHeavyAttackPressed;
    public event Action OnInteractPressed;
    public event Action OnGameplayPausePressed;

    //Events for UI binds
    public event Action OnUIPausePressed;

    // Map switching event
    public event Action<InputMap> OnInputMapChanged;

    // Player Input Actions
    private PlayerActions playerActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playerActions = new PlayerActions();

        CurrMap = defaultMap;

        playerActions.Gameplay.Dodge.performed += HandleDodge;
        playerActions.Gameplay.LockOn.performed += HandleLockOn;
        playerActions.Gameplay.LightAttack.performed += HandleLightAttack;
        playerActions.Gameplay.HeavyAttack.performed += HandleHeavyAttack;
        playerActions.Gameplay.Interact.performed += HandleInteract;
        playerActions.Gameplay.Pause.performed += HandleGameplayPause;

        playerActions.UI.Pause.performed += HandleUIPause;
    }

    private void Update()
    {
        switch (CurrMap)
        {
            case InputMap.Gameplay:
                PlayerMoveInput = playerActions.Gameplay.Move.ReadValue<Vector2>();
                LookInput = playerActions.Gameplay.Look.ReadValue<Vector2>();
                SprintHeld = playerActions.Gameplay.Sprint.IsPressed();
                
                if (playerActions.Gameplay.Look.activeControl != null)
                {
                    LookIsGamepad = playerActions.Gameplay.Look.activeControl.device is Gamepad;
                }
                break;
            case InputMap.UI:
                UIMoveInput = playerActions.UI.Move.ReadValue<Vector2>();
                break;
        }
    }

    // Input cleanup methods:
    private void OnEnable()
    {
        if (Instance != this) return;
        SwitchInputMap(CurrMap);
    }
    private void OnDisable()
    {
        if (Instance != this) return;
        playerActions.Disable();
    }
    private void OnDestroy()
    {
        if (Instance != this) return;

        playerActions.Gameplay.Dodge.performed -= HandleDodge;
        playerActions.Gameplay.LockOn.performed -= HandleLockOn;
        playerActions.Gameplay.LightAttack.performed -= HandleLightAttack;
        playerActions.Gameplay.HeavyAttack.performed -= HandleHeavyAttack;
        playerActions.Gameplay.Interact.performed -= HandleInteract;
        playerActions.Gameplay.Pause.performed -= HandleGameplayPause;

        playerActions.UI.Pause.performed -= HandleUIPause;

        playerActions.Dispose();
        Instance = null;
    }

    // Switch the current Input Map to newMap
    public void SwitchInputMap(InputMap newMap)
    {
        bool alreadyActive = false;
        switch (newMap)
        {
            case InputMap.Gameplay:
                alreadyActive = playerActions.Gameplay.enabled;
                break;
            case InputMap.UI:
                alreadyActive = playerActions.UI.enabled;
                break;
        }

        if (alreadyActive) return;

        playerActions.Disable();
        CurrMap = newMap;

        switch (CurrMap)
        {
            case InputMap.Gameplay:        
                playerActions.Gameplay.Enable();
                UIMoveInput = Vector2.zero;
                break;
            case InputMap.UI: 
                playerActions.UI.Enable();
                PlayerMoveInput = Vector2.zero;
                LookInput = Vector2.zero;
                SprintHeld = false;
                break;
        }

        OnInputMapChanged?.Invoke(CurrMap);
    }

    // Action Handlers
    private void HandleDodge(InputAction.CallbackContext ctx) => OnDodgePressed?.Invoke();
    private void HandleLockOn(InputAction.CallbackContext ctx) => OnLockOnPressed?.Invoke();
    private void HandleLightAttack(InputAction.CallbackContext ctx) => OnLightAttackPressed?.Invoke();
    private void HandleHeavyAttack(InputAction.CallbackContext ctx) => OnHeavyAttackPressed?.Invoke();
    private void HandleInteract(InputAction.CallbackContext ctx) => OnInteractPressed?.Invoke();
    private void HandleGameplayPause(InputAction.CallbackContext ctx) => OnGameplayPausePressed?.Invoke();
    private void HandleUIPause(InputAction.CallbackContext ctx) => OnUIPausePressed?.Invoke();
}
