using System.Collections;
using System;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public event Action<GameObject> OnHit;

    [SerializeField] Animator animator;
    protected GameObject spellTarget;
    public virtual void Initialize(Unit target)
    {
        spellTarget = target.gameObject;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (target.transform.position.x < this.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject objHit = collision.gameObject;
        if (objHit == spellTarget)
        {
            transform.position = spellTarget.transform.position;
            OnHit?.Invoke(objHit);
            animator.SetTrigger("OnImpact");
            Debug.Log("Spell Hit");
        }
    }
}
