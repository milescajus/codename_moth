using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PickUps : MonoBehaviour
{
    [Header("Aspen")]
    [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeValue = 0;

    // Scaling
    private Vector3 initialScale;

    // Lighting
    private SpriteRenderer r;
    private Light2D lt;
    private float initialIntensity;

    // Collecting
    private Vector3 shrinkFactor;

    [Header("Pulsing")]
    [SerializeField] float pulseRate = 0.03f;
    [SerializeField] float rotateFactor = 1.0f;
    [SerializeField] float scaleAmp = 0.05f;
    [SerializeField] float glowAmp = 0.5f;
    [SerializeField] bool rotate = false;
    [SerializeField] bool scale = false;
    [SerializeField] bool glow = false;

    private Vector3 rotateVect;
    private double pulsePeriod;
    private float pulseTime;
    private Coroutine pulsing;

    void Start()
    {
        lt = GetComponentInChildren<Light2D>(true);
        r = GetComponent<SpriteRenderer>();

        initialIntensity = lt.intensity;
        initialScale = transform.localScale;

        shrinkFactor = initialScale / 5;
        rotateVect = new Vector3(0, 0, rotateFactor);

        pulseTime = 0f;
        pulsePeriod = 2*Math.PI / pulseRate;

        pulsing = StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        // uses periodic function for smooth pulsing

        while(true) {
            float scaleFactor = (float) (initialScale.x + scaleAmp*Math.Sin(pulseTime));
            float glowFactor = (float) (initialIntensity + glowAmp*Math.Sin(pulseTime));

            if (rotate)
                transform.Rotate(-rotateVect);
            if (scale)
                transform.localScale = new Vector3(scaleFactor, scaleFactor);
            if (glow)
                lt.intensity = glowFactor;

            if (pulseTime == pulsePeriod)
                pulseTime = 0f;
            else
                pulseTime += pulseRate;

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            if (gameObject.CompareTag("KeyStone")) {
                LevelEndManger.totalNumofStone--;
            }

            aspenObject.currentCharge += chargeValue;
            StartCoroutine(Collect());
        }
    }

    private IEnumerator Collect()
    {
        StopCoroutine(pulsing);

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
