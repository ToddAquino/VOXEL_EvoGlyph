using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamagePopUP : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageText;
    public void SetupText(string text)
    {
        damageText.text = text;
    }

    public void RemovePopUP()
    {
        UIPopUpGenerator.ReturnObjectToPool(this.gameObject);
    }

}
