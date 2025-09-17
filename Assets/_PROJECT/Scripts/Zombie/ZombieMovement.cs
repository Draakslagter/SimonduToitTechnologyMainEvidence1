using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    private Rigidbody _characterRb;
    private Transform _characterTransform;
    private Transform _playerTransform;
    [SerializeField] private float speedMultiplier;

    private void Start()
    {
        _characterRb = GetComponent<Rigidbody>();
        _characterTransform = GetComponent<Transform>();
        _playerTransform = PlayerMovementAndControlSetup.Instance.gameObject.transform;
    }

    private void Update()
    {
        _characterTransform.position = Vector3.MoveTowards(_characterTransform.position, _playerTransform.position, speedMultiplier * Time.fixedDeltaTime);
    }
}
