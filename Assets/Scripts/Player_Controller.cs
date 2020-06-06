using UnityEngine;

[RequireComponent(typeof(Player_Movements))]
public class Player_Controller : MonoBehaviour
{
    [SerializeField]
    private float fVitesse = 1f; //Note fVitesse pour une variable vitesse de type float
    [SerializeField]
    private float fSensibiliteRegard = 70f; //Pour la sensibilité de la souris tourner le joueur. //Réglé à 70f par défaut, rotation en Y, à modifier dans le préfab du joueur.
    [SerializeField]
    private float fSensibiliteCamera = 50f; //Pour la sensibilité de la souris pour orienter la camera. //Réglé à 50f par défaut, rotation en X, à modifier dans le préfab du joueur.
    [SerializeField]
    private float fForceSaut = 300f; //Pour la force du saut.

    private Player_Movements sMouvement; //sMouvement pour une variable mouvement de type script


    //A priori je nommerais mes scripts et objets supérieurs en anglais et les composants internes aux scripts en français.

    private void Start()
    {
        sMouvement = GetComponent<Player_Movements>();
    }

    private void Update()
    //BUT : Calculer la vitesse et la rotation du jouer en un vecteur 3D.
    //ENTREE : Les inputs du joueurs.
    //SORTIE : Le vecteur de déplacement.
    {
        //Gestion des déplacements joueur.
        float fMouvementX = Input.GetAxisRaw("Horizontal"); //Utilisations des inputs d'axes par défaut.
        float fMouvementZ = Input.GetAxisRaw("Vertical");

        Vector3 vMouvementHorizontal = transform.right * fMouvementX; //transform de l'objet auquel le script est attaché par défaut.
        Vector3 vMouvementVertical = transform.forward * fMouvementZ;

        Vector3 vVelocite = (vMouvementHorizontal + vMouvementVertical).normalized * fVitesse;

        sMouvement.Mouvement(vVelocite);

        //Gestion des rotations de la caméra.
        //Rotation selon Y pour tout le joueur.
        float fRotationY = Input.GetAxisRaw("Mouse X"); //Récupération des inputs d'axes par défaut avec l'axe horizontal de la souris.

        Vector3 vRotation = new Vector3(0, fRotationY, 0)* fSensibiliteRegard * Time.deltaTime;

        sMouvement.Rotation(vRotation);


        //Rotation selon X pour la caméra.
        float fRotationX = Input.GetAxisRaw("Mouse Y");

        float vRotationCameraX = fRotationX * fSensibiliteCamera * Time.deltaTime;

        sMouvement.RotationCamera(vRotationCameraX);

        //Gestion de la force du saut.
        Vector3 vForceSaut = Vector3.zero;
        if (Input.GetButton("Jump")) //Input jump existe dans unity
        {
            vForceSaut = Vector3.up *fForceSaut;
        }
        //Appliquer la force du saut.
        sMouvement.Saut(vForceSaut);
    }
}
