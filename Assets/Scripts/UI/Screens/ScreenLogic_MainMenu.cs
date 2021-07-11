using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLogic_MainMenu : ScreenLogic
{
    [SerializeField] private List<Selectable> interactables;
    public override void TurnScreenOff()
    {
        foreach (Selectable selectable in interactables)
        {
            selectable.interactable = false;
        }
    }

    public override void TurnScreenOn()
    {
        foreach (Selectable selectable in interactables)
        {
            selectable.interactable = true;
        }
    }
}
