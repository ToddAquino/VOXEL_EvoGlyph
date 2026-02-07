using System.Collections.Generic;
using UnityEngine;

public class TutorialProgressManager : MonoBehaviour
{
    public static TutorialProgressManager Instance;
    public List<TutorialMenuNodeData> completedTutorialMenuNodeDatas = new List<TutorialMenuNodeData>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public bool IsCompleted(TutorialMenuNodeData tutorialID)
    {
        return completedTutorialMenuNodeDatas.Contains(tutorialID);
    }
    public bool IsUnlocked(TutorialMenuNodeData tutorialID)
    {
        if (tutorialID.PrerequisiteID == null) 
        {
            Debug.Log($"Tutorial: {tutorialID} unlocked status = No Prerequisites");
            return true;
        }
        Debug.Log($"Tutorial: {tutorialID} unlocked status = {tutorialID.PrerequisiteID}");
        return IsCompleted(tutorialID.PrerequisiteID);
    }

    public void MarkCompleted(TutorialMenuNodeData tutorialID)
    {
        if(completedTutorialMenuNodeDatas.Contains(tutorialID)) return;
        completedTutorialMenuNodeDatas.Add(tutorialID);
    }

}
