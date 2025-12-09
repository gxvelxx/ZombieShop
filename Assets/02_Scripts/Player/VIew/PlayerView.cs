using UnityEngine.UI;

public class PlayerView
{
    private Slider _HPbar;

    public PlayerView(Slider hpSlider)
    {
        _HPbar = hpSlider;
    }

    public void InitializeHP(int maxHP)
    {
        _HPbar.maxValue = maxHP;
        _HPbar.value = maxHP;
    }

    public void UpdateHP(int currentHP)
    {
        _HPbar.value = currentHP;
    }
}
