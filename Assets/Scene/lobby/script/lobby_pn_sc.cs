using UnityEngine;
using Mirror;
using TMPro;

public class lobby_pn_sc : NetworkBehaviour
{
    public TMP_InputField _room_name_field;
    public TextMeshProUGUI _room_name_txt;
    public TextMeshProUGUI _server_ip_txt;
    public GameObject _lobby_pn, _start_b;
    public Transform _player_slot_cont;

    public GameObject _player;

    public NetworkManager _net_manager;
    [SerializeField] GameObject _game_prefab;

    private void Start()
    {
        if (isServer)
        {
            _net_manager.networkAddress = IPManager.GetIP(ADDRESSFAM.IPv4);
            _server_ip_txt.gameObject.SetActive(true);
            _server_ip_txt.text = _net_manager.networkAddress;
        }
        else if (!isServer)
        {
            _server_ip_txt.gameObject.SetActive(false);
        }
    }

    public void Start_B()
    {
        if (isServer)
        {
            Srv_Scene_Load();
        }
    }
    [Server]
    void Srv_Scene_Load()
    {
        _net_manager.playerPrefab = _game_prefab;
        _net_manager.ServerChangeScene("game");
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
