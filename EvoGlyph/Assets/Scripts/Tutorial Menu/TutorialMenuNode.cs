using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialMenuNode : MonoBehaviour
{
    [Header("Node UI")]
    [SerializeField] TextMeshProUGUI tutorialUINameText;
    [SerializeField] TextMeshProUGUI tutorialUIDescriptionText;
    [SerializeField] Image statusIcon;
    [Header("Node Variables")]
    [SerializeField] TutorialMenuNodeData nodeData;
    public bool isClickable;

    private void OnEnable()
    {
        RefreshNode();
    }

    void RefreshNode()
    {
        isClickable = TutorialProgressManager.Instance.IsUnlocked(nodeData);
        if(TutorialProgressManager.Instance.IsCompleted(nodeData))
        {
            statusIcon.color = Color.aquamarine;
        }
        else if (TutorialProgressManager.Instance.IsUnlocked(nodeData) && !TutorialProgressManager.Instance.IsCompleted(nodeData))
        {
            statusIcon.color = Color.blue;
        }
        else
        {
            statusIcon.color = Color.gray;
        }
        tutorialUINameText.text = nodeData.Title;
        tutorialUIDescriptionText.text = nodeData.Description;
    }

    public void OnClick()
    {
        if(isClickable)
        {
            GameSceneManager.Instance.LoadScene(nodeData.TutorialSceneToLoad);
        }
    }
}
