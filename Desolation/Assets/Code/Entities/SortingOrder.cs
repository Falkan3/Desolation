using UnityEngine;
using System.Collections;

public class SortingOrder : MonoBehaviour {

    private Renderer objRenderer;

    // Use this for initialization
    void Start ()
    {
        objRenderer = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        objRenderer.sortingOrder = (int)(transform.position.y * -10);
    }
}
