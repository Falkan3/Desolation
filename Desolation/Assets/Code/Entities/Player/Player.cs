using UnityEngine;
using System.Collections;

public class Player : Entity {

    Animator anim;
    Rigidbody2D rb;
    static int[] direction = new int[2];

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.drag = speed * 5f;
    }

    void FixedUpdate()
    {
        float input_x = Input.GetAxisRaw("Horizontal");
        float input_y = Input.GetAxisRaw("Vertical");

        bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;

        anim.SetBool("isWalking", isWalking);
        if (isWalking)
        {
            anim.SetFloat("x", input_x);
            anim.SetFloat("y", input_y);
            direction[0] = Mathf.RoundToInt(input_x);
            direction[1] = Mathf.RoundToInt(input_y);

            //transform.position += new Vector3(input_x, input_y).normalized * speed * Time.deltaTime;
            //rb.velocity = new Vector3(input_x, input_y).normalized * speed * Time.deltaTime;
            rb.velocity = new Vector3(input_x, input_y).normalized * speed;
        }
    }
}
