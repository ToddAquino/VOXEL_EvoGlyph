using UnityEngine;

public abstract class Glyph : MonoBehaviour
{
    public bool IsActivated = false;
    public GlyphData GlyphData;
    public virtual void Activate(Unit user)
    {
        if (IsActivated)
        {
            Debug.Log($"Glyph: {GlyphData.name} Already Activated");

        }
        IsActivated = true;
    }
}
