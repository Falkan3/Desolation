using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class Selection_levels : MonoBehaviour {

    public GameObject parent;
    public Selection parent_selection;

    public List<GameObject> objectList;
    public bool[] disabledIndexes;
    public Color deselected_color;
    public Color disabled_color;
    public Color selected_color;

    public int selected_index = 0;
    private float countdown = 0;
    private bool can_select = true;
    public float delay = 0.5f;
    float input_y;

    void Start () {
        parent_selection = parent.GetComponent<Selection>();
        GetComponent<Emerge>().enabled = false;

        this.enabled = false;
        checkIfDisabled(false);
        changeColor();
    }

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            switch (selected_index)
            {
                case 0:
                    break;
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (GetComponent<Emerge>() == null)
            {
                Emerge sc = gameObject.AddComponent<Emerge>() as Emerge;
                sc.speed = -25f;
                sc.enabled = true;
                parent_selection.disabled = false;
                enabled = false;
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
}
