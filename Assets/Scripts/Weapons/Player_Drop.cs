using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player_Drop : NetworkBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            CmdDropWeapon();
        }
    }

    [Command]
    public void CmdDropWeapon()
    {
        Player_Weapon arme = GetComponent<Player_Shoot>().arme;
        Vector3 dropPosition = new Vector3(transform.position.x, 1, transform.position.z);
        GameObject weaponDropped = Instantiate(arme.pickable, dropPosition, transform.rotation);
        NetworkServer.Spawn(weaponDropped);
        GetComponent<Player_Switch>().playerWeapon.Remove(arme.sNomArme);
        transform.GetChild(0).Find(arme.sNomArme).gameObject.SetActive(false);
        GetComponent<Player_Shoot>().arme = null;
        if (!isLocalPlayer)
        {
            RpcSyncDrop();
        }
    }

    [ClientRpc]
    public void RpcSyncDrop()
    {
        transform.GetChild(0).Find(GetComponent<Player_Shoot>().arme.sNomArme).gameObject.SetActive(false);
        GetComponent<Player_Shoot>().arme = null;
        GetComponent<Player_Switch>().playerWeapon.Remove(GetComponent<Player_Shoot>().arme.sNomArme);
    }
}
