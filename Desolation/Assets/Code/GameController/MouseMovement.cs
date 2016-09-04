using UnityEngine;
using System.Collections;

public class MouseMovement : Entity {
	
	// Update is called once per frame
	void Update () {
        Vector3 thePos = new Vector3(Input.mousePosition.x, transform.position.y, Input.mousePosition.y);
        transform.position = thePos;
    }
}
