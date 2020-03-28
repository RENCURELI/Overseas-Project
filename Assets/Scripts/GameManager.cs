using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
//Premier jet pour un GameManager pour stocker les joueurs en jeu et partager les informations.
{
    private const string PREFIX_ID_JOUEUR = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>(); //On crée un dictionnaire pour stocker les joueurs.

    public static void EnregistrerJoueur(string netID, Player joueurScript)
    {
        string playerID = PREFIX_ID_JOUEUR + netID;
        players.Add(playerID, joueurScript);
        joueurScript.transform.name = playerID; //Cette ligne permet de renommer le joueur.
    }

    public static void DesenregisterJoueur(string JoueurID)
    {
        players.Remove(JoueurID);
    }

    public static Player GetPlayer(string sIDJoueur)
    {
        return players[sIDJoueur];
    }

    //Cette fonction peut être mise en commentaire, c'est juste une aide au développement.
    private void OnGUI() //Notre UI de développement.
    {
        GUILayout.BeginArea(new Rect(0, 0, 200, 500));
        GUILayout.BeginVertical();

        foreach(string JoueurID in players.Keys)
        {
            GUILayout.Label(JoueurID + "  -  " + players[JoueurID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
