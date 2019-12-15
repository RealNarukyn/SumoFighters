using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    [SerializeField]
    private Material[] player_materials;
    [SerializeField]
    private GameObject player_prefab;

    public int num_players = 0;

    private List<GameObject> fighters;
    private List<CharacterMovement> movements;
    private List<CharacterPush> pushes;

    private MultipleTargetCamera cam;
    
    private bool players_loaded = false;
    private bool properties_loaded = false;
    private bool is_playable = false;



    private void Start()
    {
        fighters = new List<GameObject>();
        movements = new List<CharacterMovement>();
        pushes = new List<CharacterPush>();
        
    }

    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "BattleArena")
        {
            if (!players_loaded)
            {
                LoadPlayers();

                foreach (GameObject c in fighters)
                {
                    Debug.Log(c.name);
                }
            }

            if (is_playable)
            {
                PlayersMove();
                PlayersPush();
            }
        }
    }

    private void FixedUpdate()
    {
        if (is_playable)
        {
            foreach (CharacterMovement move in movements)
            {
                move.FixJump();
            }
        }
    }

    private void LoadPlayers()
    {
        cam = FindObjectOfType<MultipleTargetCamera>();

        for (int i = 0; i < num_players; i++)
        {
            Vector3 position;

            if(i == 3)
                position = new Vector3(5, 5, 10);
            else if(i == 2)
                position = new Vector3(5, 5, -10);
            else if (i == 1)
                position = new Vector3(-5, 5, 10);
            else 
                position = new Vector3(-5, 5, -10);

            fighters.Add(Instantiate(player_prefab, position, Quaternion.Euler(0, 0, 0)));
            cam.AddTargetToCamera(fighters[i].transform);

            movements.Add(fighters[i].GetComponent<CharacterMovement>());
            pushes.Add(fighters[i].GetComponentInChildren<CharacterPush>());


            fighters[i].GetComponent<MeshRenderer>().material = player_materials[i];
            fighters[i].name = player_materials[i].name;
            fighters[i].transform.LookAt(new Vector3(0,5,0));
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
        }
    }
    
}
