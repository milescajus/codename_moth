using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    [SerializeField] public CharacterController2D Aspen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen"))
        {
            if (gameObject.CompareTag("ChargeParticle") && Aspen.chargeLevel < 3)
            {
                Aspen.chargeLevel += 1;
            }

            //else if (gameObject.CompareTag(""))
            //{

            //}

            Destroy(gameObject);
        }
    }
}
