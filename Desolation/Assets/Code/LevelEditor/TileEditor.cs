using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

public class TileEditor : MonoBehaviour {

    private Level levelManager;

    public string pathmain = "placeholder.png";
    public string pathmisc = "placeholdermisc.png";
    public Texture2D level;
    public Texture2D levelmisc;
    public GameObject editorTexture;
    private SpriteRenderer editorTextureRenderer;
    private Sprite editorDefaultSprite;
    private Vector2 ray;

    private List<Level.tile> maintiles = new List<Level.tile>();
    private List<Level.tile> misctiles = new List<Level.tile>();

    //Editor buttons
    private List<GameObject> listOfFloorButtons;
    private int floorTileIndex = 0; //index for iterating through maintile list

    private int layer;
    private int activeObjectIndex;
    private Color activeObjectColor;
    private Color emptycolor;
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

        editorDefaultSprite = editorTextureRenderer.sprite;

        generateTileEditor();
        layer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        ray.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        ray.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        ray.x = Mathf.Round(ray.x);
        ray.y = Mathf.Round(ray.y);

        editorTexture.transform.position = new Vector2(ray.x, ray.y);

        if (Input.GetMouseButtonDown(0))
        {
            if(activeObject!=null && activeObjectColor!=emptycolor)
            {
                switch (layer)
                {
                    case 0:
                        {
                            objtodestroy = maintiles.Where(item => item.x == ray.x && item.y == ray.y).Select(item => item.tileobj).FirstOrDefault();
                            index = maintiles.FindIndex(item => item.x == ray.x && item.y == ray.y);

                            Debug.Log(activeObjectColor);
                            PlaceNewTile(objtodestroy, activeObject, activeObjectColor, maintiles, index, layer, Mathf.RoundToInt(ray.x), Mathf.RoundToInt(ray.y));
                            break;
                        }
                    case 1:
                        {
                            objtodestroy = misctiles.Where(item => item.x == ray.x && item.y == ray.y).Select(item => item.tileobj).FirstOrDefault();
                            index = maintiles.FindIndex(item => item.x == ray.x && item.y == ray.y);

                            PlaceNewTile(objtodestroy, activeObject, activeObjectColor, misctiles, index, layer, Mathf.RoundToInt(ray.x), Mathf.RoundToInt(ray.y));
                            break;
                        }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (activeObject == null || activeObjectColor == emptycolor)
            {
                Debug.Log(ray.x + " " + ray.y);

                switch (layer)
                {
                    case 0:
                        {
                            objtodestroy = maintiles.Where(item => item.x == ray.x && item.y == ray.y).Select(item => item.tileobj).FirstOrDefault();
                            index = maintiles.FindIndex(item => item.x == ray.x && item.y == ray.y);
                            DestroyTile(objtodestroy, maintiles, index, layer);
                            break;
                        }
                    case 1:
                        {
                            objtodestroy = misctiles.Where(item => item.x == ray.x && item.y == ray.y).Select(item => item.tileobj).FirstOrDefault();
                            index = misctiles.FindIndex(item => item.x == ray.x && item.y == ray.y);
                            DestroyTile(objtodestroy, misctiles, index, layer);
                            break;
                        }
                }
            }
            else
            {
                ResetActiveTile();
            }
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

    void PlaceNewTile(GameObject obj, GameObject objToPlace, Color newColor, List<Level.tile> list, int index, int layer, int x, int y)
    {
        Debug.Log(obj + " index = " + index);
        if ((x >= 0 && y >= 0) && (x<level.width && y<level.height))
        {
            if (index != -1)
            {
                //list[index].tileobj = objToPlace;
                //Instantiate(objToPlace, new Vector3(x, y), Quaternion.identity);

                GameObject tileobj = Instantiate(objToPlace, new Vector3(x, y), Quaternion.identity) as GameObject;
                list.Add(new Level.tile(x, y, tileobj));
                DestroyTile(obj, list, index, layer);
            }
            else
            {
                GameObject tileobj = Instantiate(objToPlace, new Vector3(x, y), Quaternion.identity) as GameObject;
                list.Add(new Level.tile(x, y, tileobj));
            }
            switch (layer)
            {
                case 0:
                    {
                        /*
                            if (index != -1)
                                level.SetPixel(list[index].x, list[index].y, newColor);
                            else*/
                        level.SetPixel(x, y, newColor);
                        break;
                    }
                case 1:
                    {
                        levelmisc.SetPixel(list[index].x, list[index].y, newColor);
                        break;
                    }
            }
        }
        else
        {
            Debug.Log("Cannot place on negative indexes or exceeding level bounds! (bounds: " + level.width + " " + level.height + ")");
        }

    }

    void DestroyTile(GameObject obj, List<Level.tile> list, int index, int layer)
    {

        Debug.Log(obj);
        if (index != -1)
        {
            switch (layer)
            {
                case 0:
                    {
                        if (level != null)
                        {
                            level.SetPixel(list[index].x, list[index].y, new Color(0f, 0f, 0f, 0f));
                        }
                        break;
                    }
                case 1:
                    {
                        if (levelmisc != null)
                        {
                            levelmisc.SetPixel(list[index].x, list[index].y, new Color(0f, 0f, 0f, 0f));
                        }
                        break;
                    }
            }
            list.RemoveAt(index);
        }
        Destroy(obj);
    }

    void populateMenu(List<GameObject> listOfButtons, byte list)
    {   
        //list 0 for floor, 1 for walls, 2 for misc;
        for (int i = 0; i < listOfButtons.Count; i++)
        {
            SetActiveTile script = listOfButtons[i].GetComponent<SetActiveTile>();
            script.index = floorTileIndex+i;
            script.list = list; // 0 = floor, 1 = walls, 2 = misc
            script.changeSelf(levelManager.Sprite_FloorTileList[floorTileIndex+i], levelManager.Color_FloorTileList[floorTileIndex + i]);
        }
    }

    public void ChangeActiveTile(int ind, GameObject obj, Color color)
    {
        activeObjectIndex = ind;
        activeObject = obj;
        activeObjectColor = color;
        SpriteRenderer newSprite = obj.GetComponent<SpriteRenderer>();
        editorTextureRenderer.color = newSprite.color;
        editorTextureRenderer.sprite = newSprite.sprite;
    }

    public void ResetActiveTile()
    {
        activeObjectIndex = -1;
        activeObject = null;
        activeObjectColor = emptycolor;
        editorTextureRenderer.color = new Color(editorTextureRenderer.color.r, editorTextureRenderer.color.g, editorTextureRenderer.color.b, 0.5f);
        editorTextureRenderer.sprite = editorDefaultSprite;
    }

    public void saveLevel()
    {
        //pathmain = EditorUtility.SaveFilePanel("Save main level texture", "", level.name + ".png", "png");
        //pathmisc = EditorUtility.SaveFilePanel("Save misc level texture", "", levelmisc.name + ".png", "png");

        byte[] bytes = level.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Levels/" + pathmain, bytes);

        byte[] bytesmisc = levelmisc.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Levels/" + pathmisc, bytesmisc);

        applyLevel();
    }

    public void applyLevel()
    {
        /*
        string[] stringSeparators = new string[] { "Assets/" };
        string[] result; string[] resultmisc;
        result = pathmain.Split(stringSeparators, System.StringSplitOptions.None);
        resultmisc = pathmisc.Split(stringSeparators, System.StringSplitOptions.None);
        */

        string pathtomain;
        string pathtomisc;

        pathtomain = "Assets/Levels/" + pathmain;
        pathtomisc = "Assets/Levels/" + pathmisc;

        Debug.Log(pathtomain);

        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(pathtomain);
        AssetDatabase.ImportAsset(pathtomisc);

        TextureImporter importermain = AssetImporter.GetAtPath(pathtomain) as TextureImporter;
        TextureImporter importermisc = AssetImporter.GetAtPath(pathtomisc) as TextureImporter;

        importermain.textureType = TextureImporterType.Advanced;
        importermain.mipmapFilter = TextureImporterMipFilter.BoxFilter;
        importermain.wrapMode = TextureWrapMode.Clamp;
        importermain.filterMode = FilterMode.Point;
        importermain.textureFormat = TextureImporterFormat.RGBA32;
        importermain.isReadable = true;

        importermisc.textureType = TextureImporterType.Advanced;
        importermisc.mipmapFilter = TextureImporterMipFilter.BoxFilter;
        importermisc.wrapMode = TextureWrapMode.Clamp;
        importermisc.filterMode = FilterMode.Point;
        importermisc.textureFormat = TextureImporterFormat.RGBA32;
        importermisc.isReadable = true;

        AssetDatabase.WriteImportSettingsIfDirty(pathtomain);
        AssetDatabase.WriteImportSettingsIfDirty(pathtomisc);

        //level.LoadImage(bytes);
        //levelmisc.LoadImage(bytesmisc);
    }

    public void loadLevel()
    {
        string loadfilepath = EditorUtility.OpenFilePanel("Load a main map", Application.dataPath + "/Levels/", "png");
        string loadfilepathmisc = EditorUtility.OpenFilePanel("Load a misc map", Application.dataPath + "/Levels/", "png");

        string[] stringSeparators = new string[] { "/" };
        string[] result;
        result = loadfilepath.Split(stringSeparators, System.StringSplitOptions.None);
        pathmain = result[result.Length-1];
        result = loadfilepathmisc.Split(stringSeparators, System.StringSplitOptions.None);
        pathmisc = result[result.Length-1];

        GameObject inputFieldGo = GameObject.Find("InputField");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        stringSeparators = new string[] { "." };
        result = pathmain.Split(stringSeparators, System.StringSplitOptions.None);
        inputFieldCo.text = result[0];

        if (loadfilepath.Length != 0)
        {
            byte[] bytes = File.ReadAllBytes(loadfilepath);
            Texture2D tempmaintexture = new Texture2D(128, 128);
            tempmaintexture.LoadImage(bytes);
            level = tempmaintexture;
            level.name = pathmain;
            levelManager.levelTexture = tempmaintexture;
            levelManager.levelTexture.name = pathmain;
        }

        if (loadfilepathmisc.Length != 0)
        {
            byte[] bytesmisc = File.ReadAllBytes(loadfilepathmisc);
            Texture2D tempmisctexture = new Texture2D(128, 128);
            tempmisctexture.LoadImage(bytesmisc);
            levelmisc = tempmisctexture;
            levelmisc.name = pathmisc;
            levelManager.levelMiscTexture = tempmisctexture;
            levelManager.levelMiscTexture.name = pathmisc;
        }

        try
        {
            foreach (Level.tile tile in maintiles)
            {
                Destroy(tile.tileobj);
            }
            maintiles.Clear();
            foreach (Level.tile tile in misctiles)
            {
                Destroy(tile.tileobj);
            }
            misctiles.Clear();

            levelManager.loadLevel();
            levelManager.loadMisc();

            Debug.Log("Loaded " + pathmain + " | " + pathmisc);
        }
        catch (System.Exception)
        {
            Debug.Log("Failed to load selected map!");
        }
    }

    public void SetMapName(string name)
    {
        pathmain = name + ".png";
        pathmisc = name + "misc.png";
    }

    public int upper_power_of_two(int v)
    {
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;
        return v;
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
