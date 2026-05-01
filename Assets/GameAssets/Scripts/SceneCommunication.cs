using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c> SceneCommunication </c> is a class which allows information to be passed between the menu and game scenes.
/// </summary>
public static class SceneCommunication
{
    static List<Player> playerList = new();

    /// <summary>
    /// Adds a player to the playerlist.
    /// </summary>
    /// <param name="ID">The player's ID.</param>
    /// <param name="characterCard">The card the player will be using.</param>
    /// <param name="human">Whether the player is human or not.</param>
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

    /// <summary>
    /// Returns the list of players.
    /// </summary>
    /// <returns>The list of players.</returns>
    public static List<Player> GetPlayers()
    {
        return playerList;
    }
}