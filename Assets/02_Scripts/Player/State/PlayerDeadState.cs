using UnityEngine;

public class PlayerDeadState : IPlayerState
{
    PlayerController _player;
    public PlayerDeadState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        Debug.Log("Enter Dead");
        GameManager.Instance.OnPlayerDead();
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
