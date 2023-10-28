using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float staminaSec = 10f;
    [Space]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float sprintSpeed = 2f;
    [Space]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private float currentSpeed;
    private float currentStamina;

    private CharacterController characterController;

    private bool isMoving = false;


    void Start()
    {
        InputManager.Instance.onWASD += MovePlayer;

        characterController = GetComponent<CharacterController>();
        currentStamina = staminaSec;
    }

    void Update()
    {
        if (!InputManager.Instance.isSprinting || !isMoving)
            IncreaseStamina();

        // resetting variable for next frame
        isMoving = false;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, virtualCamera.transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void MovePlayer(Vector2 movement)
    {
        isMoving = true;

        if (InputManager.Instance.isSprinting && currentStamina > Mathf.Epsilon)
        {
            DecreaseStamina();
            currentSpeed = sprintSpeed;
        }
        else
            currentSpeed = movementSpeed;

        // Vector3 move = new Vector3(movement.x, 0, movement.y);
        Vector3 move = transform.forward * movement.y + transform.right * movement.x;
        characterController.SimpleMove(move * currentSpeed);

        if (InputManager.Instance.isSprinting)
            GameManager.Instance.OnNoise.Invoke(transform.position);
    }

    void IncreaseStamina()
    {
        currentStamina += Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, staminaSec);
    }

    void DecreaseStamina()
    {
        currentStamina -= Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }
}
