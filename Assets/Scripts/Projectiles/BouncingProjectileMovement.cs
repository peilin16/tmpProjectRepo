using UnityEngine;

public class BouncingProjectileMovement : ProjectileMovement
{
    private Vector3 direction;
    private int remainingBounces;
    private float radius;
    private bool initialized = false;

    public BouncingProjectileMovement(float speed, int maxBounces = 3, float colliderRadius = 0.1f) : base(speed)
    {
        this.remainingBounces = maxBounces;
        this.radius = colliderRadius;
    }

    public override void Movement(Transform transform)
    {
        if (!initialized)
        {
            direction = transform.right.normalized; // Use rotation-based direction once
            initialized = true;
        }

        Vector3 move = direction * speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, direction, move.magnitude);
        if (hit.collider != null)
        {
            direction = Vector3.Reflect(direction, hit.normal).normalized;

            remainingBounces--;
            if (remainingBounces < 0)
            {
                GameObject.Destroy(transform.gameObject);
                return;
            }

            // Push out slightly from wall to avoid clipping
            transform.position = hit.point + hit.normal * 0.01f;
        }
        else
        {
            transform.position += move;
        }
    }
}
