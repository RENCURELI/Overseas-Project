using UnityEngine;
using Mirror;

public class Player_Shoot : NetworkBehaviour
//Gestion des tirs des joueurs.
{
    public Player_Weapon arme;

    [SerializeField]
    private Camera cam=null; //On fait le tir depuis le centre de la caméra du joueur.

    [SerializeField]
    private LayerMask mask=default; //Pour stocker les layers touchables avec les tirs.

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("Pas de caméra référencée pour ce joueur.");
            this.enabled = false; //On désactive le script pour éviter les erreurs.
        }
    }

    void Update()
    {
        //On garde le principe d'une arme semi-automatique, on tire les balles une par une pour l'instant.
        if (Input.GetButtonDown("Fire1")) //Fire1 est le clique gauche de la souris.
        {
            Tir();
        }
    }

    [Client] //On assigne cette fonction en tant que fonction client.
    private void Tir()
    {
        RaycastHit touche;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out touche, arme.fPortee, mask))
        {
            Debug.DrawLine(cam.transform.position, touche.point, Color.red, 5f);
            //Si on touche quelque chose, notre RaycasHit touche contient toutes les infos de ce qu'on a touché.
            Debug.Log("Objet touché : " + touche.collider.name); //Cette information est envoyée dans la console locale (donc pas sur le serveur).
            if(touche.collider.tag=="Player")
            {
                CmdTirJoueur(touche.collider.name, arme.nDommage); //Cette information est envoyée dans la console serveur, car comprise dans une fonction command.
            }
        }
    }

    [Command] //On assigne cette fonction en tant que fonction serveur.
    private void CmdTirJoueur (string IDJoueur, int nDommage)
    {
        Debug.Log(IDJoueur + " a été touché pour ."+nDommage+" dégâts");

        Player pJoueur = GameManager.GetPlayer(IDJoueur);
        pJoueur.RpcDegatInflige(nDommage);
    }
}
