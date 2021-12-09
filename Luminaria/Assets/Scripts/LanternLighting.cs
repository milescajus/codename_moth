using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternLighting : MonoBehaviour
{
    new private Light2D light;
    private float maxIntensity;
    // Start is called before the first frame update
    void Start()
    {
         light = gameObject.GetComponentInChildren<Light2D>();
         light.enabled = false;
         maxIntensity = light.intensity;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Aspen"))
        {
            if (light.enabled == false) {
                light.enabled = true;
                StartCoroutine(FadeIn());
            }
        }
    }

    private IEnumerator FadeIn()
    {
        for (float ft = 0; ft < maxIntensity; ft += 0.1f) {
            light.intensity = ft;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
