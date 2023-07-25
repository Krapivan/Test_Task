using Mirror;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class game_manager_sc : NetworkBehaviour
{
    [SerializeField] GameObject _coin_prefab;
    [SyncVar] public List<GameObject> _live_player;
    [SyncVar] public int _con_count;
    [SerializeField] TextMeshProUGUI _win_txt;
    [SerializeField] bool _game_start = false;


    private void Start()
    {
        _win_txt.gameObject.SetActive(false);
        if (isServer)
        {
            Rpc_Set_Con_Count();
            Spawn_Coin();
        }
    }
    private void FixedUpdate()
    {
        if (_con_count == _live_player.Count)
        {
            _game_start = true;
        }
        Check_Players_Count();
    }
    [ClientRpc]
    void Rpc_Set_Con_Count()
    {
        _con_count = NetworkServer.connections.Count;
    }
    void Spawn_Coin()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-20f,21f), Random.Range(-20f, 21f));
            GameObject coin = Instantiate(_coin_prefab, pos, Quaternion.identity);
            NetworkServer.Spawn(coin);
        }
    }


    void Check_Players_Count()
    {
        if (_live_player.Count <= 1 && _game_start == true)
        {
            _win_txt.text = "Winner ID: " + _live_player[0].GetComponent<player_sc>().netId + "\n" +
            "Coin: " + _live_player[0].GetComponent<player_sc>()._coin;
            _win_txt.gameObject.SetActive(true);
        }
    }


    public void Exit_B()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }
    }
}
