using System.Collections;
using TMPro;
using UnityEngine;

public class SpellBlast : Spell
{
    [SerializeField] float extendDuration = 0.6f;
    Vector3 startPosition;
    Vector3 targetPosition;

    Vector3 hiddenProjectile;
    bool blastActive = false;

    public override void Initialize(Unit target)
    {
        spellTarget = target.gameObject;

        hiddenProjectile = transform.position;
        startPosition = transform.position;
        targetPosition = spellTarget.transform.position;

        blastActive = true;
        StartCoroutine(RunBlastProjectile());
    }

    private IEnumerator RunBlastProjectile()
    {
        float elapsed = 0f;

        while (elapsed < extendDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / extendDuration;

            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);
            Vector3 hiddenProjectile = currentPos;

            yield return null;
        }
        hiddenProjectile = targetPosition;
        TriggerHit(spellTarget);
        yield return new WaitForSeconds(0.4f);
        blastActive = false;
    }

    void OnDestroy()
    {
        blastActive = false;
        StopAllCoroutines();
    }
}
