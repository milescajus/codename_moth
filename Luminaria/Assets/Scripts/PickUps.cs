using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    [SerializeField] private CharacterController2D Aspen;
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
        if (other.gameObject.CompareTag("Aspen")) {

            if (gameObject.CompareTag("ChargeParticle")) {
                Aspen.currentCharge++;
            }

            else if (gameObject.CompareTag("KeyStone")) {
                LevelEndManger.totalNumofStone--;
            }

            Destroy(gameObject);
        }
    }
}
