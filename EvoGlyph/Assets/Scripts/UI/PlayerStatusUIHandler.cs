using UnityEngine;

public class PlayerStatusUIHandler : MonoBehaviour
{
    public Healthbar Healthbar;
    private void Initialize(float maxHealth)
    {
        Healthbar.SetupHealthbar(maxHealth);
    }
}
