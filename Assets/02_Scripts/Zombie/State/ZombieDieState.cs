using UnityEngine;

public class ZombieDieState : IZombieState
{
    private ZombieController _zombie;
    public ZombieDieState(ZombieController zombie)
    {
        _zombie = zombie;
    }

    public void Enter()
    {
        Debug.Log("Enter Die");

        //Blend 0으로
        _zombie.Animator.SetFloat("Blend", 0f);

        _zombie.Animator.SetTrigger("Die");
        _zombie.Rigid.isKinematic = true;

        //죽을시 상호작용X
        _zombie.GetComponent<Collider>().enabled = false;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        
    }
}
