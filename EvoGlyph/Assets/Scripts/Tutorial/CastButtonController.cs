using UnityEngine;
using UnityEngine.UI;

public class CastButtonController : MonoBehaviour
{
    private Button castButton;

    private void Start()
    {
        FindCastButton();
    }

    private void FindCastButton()
    {
        // Search for PlayerUnit or PlayerUnit(Clone)
        GameObject playerUnit = GameObject.Find("PlayerUnit");

        if (playerUnit == null)
        {
            playerUnit = GameObject.Find("PlayerUnit(Clone)");
        }

        if (playerUnit == null)
        {
            Debug.LogError("PlayerUnit or PlayerUnit(Clone) not found!");
            return;
        }

        // Navigate to the Cast Button
        Transform glyphSequencer = playerUnit.transform.Find("Player Glyph Sequencer/Canvas/GlyphSequencer/Cast Button");

        if (glyphSequencer == null)
        {
            Debug.LogError("Cast Button not found in hierarchy!");
            return;
        }

        castButton = glyphSequencer.GetComponent<Button>();

        if (castButton == null)
        {
            Debug.LogError("Button component not found on Cast Button!");
            return;
        }

        Debug.Log("Cast Button found successfully!");
    }

    public void EnableCastButton()
    {
        if (castButton != null)
        {
            castButton.interactable = true;
            Debug.Log("YOU SHOULD ENABLE YOURSELF... NOW!!!");
        }
        else
        {
            Debug.LogWarning("Cast Button reference is null! Attempting to find it again...");
            FindCastButton();
            if (castButton != null)
            {
                castButton.interactable = true;
            }
        }
    }

    public void DisableCastButton()
    {
        if (castButton != null)
        {
            castButton.interactable = false;
            Debug.Log("YOU SHOULD DISABLE YOURSELF... NOW!!!");
        }
        else
        {
            Debug.LogWarning("Cast Button reference is null! Attempting to find it again...");
            FindCastButton();
            if (castButton != null)
            {
                castButton.interactable = false;
            }
        }
    }

    // Optional: Manual refresh if the button reference is lost
    public void RefreshCastButton()
    {
        FindCastButton();
    }
}