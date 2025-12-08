using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    IPlayerState _currentState;

    public PlayerModelData ModelData;

    public Slider HPSlider;
    public PlayerView View { get; private set; }

    public DamageFlash DamageFlash;

    private void Awake()
    {
        ModelData.Initialize();
        View = new PlayerView(HPSlider);
        View.InitializeHP(ModelData._maxHP);
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
        View.UpdateHP(ModelData._currentHP);

        if (DamageFlash != null)
        {
            DamageFlash.PlayFlash();
        }
        
        if (ModelData.IsDead())
        {
            Debug.Log("Player Dead");
        
            ChangeState(new PlayerDeadState(this));
        }
    }
}
