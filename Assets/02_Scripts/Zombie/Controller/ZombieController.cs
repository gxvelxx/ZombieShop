using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [Header("Zombie Setting")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private int _maxHP = 10;
    [SerializeField] private int _attackDamage = 2;

    private float _rotationSpeed = 6f;

    private int _currentHP;
    private bool isDead = false;

    private bool _hitCoolDown = false;
    private float _hitDelay = 0.2f; // 안그럼 한방에 죽음

    private Rigidbody _rigid;    
    private Transform _player;
    private Animator _animator;

    private IZombieState _currentState;

    [Header("Properties")]
    public float MoveSpeed => _moveSpeed;
    public float AttackRange => _attackRange;
    public int AttackDamage => _attackDamage;
    public Rigidbody Rigid => _rigid;
    public Transform Player => _player;
    public Animator Animator => _animator;
    public bool IsDead => isDead;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        
        _player = GameObject.Find("PlayerBody").transform;

        _currentHP = _maxHP;
    }

    private void OnEnable()
    {
        ChangeState(new ZombiePatrolState(this)); // 기본상태
    }

    private void Update()
    {
        LookTarget();
        _currentState?.Update();
    }

    private void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    /// <summary>
    /// 상태 관리 함수
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IZombieState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    /// <summary>
    /// 시선 회전 처리
    /// </summary>
    private void LookTarget()
    {
        if (_player == null || isDead) return;

        Vector3 direction = _player.position - transform.position;
        direction.y = 0f;  // 기울어짐 방지

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 데미지 처리
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        _currentHP -= damage;

        if (_currentHP <= 0)
        {
            isDead = true;
            ChangeState(new ZombieDieState(this));
        }
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (_hitCoolDown)
            return;

        SwordDamageSender sender = other.GetComponent<SwordDamageSender>();

        if (sender != null)
        {
            StartCoroutine(HitCoolDown());
            int damage = sender.GetDamage();
            TakeDamage(damage); // 데미지
        }
    }

    private IEnumerator HitCoolDown()
    {
        _hitCoolDown = true;
        yield return new WaitForSeconds(_hitDelay);
        _hitCoolDown = false;
    }

    /// <summary>
    /// 공격 처리
    /// </summary>
    public void OnAttackDamage()
    {
        if (isDead)
            return;
        if (_player == null)
            return;

        float gap = Vector3.Distance(transform.position, _player.position);

        if (gap <= _attackRange + 0.1f)
        {
            PlayerController player = _player.GetComponent<PlayerController>();

            if (player != null)
            {
                player.TakeDamage(_attackDamage);
            }
        }
    }
}
