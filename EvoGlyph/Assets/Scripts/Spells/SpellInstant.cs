using UnityEngine;

public class SpellInstant : Spell
{
    private void Awake()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
    public override void Initialize(Unit target)
    {
        spellTarget = target.gameObject;
        this.transform.position = target.transform.position;
        this.GetComponent<BoxCollider2D>().enabled = true;
        base.Initialize(target);
    }
}
