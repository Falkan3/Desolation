using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetActiveTile : MonoBehaviour {

    public int index;
    public byte list; // 0 = floor, 1 = walls, 2 = misc
    public GameObject tile;
    public Color tilecolor;
    private Button button;
    private TileEditor editor;
    GameObject levelmanager;

    void Start()
    {
        levelmanager = GameObject.Find("Level1Manager");
        editor = levelmanager.GetComponent<TileEditor>();
    }

    public void setTile()
    {
        editor.ChangeActiveTile(index, tile, tilecolor);
    }

    public void changeSelf(GameObject obj, Color color)
    {
        tile = obj;
        tilecolor = color;
        button = GetComponent<Button>();
        button.image.sprite = obj.GetComponent<SpriteRenderer>().sprite;
    }

}
