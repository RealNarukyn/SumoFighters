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
    private bool players_loaded = false;

    private void Start()
    {
        fighters = new List<GameObject>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "BattleArena")
        {
            if(!players_loaded)
                LoadPlayers();
        }
    }

    private void LoadPlayers()
    {
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
            fighters[i].GetComponent<MeshRenderer>().material = player_materials[i];
            fighters[i].name = player_materials[i].name;
            fighters[i].transform.LookAt(new Vector3(0,5,0));
        }

        players_loaded = !players_loaded;
    }

}
