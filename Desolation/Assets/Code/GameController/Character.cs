using UnityEngine;
using System.Collections;

[System.Serializable]
public class Character
{
    public string name;
    public int level;
    public int experience;

    public Character()
    {
        this.name = "";
        this.level = 0;
        this.experience = 0;
    }
}
