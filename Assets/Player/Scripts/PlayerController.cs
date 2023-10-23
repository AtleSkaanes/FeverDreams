using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
=======

[RequireComponent(typeof(PlayerInput))]
>>>>>>> 779743416dd13391a7cbe53d53afc193c0c690df
public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerMovemntSpeed = 1f;

    private PlayerInput input;
<<<<<<< HEAD
    private CharacterController characterController;

=======
>>>>>>> 779743416dd13391a7cbe53d53afc193c0c690df

    void Start()
    {
        input = GetComponent<PlayerInput>();
<<<<<<< HEAD
        input.onWASD += MovePlayer;

        characterController = GetComponent<CharacterController>();
=======
        input.onWASD += new PlayerInput.OnWASD(MovePlayer);
>>>>>>> 779743416dd13391a7cbe53d53afc193c0c690df
    }

    void MovePlayer(Vector2 movement)
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
<<<<<<< HEAD
        characterController.SimpleMove(move * playerMovemntSpeed * Time.deltaTime);
=======
        transform.Translate(move * playerMovemntSpeed * Time.deltaTime);
>>>>>>> 779743416dd13391a7cbe53d53afc193c0c690df
    }
}
