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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Play();
    }

    public void Play()
    {
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
        GameManager.Instance.ExplorationData.RegisterCutsceneFinished(this.GetCutsceneID());
        OnEnd?.Invoke();
    }

    public string GetCutsceneID()
    {
        return this.gameObject.name;
    }
}
