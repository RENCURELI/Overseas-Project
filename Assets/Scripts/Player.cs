using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxVie = 100; //On prend pour base que les joueurs ont 100 pv.

    [SyncVar] //Pour synchroniser la variable sur le serveur.
    private int nVieActuelle;

    private void Awake()
    {
        SetDefaults();
    }

    public void DegatInflige(int nDegats)
    {
        nVieActuelle -= nDegats;
        Debug.Log(transform.name + " a maintenant : " + nVieActuelle + " points de vie.");
    }

    public void SetDefaults()
    {
        nVieActuelle = maxVie;
    }
}
