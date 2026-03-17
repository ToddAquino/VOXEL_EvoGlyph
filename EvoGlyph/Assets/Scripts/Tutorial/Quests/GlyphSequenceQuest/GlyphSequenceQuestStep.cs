using System.Collections.Generic;
using UnityEngine;

public class GlyphSequenceQuestStep : QuestStep
{
    public GlyphSequencer GlyphSequencer;
    public Glyph requiredGlyph;
    protected override void DisableStep()
    {
        GlyphSequencer.OnEndSequence -= SequenceCreated;
    }

    protected override void EnableStep()
    {
        GlyphSequencer.OnEndSequence += SequenceCreated;
    }

    private void SequenceCreated(List<Glyph> sequence)
    {
        if (sequence == null || sequence.Count == 0)
        {
            QuestFailed();
            return;
        }

        if (requiredGlyph == null)
        {
            FinishQuestStep();
            return;
        }
        bool containsRequired = sequence.Exists(g =>
        g != null && g.pattern == requiredGlyph.pattern);

        if (containsRequired)
            FinishQuestStep();
        else
            QuestFailed();
    }
    private void QuestFailed()
    {
        FailedQuestStep();
    }
}
