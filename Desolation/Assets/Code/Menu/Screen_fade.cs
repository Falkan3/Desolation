﻿using UnityEngine;
using System.Collections;

public class Screen_fade : MonoBehaviour {

    public Texture2D fadeTexture;
    float drawDepth = -1000f;
    private float alpha = 0.0f;
    private float fadeDir = -1f;
    public float delay;

    // Use this for initialization
    void Start () {
        this.enabled = false;
	}

    void OnGUI()
    {
            float fadeSpeed = 1.0f / delay;

            alpha -= fadeDir * fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

            GUI.depth = (int)drawDepth;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }
}
