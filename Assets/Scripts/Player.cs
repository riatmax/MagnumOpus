using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.OnDodgePressed += Dodge;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnDodgePressed -= Dodge;
    }

    private void Dodge()
    {
        Debug.Log("dodge");
    }
}
