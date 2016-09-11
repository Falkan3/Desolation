using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetActiveTile : MonoBehaviour {

    public int index;
    public byte list; // 0 = floor, 1 = walls, 2 = misc
    public GameObject tile;
    private Button button;

    public void setTile()
    {
        GameObject levelmanager = GameObject.Find("Level1Manager");
        TileEditor editor = levelmanager.GetComponent<TileEditor>();
        editor.ChangeActiveTile(index, tile);

    }

    public void changeSelf(GameObject obj)
    {
        tile = obj;
        button = GetComponent<Button>();
        button.image.sprite = obj.GetComponent<SpriteRenderer>().sprite;
    }

}
