using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.OnGameplayPausePressed += Pause;
        InputManager.Instance.OnUIPausePressed += Unpause;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnGameplayPausePressed -= Pause;
            InputManager.Instance.OnUIPausePressed -= Unpause;
        }
    }

    private void Pause()
    {
        InputManager.Instance.SwitchInputMap(InputManager.InputMap.UI);
    }

    private void Unpause()
    {
        InputManager.Instance.SwitchInputMap(InputManager.InputMap.Gameplay);
    }
}
