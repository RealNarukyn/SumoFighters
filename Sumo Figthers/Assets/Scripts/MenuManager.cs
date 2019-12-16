using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject menu_panel;
    [SerializeField]
    GameObject selection_panel;

    public void ButtonPlay()
    {
        menu_panel.SetActive(false);
        selection_panel.SetActive(true);
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void SelectPlayers()
    {
        string[] splitArray = EventSystem.current.currentSelectedGameObject.name.Split(char.Parse(" "));
        int players = int.Parse(splitArray[1]);


        GameManager.instance.num_players = players;

        SceneManager.LoadScene("BattleArena");
    }
}
