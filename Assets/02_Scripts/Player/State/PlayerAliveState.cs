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
        _player.TakeDamage(1);
        Debug.Log("HP: " + _player.ModelData._currentHP);
    }

    public void FixedUpdate()
    {
        
    }
}
