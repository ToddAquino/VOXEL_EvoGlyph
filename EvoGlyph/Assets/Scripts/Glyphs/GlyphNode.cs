using UnityEngine;

public class GlyphNode : MonoBehaviour
{
    [Header("Node Sprites")]
    [SerializeField] Sprite inactiveSprite;
    [SerializeField] Sprite activeSprite;

    [SerializeField] private SpriteRenderer nodeSprite;
    [SerializeField] private Collider2D collider2D;
    //public List<GlyphNode> neighbors = new List<GlyphNode>();

    public int index;
    public int X;
    public int Y;

    public bool IsActivated = false;
    void Start()
    {
        collider2D.enabled = true;   
    }

    public void SetNodeActive()
    {
        if (!IsActivated)
        {
            IsActivated = true;
            if (activeSprite != null)
                nodeSprite.sprite = activeSprite;
            //Debug.Log($"{this}: Activated ");
        }
    }
    public void SetNodeInactive()
    {
        if(IsActivated)
        {
            IsActivated = false;
            if (inactiveSprite != null)
                nodeSprite.sprite = inactiveSprite;
            //Debug.Log($"{this}: Deactivated ");
        }
    }

    public void ResetNode()
    {
        SetNodeInactive();
    }    
}
