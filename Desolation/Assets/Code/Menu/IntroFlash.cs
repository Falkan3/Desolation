using UnityEngine;
using System.Collections;

public class IntroFlash : MonoBehaviour {

    Color textureColor;
    private float duration = 1.0f;
    public Emerge emerge;

    void Start()
    {
        textureColor = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        textureColor.a = Mathf.PingPong(Time.time, duration) / duration;
        GetComponent<Renderer>().material.color = textureColor;

        if (Input.anyKeyDown)
        {
            emerge.enabled = true;
            Destroy(gameObject);
        }
    }


}
