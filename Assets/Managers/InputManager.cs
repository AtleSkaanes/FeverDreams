using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    public delegate void OnWASD(Vector2 movement);

    public delegate void OnInteract();

    public delegate void OnSprint();

    // item
    public delegate void OnUsePrimary(Ray direction);
    public delegate void OnUseSecondary();

    public OnWASD onWASD;
    public OnInteract onInteract;
    public OnSprint onSprint;

    public OnUsePrimary onUsePrimary;
    public OnUseSecondary onUseSecondary;

    public bool isSprinting = false;


    private InputController inputController;

    new private void Awake()
    {
        base.Awake();
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

        if (inputController.Player.UseItemPrimary.triggered && onUsePrimary != null)
            onUsePrimary.Invoke(new Ray(Camera.main.transform.position, Camera.main.transform.forward));

        if (inputController.Player.UseItemSecondary.triggered && onUseSecondary != null)
            onUseSecondary.Invoke();
    }
}
