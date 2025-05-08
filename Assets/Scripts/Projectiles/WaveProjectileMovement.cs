using UnityEngine;

public class WaveProjectileMovement : ProjectileMovement
{
    private Vector3 direction;
    private Vector3 waveAxis; // axis perpendicular to forward direction
    private Vector3 startPosition;
    private float frequency;
    private float magnitude;
    private float elapsed;
    private bool initialized = false;

    public WaveProjectileMovement(float speed, float frequency = 5f, float magnitude = 3.7f) : base(speed)
    {
        this.frequency = frequency;
        this.magnitude = magnitude;
    }

    public override void Movement(Transform transform)
    {
        if (!initialized)
        {
            direction = transform.right.normalized;
            waveAxis = Vector3.Cross(direction, Vector3.forward).normalized; // perpendicular in 2D
            startPosition = transform.position;
            initialized = true;
        }

        elapsed += Time.deltaTime;

        // forward movement
        Vector3 forwardMove = direction * speed * Time.deltaTime;

        // sideways wave offset
        float waveOffset = Mathf.Sin(elapsed * frequency) * magnitude;
        Vector3 offset = waveAxis * waveOffset;

        transform.position += forwardMove + offset * Time.deltaTime;
    }
}