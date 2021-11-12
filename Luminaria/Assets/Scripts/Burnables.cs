using UnityEngine;

public class Burnables : MonoBehaviour
{
    [SerializeField] private bool triggerActive = false;
    [SerializeField] public CharacterController2D Aspen;
    [SerializeField] private int ChargeCost = 1;
    private bool hasDepleted = false;

    public void Start()
    {
        GameObject particles = transform.GetChild(1).transform.GetChild(0).gameObject;
        particles.GetComponent<ParticleSystem> ().GetComponent<Renderer> ().sortingLayerName = "Foreground";
        particles.GetComponent<ParticleSystem> ().GetComponent<Renderer> ().sortingOrder = 21;
    }

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
        if (triggerActive && Aspen.isBurning && (Aspen.chargeLevel != 0)) {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            Destroy(gameObject, 1);

            if (!hasDepleted) {
                Aspen.chargeLevel -= ChargeCost;
                hasDepleted = true;
            }
        }
    }
}
