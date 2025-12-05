using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPlayerState _currentState;

    public PlayerModelData ModelData;
    public PlayerView View { get; private set; }

    private void Awake()
    {
        ModelData.Initialize();
        View = new PlayerView();
    }

    private void OnEnable()
    {
        ChangeState(new PlayerAliveState(this));
    }

    private void Update()
    {
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
    public void ChangeState(IPlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    /// <summary>
    /// 데미지 처리
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        ModelData.TakeDamage(damage);
        View.OnDamage();
        
        if (ModelData.IsDead())
        {
            Debug.Log("Player Dead");
        
            ChangeState(new PlayerDeadState(this));
        }
    }
}
