using UnityEngine;

public class PlayerAliveState : IPlayerState
{
    PlayerController _player;
    public PlayerAliveState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        Debug.Log("Enter Alive");
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
