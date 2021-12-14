using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndManger : MonoBehaviour
{
    [SerializeField] private GameObject ActivePortal;
    [SerializeField] private GameObject[] KeyStone;
    [SerializeField] Animator animator;
    public static int totalNumofStone;
    [SerializeField] private bool conditionClear = false;
    [SerializeField] private bool portalActivatedOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("PortalActivating", false);
        ActivePortal.SetActive(false);
        for (int i = 0; i < KeyStone.Length; i++)
        {
            totalNumofStone++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (totalNumofStone <= 0)
        {
            conditionClear = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Aspen"))
        {
            if (conditionClear)
            {
                StartCoroutine(SetPortal());
            }
        }
    }


    IEnumerator SetPortal()
    {
        if (conditionClear && !portalActivatedOnce)
        {
            portalActivatedOnce = true;
            animator.SetBool("PortalActivating", true);
            yield return new WaitForSeconds(8);
            ActivePortal.SetActive(true);
        }
    }
}
