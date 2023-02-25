using UnityEngine;
using System.Collections;

public class MenuManager : SingletonMonobehaviour<MenuManager> {

    [SerializeField]
    private GameObject m_mainMenu;

    [SerializeField]
    private GameObject m_ingameUI;

    [SerializeField]
    private GameObject m_gameOverScreen;    

    protected override void Awake()
    {
        base.Awake();
        HideAll();    
    }

    public void HideAll()
    {
        m_mainMenu.SetActive(false);
        m_ingameUI.SetActive(false);
        m_gameOverScreen.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAll();
        m_mainMenu.SetActive(true);
    }

    public void ShowInamgeUI()
    {
        HideAll();
        m_ingameUI.SetActive(true);
    }

    public void ShowGameOver()
    {
        HideAll();
        m_gameOverScreen.SetActive(true);
    }

}
