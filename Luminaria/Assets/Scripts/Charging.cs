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
        if (triggerActive && Aspen.isBurning && (Aspen.chargeLevel < 3)) {
            Aspen.chargeLevel = 3;
        }
    }
}
