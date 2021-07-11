using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLogic_ChooseLevel : ScreenLogic
{
    [SerializeField] private Selectable m_SecondLevelButon;
    public override void TurnScreenOff()
    {
        gameObject.SetActive(false);
    }

    public override void TurnScreenOn()
    {
        gameObject.SetActive(true);
        if(PlayerPrefs.GetInt("CanPlayLevel2") == 1)
        {
            m_SecondLevelButon.interactable = true;
        }
        else
        {
            m_SecondLevelButon.interactable = false;
        }
    }
}
