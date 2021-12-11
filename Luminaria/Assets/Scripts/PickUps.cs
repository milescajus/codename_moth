using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PickUps : MonoBehaviour
{
    [SerializeField] CharacterController2D Aspen;
    [SerializeField] int chargeValue;
    private Light2D lt;
    private SpriteRenderer r;
    private Vector3 shrinkFactor;
    private Vector3 rotateFactor;
    private float pulseTime;
    private float pulsePeriod;

    void Start()
    {
        lt = GetComponentInChildren<Light2D>(true);
        r = GetComponent<SpriteRenderer>();
        shrinkFactor = transform.localScale / 5;
        rotateFactor = new Vector3(0, 0, -1.0f);
        pulseTime = 0f;
        pulsePeriod = 0.03f;

        if (gameObject.CompareTag("LightOrb"))
            StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        while(true) {
            transform.Rotate(rotateFactor);

            float ft = (float) (0.37f + 0.05*Math.Sin(pulseTime));       // periodic function for smooth pulsing
            transform.localScale = new Vector3(ft, ft, ft);
            pulseTime += pulsePeriod;

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {

            if (gameObject.CompareTag("LightOrb")) {
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
            transform.localScale -= shrinkFactor;

            yield return null;
        }

        Destroy(gameObject);
    }
}
