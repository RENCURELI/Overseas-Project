using UnityEngine;

[System.Serializable] //Pour pouvoir charger et sauvegarder une classe d'arme.

public class Player_Weapon : MonoBehaviour
//Création d'une classe de base pour les armes.
{
    public string sNomArme = "Base";
    public int nDommage = 10;
    public float fPortee = 100f;
}