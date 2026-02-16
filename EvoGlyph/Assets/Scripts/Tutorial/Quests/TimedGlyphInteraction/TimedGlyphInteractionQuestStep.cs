using System.Collections;
using UnityEngine;

public class TimedGlyphInteractionQuestStep : QuestStep
{
    [SerializeField] GlyphController controller;
    protected override void EnableStep()
    {
        //controller.OnTimerRanOut.AddListener(OnTimerFinished);
    }

    protected override void DisableStep()
    {
        //controller.OnTimerRanOut.RemoveListener(OnTimerFinished);
    }

    private void OnTimerFinished()
    {
        StartCoroutine(DoTimerFinished());
    }

    IEnumerator DoTimerFinished()
    {
        yield return new WaitForSeconds(3f);
        FinishQuestStep();
    }
}