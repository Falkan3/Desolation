using UnityEngine;
using System.Collections.Generic;

public class Emerge : MonoBehaviour {

    private float height;
    private float height_2;
    private float iteration = 0f;

    public float speed = 0.1f;
    public float distance_between_options = 1.2f;
    public float distance_from_top = 0.6f;
    Renderer ren;

    public List<GameObject> objectList;

	// Use this for initialization
	void Start () {
        this.enabled = false;
        height = GetComponent<Renderer>().bounds.size.y;
        height_2 = height / 2;
        ren = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        for(int i=0; i<objectList.Count;i++)
        {
            objectList[i].transform.position = new Vector2(ren.transform.position.x, ren.transform.position.y+(height_2* distance_from_top) - (i * (objectList[i].GetComponent<Renderer>().bounds.size.y)* distance_between_options));
        }

        if (iteration < height)
        {
            ren.transform.position = new Vector2(0, ren.transform.position.y + speed);
            iteration += speed;
        }
        else
        {
            GetComponent<Selection>().enabled = true;
            Destroy(GetComponent<Emerge>());
        }
	}
}
