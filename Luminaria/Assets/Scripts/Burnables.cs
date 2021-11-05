using UnityEngine;

public class Burnables : MonoBehaviour
{
    [SerializeField] private bool triggerActive = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            triggerActive = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            triggerActive = false;
        }
    }

    private void Update()
    {
        if (triggerActive && (Input.GetKeyDown(KeyCode.LeftShift)) || Input.GetKeyDown(KeyCode.RightShift)) {
            Destroy(gameObject);
        }
    }
}
