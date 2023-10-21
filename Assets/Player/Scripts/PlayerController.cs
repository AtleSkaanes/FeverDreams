using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerMovemntSpeed = 1f;

    private PlayerInput input;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        input.onWASD += new PlayerInput.OnWASD(MovePlayer);
    }

    void MovePlayer(Vector2 movement)
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        transform.Translate(move * playerMovemntSpeed * Time.deltaTime);
    }
}
