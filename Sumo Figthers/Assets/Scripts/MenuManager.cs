using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject menu_panel;
    [SerializeField]
    GameObject selection_panel;
    [SerializeField]
    Slider sliderMusic;
    [SerializeField]
    Slider sliderSFX;


    private void Start()
    {
        menu_panel = FindObjectOfType<Canvas>().transform.GetChild(0).gameObject;
        selection_panel = FindObjectOfType<Canvas>().transform.GetChild(1).gameObject;

        sliderMusic.value = 1;
        sliderSFX.value = 1;

        AudioManager.Instance.PlayMusic((int)AudioManager.MusicClips.menu);
    }

    public void ButtonPlay()
    {
        menu_panel.SetActive(false);
        selection_panel.SetActive(true);

        AudioManager.Instance.PlaySFX((int)AudioManager.SFXClips.button);
    }

    public void ButtonQuit()
    {
        AudioManager.Instance.PlaySFX((int)AudioManager.SFXClips.button);

        Application.Quit();
    }

    public void SetMusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(sliderMusic.value);
    }
    public void SetSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sliderSFX.value);
    }

    public void SelectPlayers()
    {
        AudioManager.Instance.PlaySFX((int)AudioManager.SFXClips.button);

        string[] splitArray = EventSystem.current.currentSelectedGameObject.name.Split(char.Parse(" "));
        int players = int.Parse(splitArray[1]);

        GameManager.instance.num_players = players;

        SceneManager.LoadScene("BattleArena");
    }
}
