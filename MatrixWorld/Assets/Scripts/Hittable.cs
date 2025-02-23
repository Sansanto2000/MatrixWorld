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
        float angle = 0;
        var mainModule = hitEffect.main;
        float particleRotation = 0;
        switch (diffx, diffy) {
            case (>0,_):
                angle = -270;
                particleRotation = 270;
                Debug.Log(angle);
                break;
            case (<0,_): 
                angle = -90;
                particleRotation = 90;
                Debug.Log(angle);
                break;
            case (_,>0):
                angle = 180;
                particleRotation = 180;
                Debug.Log(angle);
                break;
            case (_,<0):
                angle = 0;
                particleRotation = 0;
                Debug.Log(angle);
                break;  
        }

        mainModule.startRotation = particleRotation;
        hitEffect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
