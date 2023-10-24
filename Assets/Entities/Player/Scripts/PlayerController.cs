using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
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

    private PlayerInput input;
    private CharacterController characterController;


    void Start()
    {
        input = GetComponent<PlayerInput>();
        input.onWASD += MovePlayer;

        characterController = GetComponent<CharacterController>();
        currentStamina = staminaSec;
    }

    void Update()
    {
        Sprint();
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, virtualCamera.transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void MovePlayer(Vector2 movement)
    {
        DecreaseStamina();
        //Vector3 move = new Vector3(movement.x, 0, movement.y);
        Vector3 move = transform.forward * movement.y + transform.right * movement.x;
        characterController.SimpleMove(move * currentSpeed * Time.deltaTime);
    }

    void Sprint()
    {
        if (input.isSprinting && currentStamina > Mathf.Epsilon)
        {
            currentSpeed = sprintSpeed;
            transform.eulerAngles = new Vector3(7.5f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            currentSpeed = movementSpeed;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
            IncreaseStamina();
        }
    }

    void IncreaseStamina()
    {
        if (input.isSprinting) return;

        currentStamina += Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, staminaSec);
    }

    void DecreaseStamina()
    {
        currentStamina -= Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, staminaSec);
    }
}
