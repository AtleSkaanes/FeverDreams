using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void OnWASD(Vector2 movement);

    public delegate void OnInteract();

    public OnWASD onWASD;
    public OnInteract onInteract;


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
        if (inputController.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
            onWASD(inputController.Player.Movement.ReadValue<Vector2>());

        if (inputController.Player.Interact.triggered)
            onInteract();
    }
}
