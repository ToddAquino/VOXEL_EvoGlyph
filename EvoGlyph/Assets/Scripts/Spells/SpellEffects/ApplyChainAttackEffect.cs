using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ApplyChainAttackEffect : SpellEffect
{
    public int DamageAmount;
    public int maxChains = 3;
    public float chainRadius = 6f;
    public List<GameObject> hitTargets = new List<GameObject>();
    public LayerMask targetLayer;
    public override void Apply(GameObject target, SpellController controller)
    {
        hitTargets.Clear();
        ChainFrom(target, maxChains);
        EffectSuccessfullyApplied();
    }
    private void ChainFrom(GameObject currentTarget, int chainsRemaining)
    {
        if (currentTarget == null || chainsRemaining <= 0)
            return;
        if (currentTarget.CompareTag("Player"))
            return;
        hitTargets.Add(currentTarget);

        var damageable = currentTarget.GetComponent<IDamageable>();
        damageable?.TakeDamage(DamageAmount);
        Debug.Log("Chain hit: " + currentTarget.name + ", HealthComponent: " + currentTarget.GetComponent<HealthComponent>());
        Collider2D[] nearbyTargets = Physics2D.OverlapCircleAll(currentTarget.transform.position,chainRadius, targetLayer);

        GameObject nextTarget = null;
        float closestDistance = chainRadius;

        foreach (var col in nearbyTargets)
        {
            if (hitTargets.Contains(col.gameObject))
                continue;

            float dist = Vector2.Distance(currentTarget.transform.position,col.transform.position);

            if (dist <= closestDistance)
            {
                closestDistance = dist;
                nextTarget = col.gameObject;
            }
        }

        if (nextTarget != null)
        {
            ChainFrom(nextTarget, chainsRemaining - 1);
        }
    }
}
