using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialMenuController : MonoBehaviour
{
    [SerializeField] List<TutorialMenuNodeData> tutorialDatas;
    [SerializeField] List<TutorialMenuNode> tutorialMenus;

    int currentLeftPageIndex = 0;
    void Start()
    {
        UpdatePages();
    }

    private void UpdatePages()
    {
        for (int i = 0; i < tutorialMenus.Count; i++)
        {
            int dataIndex = currentLeftPageIndex + i;
            if (dataIndex < tutorialDatas.Count)
            {
                tutorialMenus[i].SetNodeData(tutorialDatas[dataIndex]);
            }
            else
            {
                tutorialMenus[i].SetNodeData(null);
            }
        }
    }

    public void NextPages()
    {
        // Move forward by menu slots count
        int slotCount = tutorialMenus.Count;
        if (currentLeftPageIndex + slotCount < tutorialDatas.Count)
        {
            currentLeftPageIndex += slotCount;
            UpdatePages();
        }
    }

    public void PreviousPages()
    {
        // Move backward by menu slots count
        int slotCount = tutorialMenus.Count;
        if (currentLeftPageIndex - slotCount >= 0)
        {
            currentLeftPageIndex -= slotCount;
            UpdatePages();
        }
    }
}
