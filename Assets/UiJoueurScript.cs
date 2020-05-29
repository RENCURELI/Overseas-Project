using UnityEngine;
using UnityEngine.UI;

public class UiJoueurScript : MonoBehaviour
{
    [SerializeField]
    private Text VieValeur = null;

    private Player ScripteJoueur;


    public void SetScripteJoueur (Player _ScripteJoueur)
    {
        ScripteJoueur = _ScripteJoueur;
    }

    void SetVieValeur (int nVie)
    {
        VieValeur.text = nVie.ToString();
    }

    private void Update()
    {
        SetVieValeur(ScripteJoueur.GetVieJoueur());
    }
}
