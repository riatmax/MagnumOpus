using UnityEngine;

public class CursorController : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.OnInputMapChanged += ChangeCursor;
        ChangeCursor(InputManager.Instance.CurrMap);
    }
    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInputMapChanged -= ChangeCursor;
    }
    private void ChangeCursor(InputManager.InputMap map)
    {
        switch(map)
        {
            case InputManager.InputMap.Gameplay:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case InputManager.InputMap.UI:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}
