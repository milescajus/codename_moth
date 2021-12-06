using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndManger : MonoBehaviour
{
    [SerializeField] private GameObject[] KeyStone;
    public static int totalNumofStone;
    [SerializeField] private bool conditionClear = false;
    [SerializeField] private GameObject ActivePortal;
    [SerializeField] private bool portalActivatedOnce = false;
    // Start is called before the first frame update
    void Start()
    {
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
            SetPortal();
        }

    }


    void SetPortal()
    {
        if (conditionClear && !portalActivatedOnce)
        {
            ActivePortal.SetActive(true);
            portalActivatedOnce = true;
        }
    }
}
