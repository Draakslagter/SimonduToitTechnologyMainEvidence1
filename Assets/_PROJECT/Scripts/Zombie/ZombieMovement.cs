using UnityEngine;

public class ZombieMovement : MonoBehaviour,IDamageable
{
    [Header("Movement")] 
    private Rigidbody _characterRb;
    private Transform _characterTransform;
    private Vector3 _movementVector;
    
    private Transform _fireTransform;
    [SerializeField] private CharacterStats zombieStats;

    private void Start()
    {
        if (_characterRb == null)
        {
            _characterRb = GetComponent<Rigidbody>();
        }

        if (_characterTransform == null)
        {
            _characterTransform = GetComponent<Transform>();
        }
    }
    public void SetTargets(Transform fireTransform)
    {
        _fireTransform = fireTransform;
        Debug.Log("Set Targets");
    }

    private void FixedUpdate()
    {
        if (!_fireTransform) return;
        _movementVector = (_fireTransform.position - _characterTransform.position).normalized;
        _characterRb.transform.Translate(_movementVector * (Time.deltaTime * zombieStats.MoveSpeedMultiplier));
    }

    public void TakeDamage(float damage)
    {
        zombieStats.Health -= damage;
    }
}
