using System;
using UnityEngine;
public class ExplorerUIHandler : MonoBehaviour
{
    public static event Action OnBookOpen;
    public static event Action OnBookClosed;
    [SerializeField] GameObject BookUI;
    [SerializeField] GameObject BookPopUP;
    public void BookOpen()
    {
        DoClickSound();
        BookUI.SetActive(false);
        BookPopUP.SetActive(true);
        OnBookOpen?.Invoke();
    }

    public void BookClose() 
    {
        DoClickSound();
        BookUI.SetActive(true);
        BookPopUP.SetActive(false);
        OnBookClosed?.Invoke();
    }
    public void DoClickSound()
    {
        AudioManager.Instance.PlaySFX("click", 0.5f);
    }
}
