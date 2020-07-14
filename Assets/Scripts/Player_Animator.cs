using UnityEngine;
using Mirror;


[RequireComponent(typeof(NetworkAnimator))]
public class Player_Animator : MonoBehaviour
{
    NetworkAnimator NetAnim;
    // Start is called before the first frame update
    void Start()
    {
        NetAnim = GetComponent<NetworkAnimator>();
    }

    public void SetForward()
    {
        NetAnim.SetTrigger("Forward");
    }

    public void SetBackward()
    {
        NetAnim.SetTrigger("Backward");
    }

    public void SetRight()
    {
        NetAnim.SetTrigger("Right");
    }

    public void SetLeft()
    {
        NetAnim.SetTrigger("Left");
    }

    public void SetDead()
    {
        NetAnim.SetTrigger("Die");
    }

    public void SetJump()
    {
        //Debug.Log("Jump");
        NetAnim.SetTrigger("Jump");
    }

    public void SetShoot()
    {
        NetAnim.SetTrigger("Shoot");
    }
}
