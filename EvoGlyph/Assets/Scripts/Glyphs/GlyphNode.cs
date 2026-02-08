using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GlyphNode : MonoBehaviour
{
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
            nodeSprite.color = Color.aquamarine;
            Debug.Log($"{this}: Activated ");
            //CheckForBonds();
        }
    }
    public void SetNodeInactive()
    {
        if(IsActivated)
        {
            IsActivated = false;
            nodeSprite.color = Color.white;
            Debug.Log($"{this}: Deactivated ");
        }
    }

    public void ResetNode()
    {
        SetNodeInactive();
    }    
    //void CheckForBonds()
    //{
    //    foreach (GlyphNode neighbor in neighbors)
    //    {
    //        if (neighbor.IsActivated)
    //        {
    //            FormBond(neighbor);
    //        }
    //    }
    //}

    void FormBond(GlyphNode pair)
    {
        Debug.Log($"Bond formed between {this} and {pair}");
    }
}
