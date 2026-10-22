using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Input Manager Singleton
    public static InputManager Instance { get; private set; }

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
    }

    // Input Map state
    public enum InputMap
    {
        UI,
        Gameplay
    }
    public InputMap CurrMap { get; private set; }

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
        playerActions.Dispose();
        Instance = null;
    }

    // Switch the current Input Map to newMap
    public void SwitchInputMap(InputMap newMap)
    {
        playerActions.Disable();
        CurrMap = newMap;

        switch (CurrMap)
        {
            case InputMap.Gameplay:        
                playerActions.Gameplay.Enable();
                break;
            case InputMap.UI: 
                playerActions.UI.Enable();
                break;
        }
    }
}
