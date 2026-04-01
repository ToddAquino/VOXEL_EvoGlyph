using System.Collections;
using UnityEngine;

public class SpellBeam : Spell
{
    [Header("Beam Animation")]
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Texture[] textures;
    int animationStep;
    [SerializeField] float fps = 30f;
    float fpsCounter;

    [SerializeField] float extendDuration = 0.6f;

    Vector3 startPosition;
    Vector3 targetPosition;
    private bool beamActive = false;

    public override void Initialize(Unit target)
    {
        spellTarget = target.gameObject;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;

        startPosition = transform.position;
        targetPosition = spellTarget.transform.position;

        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        beamActive = true;
        StartCoroutine(ExtendBeamRoutine());
    }

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
        beamActive = false;
    }
    void Update()
    {
        if (!beamActive || !lineRenderer.enabled) return;

        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f / fps)
        {
            animationStep++;
            if(animationStep == textures.Length)
                animationStep = 0;

            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);
            fpsCounter = 0f;
        }
    }
    void OnDestroy()
    {
        beamActive = false;
        StopAllCoroutines();                 
    }
}
