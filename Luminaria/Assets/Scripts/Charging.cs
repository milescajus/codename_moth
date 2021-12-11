using UnityEngine;

public class Charging : MonoBehaviour
{
    [SerializeField] CharacterController2D Aspen;
    [SerializeField] bool triggerActive = false;

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
        if (triggerActive && Aspen.isBurning)
            Aspen.currentCharge = Aspen.maxCharge;
    }
}
