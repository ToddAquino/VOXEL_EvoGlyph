using System.Collections;
using TMPro;
using UnityEngine;

public class SpellBlast : Spell
{
    [SerializeField] float extendDuration = 0.6f;
    Vector3 startPosition;
    Vector3 targetPosition;

    //Vector3 hiddenProjectile;
    bool blastActive = false;

    [Header("Beam")]
    [SerializeField] LineRenderer lineRenderer;


    public override void Initialize(Unit target)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (target.transform.position.x < this.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        spellTarget = target.gameObject;

        //hiddenProjectile = transform.position;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;

        startPosition = transform.position;
        targetPosition = spellTarget.transform.position;

        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        blastActive = true;
        //StartCoroutine(RunBlastProjectile());
        StartCoroutine(ExtendBeamRoutine());
    }

    //private IEnumerator RunBlastProjectile()
    //{
    //    float elapsed = 0f;

    //    while (elapsed < extendDuration)
    //    {
    //        elapsed += Time.deltaTime;
    //        float t = elapsed / extendDuration;

    //        Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);
    //        Vector3 hiddenProjectile = currentPos;

    //        yield return null;
    //    }
    //    hiddenProjectile = targetPosition;
    //    TriggerHit(spellTarget);
    //    yield return new WaitForSeconds(0.4f);
    //    blastActive = false;
    //}

    private IEnumerator ExtendBeamRoutine()
    {
        float elapsed = 0f;

        while (elapsed < extendDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / extendDuration;

            Vector3 currentEnd = Vector3.Lerp(startPosition, targetPosition, t);
            lineRenderer.SetPosition(1, currentEnd);

            yield return null;
        }
        lineRenderer.SetPosition(1, targetPosition);
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
