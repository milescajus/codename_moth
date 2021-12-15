using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuideUI : MonoBehaviour
{
    [SerializeField] private GameObject ThingToGuide;
    [SerializeField] bool destroyOnFadeOut;
    private Coroutine fading;

    // Start is called before the first frame update
    private void Start()
    {
        Color box = GetComponentInChildren<SpriteRenderer>().color;
        Color text = GetComponentInChildren<TMP_Text>().color;
        box.a = 0;
        text.a = 0;
        GetComponentInChildren<SpriteRenderer>().color = box;
        GetComponentInChildren<TMP_Text>().color = text;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Aspen")
        {
            fading = StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Aspen")
        {
            fading = StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        if (fading != null)     // currently fading
            StopCoroutine(fading);

        Color start = GetComponentInChildren<SpriteRenderer>().color;

        for (float ft = start.a; ft < 1f; ft += 0.1f)
        {
            Color box = GetComponentInChildren<SpriteRenderer>().color;
            Color text = GetComponentInChildren<TMP_Text>().color;
            box.a = ft;
            text.a = ft;
            GetComponentInChildren<SpriteRenderer>().color = box;
            GetComponentInChildren<TMP_Text>().color = text;

            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeOut()
    {
        if (fading != null)     // currently fading
            StopCoroutine(fading);

        Color start = GetComponentInChildren<SpriteRenderer>().color;

        for (float ft = start.a; ft >= 0f; ft -= 0.1f)
        {
            Color box = GetComponentInChildren<SpriteRenderer>().color;
            Color text = GetComponentInChildren<TMP_Text>().color;
            box.a = ft;
            text.a = ft;
            GetComponentInChildren<SpriteRenderer>().color = box;
            GetComponentInChildren<TMP_Text>().color = text;

            yield return new WaitForSeconds(0.02f);
        }

        if (destroyOnFadeOut)
            Destroy(gameObject);
    }
}
