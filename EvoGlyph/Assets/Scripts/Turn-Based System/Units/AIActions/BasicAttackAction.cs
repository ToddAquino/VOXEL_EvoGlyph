using UnityEngine;

public class BasicAttackAction : AIAction
{
    [SerializeField] float damage;
    float newDamage;
    Unit caster;
    Unit target;
    bool isInterrupted;
    public override void DoAction(Unit user)
    {
        base.DoAction(user);
        caster = user;
        target = caster.GetTarget();
        isInterrupted = false;
        user.animator.SetTrigger("OnAttack");
        ApplyQTEResult();
    }

    public void ReleaseAttack()
    {
        if (target == null || caster == null || isInterrupted) return;

        //Move to current attack target position
        this.transform.position = target.transform.position;

        var damageable = target.GetComponent<IDamageable>();
        float multiplier = 1f;
        ElementType attackingElement = ElementType.None;
        EnemyUnit targetUnit = target.GetComponent<EnemyUnit>();
        if (targetUnit != null)
        {
            ElementType defendingElement = targetUnit.enemyUnitData.Element.Type;
            float elementalMultiplier = GameManager.Instance.ElementHandler.GetEffectiveness(attackingElement, defendingElement);
            multiplier *= elementalMultiplier;
        }
        // calculate total damage with QTE Result
        float baseDamage = damage;
        if (newDamage > 0)
        {
            baseDamage = newDamage;
        }
        int finalDamage = Mathf.RoundToInt(baseDamage * multiplier);

        damageable?.TakeDamage(finalDamage);
    }
    private void ApplyQTEResult()
    {
        switch (qteResult)
        {
            case QuickTimeEventResult.Success:
                OnInterrupted();
                break;

            case QuickTimeEventResult.Perfect:
                newDamage = damage * 0.25f;
                OnSpellDeflected();
                break;

            default:
                newDamage = damage;
                break;
        }
    }

    void OnInterrupted()
    {
        isInterrupted = true;
        HandleActionResolved();
    }

    void OnSpellDeflected()
    {
        target = caster;
    }
}
