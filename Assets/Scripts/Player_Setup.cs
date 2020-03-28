using UnityEngine;
//using UnityEngine.Networking;
using Mirror; //Unet étant déprécié, Mirror le remplace.

public class Player_Setup : NetworkBehaviour
//BUT : Lorsqu'un joueur apparait, désactiver les contrôles, caméras et audios des autres joueurs.
//ENTREE : Les scripts de contrôle et les caméras des autres joueurs à désactiver.
//SORTIE : Les composants non désirés désactivés.
{
    [SerializeField]
    Behaviour[] composantDesactivable; //On place les scripts et composants à désactiver ici depuis unity.

    [SerializeField] //Pour pouvoir choisir quel Layer on assigne aux autres joueurs.
    private string NomLayerAutre = "RemotePlayer";

    Camera Main_Camera; //On récupère également la caméra principale pour la désactiver quand le joueur apparait, ou la réactiver quand il disparait.

    private void Start()
    {
        if (!isLocalPlayer) //isLocalPlayer est une variable booléenne relative à la classe NetworkBehaviour
        {
            DesactiverComposants();
            AssignerLayerAutreJoueur();
        }
        else //Si on est sur notre joueur local, on désactive la caméra globable aussi.
        {
            Main_Camera = Camera.main;
            if (Main_Camera != null)
            {
                Main_Camera.gameObject.SetActive(false);
            }
        }
        EnregistrerJoueur();
    }

    private void EnregistrerJoueur() //Penser à plus tard enregistrer les joueurs dans un dictionnaire. 
    {
        string IDPlayer = "Joueur " + GetComponent<NetworkIdentity>().netId;
        transform.name = IDPlayer;
    }

    private void DesactiverComposants()
    {
        //Boucle pour désactiver les composants des autres joueurs sur notre instance.
        for (int nI = 0; nI < composantDesactivable.Length; nI++)
        {
            composantDesactivable[nI].enabled = false;
        }
    }
    private void AssignerLayerAutreJoueur()
    {
        gameObject.layer = LayerMask.NameToLayer(NomLayerAutre); //On assigne le Layer aux autres joueurs.
    }

    private void OnDisable()    //Si le joueur se déconnecte il a quand même la caméra globale lancée
    {                           //Note : Ce sera sans doute remplacé plus tard par l'écran de connexion.
        if(Main_Camera!=null)
        {
            Main_Camera.gameObject.SetActive(true);
        }
    }
}
