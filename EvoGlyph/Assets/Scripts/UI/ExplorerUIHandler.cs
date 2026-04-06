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
        BookUI.SetActive(false);
        BookPopUP.SetActive(true);
        OnBookOpen?.Invoke();
    }

    public void BookClose() 
    {
        BookUI.SetActive(true);
        BookPopUP.SetActive(false);
        OnBookClosed?.Invoke();
    }
}
