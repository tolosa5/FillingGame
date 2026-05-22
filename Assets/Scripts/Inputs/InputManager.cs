using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    
    public Action<Vector2> OnLeftClickEvent;

    private Camera cameraMain;
    
    [SerializeField] private InputActionReference leftClickReference;
    public InputActionReference LeftClickReference => leftClickReference;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one InputManager in the scene.");
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        leftClickReference.action.Enable();
    }

    private void Start()
    {
        cameraMain = Camera.main;
    }

    private void Update()
    {
        if (leftClickReference.action.WasPressedThisFrame())
        {
            Vector2 position = Utils.GetMouseWorldPosition(cameraMain);
            Debug.Log(position);
            OnLeftClickEvent?.Invoke(position);
        }
    }

    private void OnDisable()
    {
        leftClickReference.action.Disable();
    }
}
