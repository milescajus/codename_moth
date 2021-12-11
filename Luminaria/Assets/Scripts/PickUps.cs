using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PickUps : MonoBehaviour
{
    [SerializeField] CharacterController2D Aspen;
    [SerializeField] int chargeValue;
    private Light2D lt;
    private SpriteRenderer r;

    void Start()
    {
        lt = GetComponentInChildren<Light2D>(true);
        r = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {

            if (gameObject.CompareTag("ChargeParticle")) {
                Aspen.currentCharge += chargeValue;
            }

            else if (gameObject.CompareTag("KeyStone")) {
                LevelEndManger.totalNumofStone--;
            }

            StartCoroutine(Collect());
        }
    }

    private IEnumerator Collect()
    {
        for (float ft = lt.intensity; ft < 6; ft += 0.8f) {
            if (lt != null)
                lt.intensity = ft;
            Color c = r.color;
            c.a = 1.0f - ft / 6;
            r.color = c;

            yield return null;
        }

        for (float ft = lt.intensity; ft >= 0; ft -= 1.2f) {
            if (lt != null)
                lt.intensity = ft;

            yield return null;
        }

        Destroy(gameObject);
    }
}
