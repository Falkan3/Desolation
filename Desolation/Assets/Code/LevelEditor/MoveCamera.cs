using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MoveCamera : MonoBehaviour {
    public int speed;
    Vector2 moveVector;

    public float scrollsensitivity;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void FixedUpdate() {
        float input_x = Input.GetAxisRaw("Horizontal");
        float input_y = Input.GetAxisRaw("Vertical");

        bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0;

        if (isWalking)
        {
            moveVector = new Vector2(input_x, input_y).normalized * speed * Time.deltaTime;
            transform.Translate(moveVector);
        }

        if (!EventSystem.current.IsPointerOverGameObject())
            Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollsensitivity;
    }
}
