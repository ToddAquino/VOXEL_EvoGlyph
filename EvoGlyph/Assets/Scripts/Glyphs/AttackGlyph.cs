using UnityEngine;

public class AttackGlyph : Glyph
{
    [SerializeField] private int damage;

    public override void Activate(Unit user)
    {
        base.Activate(user);
        //if(TurnBaseSystem.Instance.player != null && TurnBaseSystem.Instance.player.target != null)
        //{
        //    TurnBaseSystem.Instance.player.target.HealthComponent.TakeDamage(damage);
        //    Debug.Log($"<color=red> {damage} Damage Dealt to {TurnBaseSystem.Instance.player.target.UnitName}</color>");
        //}
        user.GetTarget().HealthComponent.TakeDamage(damage);
    }
}
