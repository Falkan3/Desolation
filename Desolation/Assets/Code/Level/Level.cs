using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    [System.Serializable]
    public class tile
    {
        public int x, y;
        public GameObject tileobj;
        public Color color;

        public tile(int x_coordinate, int y_coordinate, GameObject obj, Color col)
        {
            x = x_coordinate;
            y = y_coordinate;
            tileobj = obj;
            color = col;
        }
    }

    [System.Serializable]
    public class map
    {
        public Texture2D mainLevelTexture;
        public Texture2D miscLevelTexture;

        public map(Texture2D mainlt, Texture2D misclt)
        {
            mainLevelTexture = mainlt;
            miscLevelTexture = misclt;
        }
    }

    [System.Serializable]
    public class scenario
    {
        public map levelMap;
        public tile tileobj;

        public scenario(tile ntile, map nmap)
        {
            tileobj = ntile;
            levelMap = nmap;
        }
    }

    private int levelWidth;
    private int levelHeight;

    //LEVELORDER
    public List<scenario> scenarioList = new List<scenario>();

    //create lists of tiles
    public List<tile> maintilelist = new List<tile>();
    public List<tile> misctilelist = new List<tile>();
    private GameObject tileobj;

    //Array of tile colors corresponding to a particular tile
    private Color[] tileColors;

    //Texture containing information about tile layout
    public Texture2D levelTexture;
    public Texture2D levelMiscTexture;

    public Entity player;

    public GameObject spawnPlatform1;
    public Color spawnPointColour;

    //TILES
    public List<GameObject> Sprite_FloorTileList;
    public List<GameObject> Sprite_WallTileList;

    //COLORS
    public List<Color> Color_FloorTileList;
    public List<Color> Color_WallTileList;

    //MISC
    public List<GameObject> Sprite_MiscList;
    public List<Color> Color_Misc;


    // Use this for initialization
    void Start()
    {
        LoadPrefabs();
        if (levelTexture != null)
        {
            loadLevel();
            loadMisc();
        }
        //Debug.Log(maintilelist.Count);
    }

    public void loadLevel()
    {
        levelWidth = levelTexture.width;
        levelHeight = levelTexture.height;

        tileColors = new Color[levelWidth * levelHeight];
        tileColors = levelTexture.GetPixels();

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                if (spawnPointColour == tileColors[x + y * levelWidth])
                {
                    tileobj = Instantiate(spawnPlatform1, new Vector3(x, y), Quaternion.identity) as GameObject;
                    Vector2 pos = new Vector2(x, y);
                    maintilelist.Add(new tile(x, y, tileobj, spawnPointColour));
                    if (player != null)
                        player.transform.position = pos;
                }

                for (int i = 0; i < Color_FloorTileList.Count; i++)
                {
                    if (Color_FloorTileList[i] == (tileColors[x + y * levelWidth]))
                    {
                        tileobj = Instantiate(Sprite_FloorTileList[i], new Vector3(x, y), Quaternion.identity) as GameObject;
                        maintilelist.Add(new tile(x, y, tileobj, Color_FloorTileList[i]));
                    }
                }

                for (int i = 0; i < Color_WallTileList.Count; i++)
                {
                    if (Color_WallTileList[i] == (tileColors[x + y * levelWidth]))
                    {
                        tileobj = Instantiate(Sprite_WallTileList[i], new Vector3(x, y), Quaternion.identity) as GameObject;
                        maintilelist.Add(new tile(x, y, tileobj, Color_WallTileList[i]));
                    }
                }
            }
        }

    }

    public void loadMisc()
    {
        tileColors = new Color[levelWidth * levelHeight];
        tileColors = levelMiscTexture.GetPixels();

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                for (int i = 0; i < Color_Misc.Count; i++)
                {
                    if (Color_Misc[i] == (tileColors[x + y * levelWidth]))
                    {
                        tileobj = Instantiate(Sprite_MiscList[i], new Vector3(x, y), Quaternion.identity) as GameObject;
                        misctilelist.Add(new tile(x, y, tileobj, Color_Misc[i]));
                    }
                }
            }
        }
    }

    public void LoadPrefabs(string mainfolder = "Prefabs/Tiles")
    {
        GameObject[] prefabFloor; GameObject[] prefabWalls; GameObject[] prefabMiscFloor; GameObject[] prefabMisc;
        prefabFloor = Resources.LoadAll<GameObject>(mainfolder + "/1/Floor");
        prefabWalls = Resources.LoadAll<GameObject>(mainfolder + "/1/Walls");
        prefabMiscFloor = Resources.LoadAll<GameObject>(mainfolder + "/1/MiscFloor");
        prefabMisc = Resources.LoadAll<GameObject>(mainfolder + "/1/Misc");

        for(int i=0; i<prefabFloor.Length; i++)
        {
            Sprite_FloorTileList.Add(prefabFloor[i]);
            Color_FloorTileList.Add(new Color(0, 0, 1f - ((float)Color_FloorTileList.Count / 255f)));
        }
        for (int i = 0; i < prefabWalls.Length; i++)
        {
            Sprite_WallTileList.Add(prefabWalls[i]);
            Color_WallTileList.Add(new Color(0, 1f - ((float)Color_WallTileList.Count / 255f), 0));
        }
        for (int i = 0; i < prefabMiscFloor.Length; i++)
        {
            Sprite_MiscList.Add(prefabMiscFloor[i]);
            Color_Misc.Add(new Color(1f - ((float)Color_Misc.Count / 255f), 0, 0));
        }
        for (int i = 0; i < prefabMisc.Length; i++)
        {
            Sprite_MiscList.Add(prefabMisc[i]);
            Color_Misc.Add(new Color(1f - ((float)Color_Misc.Count / 255f), 0, 0));
        } 
    }

    /*
    int findGreatest(int[] number)
    {
        int output;
        int count = number.Length;

        if (count > 0)
        {
            if (count > 1)
            {
                output = number[0];

                for (int i = 1; i < count; i++)
                {
                    if (number[i] > output)
                    {
                        output = number[i];
                    }
                }
            }
            else
                output = number[0];
        }
        else
            output = 0;

        return output;
    }
    */
}
