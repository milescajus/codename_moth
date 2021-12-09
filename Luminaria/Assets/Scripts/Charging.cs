using System.Collections;
using UnityEngine;

public class Charging : MonoBehaviour
{
    [SerializeField] private bool triggerActive = false;
    [SerializeField] public CharacterController2D Aspen;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {
            triggerActive = false;
        }
    }

    private void Update()
    {
        if (triggerActive && Aspen.isBurning) {
            StartCoroutine(Charge());
        } else {
            StopCoroutine(Charge());
        }
    }

    private IEnumerator Charge()
    {
        while(true) {
            Aspen.UpdateCharge(1);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
