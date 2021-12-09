using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternLighting : MonoBehaviour
{
    new private Light2D light;
    // Start is called before the first frame update
    void Start()
    {
         light = gameObject.GetComponentInChildren<Light2D>();
         light.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Aspen"))
        {
            light.enabled = true;
        }
    }
}
