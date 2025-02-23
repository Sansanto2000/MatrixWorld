using UnityEngine;

public class Hittable : MonoBehaviour
{

    private AudioSource audioSource;
    private ParticleSystem hitEffect;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        hitEffect = GetComponent<ParticleSystem>();
    }

    public void Hit(Vector3 hitterPosition)
    {
        Vector3 position = hitEffect.transform.position;

        float diffx = hitterPosition.x - position.x;
        float diffy = hitterPosition.y - position.y;
        float angle = Mathf.Atan2(diffy, diffx) * Mathf.Rad2Deg;
        float fixAngle = angle + 90;

        hitEffect.transform.rotation = Quaternion.Euler(0f, 0f, fixAngle);

        hitEffect.Play();
        audioSource.Play();
        
    }
}
