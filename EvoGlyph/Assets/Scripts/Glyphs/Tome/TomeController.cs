using System.Collections.Generic;
using UnityEngine;

public class TomeController : MonoBehaviour
{
    [Header("Page Display References")]
    public SpriteRenderer leftPageRenderer;
    public SpriteRenderer rightPageRenderer;

    [Header("Page Sprites (In Order)")]
    public List<Sprite> pageSprites = new List<Sprite>();
    public int currentLeftPageIndex = 0;
    private void Start()
    {
        UpdatePages();
    }

    public void NextPages()
    {
        // Move forward 2 pages
        if (currentLeftPageIndex + 2 < pageSprites.Count)
        {
            currentLeftPageIndex += 2;
            UpdatePages();
        }
    }

    public void PreviousPages()
    {
        // Move backward 2 pages
        if (currentLeftPageIndex - 2 >= 0)
        {
            currentLeftPageIndex -= 2;
            UpdatePages();
        }
    }

    private void UpdatePages()
    {
        // Left page
        if (currentLeftPageIndex < pageSprites.Count)
            leftPageRenderer.sprite = pageSprites[currentLeftPageIndex];

        // Right page
        if (currentLeftPageIndex + 1 < pageSprites.Count)
        {
            rightPageRenderer.enabled = true;   
            rightPageRenderer.sprite = pageSprites[currentLeftPageIndex + 1];
        }
        else
            rightPageRenderer.enabled = false;
    }
}
