using UnityEngine;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    Material material;
    float offsetVal;
    float segmentCount;
    float healthPerCell = 15f;

    private void Awake()
    {
        var image = GetComponent<Image>();
        image.material = new Material(image.material);
        material = image.material;
    }

    public void SetupHealthbar(float maxHealth)
    {
        segmentCount = maxHealth / healthPerCell;
        material.SetFloat("_segment", segmentCount);
    }
    public void ResetHealthbar()
    {
        offsetVal = 0.5f;
        material.SetFloat("_Offset", offsetVal);
    }

    public void UpdateHealthbar(float currentHealth, float maxHealth)
    {
        offsetVal = currentHealth / maxHealth - 0.5f;
        material.SetFloat("_Offset", offsetVal);
    }
}
