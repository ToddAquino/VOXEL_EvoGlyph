using UnityEngine;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    public Image[] HealthPoints;
    public HorizontalLayoutGroup layoutGroup;
    //public float widthPerPoint = 0.25f; //1 health point = 0.25 width
    private float healthPerPoint = 10f;

    public void SetupHealthbar(float maxHealth)
    {
        int activePoints = Mathf.CeilToInt(maxHealth / healthPerPoint);

        // Resize the container
        //if (barSize != null)
        //{
        //    float newWidth = activePoints * widthPerPoint;
        //    barSize.sizeDelta = new Vector2(newWidth, barSize.sizeDelta.y);
        //}

        // Resize contents
        if(layoutGroup != null )
            layoutGroup.enabled = true;

        // Activate only the needed icons
        for (int i = 0; i < HealthPoints.Length; i++)
        {
            if (HealthPoints[i] != null)
            {
                HealthPoints[i].gameObject.SetActive(i < activePoints);
            }
        }
        if (layoutGroup != null)
        {
            // Force layout rebuild
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
            layoutGroup.enabled = false;
        }
    }

    public void UpdateHealthbar(float currentHealth)
    {
        for (int i = 0; i < HealthPoints.Length; i++)
        {
            if (HealthPoints[i] != null && HealthPoints[i].gameObject.activeSelf)
            {
                // Show the icon if current health is high enough for this point
                bool shouldShow = currentHealth > (i * healthPerPoint);
                HealthPoints[i].enabled = shouldShow;
            }
        }
    }
}
