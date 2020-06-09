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
    private float fRotationCameraX = 0f;
    private float fRotationActuelleCamerax = 0f;

    //Gestion du double Saut
    private float fTempsSautCompteur = 1f; //Valeur actuelle restante pour continuer le saut en maintenant espace.
    [SerializeField]
    private float fTempsSaut = 1f; //Valeur du temps max pour le saut.
    private bool bEnSaut; //Booléen pour savoir si le saut est en cours.
    [SerializeField]
    private float fForceSautContinue=1f; //La force que le joueur ajoute en laissant la barre espace appuyée.
    //Fin Gestion du double Saut.

    [SerializeField]
    private bool bSautPossible = false;

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

    private void Update()
    {
        bSautPossible = SautPossible(); //Vérifie si le saut est possible.
    }

    private void EffectuerMouvement()
    {
        if (vecVelocite!= Vector3.zero && bSautPossible) //On effectue les mouvements horizontaux si on n'est pas en train de sauter.
        {
            rb.MovePosition(rb.position + vecVelocite * Time.fixedDeltaTime);
        }
        else if (vecVelocite != Vector3.zero && !bSautPossible) //On ralenti un peu le mouvement si on est en train de sauter, ici de -10%.
        {
            rb.MovePosition(rb.position + vecVelocite * 0.9f * Time.fixedDeltaTime);
        }

        if (vecForceSaut!= Vector3.zero && bSautPossible) //On effectue le saut.
        {
            rb.AddForce(vecForceSaut * Time.fixedDeltaTime, ForceMode.Impulse);
            bSautPossible = false;
            bEnSaut = true;
            fTempsSautCompteur = fTempsSaut;
        }

        if (vecForceSaut != Vector3.zero && bEnSaut == true)
        {
            if (fTempsSautCompteur>0)
            {
                rb.position += transform.up * Time.deltaTime * fForceSautContinue;
                fTempsSautCompteur -= Time.deltaTime;
            }
            else
            {
                bEnSaut = false; //Si il ne peut plus sauter, il cesse de gagner en hauteur.
            }
        }

        if (vecForceSaut == Vector3.zero) //Le saut n'est plus en cours.
        {
            bEnSaut = false;
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


    bool SautPossible ()
    //BUT : Déterminer si un saut est possible.
    //ENTREE : La position du joueur et un raycast vers le bas d'une longueur de 0.1 unité.
    //SORTIE : VRAI si le saut est possible et FAUX si il ne l'est pas.
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        Debug.DrawRay(transform.position,down*30f,Color.red);
        return Physics.Raycast(transform.position, down, 0.1f); //Renvoie vrai en cas de collisione t faux sinon.
    }
}
