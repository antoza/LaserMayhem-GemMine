using UnityEngine;

public class PlayerData : ScriptableObject
{
    public int PlayerID;

    public PlayerData(int playerID)
    {
        PlayerID = playerID;
    }
}