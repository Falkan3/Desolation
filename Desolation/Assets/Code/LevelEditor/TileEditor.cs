using UnityEngine;
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

    //Editor buttons
    private List<GameObject> listOfFloorButtons;
    private int floorTileIndex = 0; //index for iterating through maintile list

    private int activeObjectIndex;
    private GameObject activeObject; //active tile to place
    private GameObject objtodestroy;

    
    private int index = 0; //index of tile to destroy
    //for left tile menu
    private Vector3 panelPosition;
    private int editorTileCount;
    private GameObject newTile;
    public GameObject menutile;
    List<GameObject> floorButtonList = new List<GameObject>();

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

        generateTileEditor();
    }
	
	// Update is called once per frame
	void Update () {
        ray.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        ray.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        ray.x = Mathf.Round(ray.x);
        ray.y = Mathf.Round(ray.y);

        editorTexture.transform.position = new Vector2(ray.x, ray.y);

        if (Input.GetMouseButtonDown(0))
            Debug.Log("Left click");

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

    void generateTileEditor()
    {
        panelPosition = GameObject.Find("Floor_panel").transform.position;
        editorTileCount = Mathf.FloorToInt(Camera.main.pixelHeight / 32f);
        Debug.Log(Camera.main.pixelHeight);
        for (int i = 0; i < editorTileCount; i++)
        {
            newTile = Instantiate(menutile, new Vector3(0, -i * 32 - 16), Quaternion.identity) as GameObject;
            Vector2 pos = new Vector2(0, panelPosition.y + (- i * 32 - 16));
            newTile.transform.SetParent(GameObject.Find("Floor_panel").transform);
            newTile.transform.localScale = new Vector3(1, 1, 1);
            newTile.transform.localPosition = pos;
            /*
            SetActiveTile script = newTile.GetComponent<SetActiveTile>();
            script.index=i;
            script.list = 0; // 0 = floor, 1 = walls, 2 = misc
            script.changeSelf(levelManager.Sprite_FloorTileList[tileIndex]); 
            tileIndex++;
            */
            floorButtonList.Add(newTile);
        }
        Debug.Log(floorButtonList.Count);
        populateMenu(floorButtonList, 0);
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

    void populateMenu(List<GameObject> listOfButtons, byte list)
    {
        //list 0 for floor, 1 for walls, 2 for misc;
        for (int i = 0; i < listOfButtons.Count; i++)
        {
            SetActiveTile script = listOfButtons[i].GetComponent<SetActiveTile>();
            script.index = floorTileIndex+i;
            script.list = list; // 0 = floor, 1 = walls, 2 = misc
            script.changeSelf(levelManager.Sprite_FloorTileList[floorTileIndex+i]);
        }
    }

    public void ChangeActiveTile(int ind, GameObject obj)
    {
        activeObjectIndex = ind;
        activeObject = obj;
        SpriteRenderer newSprite = obj.GetComponent<SpriteRenderer>();
        editorTextureRenderer.color = newSprite.color;
        editorTextureRenderer.sprite = newSprite.sprite;
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
