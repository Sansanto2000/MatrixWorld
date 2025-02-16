using UnityEngine;
using System.Collections; 

public class CameraShake : MonoBehaviour
{
    public float defaultDuration = 1.5f;
    public float defaultMagnitude = 1.5f;

    public IEnumerator Shake() {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;
        while (elapsed < defaultDuration) {
            float x = originalPosition.x + Random.Range(-1f, 1f) * defaultMagnitude;
            float y = originalPosition.y + Random.Range(-1f, 1f) * defaultMagnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
