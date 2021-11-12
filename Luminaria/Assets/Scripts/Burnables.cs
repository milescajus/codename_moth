using UnityEngine;
using System.Collections;

public class Burnables : MonoBehaviour
{
    [SerializeField] private bool triggerActive = false;
    [SerializeField] public CharacterController2D Aspen;
    [SerializeField] private int ChargeCost = 1;
    private bool hasPlayedClip = false;
    private bool hasDepleted = false;
    private bool hasBurned = false;
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
            if (!hasBurned) {
                Burn();
                hasBurned = true;
            }
        }
    }

    private void Burn()
    {
        if (!hasPlayedClip) {
            soundClip.Play();
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            hasPlayedClip = true;
        }

        Destroy(transform.GetChild(0).gameObject, 1);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

        transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();

        if (!hasDepleted) {
            Aspen.chargeLevel -= ChargeCost;
            hasDepleted = true;
        }
    }
}
