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

    public void Hit()
    {
        audioSource.Play();
        hitEffect.Play();
    }
}
