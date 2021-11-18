using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideUI : MonoBehaviour
{
    [SerializeField] private GameObject ThingToGuide;
    // Start is called before the first frame update
    private void Start()
    {
        ThingToGuide.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Aspen")
        {
            ThingToGuide.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Aspen")
        {
            ThingToGuide.SetActive(false);
        }
    }
}
