using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rigid;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpPower;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float xVelocity = horizontalInput * speed;
        rigid.velocity = new Vector3(xVelocity, rigid.velocity.y, rigid.velocity.z);

        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector3(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y, rigid.velocity.z);
        }
        //jump
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
