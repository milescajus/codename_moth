using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
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
                StartCoroutine(IsBurning());
                Burn();
                hasBurned = true;
            }
        }
    }

    private void Burn()
    {
        if (!hasPlayedClip) {
            soundClip.Play();
            transform.GetChild(1).gameObject.SetActive(true);   // fire element
            transform.GetChild(2).gameObject.SetActive(true);   // fire light
            hasPlayedClip = true;
        }

        if (!hasDepleted) {
            Aspen.chargeLevel -= ChargeCost;
            hasDepleted = true;
        }
    }

    private IEnumerator IsBurning()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.1f) 
        {
            Color c = GetComponent<SpriteRenderer>().color;
            c.a = ft;
            GetComponent<SpriteRenderer>().color = c;
            transform.GetChild(2).gameObject.GetComponent<Light2D>().intensity = 3*ft;
            yield return new WaitForSeconds(.15f);
        }

        transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        Destroy(transform.GetChild(2).gameObject);           // fire light
        Destroy(transform.GetChild(0).gameObject);           // edge collider (barrier)
    }
}
