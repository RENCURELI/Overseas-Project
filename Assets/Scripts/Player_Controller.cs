using UnityEngine;

[RequireComponent(typeof(Player_Movements))]
public class Player_Controller : MonoBehaviour
{
    [SerializeField]
    private float fVitesse=1; //Note fVitesse pour une variable vitesse de type float

    private Player_Movements sMouvement; //sMouvement pour une variable mouvement de type script

    //A priori je nommerais mes scripts et objets supérieurs en anglais et les composants internes aux scripts en français.

    private void Start()
    {
        sMouvement = GetComponent<Player_Movements>();
    }

    private void Update()
    //BUT : Calculer la vitesse du jouer en un vecteur 3D.
    //ENTREE : Les inputs du joueurs.
    //SORTIE : Le vecteur de déplacement.
    {
        float fMouvementX = Input.GetAxisRaw("Horizontal"); //Utilisations des inputs d'axes par défaut.
        float fMouvementZ = Input.GetAxisRaw("Vertical");

        Vector3 vMouvementHorizontal = transform.right * fMouvementX; //transform de l'objet auquel le script est attaché par défaut.
        Vector3 vMouvementVertical = transform.forward * fMouvementZ;

        Vector3 vVelocite = (vMouvementHorizontal + vMouvementVertical).normalized * fVitesse;

        sMouvement.Mouvement(vVelocite);
    }
}
