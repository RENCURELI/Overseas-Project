using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player_Pickup : NetworkBehaviour
{

    void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<PickUpWeapon>())
        {
            if ((Input.GetKeyDown(KeyCode.F)))
            {
                CmdPickupWeapon(collider.gameObject);
            }
        }

    }

    [Command]
    public void CmdPickupWeapon(GameObject collider)
    {
        string weaponName = collider.gameObject.GetComponent<PickUpWeapon>().arme.sNomArme;
        transform.GetChild(0).Find(weaponName).gameObject.SetActive(true);
        GetComponent<Player_Shoot>().arme = transform.GetChild(0).Find(weaponName).GetComponent<Player_Weapon>();
        NetworkServer.UnSpawn(collider.gameObject);
        Destroy(collider.gameObject);

            RpcSyncPickup(weaponName);
        
    }

    [ClientRpc]
    public void RpcSyncPickup(string weaponName)
    {
        transform.GetChild(0).Find(weaponName).gameObject.SetActive(true);
        GetComponent<Player_Shoot>().arme = transform.GetChild(0).Find(weaponName).GetComponent<Player_Weapon>();
        GetComponent<Player_Switch>().playerWeapon.Add(weaponName);
        GetComponent<Player_Switch>().selectedWeapon++;
    }
}
