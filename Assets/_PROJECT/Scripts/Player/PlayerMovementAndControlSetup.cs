using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovementAndControlSetup : MonoBehaviour
{
    #region Variables

    private static PlayerMovementAndControlSetup _instance;
    public static PlayerMovementAndControlSetup Instance => _instance;

    [Header("Control")] private CharacterInput _characterInputMap;

    [Header("Viewport Movement")] [SerializeField]
    private CinemachinePanTilt cineCamera;
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactLayer;

    [Header("Movement")] private Rigidbody _characterRb;
    private Vector3 _movementVector;

    [Header("Jump")] [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    [Header("Stats")] [SerializeField] private CharacterStats playerStats;

    [Header("Events")] public UnityEvent triggerPauseMenu;
    public static Action TriggerClearPreInteract;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

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

    #endregion

    #region Movement Controls

    private void FixedUpdate()
    {
        
        var panAngle = cineCamera.PanAxis.Value;
        var panRotation = Quaternion.Euler(0, panAngle, 0);
        var movementDirection = panRotation * _movementVector;
        _characterRb.transform.Translate(movementDirection * (Time.deltaTime * playerStats.MoveSpeedMultiplier), Space.World);
        transform.localRotation = panRotation;
        PreInteract();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        var groundArray = Physics.OverlapSphere(groundCheckTransform.position, groundCheckRadius, groundLayer);
        if (groundArray.Length == 0) return;
        var jumpVector = new Vector3(0, playerStats.JumpHeightMultiplier, 0);
        _characterRb.AddForce(jumpVector, ForceMode.Impulse);
    }

    #endregion

    #region Interaction Controls

    private void PreInteract()
    {
        Physics.Raycast(cineCamera.transform.position, cineCamera.transform.forward, out var hit, interactDistance, interactLayer);
        if (!hit.collider)
        {
            TriggerClearPreInteract.Invoke();
            return;
        }
        var interactableObject = hit.collider.gameObject.GetComponent<IInteractible>();
        interactableObject?.PreInteract();
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Physics.Raycast(cineCamera.transform.position, cineCamera.transform.forward, out var hit, interactDistance, interactLayer);
        if (hit.collider == null) return;
        if (hit.collider.gameObject.GetComponent<IInteractible>() == null) return;
        var interactableObject = hit.collider.gameObject.GetComponent<IInteractible>();
        interactableObject?.Interact();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Physics.Raycast(cineCamera.transform.position, cineCamera.transform.forward, out var hit, interactDistance);
        if (hit.collider == null) return;
        if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
        {
            var damageableObject = hit.collider.gameObject.GetComponent<IDamageable>();
            damageableObject?.TakeDamage(playerStats.Damage);
        }
        if (hit.collider.gameObject.GetComponent<IInteractible>() == null) return;
        var interactableObject = hit.collider.gameObject.GetComponent<IInteractible>();
        interactableObject?.Interact();
    }

    #endregion

    #region MenuControls

    public void OnPause(InputAction.CallbackContext context)
    {
        triggerPauseMenu.Invoke();
    }

    public void OnUINext(InputAction.CallbackContext context)
    {
        Physics.Raycast(cineCamera.transform.position, cineCamera.transform.forward, out var hit, interactDistance, interactLayer);
        if (hit.collider == null) return;
        if (hit.collider.gameObject.GetComponent<IInteractible>() == null) return;
        var interactableObject = hit.collider.gameObject.GetComponent<IInteractible>();
        interactableObject?.UIInteract(+1);
    }

    public void OnUIPrevious(InputAction.CallbackContext context)
    {
        Physics.Raycast(cineCamera.transform.position, cineCamera.transform.forward, out var hit, interactDistance, interactLayer);
        if (hit.collider == null) return;
        if (hit.collider.gameObject.GetComponent<IInteractible>() == null) return;
        var interactableObject = hit.collider.gameObject.GetComponent<IInteractible>();
        interactableObject?.UIInteract(-1);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        Gizmos.DrawRay(cineCamera.transform.position, cineCamera.transform.forward);
    }
}