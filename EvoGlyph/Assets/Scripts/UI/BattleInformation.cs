using System.Collections;
using TMPro;
using UnityEngine;

public class BattleInformation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI phaseInfoText;

    public void UpdateText(BattlePhase state)
    {
        StopAllCoroutines();
        phaseInfoText.gameObject.SetActive(true);
        StartCoroutine(DoUpdateText(state));

    }
    IEnumerator DoUpdateText(BattlePhase state)
    {
        switch (state)
        {
            case BattlePhase.PlayerAction :
                phaseInfoText.text = "Player Action Phase";
                break;
            case BattlePhase.EnemyAction:
                phaseInfoText.text = "Enemy Action Phase";
                break;
            case BattlePhase.Won:
                phaseInfoText.text = "VICTORY";
                break;
            case BattlePhase.Lost:
                phaseInfoText.text = "DEFEAT";
                break;
        }
        yield return new WaitForSeconds(1f);
        phaseInfoText.gameObject.SetActive(false);
    }

    //Tutorial Stuff
    public void ShowStatePlayerAction()
    {
        StopAllCoroutines();
        phaseInfoText.gameObject.SetActive(true);
        StartCoroutine(DoUpdateText(BattlePhase.PlayerAction));
    }

    public void ShowStateEnemyAction()
    {
        StopAllCoroutines();
        phaseInfoText.gameObject.SetActive(true);
        StartCoroutine(DoUpdateText(BattlePhase.EnemyAction));
    }

    public void ShowStateWon()
    {
        StopAllCoroutines();
        phaseInfoText.gameObject.SetActive(true);
        StartCoroutine(DoUpdateText(BattlePhase.Won));
    }
}
