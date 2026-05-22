using UnityEngine;
using UnityEngine.InputSystem;

public static class Utils
{
    public static Vector2 GetMouseWorldPosition(Camera currentCamera)
    {
        Vector3 v = currentCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return v;
    }
}
