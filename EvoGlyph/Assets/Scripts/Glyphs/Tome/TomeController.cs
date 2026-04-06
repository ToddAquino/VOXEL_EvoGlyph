using System.Collections.Generic;
using UnityEngine;

public class TomeController : MonoBehaviour
{

    [Header("Page Contents (In Order)")]
    public List<GameObject> pageContentsObj = new List<GameObject>();
    public GameObject hasLeftPageIndicator;
    public GameObject hasRightPageIndicator;
    public int currentPageIndex = 0;
    private void Start()
    {
        UpdatePages();
    }

    public void NextPage()
    {
        // Move forward 2 pages
        if (currentPageIndex < pageContentsObj.Count - 1)
        {
            currentPageIndex++;
            UpdatePages();
        }
        
    }

    public void CheckIndicators()
    {
        //Check Right
        if (currentPageIndex < pageContentsObj.Count - 1)
        {
            hasRightPageIndicator.SetActive(true);
        }
        else
        {
            hasRightPageIndicator.SetActive(false);
        }

        //Check Left
        if (currentPageIndex > 0)
        {
            hasLeftPageIndicator.SetActive(true);
        }
        else
        {
            hasLeftPageIndicator.SetActive(false);
        }
    }
    public void PreviousPage()
    {
        // Move backward 2 pages
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePages();
        }
        
    }

    private void UpdatePages()
    {
        DoClickSound();
        for (int i = 0; i < pageContentsObj.Count; i++)
        {
            pageContentsObj[i].SetActive(i == currentPageIndex);
        }
        CheckIndicators();
    }
    public void DoClickSound()
    {
        AudioManager.Instance.PlaySFX("click", 0.5f);
    }

}
