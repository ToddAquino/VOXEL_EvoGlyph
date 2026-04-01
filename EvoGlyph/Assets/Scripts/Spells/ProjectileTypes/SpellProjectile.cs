using UnityEngine;

public class SpellProjectile : Spell
{
    [SerializeField] float projectileSpeed = 5f;
    bool isMoving;
    Vector3 projectileDirection;
    public override void Initialize(Unit target)
    {
        base.Initialize(target);

        if (target.transform.position.x < this.transform.position.x)
        {
            projectileDirection = Vector3.left;
        }
        else 
        {
            projectileDirection = Vector3.right;
        }
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position += projectileDirection * projectileSpeed * Time.deltaTime;
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject objHit = collision.gameObject;
        if (objHit == spellTarget)
        {
            isMoving = false;      
        }
        base.OnTriggerEnter2D(collision);
    }  
}
