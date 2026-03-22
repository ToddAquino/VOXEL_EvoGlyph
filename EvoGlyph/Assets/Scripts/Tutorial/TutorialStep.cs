using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialStep
{
    //public DialogueLine dialogueIntro;
    //public DialogueLine dialogueFeedback;
    public UnityEvent StepIntro;
    //public bool hideDialogueOnEnd = true;
    public QuestStep questStep;
}

//[System.Serializable]
//public class DialogueLine
//{
//    public List<DialogueSO> DialgoueText;
//    public UnityEvent OnLineEnd;
//}
