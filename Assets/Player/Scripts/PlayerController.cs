using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerMovemntSpeed = 1f;

    private PlayerInput input;
    private CharacterController characterController;


    void Start()
    {
        input = GetComponent<PlayerInput>();
        input.onWASD += MovePlayer;

        characterController = GetComponent<CharacterController>();
    }

    void MovePlayer(Vector2 movement)
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        characterController.SimpleMove(move * playerMovemntSpeed * Time.deltaTime);
    }
}
