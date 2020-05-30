using UnityEngine;
using Mirror;
using System.Collections;

public class Player : NetworkBehaviour
{
    private bool bMort = false;
    public bool Mort
    {
        get { return bMort; }
        protected set { bMort = value;}
    }

    [SerializeField]
    private int maxVie = 100; //On prend pour base que les joueurs ont 100 pv.

    [SyncVar] //Pour synchroniser la variable sur le serveur.
    private int nVieActuelle;

    [SerializeField]
    private Behaviour[] bDesactiveMort=null;
    private bool[] bEtaitActif;

    public void Setup()
    {
        bEtaitActif = new bool[bDesactiveMort.Length]; //on enregistre si le script était actif ou non avant la mort.
        for (int nI = 0; nI < bDesactiveMort.Length; nI++)
        {
            bEtaitActif[nI] = bDesactiveMort[nI].enabled;
        }

        SetDefaults();
    }

    //Cette fonction Update sera retirée lors de la release pour l'instant cela permet de se suicider en appuyant sur "k"
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K)) //Pour se suicider, pour accélérer les tests.
        {
            RpcDegatInflige(9999);
        }
    }

    [ClientRpc]
    public void RpcDegatInflige(int nDegats)
    {
        if(bMort)
        {
            return;
        }

        nVieActuelle -= nDegats;
        Debug.Log(transform.name + " a maintenant : " + nVieActuelle + " points de vie.");

        if (nVieActuelle<=0)
        {
            Mourir();
        }
    }

    private void Mourir()
    {
        bMort = true;
        //Desactiver les composants du joueur ici. (déplacements, tirs, etc...)
        for (int nI = 0; nI < bDesactiveMort.Length; nI++)
        {
            bDesactiveMort[nI].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
        
        Debug.Log(transform.name + " est mort.");

        //Appeler la fonction de respawn.

        StartCoroutine(Reapparition());
    }

    private IEnumerator Reapparition()
    {
        yield return new WaitForSeconds(GameManager.Instance.matchSettings.fTempsReapparition);
        SetDefaults();
        Transform tSpawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = tSpawnPoint.position;
        transform.rotation = tSpawnPoint.rotation;

        Debug.Log(transform.name + " est réapparu.");
    }

    public void SetDefaults()
    {
        bMort = false;
        nVieActuelle = maxVie;

        for (int nI = 0; nI < bDesactiveMort.Length; nI++) //On active les composants qui étaient actifs avant la mort.
        {
           bDesactiveMort[nI].enabled= bEtaitActif[nI];
        }

        Collider col = GetComponent<Collider>();
        if(col!=null)
        {
            col.enabled = true;
        }
    }
}
