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

    public int num_players = 0;

    private List<GameObject> fighters;
    private List<CharacterMovement> movements;
    private List<CharacterPush> pushes;

    private MultipleTargetCamera cam;

    private bool players_loaded = false;
    private bool is_playable = false;




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

            if (is_playable)
            {
                PlayersMove();
                PlayersPush();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
            {
                ReloadPlayers();
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

            CheckPlayers();
        }
    }

    //@TODO:
    //Cambiar esta variable para que no exista de manera global
    Vector3 position;
    private void SelectSpawnPoints(int player)
    {
        switch (num_players)
        {
            case 1:
                position = new Vector3(0, 5, 0);
                break;
            case 2:
                if (player == 1)
                    position = new Vector3(5, 5, 10);
                else
                    position = new Vector3(-5, 5, -10);
                break;
            case 3:
                if (player == 2)
                    position = new Vector3(0, 5, 10);
                else if (player == 1)
                    position = new Vector3(5, 5, -10);
                else
                    position = new Vector3(-5, 5, -10);
                break;
            case 4:
                if (player == 3)
                    position = new Vector3(5, 5, 10);
                else if (player == 2)
                    position = new Vector3(5, 5, -10);
                else if (player == 1)
                    position = new Vector3(-5, 5, 10);
                else
                    position = new Vector3(-5, 5, -10);
                break;

            default: break;
        }
    }

    //@TODO:
    //Mover esta variable players_out = 0;
    private float players_out = 0;
    private void CheckPlayers()
    {
        foreach (GameObject player in fighters)
        {
            if (player.transform.position.y < -5)
            {
                player.SetActive(false);

                player.transform.position = new Vector3(0, 20, 0);
                players_out++;
                Debug.Log("PLAYERS OUT: " + players_out);
            }
        }

        if (players_out >= num_players - 1)
        {
            Time.timeScale = 0;
            panel_escape.SetActive(true);
        }
    }

    private void LoadPlayers()
    {
        cam = FindObjectOfType<MultipleTargetCamera>();
        panel_escape = FindObjectOfType<Canvas>().transform.GetChild(0).gameObject;

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
                fighters[i].transform.LookAt(new Vector3(0, 5, 0));
        }
        

        players_loaded = !players_loaded;
        is_playable = !is_playable;
    }

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
        }
    }

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

        panel_escape.SetActive(false);
        players_out = 0;
        Time.timeScale = 1;
    }

}
