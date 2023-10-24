using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void OnWASD(Vector2 movement);

    public delegate void OnInteract();

    public delegate void OnSprint();

    public OnWASD onWASD;
    public OnInteract onInteract;
    public OnSprint onSprint;

    public bool isSprinting = false;


    private InputController inputController;
    
    private void Awake()
    {
        inputController = new InputController();
    }

    private void OnEnable()
    {
        inputController.Enable();
    }

    private void OnDisable()
    {
        inputController.Disable();
    }
    
    private void Update()
    {
        if (inputController.Player.Movement.ReadValue<Vector2>() != Vector2.zero && onWASD != null)
            onWASD.Invoke(inputController.Player.Movement.ReadValue<Vector2>());

        if (inputController.Player.Interact.triggered && onInteract != null)
            onInteract.Invoke();

        if (inputController.Player.Sprint.IsPressed() && onSprint != null)
            onSprint.Invoke();

        isSprinting = inputController.Player.Sprint.IsPressed();
    }
}
