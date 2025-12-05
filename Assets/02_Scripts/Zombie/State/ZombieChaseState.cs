using UnityEngine;

public class ZombieChaseState : IZombieState
{
    private ZombieController _zombie;
    public ZombieChaseState(ZombieController zombie)
    {
        _zombie = zombie;
    }

    public void Enter()
    {
        Debug.Log("Enter Chase");

        //Blend Tree -> Run
        _zombie.Animator.SetFloat("Blend", 1f);
    }

    public void Exit()
    {
        
    }    

    public void Update()
    {
        if (_zombie.Player == null)
            return;

        float gap = Vector3.Distance(_zombie.transform.position, _zombie.Player.position);
        //플레이어가 멀어지면 다시 정찰
        if (gap > 10f)
        {
            _zombie.ChangeState(new ZombiePatrolState(_zombie));
            return;
        }

        //공격사거리 진입시
        if (gap <= _zombie.AttackRange)
        {
            _zombie.ChangeState(new ZombieAttackState(_zombie));
            return;
        }

        //추적 이동
        Vector3 direction = (_zombie.Player.position - _zombie.transform.position).normalized;
        _zombie.transform.forward = Vector3.Lerp(_zombie.transform.forward, direction, Time.deltaTime * 10f);
        _zombie.Rigid.MovePosition(
            _zombie.transform.position + direction * _zombie.MoveSpeed * Time.deltaTime);
    }

    public void FixedUpdate()
    {

    }
}
