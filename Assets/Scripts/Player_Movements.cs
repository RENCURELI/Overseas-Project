using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_Movements : MonoBehaviour
{
    private Vector3 vecVelocite; // vec et pas v pour avoir un nom différent de sa référence.
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Mouvement(Vector3 vVelocite)
    {
        vecVelocite = vVelocite;
    }

    private void FixedUpdate()
    {
        EffectuerMouvement();
    }

    private void EffectuerMouvement()
    {
        if (vecVelocite!= Vector3.zero)
        {
            rb.MovePosition(rb.position + vecVelocite * Time.fixedDeltaTime);
        }
    }
}
