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
    public List<CharacterMovement> movements;
    private List<CharacterPush> pushes;

    private MultipleTargetCamera cam;
    private ArenaScript arena;

    private bool players_loaded = false;
    private bool is_playable = false;
    private bool is_advicing = true;

    public int players_in;



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

            if (Input.GetKeyDown(KeyCode.R) && Time.timeScale == 0)
            {
                ReloadPlayers();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                RestartPlayers();
            }
        }
    }
    
    
    private void LateUpdate()
    {
        if (is_playable)
        {
            foreach (CharacterMovement move in movements)
            {
                move.FixMove();
            }

            CheckPlayers();
        }
    }




    private void StartPlay()
    {
        is_advicing = !is_advicing;
        panel_advice.SetActive(false);
        is_playable = !is_playable;
    }

    private void CheckPlayers()
    {

        Debug.Log("PLAYERS IN: " + players_in);
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
                    cam.ClearCamera();

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
        int pos_1 = 7;
        int pos_y = 4;
        int pos_2 = 0;

        switch (num_players)
        {
            case 1:
                position = new Vector3(0, pos_y, 0);
                break;
            case 2:
                if (player == 1)
                    position = new Vector3(pos_1, pos_y, pos_2);
                else
                    position = new Vector3(-pos_1, pos_y, pos_2);
                break;
            case 3:
                if (player == 2)
                    position = new Vector3(pos_2, pos_y, pos_1);
                else if (player == 1)
                    position = new Vector3(pos_1, pos_y, pos_2);
                else
                    position = new Vector3(-pos_1, pos_y, pos_2);
                break;
            case 4:
                if (player == 3)
                    position = new Vector3(6, pos_y, 0);
                else if (player == 2)
                    position = new Vector3(-pos_1, pos_y, pos_2);
                else if (player == 1)
                    position = new Vector3(pos_2, pos_y, pos_1);
                else
                    position = new Vector3(pos_2, pos_y, -pos_1);
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

            movements.Add(fighters[i].GetComponent<CharacterMovement>());
            pushes.Add(fighters[i].GetComponentInChildren<CharacterPush>());

            cam.AddTargetToCamera(fighters[i].transform, movements[i]);

            fighters[i].GetComponent<MeshRenderer>().material = player_materials[i];
            fighters[i].name = player_materials[i].name;

            movements[i].setPlayer(i);

            if (num_players > 1)
                fighters[i].transform.LookAt(new Vector3(0, 4, 0));
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
                movements[i].Jump();
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
        cam.ClearCamera();

        for (int i = 0; i < num_players; i++)
        {
            cam.AddTargetToCamera(fighters[i].transform, movements[i]);

            SelectSpawnPoints(i);
            fighters[i].transform.position = position;

            if (num_players > 1)
                fighters[i].transform.LookAt(new Vector3(0, 4, 0));

            fighters[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            movements[i].changeCheckedCondition(false);
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
