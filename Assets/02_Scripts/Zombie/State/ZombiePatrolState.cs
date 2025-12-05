using UnityEngine;

public class ZombiePatrolState : IZombieState
{
    private ZombieController _zombie;
    private Vector3 _patrolPosition; // 정찰할 위치
    public ZombiePatrolState(ZombieController zombie)
    {
        _zombie = zombie;
    }

    public void Enter()
    {
        Debug.Log("Enter Patrol");

        //Blend Tree -> Walk
        _zombie.Animator.SetFloat("Blend", 0.5f);

        //주위 랜덤
        _patrolPosition = _zombie.transform.position +
            new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));        
    }

    public void Exit()
    {
        
    }    

    public void Update()
    {
        if (_zombie.Player == null)
            return;

        //빌견하면 추적상태로
        float gap = Vector3.Distance(_zombie.transform.position, _zombie.Player.position);
        if (gap < 7f)
        {
            _zombie.ChangeState(new ZombieChaseState(_zombie));
            return;
        }

        //정찰 이동
        Vector3 direction = (_patrolPosition - _zombie.transform.position).normalized;

        direction.y = 0;    // Y값 없애야 경사면 수월
        direction = direction.normalized;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion patrolRotation = Quaternion.LookRotation(direction);
            _zombie.transform.rotation = Quaternion.Slerp(
           _zombie.transform.rotation, patrolRotation, Time.deltaTime * 6f); // 회전 속도
        }

        _zombie.Rigid.MovePosition(
            _zombie.transform.position + direction * _zombie.MoveSpeed * Time.deltaTime);

        //다시 정찰
        if (Vector3.Distance(_zombie.transform.position, _patrolPosition) < 1f)
        {
            _zombie.ChangeState(new ZombiePatrolState(_zombie));
        }
    }

    public void FixedUpdate()
    {

    }
}
