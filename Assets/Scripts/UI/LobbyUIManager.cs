using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private List<ScreenLogic> m_Screens;
    [SerializeField] private string m_LevelName;
    private LobbyUIScreens m_CurrentScreen;

    void Start()
    {
        turnDownScreens();
        turnScreenOn(LobbyUIScreens.MainMenu);
        m_CurrentScreen = LobbyUIScreens.MainMenu;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }

    public void OpenScreen(LobbyUIScreens targetScreen)
    {
        turnScreenOff(m_CurrentScreen);
        m_CurrentScreen = targetScreen;
        turnScreenOn(m_CurrentScreen);
    }

    public void OpenScreen(int targetScreenIndex)
    {
        LobbyUIScreens targetScreen = (LobbyUIScreens)targetScreenIndex;
        turnScreenOff(m_CurrentScreen);
        m_CurrentScreen = targetScreen;
        turnScreenOn(m_CurrentScreen);
    }

    private void turnScreenOff(LobbyUIScreens targetScreen)
    {
        getScreen(targetScreen).TurnScreenOff();
    }

    private ScreenLogic getScreen(LobbyUIScreens targetScreen)
    {
        return m_Screens.Find(a => a.ScreenType == targetScreen);
    }

    private void turnDownScreens()
    {
        foreach (ScreenLogic screen in m_Screens)
        {
            screen.TurnScreenOff();
        }
    }

    private void turnScreenOn(LobbyUIScreens targetScreen)
    {
        getScreen(targetScreen).TurnScreenOn();
    }

    public void Exit()
    {
        switch (m_CurrentScreen)
        {
            case LobbyUIScreens.MainMenu:
                Application.Quit();
                break;
            default:
                OpenScreen(LobbyUIScreens.MainMenu);
                break;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        Settings.ChangeLevel((EMap)levelIndex);
    }
}
