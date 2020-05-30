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
        Vector3 dropPosition = new Vector3(transform.position.x, 1, transform.position.z);
        GameObject weaponDropped = Instantiate(GetComponent<Player_Shoot>().arme.pickable, dropPosition, transform.rotation);
        NetworkServer.Spawn(weaponDropped);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        GetComponent<Player_Shoot>().arme = null;
        RpcSyncDrop();
    }

    [ClientRpc]
    public void RpcSyncDrop()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        GetComponent<Player_Shoot>().arme = null;
    }
}
