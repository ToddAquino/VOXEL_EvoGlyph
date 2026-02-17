using System.Collections;
using TMPro;
using UnityEngine;

public class BattleInformation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI phaseInfoText;
    [SerializeField] GameObject InfoUIObj;
    public void UpdateText(BattlePhase state)
    {
        StopAllCoroutines();
        InfoUIObj.gameObject.SetActive(true);
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
        InfoUIObj.gameObject.SetActive(false);
    }

    //Tutorial Stuff
    public void ShowStatePlayerAction()
    {
        StopAllCoroutines();
        InfoUIObj.gameObject.SetActive(true);
        StartCoroutine(DoUpdateText(BattlePhase.PlayerAction));
    }

    public void ShowStateEnemyAction()
    {
        StopAllCoroutines();
        InfoUIObj.gameObject.SetActive(true);
        StartCoroutine(DoUpdateText(BattlePhase.EnemyAction));
    }

    public void ShowStateWon()
    {
        StopAllCoroutines();
        InfoUIObj.gameObject.SetActive(true);
        StartCoroutine(DoUpdateText(BattlePhase.Won));
    }
}
