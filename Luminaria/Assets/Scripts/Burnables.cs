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
    
    ParticleSystem ps;
    Light2D lt;
    EdgeCollider2D barrier;
    AudioSource soundClip;

    public void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>(true);
        lt = GetComponentInChildren<Light2D>(true);
        barrier = GetComponentInChildren<EdgeCollider2D>();
        soundClip = GetComponent<AudioSource>();

        ps.GetComponent<Renderer>().sortingLayerName = "Foreground";
        ps.GetComponent<Renderer>().sortingOrder = 21;
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
            ps.gameObject.SetActive(true);      // fire element
            ps.Play();
            lt.gameObject.SetActive(true);      // fire light
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

            lt.intensity = 3*ft;

            yield return new WaitForSeconds(.15f);
        }

        ps.Stop();

        Color c = GetComponent<SpriteRenderer>().color;
        c.a = 0f;
        GetComponent<SpriteRenderer>().color = c;

        Destroy(lt.gameObject);
        Destroy(barrier.gameObject);
    }
}
