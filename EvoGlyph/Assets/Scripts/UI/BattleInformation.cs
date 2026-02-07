using System.Collections;
using TMPro;
using UnityEngine;

public class BattleInformation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI phaseInfoText;

    public void UpdateText(BattleState state)
    {
        StopAllCoroutines();
        phaseInfoText.gameObject.SetActive(true);
        StartCoroutine(DoUpdateText(state));

    }
    IEnumerator DoUpdateText(BattleState state)
    {
        switch (state)
        {
            case BattleState.PlayerTurn:
                phaseInfoText.text = "PLAYER TURN";
                break;
            case BattleState.OpponentTurn:
                phaseInfoText.text = "OPPONENT TURN";
                break;
            case BattleState.Won:
                phaseInfoText.text = "VICTORY";
                break;
            case BattleState.Lost:
                phaseInfoText.text = "DEFEAT";
                break;
        }
        yield return new WaitForSeconds(1f);
        phaseInfoText.gameObject.SetActive(false);
    }
}
