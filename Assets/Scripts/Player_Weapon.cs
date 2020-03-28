using UnityEngine;

[System.Serializable] //Pour pouvoir charger et sauvegarder une classe d'arme.

public class Player_Weapon : MonoBehaviour
{
    public string sNomArme = "Base";
    public float fDommage = 10f;
    public float fPortee = 100f;
}