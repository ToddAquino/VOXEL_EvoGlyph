using UnityEngine;

public class PlayerUnit : Unit
{
    public PlayerData playerData;

    public override void Initialize()
    {
        //SetMaxHealth
        HealthComponent.SetMaxHealth(playerData.maxHP); 
        base.Initialize();
        Team = Team.Player;

    }

    public void PerformBasicAttack()
    {
        Unit target = GetTarget();
        if (target == null) return;

        int damage = playerData.baseAttack;

        var damageable = target.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage);
        GainMana(1);
    }

    public void GainMana(int amount)
    {
        GameManager.Instance.PlayerData.RefillMana(amount);
    }
}
