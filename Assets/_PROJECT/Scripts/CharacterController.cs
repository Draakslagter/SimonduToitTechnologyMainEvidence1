using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    private CharacterInput characterInputMap;

    private Rigidbody characterRB;
    private Vector3 movementVector;
    [SerializeField] private float speedMultiplier;

    private void Awake()
    {
        characterInputMap = new CharacterInput();

        characterInputMap.Enable();

        characterInputMap.PlayerMap.Jump.performed += OnJump;
        characterInputMap.PlayerMap.Jump.canceled -= OnJump;

        characterInputMap.PlayerMap.Attack.performed += OnAttack;
        characterInputMap.PlayerMap.Attack.canceled -= OnAttack;

        characterInputMap.PlayerMap.Pause.performed += OnPause;
        characterInputMap.PlayerMap.Pause.canceled -= OnPause;

        characterInputMap.PlayerMap.Interact.performed += OnInteract;
        characterInputMap.PlayerMap.Interact.canceled -= OnInteract;

        characterInputMap.PlayerMap.Movement.performed += x => OnPlayerMove(x.ReadValue<Vector2>());
        characterInputMap.PlayerMap.Movement.canceled += x => OnPlayerStopMove(x.ReadValue<Vector2>());

        if (characterRB == null)
        {
            characterRB = GetComponent<Rigidbody>();
        }
    }

    private void OnDisable()
    {
        characterInputMap.PlayerMap.Movement.performed -= x => OnPlayerMove(x.ReadValue<Vector2>());
        characterInputMap.PlayerMap.Movement.canceled -= x => OnPlayerStopMove(x.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
       characterRB.linearVelocity = movementVector * speedMultiplier;
    }

    private void OnPlayerMove(Vector2 incomingVector2)
    {
        movementVector = new Vector3(incomingVector2.x, 0, incomingVector2.y);
    }

    private void OnPlayerStopMove(Vector2 incomingVector2)
    {
        movementVector = Vector3.zero;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("We Interacted");
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("We Paused");
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("We Attacked");
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("We Jumped");
    }

}
