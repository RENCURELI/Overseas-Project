using UnityEngine;
using Mirror;

public class Player_Shoot : NetworkBehaviour
//Gestion des tirs des joueurs.
{
    public Player_Weapon arme;
    public GameObject bulletPrefab;
    [SerializeField]
    private Camera cam; //On fait le tir depuis le centre de la caméra du joueur.

    [SerializeField]
    private LayerMask mask; //Pour stocker les layers touchables avec les tirs.

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
            TirProjectile();
        }
    }

    [Client] //On assigne cette fonction en tant que fonction client.
    private void Tir()
    {

        RaycastHit touche;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out touche, arme.fPortee, mask))
        {
            //Si on touche quelque chose, notre RaycasHit touche contient toutes les infos de ce qu'on a touché.
            Debug.Log("Objet touché : " + touche.collider.name); //Cette information est envoyée dans la console locale (donc pas sur le serveur).
            if(touche.collider.tag=="Player")
            {
                CmdTirJoueur(touche.collider.name, arme.nDommage); //Cette information est envoyée dans la console serveur, car comprise dans une fonction command.
            }
        }

        
    }


    private void TirProjectile()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, cam.transform.position + cam.transform.forward, cam.transform.rotation);
        //bulletInstance.transform.SetPositionAndRotation(cam.transform.position + cam.transform.forward, cam.transform.rotation);
        NetworkServer.Spawn(bulletInstance);
    }

    [Command] //On assigne cette fonction en tant que fonction serveur.
    private void CmdTirJoueur (string IDJoueur, int nDommage)
    {
        Debug.Log(IDJoueur + " a été touché pour ."+nDommage+" dégâts");

        Player pJoueur = GameManager.GetPlayer(IDJoueur);
        pJoueur.RpcDegatInflige(nDommage);
    }

}
