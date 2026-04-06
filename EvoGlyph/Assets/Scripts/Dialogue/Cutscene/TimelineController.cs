using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class TimelineController : MonoBehaviour
{
    public UnityEvent OnPlay;
    public UnityEvent OnEnd;
    public GameObject[] toShow;
    public GameObject[] toHide;
    public PlayableDirector playableDirector;
    public bool isPlayed = false;

    public void Play()
    {
        UIManager.Instance.ExplorationUIController.DeInitialize();
        if (!isPlayed)
        {
            isPlayed = true;
            foreach (var obj in toShow)
            {
                obj.SetActive(true);
            }
            foreach (var obj in toHide)
            {
                obj.SetActive(false);
            }
            playableDirector.Play();
        }
    }

    public void OnPlayableEnded()
    {
        UIManager.Instance.ShowExplorationUI();
        GameManager.Instance.ExplorationData.RegisterCutsceneFinished(this.GetCutsceneID());
        OnEnd?.Invoke();
    }

    public string GetCutsceneID()
    {
        return this.gameObject.name;
    }
}
