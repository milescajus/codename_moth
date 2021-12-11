using UnityEngine;

public class PickUps : MonoBehaviour
{
    [SerializeField] CharacterController2D Aspen;
    [SerializeField] int chargeValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Aspen")) {

            if (gameObject.CompareTag("ChargeParticle")) {
                Aspen.currentCharge += chargeValue;
            }

            else if (gameObject.CompareTag("KeyStone")) {
                LevelEndManger.totalNumofStone--;
            }

            Destroy(gameObject);
        }
    }
}
