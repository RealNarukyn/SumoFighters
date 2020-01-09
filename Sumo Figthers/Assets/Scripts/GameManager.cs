using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField]
    private Material[] player_materials;
    [SerializeField]
    private GameObject player_prefab;
    [SerializeField]
    private GameObject panel_escape;
    [SerializeField]
    private GameObject panel_advice;

    public int num_players = 0;

    private List<GameObject> fighters;
    private List<CharacterMovement> movements;
    private List<CharacterPush> pushes;

    private MultipleTargetCamera cam;
    private ArenaScript arena;

    private bool players_loaded = false;
    private bool is_playable = false;
    private bool is_advicing = true;
    
    



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        fighters = new List<GameObject>();
        movements = new List<CharacterMovement>();
        pushes = new List<CharacterPush>();
    }
    

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "BattleArena")
        {
            if (!players_loaded)
            {
                LoadPlayers();
            }

            if (is_advicing && Input.GetKeyDown(KeyCode.G))
            {
                StartPlay();
            }

            if (!is_advicing && is_playable)
            {
                PlayersMove();
                PlayersPush();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
            {
                ReloadPlayers();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                RestartPlayers();
            }
        }
    }
    
    
    private void FixedUpdate()
    {
        if (is_playable)
        {
            foreach (CharacterMovement move in movements)
            {
                move.FixMove();
            }

            TakeOutPlayers();
            CheckPlayers();
        }
    }




    private void StartPlay()
    {
        is_advicing = !is_advicing;
        panel_advice.SetActive(false);
        is_playable = !is_playable;
    }

    //@TODO:
    //Mover esta variable players_out = 0;
    private float players_in = 0;
    private void TakeOutPlayers()
    {
        foreach (GameObject player in fighters)
        {
            if (player.transform.position.y < -5)
            {
                player.SetActive(false);

                player.transform.position = new Vector3(0, 20, 0);
                players_in--;
                Debug.Log("PLAYERS IN GAME: " + players_in);
            }
        }
    }

    private void CheckPlayers()
    {
        switch (num_players)
        {
            case 1:
                //For the moment I don't need any funcitonality here.
                //This is the case for the IA.
                break;
            case 2:
            case 3:
            case 4:
                if (players_in < 3)
                {
                    //arena.UpdateSize();
                }
                
                if (players_in <= 1)
                {
                    Time.timeScale = 0;
                    panel_escape.SetActive(true);
                }
                break;
        }
    }

    //@TODO:
    //Cambiar esta variable para que no exista de manera global
    Vector3 position;
    private void SelectSpawnPoints(int player)
    {
        int pos_x = 7;
        int pos_y = 0;
        int pos_z = 6;

        switch (num_players)
        {
            case 1:
                position = new Vector3(0, pos_y, 0);
                break;
            case 2:
                if (player == 1)
                    position = new Vector3(pos_x, pos_y, pos_z);
                else
                    position = new Vector3(-pos_x, pos_y, -pos_z);
                break;
            case 3:
                if (player == 2)
                    position = new Vector3(0, pos_y, pos_z);
                else if (player == 1)
                    position = new Vector3(pos_x, pos_y, -pos_z);
                else
                    position = new Vector3(-pos_x, pos_y, -pos_z);
                break;
            case 4:
                if (player == 3)
                    position = new Vector3(pos_x, pos_y, pos_z);
                else if (player == 2)
                    position = new Vector3(pos_x, pos_y, -pos_z);
                else if (player == 1)
                    position = new Vector3(-pos_x, pos_y, pos_z);
                else
                    position = new Vector3(-pos_x, pos_y, -pos_z);
                break;

            default: break;
        }
    }

    private void LoadPlayers()
    {
        cam = FindObjectOfType<MultipleTargetCamera>();
        arena = FindObjectOfType<ArenaScript>();

        panel_advice = FindObjectOfType<Canvas>().transform.GetChild(0).gameObject;
        panel_escape = FindObjectOfType<Canvas>().transform.GetChild(1).gameObject;


        Debug.Log("NUM PLAYERS: " + num_players);

        for (int i = 0; i < num_players; i++)
        {
            SelectSpawnPoints(i);

            fighters.Add(Instantiate(player_prefab, position, Quaternion.Euler(0, 0, 0)));
            cam.AddTargetToCamera(fighters[i].transform);

            movements.Add(fighters[i].GetComponent<CharacterMovement>());
            pushes.Add(fighters[i].GetComponentInChildren<CharacterPush>());


            fighters[i].GetComponent<MeshRenderer>().material = player_materials[i];
            fighters[i].name = player_materials[i].name;

            if (num_players > 1)
                fighters[i].transform.LookAt(new Vector3(0, Vector3.forward.y, 0));
        }


        players_in = num_players;
        players_loaded = !players_loaded;
        
        //is_playable = !is_playable;
    }

    #region Players Actions
    private void PlayersMove()
    {
        for (int i = 0; i < num_players; i++)
        {
            movements[i].Move(i);
           
            if (Input.GetButtonDown("Joy" + i + "Jump"))
            {
                movements[i].Jump(i);
            }
        }
    }

    private void PlayersPush()
    {
        for (int i = 0; i < num_players; i++)
        {
            if (Input.GetButton("Joy" + i + "Push"))
            {
                pushes[i].ChargePush();
            }

            if (Input.GetButtonUp("Joy" + i + "Push"))
            {
                pushes[i].Push();
            }

            pushes[i].LookPunchCD();

            pushes[i].UpdateForceSphere();
            pushes[i].UpdateForceArea();
        }
    }
    #endregion

    #region Loading Players Functions
    //This function reloads the position of the players without clearing lists.
    private void ReloadPlayers()
    {
        for (int i = 0; i < num_players; i++)
        {
            cam.AddTargetToCamera(fighters[i].transform);

            SelectSpawnPoints(i);
            fighters[i].transform.position = position;

            if (num_players > 1)
                fighters[i].transform.LookAt(new Vector3(0, 5, 0));

            fighters[i].SetActive(true);
        }

        //arena.RestartSize();
        panel_escape.SetActive(false);
        players_in = num_players;
        Time.timeScale = 1;
    }

    //This function clears lists and sends the player to the Main Menu.
    private void RestartPlayers()
    {
        players_loaded = false;
        is_playable = false;
        is_advicing = true;
        players_in = 0;

        fighters.Clear();
        movements.Clear();
        pushes.Clear();
        cam.ClearCamera();

        Time.timeScale = 1;


        SceneManager.LoadScene("Menu");
    }
    #endregion
}
