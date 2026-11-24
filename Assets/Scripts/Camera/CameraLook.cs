using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineOrbitalFollow))]
public class CameraLook : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float mouseSmoothing = 20f;
    [SerializeField] private float gamepadSensitivity = 180f;
    [SerializeField] private bool invertY = false;

    private Vector2 smoothedMouse;

    private InputManager im;
    private CinemachineOrbitalFollow cof;

    private void Awake()
    {
        cof = GetComponent<CinemachineOrbitalFollow>();
        im = InputManager.Instance;
    }

    private void Update()
    {
        Vector2 lookInput = im.LookInput;

        if (!im.LookIsGamepad)
        {
            float blend = 1 - Mathf.Exp(-mouseSmoothing * Time.deltaTime);
            smoothedMouse = Vector2.Lerp(smoothedMouse, lookInput, blend);
            lookInput = smoothedMouse;
            lookInput *= mouseSensitivity;
        }
        else
        {
            smoothedMouse = Vector2.zero;
            lookInput *= gamepadSensitivity * Time.deltaTime;
        }

        if (invertY) lookInput.y = -lookInput.y;

        cof.HorizontalAxis.Value += lookInput.x;
        cof.VerticalAxis.Value -= lookInput.y;

        cof.HorizontalAxis.Value = cof.HorizontalAxis.ClampValue(cof.HorizontalAxis.Value);
        cof.VerticalAxis.Value = cof.VerticalAxis.ClampValue(cof.VerticalAxis.Value);
    }
}
