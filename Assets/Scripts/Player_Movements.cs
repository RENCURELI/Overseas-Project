using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Camera))]

public class Player_Movements : MonoBehaviour
{
    [SerializeField]
    private Camera camJoueur;

    private Vector3 vecVelocite; // vec et pas v pour avoir un nom différent de sa référence.
    private Vector3 vecRotation;
    private Vector3 vecForceSaut;
    private float fRotationCameraX=0f;
    private float fRotationActuelleCamerax = 0f;

    private bool bSautPossible = true;

    [SerializeField]
    private float fLimiteRotationCamera = 75f;

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

    public void RotationCamera(float vRotationCameraX)
    {
        fRotationCameraX = vRotationCameraX;
    }

    public void Saut(Vector3 vForceSaut)
    {
        vecForceSaut = vForceSaut;
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
        if(vecForceSaut!= Vector3.zero && bSautPossible) //C'est ici qu'on modifie pour le saut.
        {
            rb.AddForce(vecForceSaut * Time.fixedDeltaTime, ForceMode.Impulse);
            bSautPossible = false;
        }
    }

    private void EffectuerRotation()
    {
        //Récupération de la rotation + limite de la rotation.
        rb.MoveRotation(rb.rotation * Quaternion.Euler(vecRotation));
        fRotationActuelleCamerax -= fRotationCameraX;
        fRotationActuelleCamerax = Mathf.Clamp(fRotationActuelleCamerax, -fLimiteRotationCamera, fLimiteRotationCamera);

        //Application de la rotation.
        camJoueur.transform.localEulerAngles = new Vector3(fRotationActuelleCamerax, 0f, 0f);
    }

    void OnCollisionEnter(Collision AutreObjet)
    {
        /*Debug.Log("Points colliding: " + AutreObjet.contacts.Length);
        Debug.Log("First normal of the point that collide: " + AutreObjet.contacts[0].normal);*/
        for (int nI=0; nI<AutreObjet.contactCount; nI++)
        if(AutreObjet.contacts[nI].normal.y>=0.9) //Si l'on se cogne à un objet dont la normal est comprise entre 0.9 et 1 on peut sauter.
        {
            bSautPossible = true;
        }
    }
}
