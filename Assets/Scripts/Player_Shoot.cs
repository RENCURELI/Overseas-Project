using UnityEngine;
using Mirror;
using System;
using System.Collections;
using System.Reflection;

public class Player_Shoot : NetworkBehaviour
//Gestion des tirs des joueurs.
{
    private bool allowFire = true;
    public Player_Weapon arme;
    public GameObject bulletPrefab;
    [SerializeField]
    private Camera cam = null; //On fait le tir depuis le centre de la caméra du joueur.

    [SerializeField]
    private LayerMask mask = default; //Pour stocker les layers touchables avec les tirs.

    Player_Animator Animateur;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("Pas de caméra référencée pour ce joueur.");
            this.enabled = false; //On désactive le script pour éviter les erreurs.
        }
        Animateur = gameObject.GetComponent<Player_Animator>();
    }

    void Update()
    {
        //On garde le principe d'une arme semi-automatique, on tire les balles une par une pour l'instant.
        if (Input.GetButton("Fire1") && arme != null && allowFire) //Fire1 est le clique gauche de la souris.
        {
            StartCoroutine(arme.sNomArme);
        }
    }

    [Client] //On assigne cette fonction en tant que fonction client.
    public IEnumerator DefaultGun()
    {
        allowFire = false;
        CmdTirProjectile();
        RaycastHit touche;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out touche, arme.fPortee, mask))
        {
            Debug.DrawLine(cam.transform.position, touche.point, Color.red, 5f);
            //Si on touche quelque chose, notre RaycasHit touche contient toutes les infos de ce qu'on a touché.
            Debug.Log("Objet touché : " + touche.collider.name); //Cette information est envoyée dans la console locale (donc pas sur le serveur).
            if (touche.collider.CompareTag("Player"))
            {
                CmdTirJoueur(touche.collider.name, arme.nDommage); //Cette information est envoyée dans la console serveur, car comprise dans une fonction command.
            }
        }
        yield return new WaitForSeconds(arme.fFirerate);
        allowFire = true;
    }

    public IEnumerator SubmachineGun()
    {
        StartCoroutine(nameof(DefaultGun));
        yield return null;
    }


    [Command] //On assigne cette fonction en tant que fonction serveur.
    public void CmdTirJoueur(string IDJoueur, int nDommage)
    {
        Debug.Log(IDJoueur + " a été touché pour ." + nDommage + " dégâts");

        Player pJoueur = GameManager.GetPlayer(IDJoueur);
        pJoueur.RpcDegatInflige(nDommage);
    }

    [Command]
    public void CmdTirProjectile()
    {
        Vector3 bulletSpawnPos = cam.transform.GetChild(0).GetChild(0).position;

        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPos, cam.transform.rotation);
        NetworkServer.Spawn(bulletInstance);
        Animateur.SetForward();
    }
}

