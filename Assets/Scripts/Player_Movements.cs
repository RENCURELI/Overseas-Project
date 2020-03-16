using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Camera))]

public class Player_Movements : MonoBehaviour
{
    [SerializeField]
    private Camera camJoueur;

    private Vector3 vecVelocite; // vec et pas v pour avoir un nom différent de sa référence.
    private Vector3 vecRotation;
    private Vector3 vecRotationCamera;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        camJoueur = GetComponentInChildren <Camera>();
    }

    public void Mouvement(Vector3 vVelocite)
    {
        vecVelocite = vVelocite;
    }

    public void Rotation(Vector3 vRotation)
    {
        vecRotation = vRotation;
    }

    public void RotationCamera(Vector3 vRotationCamera)
    {
        vecRotationCamera = vRotationCamera;
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
        camJoueur.transform.Rotate(-vecRotationCamera);
    }
}
