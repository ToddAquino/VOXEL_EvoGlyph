using UnityEngine;

[CreateAssetMenu(fileName = "TutorialMenuNodeData", menuName = "ScriptableObjects/TutorialMenuNodeData")]
public class TutorialMenuNodeData : ScriptableObject
{
    public string Title;
    [TextArea] public string Description;
    public TutorialMenuNodeData PrerequisiteID;
    public TutorialMenuNodeData NextTutorialID;
    public string TutorialSceneToLoad;
}
