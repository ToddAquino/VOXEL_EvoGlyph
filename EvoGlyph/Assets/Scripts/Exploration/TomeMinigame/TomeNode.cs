using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TomeNodeState
{
    Disabled,
    Pending,
    Active
}
public class TomeNode : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI inputText;
    [Header("Sprite")]
    public Sprite DisabledSprite;
    public Sprite PendingSprite;
    public Sprite ActiveSprite;

    public int index;
    public Key requiredInput;
    public TomeNodeState State;

    public void Initialize(Key key)
    {
        requiredInput = key;
        if (inputText != null)
            inputText.text = key.ToString();
    }

    public void SetState(TomeNodeState newState)
    {
        State = newState;
        switch (State)
        {
            case TomeNodeState.Disabled:
                spriteRenderer.sprite = DisabledSprite;
                if (inputText) inputText.gameObject.SetActive(false);
                break;
            case TomeNodeState.Pending:
                spriteRenderer.sprite = PendingSprite;
                if (inputText) inputText.gameObject.SetActive(true);
                break;
            case TomeNodeState.Active:
                spriteRenderer.sprite = ActiveSprite;
                if (inputText) inputText.gameObject.SetActive(false);
                break;
        }
    }
    public bool TryActivate()
    {
        if (State != TomeNodeState.Pending) return false;

        if (Keyboard.current[requiredInput].wasPressedThisFrame)
        {
            SetState(TomeNodeState.Active);
            return true;
        }
        return false;
    }
}
