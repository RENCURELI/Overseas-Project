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
        if(vecForceSaut!= Vector3.zero && rb.position.y<=0.51)
        {
            rb.AddForce(vecForceSaut * Time.fixedDeltaTime, ForceMode.Impulse);
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


}
