using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //TutorialManager tutorialManager;
    //[SerializeField] Tutorial[] tutorialQuests;
    //[SerializeField] int tutorialQuestIndex;

    public ElementalSynergyDatabase ElementalSynergyDatabase;
    public GlyphDatabase GlyphDatabase;
    public PlayerGlyphs PlayerGlyphs;
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

    void Start()
    {
        //tutorialManager = TutorialManager.Instance;
        //tutorialQuestIndex = 0;
        //GlyphBoard.Instance.GenerateField();
        //StartTutorial();
        //tutorialManager.StartQuest();
    }


    //void StartTutorial()
    //{
    //    tutorialManager.SetActiveQuest(tutorialQuests[tutorialQuestIndex]);       
    //}

    //public void StartNextTutorial()
    //{
    //    tutorialQuestIndex++;
    //    if (tutorialQuestIndex >= tutorialQuests.Length) return;
    //    tutorialManager.SetActiveQuest(tutorialQuests[tutorialQuestIndex]);
    //}
}
