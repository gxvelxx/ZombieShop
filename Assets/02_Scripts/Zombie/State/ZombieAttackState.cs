using UnityEngine;

public class ZombieAttackState : IZombieState
{
    private ZombieController _zombie;
    private float _attackCoolTime = 2f;
    private float _timer = 0f;
    public ZombieAttackState(ZombieController zombie)
    {
        _zombie = zombie;
    }
    public void Enter()
    {
        Debug.Log("Enter Attack");
        _timer = 0f;

        //공격 중에는 Blend를 0으로
        _zombie.Animator.SetFloat("Blend", 0f);

        _zombie.Animator.SetTrigger("Attack");
    }

    public void Exit()
    {
        
    }    

    public void Update()
    {
        if (_zombie.Player == null)
            return;

        float gap = Vector3.Distance(_zombie.transform.position, _zombie.Player.position);
        
        //공격사거리에서 벗어나면
        if (gap > _zombie.AttackRange + 0.1f) // 여유값 추가
        {
            _zombie.ChangeState(new ZombieChaseState(_zombie));
            return;
        }

        _timer += Time.deltaTime;
        
        //공격
        if (_timer >= _attackCoolTime)
        {
            _timer = 0f;
            _zombie.Animator.SetTrigger("Attack");
        }

    }

    public void FixedUpdate()
    {
        
    }
}
