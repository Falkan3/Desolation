using UnityEngine;
using System.Collections.Generic;

public class Emerge : MonoBehaviour {

    private float height;
    private float height_2;
    private float iteration = 0f;

    public float speed = 5;
    public float distance_between_options = 1.2f;
    public float distance_from_top = 0.6f;
    Renderer ren;

    public List<GameObject> objectList;
    private Selection selection;

    // Use this for initialization
    void Start () {
        //this.enabled = false;
        height = GetComponent<Renderer>().bounds.size.y;
        height_2 = height / 2;
        ren = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(objectList != null && objectList.Count>0)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].transform.position = new Vector2(ren.transform.position.x, ren.transform.position.y + (height_2 * distance_from_top) - (i * (objectList[i].GetComponent<Renderer>().bounds.size.y) * distance_between_options * i));
            }
        }
        
        if (Mathf.Abs(iteration) < height)
        {
            ren.transform.position = new Vector2(ren.transform.position.x, ren.transform.position.y + speed * Time.deltaTime);
            iteration += speed * Time.deltaTime;
        }
        else
        {
            selection = GetComponent<Selection>();
            if(selection != null)
                selection.enabled = true;
            Destroy(GetComponent<Emerge>());
        }
	}
}
