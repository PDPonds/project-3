using UnityEngine;

public class BulletObject : MonoBehaviour
{
    Vector3 dir;
    float speed;

    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public void Setup(Vector3 dir, float speed, float duration)
    {
        this.dir = dir;
        this.speed = speed;
        Destroy(gameObject, duration);
    }
}
