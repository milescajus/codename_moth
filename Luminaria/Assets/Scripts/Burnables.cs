using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;

public class Burnables : MonoBehaviour
{
    [SerializeField] private bool triggerActive = false;
    [SerializeField] public CharacterController2D Aspen;
    [SerializeField] private int ChargeCost = 1;
    private bool hasBurned = false;
    
    [SerializeField] private ParticleSystem ps;
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
        if (triggerActive && Aspen.isBurning && (Aspen.GetCharge() != 0)) {
            if (!hasBurned) {
                hasBurned = true;
                StartCoroutine(IsBurning());
                soundClip.Play();
                ps.gameObject.transform.parent.gameObject.SetActive(true);      // fire element
                ps.Play();
                lt.gameObject.SetActive(true);      // fire light
                Aspen.UpdateCharge(-ChargeCost);
            }
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

        Color end = GetComponent<SpriteRenderer>().color;
        end.a = 0f;
        GetComponent<SpriteRenderer>().color = end;

        Destroy(lt.gameObject);
        Destroy(barrier.gameObject);
    }
}
