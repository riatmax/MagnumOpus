using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerActions playerActions;
    
    // Input Map state
    public enum InputMap
    {
        Gameplay,
        UI
    }
    public InputMap CurrMap { get; private set; }

    private void Awake()
    {
        playerActions = new PlayerActions();
    }

    // Input cleanup methods:
    private void OnEnable() => SwitchInputMap(CurrMap);
    private void OnDisable() => playerActions.Disable();
    private void OnDestroy() => playerActions.Dispose();

    public void SwitchInputMap(InputMap newMap)
    {
        playerActions.Disable();
        CurrMap = newMap;

        switch (CurrMap)
        {
            case InputMap.Gameplay:        
                playerActions.Gameplay.Enable();
                break;
            case InputMap.UI: // Implement this when we have the map ready
                // playerActions.UI.Enable();
                break;
        }
    }
}
