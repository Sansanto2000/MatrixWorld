using UnityEngine;

public class Hittable : MonoBehaviour
{

    [Header("Characteristics")]
    [Tooltip("Vida del objetivo.")]
    public float life = 100;

    private GameMaster gameMaster;
    private AudioSource audioSource;
    private ParticleSystem hitEffect;

    void Awake()
    {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        audioSource = GetComponent<AudioSource>();
        hitEffect = GetComponent<ParticleSystem>();
    }

    void emitParticles(Vector3 hitterPosition) {
        /// <summary>
        /// Emite partículas en la dirección opuesta del que realiza el golpe
        /// </summary>
        float diffx = hitterPosition.x - hitEffect.transform.position.x;
        float diffy = hitterPosition.y - hitEffect.transform.position.y;
        float angle = Mathf.Atan2(diffy, diffx) * Mathf.Rad2Deg;
        float fixAngle = angle + 90;
        hitEffect.transform.rotation = Quaternion.Euler(0f, 0f, fixAngle);
        hitEffect.Play();
    }

    public void Hit(float damage, Vector3 hitterPosition)
    {
        emitParticles(hitterPosition);
        audioSource.Play();
        
        life -= damage;
        if (life <= 0) {
            gameMaster.EnemyDeleted(gameObject);
            Destroy(gameObject);
            return;
        }
    }
}
