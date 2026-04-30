using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class SceneCommunication
{
    static List<Player> playerList = new();

    public static void AddPlayers()
    {
        
	}

    public static void AddPlayer(int ID, CharacterCard characterCard, bool human)
    {
        Player player;
        if (human)
        {
            player = new HumanPlayer(ID, characterCard);
        }
        else
        {
            player = new AIPlayer(ID, characterCard);
        }

		playerList.Add(player);
	}

    public static List<Player> GetPlayers()
    {
        return playerList;
    }
}