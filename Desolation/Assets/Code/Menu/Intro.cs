using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

    Color textureColor;
    private float duration = 1.0f;

    void Start () {
        textureColor = GetComponent<Renderer>().material.color;
    }
	
	void Update ()
    {
        //Color textureColor = GetComponent<Renderer>().material.color;
        //textureColor.a = Mathf.PingPong(Time.time, duration) / duration;
        textureColor.a = Mathf.PingPong(Time.time, duration) / duration;
        GetComponent<Renderer>().material.color = textureColor;
        if (textureColor.a >= 0.99f)
        {
            Destroy(GetComponent<Intro>());
            textureColor.a = duration;
            GetComponent<Renderer>().material.color = textureColor;
        }
    }
}
