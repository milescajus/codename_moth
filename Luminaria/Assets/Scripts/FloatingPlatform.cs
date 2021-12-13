using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FloatingPlatform : MonoBehaviour
{
    [SerializeField] CharacterController2D aspenObject;
    [SerializeField] int chargeCost = 1;
    [SerializeField] float riseModifier = 1.0f;
    [SerializeField] float riseRate = 0.01f;
    [SerializeField] private bool isFloating = false;
    [SerializeField] private bool triggerActive;
    private float baseHeight;
    private Coroutine floating;
    new private Light2D light;
    private float maxIntensity;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = false;
        }
    }

    void Start()
    {
        light = GetComponentInChildren<Light2D>(true);
        maxIntensity = light.intensity;
        light.intensity = 0f;

        baseHeight = transform.localPosition.y;
    }

    void Update()
    {
        if (triggerActive && aspenObject.isBurning) {
            if (aspenObject.currentCharge != 0) {
                if (!isFloating) {
                    floating = StartCoroutine(Float());
                    aspenObject.currentCharge -= chargeCost;
                }
            } else {
                // NOT ENOUGH CHARGE
            }
        }
    }

    private IEnumerator Float()
    {
        isFloating = true;
        light.intensity = maxIntensity;

        for (float ft = 0; ft < 2 * Math.PI; ft += riseRate) {
            float y = (float) (1 - Math.Cos(ft));
            transform.localPosition = new Vector3(transform.localPosition.x, baseHeight + (riseModifier * y), transform.localPosition.z);

            if (ft > Math.PI)   // we've reached the apex, it's all downhill from here
                light.intensity = y * maxIntensity / 2;

            yield return null;
        }

        isFloating = false;
    }
}
