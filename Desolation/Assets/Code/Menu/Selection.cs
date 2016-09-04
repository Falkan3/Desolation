using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class Selection : MonoBehaviour {

    public List<GameObject> objectList;
    public Color deselected_color;
    public Color selected_color;

    public int selected_index = 0;
    private float countdown = 0;
    private bool can_select = true;
    public float delay = 0.5f;
    float input_y;

    //Fade
    //Audio
    public float fadeTime = 4.0f;
    enum Fade { In, Out }

    void Start () {
        this.enabled = false;
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].GetComponent<Renderer>().material.color = deselected_color;
        }
    }
	
	void Update () {
        if(Input.GetButtonDown("Submit"))
        {
            switch(selected_index)
            {
                case 0:
                    GetComponent<Screen_fade>().enabled = true;
                    GetComponent<Screen_fade>().delay = fadeTime;
                    StartCoroutine(FadeAudio(fadeTime, Fade.Out));
                    //SceneManager.LoadScene("main-level");
                    break;
                case 1:
                    Application.Quit();
                    break;
            }
        }
        if (can_select == true)
        {
            input_y = Input.GetAxisRaw("Vertical");
            if (input_y == -1f || input_y == 1f)
                changeIndex();
        }
        else
        {
            if (countdown > 0)
                countdown -= Time.deltaTime;
            else
                can_select = true;
        }

        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].GetComponent<Renderer>().material.color = deselected_color;
        }
        objectList[selected_index].GetComponent<Renderer>().material.color = selected_color;
    }

    void changeIndex()
    {
        if (input_y >= 0.1f)
        {
            if (selected_index + 1 > objectList.Count - 1)
            {
                selected_index = 0;
            }
            else
                ++selected_index;
        }
        else if (input_y <= -0.1f)
        {
            if (selected_index - 1 < 0)
            {
                selected_index = objectList.Count - 1;
            }
            else
                --selected_index;
        }
        can_select = false;
        countdown = delay;
    }

    IEnumerator FadeAudio(float timer, Fade fadeType)
    {
        float start = fadeType == Fade.In ? 0.0f : 1.0f;
        float end = fadeType == Fade.In ? 1.0f : 0.0f;
        float i = 0.0f;
        float step = 1.0f / timer; 

        while (i <= 1.0)
        {
            i += step * Time.deltaTime;
            GetComponent<AudioSource>().volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
        SceneManager.LoadScene("main-level");
    }
}
