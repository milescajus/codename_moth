using UnityEngine;

public class Burnables : MonoBehaviour
{
    [SerializeField] private bool triggerActive = false;
    [SerializeField] public CharacterController2D Aspen;
    [SerializeField] private int ChargeCost = 1;
    private bool hasPlayedClip = false;
    private bool hasDepleted = false;
    AudioSource soundClip;
    GameObject fire;

    public void Start()
    {
        GameObject fire = transform.GetChild(1).transform.GetChild(0).gameObject;
        fire.GetComponent<ParticleSystem> ().GetComponent<Renderer> ().sortingLayerName = "Foreground";
        fire.GetComponent<ParticleSystem> ().GetComponent<Renderer> ().sortingOrder = 21;
        soundClip = GetComponent<AudioSource>();
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
            if (!hasPlayedClip) {
                soundClip.Play();
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                hasPlayedClip = true;
            }

            Destroy(gameObject, 1);
            // Destroy(transform.GetChild(0).gameObject, 1);

            // fire.GetComponent<ParticleSystem>().Stop();

            if (!hasDepleted) {
                Aspen.chargeLevel -= ChargeCost;
                hasDepleted = true;
            }
        }
    }
}
