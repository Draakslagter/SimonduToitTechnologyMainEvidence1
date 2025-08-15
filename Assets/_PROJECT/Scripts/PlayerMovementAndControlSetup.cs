using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovementAndControlSetup : MonoBehaviour
{
   [Header ("Control")]
    private CharacterInput _characterInputMap;
    
    [Header ("Viewport Movement")]
    [SerializeField] private CinemachinePanTilt cineCamera;
    
    [Header ("Movement")]
    private Rigidbody _characterRb;
    private Vector3 _movementVector;
    [SerializeField] private float speedMultiplier, jumpMultiplier;
    
    [Header ("Jump")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    
    public UnityEvent onPause;

    private void Awake()
    {
        _characterInputMap = new CharacterInput();
        
        if (_characterRb == null)
        {
            _characterRb = GetComponent<Rigidbody>();
        }
    }

    private void OnEnable()
    {
        _characterInputMap.Enable();
    }
    private void OnDisable()
    {
        _characterInputMap.Disable();
    }

    private void FixedUpdate()
    {
        var panAngle = cineCamera.PanAxis.Value;
        var panRotation = Quaternion.Euler(0, panAngle, 0);
        var movementDirection = panRotation * _movementVector;
       _characterRb.transform.Translate(movementDirection * (Time.deltaTime * speedMultiplier), Space.World);
       transform.localRotation = panRotation;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("We Interacted");
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
        onPause.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("We Attacked");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        var groundArray = Physics.OverlapSphere(groundCheckTransform.position, groundCheckRadius, groundLayer);
        Debug.Log(groundArray.Length);
        if (groundArray.Length == 0) return;
        var jumpVector = new Vector3(0, jumpMultiplier, 0);
        _characterRb.AddForce(jumpVector, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        Gizmos.DrawRay(cineCamera.transform.position, cineCamera.transform.forward);
    }
}