﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileEditor : MonoBehaviour {

    private Level levelManager;

    public Texture2D level;
    public Texture2D levelmisc;
    public GameObject editorTexture;
    private SpriteRenderer editorTextureRenderer;
    private Vector2 ray;
    private List<Level.tile> maintiles = new List<Level.tile>();
    private List<Level.tile> misctiles = new List<Level.tile>();

    private GameObject activeObject;
    private GameObject objtodestroy;
    private int index;

    // Use this for initialization
    void Start () {
        levelManager = GetComponent<Level>();
        if(level==null)
            level = levelManager.levelTexture;
        if (levelmisc == null)
            levelmisc = levelManager.levelMiscTexture;
        editorTextureRenderer = editorTexture.GetComponent<SpriteRenderer>();
        editorTextureRenderer.color = new Color(editorTextureRenderer.color.r, editorTextureRenderer.color.g, editorTextureRenderer.color.b, 0.5f);
        //levelManager.loadLevel();

        maintiles = levelManager.maintilelist;
        misctiles = levelManager.misctilelist;
        Debug.Log(maintiles.Count);
        Debug.Log(misctiles.Count);
    }
	
	// Update is called once per frame
	void Update () {
        ray.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        ray.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        ray.x = Mathf.Round(ray.x);
        ray.y = Mathf.Round(ray.y);

        editorTexture.transform.position = new Vector2(ray.x, ray.y);

        if (Input.GetMouseButtonDown(0))
            Debug.Log("left click");

        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log(levelManager.maintilelist.First(item => item.x == ray.x).tileobj);
            Debug.Log(ray.x + " " + ray.y);
            Debug.Log(Mathf.Round(ray.x) + " " + Mathf.Round(ray.y));

            objtodestroy = maintiles.Where(item => item.x == Mathf.Round(ray.x) && item.y == Mathf.Round(ray.y)).Select(item => item.tileobj).FirstOrDefault();
            index = maintiles.FindIndex(item => item.x == Mathf.Round(ray.x) && item.y == Mathf.Round(ray.y));
            DestroyTile(objtodestroy, maintiles, index);

            objtodestroy = misctiles.Where(item => item.x == Mathf.Round(ray.x) && item.y == Mathf.Round(ray.y)).Select(item => item.tileobj).FirstOrDefault();
            index = maintiles.FindIndex(item => item.x == Mathf.Round(ray.x) && item.y == Mathf.Round(ray.y));
            DestroyTile(objtodestroy, misctiles, index);
        }

        /*
        Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        editorTexture.transform.position = new Vector3(v3.x,v3.y,0);
        */
    }

    void DestroyTile(GameObject obj, List<Level.tile> list, int index)
    {
        try
        {
            Debug.Log(obj);
            list.RemoveAt(index);
            Destroy(obj);
        }
        catch (System.Exception) { }
    }

    /*
    GameObject HoverSelect()
    {
        //Converting Mouse Pos to 2D (vector2) World Pos
        Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 0f);

        if (hit)
        {
            Debug.Log(hit.transform.name);
            //editorTexture.transform.localScale = new Vector2(hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x, hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
            editorTexture.transform.position = new Vector2(hit.transform.gameObject.transform.position.x, hit.transform.gameObject.transform.position.y);
            return hit.transform.gameObject;
        }
        else return null;
    }
    */
}
