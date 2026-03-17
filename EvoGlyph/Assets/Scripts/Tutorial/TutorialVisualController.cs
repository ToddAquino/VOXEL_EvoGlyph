using UnityEngine;

public class TutorialVisualController : MonoBehaviour
{
    [SerializeField] bool isActiveOnStart;
    [SerializeField] bool isShowOnce;
    [SerializeField] GameObject tutorialVisual;

    bool isShown = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isShowOnce && isShown) return;
            tutorialVisual.SetActive(true);
            isShown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorialVisual.SetActive(false);
        }
    }

    private void Start()
    {
        tutorialVisual.SetActive(isActiveOnStart);
    }
}
