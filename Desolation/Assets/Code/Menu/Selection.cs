﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class Selection : MonoBehaviour {

    public List<GameObject> objectList;
    public GameObject level_selection; //Object to emerge when level selection is chosen
    public Emerge level_selection_emerge;
    private Selection_levels level_selection_script;
    public bool[] disabledIndexes;
    public Color deselected_color;
    public Color disabled_color;
    public Color selected_color;

    public int selected_index = 0;
    public bool disabled = false;
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
        checkIfDisabled(false);
        changeColor();

        if(level_selection != null)
        {
            level_selection_script = level_selection.GetComponent<Selection_levels>();
        }
    }
	
	void Update () {
        if (disabled == false)
        {
            if (Input.GetButtonDown("Submit"))
            {
                switch (selected_index)
                {
                    case 0:
                        /*
                        GetComponent<Screen_fade>().enabled = true;
                        GetComponent<Screen_fade>().delay = fadeTime;
                        StartCoroutine(FadeAudio(fadeTime, Fade.Out));
                        */
                        //SceneManager.LoadScene("main-level");
                        break;
                    case 1:
                        if (level_selection.GetComponent<Emerge>()==null)
                        {
                            disabled = true;
                            level_selection_emerge = level_selection.AddComponent<Emerge>() as Emerge;
                            level_selection_emerge.speed = 20f;
                            level_selection_emerge.enabled = true;
                            level_selection_script.enabled = true;
                        }
                        break;
                    case 2:
                        Application.Quit();
                        break;
                }
            }
            if (can_select == true)
            {
                input_y = Input.GetAxisRaw("Vertical");
                if (input_y <= -0.1f || input_y >= 0.1f)
                    changeIndex();
            }
            else
            {
                if (countdown > 0)
                    countdown -= Time.deltaTime;
                else
                    can_select = true;
            }
        }
    }

    void changeIndex()
    {
        if (input_y <= -0.1f)
        {
            if (selected_index + 1 > objectList.Count - 1)
            {
                selected_index = 0;
            }
            else
                ++selected_index;
            checkIfDisabled(false);
            
        }
        else if (input_y >= 0.1f)
        {
            if (selected_index - 1 < 0)
            {
                selected_index = objectList.Count - 1;
            }
            else
                --selected_index;
            checkIfDisabled(true);
        }
        can_select = false;
        countdown = delay;

        changeColor();
    }

    void checkIfDisabled(bool goUp)
    {
        if (goUp)
        {
            if (disabledIndexes[selected_index] == false)
            {
                if (selected_index - 1 < 0)
                {
                    selected_index = objectList.Count - 1;
                }
                else
                    selected_index--;
                checkIfDisabled(goUp);
            }
        }
        else
        {
            if (disabledIndexes[selected_index]==false)
            {
                if (selected_index + 1 > objectList.Count - 1)
                {
                    selected_index = 0;
                }
                else
                    selected_index++;
                checkIfDisabled(goUp);
            }
        }
    }

    void changeColor()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].GetComponent<Renderer>().material.color = deselected_color;
        }
        for(int i =0; i< disabledIndexes.Length; i++)
        {
            if(disabledIndexes[i]==false)
                objectList[i].GetComponent<Renderer>().material.color = disabled_color;
        }
        objectList[selected_index].GetComponent<Renderer>().material.color = selected_color;
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
