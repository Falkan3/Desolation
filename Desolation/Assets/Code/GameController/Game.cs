using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game {
    public static Game current;
    public Character player;

    public Game()
    {
        player = new Character();
    }
}
