using UnityEngine;

public abstract class Glyph : MonoBehaviour
{
    public bool IsActivated = false;
    public GlyphData GlyphData;

    public virtual void Activate()
    {
        if (IsActivated)
        {
            Debug.Log($"Glyph: {GlyphData.Name} Already Activated");

        }
        IsActivated = true;
    }
}
