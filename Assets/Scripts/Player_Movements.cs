using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_Movements : MonoBehaviour
{
    private Vector3 vecVelocite; // vec et pas v pour avoir un nom différent de sa référence.
    private Vector3 vecRotation;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Mouvement(Vector3 vVelocite)
    {
        vecVelocite = vVelocite;
    }

    public void Rotation(Vector3 vRotation)
    {
        vecRotation = vRotation;
    }

    private void FixedUpdate()
    {
        EffectuerMouvement();
        EffectuerRotation();
    }

    private void EffectuerMouvement()
    {
        if (vecVelocite!= Vector3.zero)
        {
            rb.MovePosition(rb.position + vecVelocite * Time.fixedDeltaTime);
        }
    }

    private void EffectuerRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(vecRotation));
    }
}
