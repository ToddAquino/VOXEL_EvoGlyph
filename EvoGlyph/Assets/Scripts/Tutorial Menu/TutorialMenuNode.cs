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
    [SerializeField] Sprite statusIconInProgress;
    [SerializeField] Sprite statusIconCompleted;
    [Header("Node Variables")]
    [SerializeField] TutorialMenuNodeData nodeData;
    public bool isClickable;

    private void OnEnable()
    {
        RefreshNode();
    }

    public void SetNodeData(TutorialMenuNodeData data)
    {
        nodeData = data;
        RefreshNode();
    }

    public void RefreshNode()
    {
        if (nodeData == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        isClickable = TutorialProgressManager.Instance.IsUnlocked(nodeData);
        if(TutorialProgressManager.Instance.IsCompleted(nodeData))
        {
            statusIcon.sprite = statusIconCompleted;
        }
        else if (TutorialProgressManager.Instance.IsUnlocked(nodeData) && !TutorialProgressManager.Instance.IsCompleted(nodeData))
        {
            statusIcon.sprite = statusIconInProgress;
        }
        else
        {
            statusIcon.sprite = statusIconInProgress;
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
