using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardInput : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public bool Active {  get; set; } = true;
    public event Action<Vector3, Vector3> OnPointerReleased;

    private Vector3 _clickPosition;

    private void Update()
    {
        if (!Active || _camera == null || Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            _clickPosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector3 releasePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            OnPointerReleased?.Invoke(_clickPosition, releasePosition);
        }
    }
}
