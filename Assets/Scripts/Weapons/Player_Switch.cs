using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Mirror;

public class Player_Switch : NetworkBehaviour
{
    //numéro d'ndex de l'arme sélectionné
    public int selectedWeapon = 0;
    
    //Camera transform
    private Transform camT;

    //Liste des armes que le joueur possède
    public List<string> playerWeapon;
    
    // Start is called before the first frame update
    void Start()
    {
        camT = transform.GetChild(0);
        playerWeapon.Add("DefaultGun");
    }

    // Update is called once per frame
    void Update()
    {
        //Lors du scroll de la souris, changement d'arme
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            CmdSwitchWeapon();
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            CmdSwitchWeapon();
        }
    }

    [Command]
    public void CmdSwitchWeapon()
    {
        //Désactive l'arme déjà présente et active l'arme suivant
        camT.Find(playerWeapon[selectedWeapon]).gameObject.SetActive(false);
        selectedWeapon = (selectedWeapon+1) % playerWeapon.Count;
        camT.Find(playerWeapon[selectedWeapon]).gameObject.SetActive(true);
        
        //Change l'arme dans la gestion des tirs
        GetComponent<Player_Shoot>().arme = camT.Find(playerWeapon[selectedWeapon]).gameObject.GetComponent<Player_Weapon>();
        
        //Synchronisation Server-Client
        if (!isLocalPlayer)
        {
            RpcSyncSwitch(selectedWeapon);
        }
    }

    [ClientRpc]
    public void RpcSyncSwitch(int selected)
    {
        camT.Find(playerWeapon[selected]).gameObject.SetActive(true);
        GetComponent<Player_Shoot>().arme = camT.Find(playerWeapon[selected]).gameObject.GetComponent<Player_Weapon>();
    }
}
