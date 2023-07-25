using Mirror;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class player_slot_sc : NetworkBehaviour
{
    [SerializeField] GameObject _lobby_pn;
    [SerializeField] TextMeshProUGUI _client_name;
    [SerializeField] string _id;


    private void Start()
    {
        Find_Lobby_Pn();
        Lobby_Pn_Load();
        True_Room_Check();
        Set_Pos();
    }
    private void Update()
    {
        Send_All_My_Name();
    }


    void Find_Lobby_Pn()
    {
        _lobby_pn = GameObject.Find("lobby_pn");
        _lobby_pn.GetComponent<lobby_pn_sc>()._player = gameObject;
    }
    void True_Room_Check()
    {
        if (!isServer)
        {
            Cmd_True_Room_Check(_lobby_pn.GetComponent<lobby_pn_sc>()._room_name_field.text);
        }
    }
    void Set_Pos()
    {
        transform.SetParent(_lobby_pn.GetComponent<lobby_pn_sc>()._player_slot_cont);
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }
    void Lobby_Pn_Load()
    {
        _id = netId.ToString();
        if (isServer)
        {
            string room_name = _lobby_pn.GetComponent<lobby_pn_sc>()._room_name_field.text;
            _lobby_pn.GetComponent<lobby_pn_sc>()._room_name_txt.text = room_name;
            _lobby_pn.GetComponent<lobby_pn_sc>()._start_b.SetActive(true);
        }
        else if (!isServer)
        {
            _lobby_pn.GetComponent<lobby_pn_sc>()._start_b.SetActive(false);
            Srv_Set_Room_Name();
        }
    }



    [Command]
    void Cmd_True_Room_Check(string room_name)
    {
        bool check = true;
        if (Get_Server_Room_Name() != room_name)
        {
            check = false;
        }
        Rpc_True_Room_Check_Action(check);
    }
    [TargetRpc]
    void Rpc_True_Room_Check_Action(bool check)
    {
        if (check == false)
        {
            NetworkManager.singleton.StopClient();
        }
    }


    void Send_All_My_Name()
    {
        if (isServer)
        {
            Serv_Send_All_My_Name(gameObject, _id);
        }
        else if (!isServer)
        {
            Cmd_Send_All_My_Name(gameObject, _id);
        }
    }
    [Command]
    void Cmd_Send_All_My_Name(GameObject obj, string id)
    {
        Serv_Send_All_My_Name(obj, _id);
    }
    [Server]
    void Serv_Send_All_My_Name(GameObject obj, string id)
    {
        Rpc_Send_All_My_Name(obj, id);
    }
    [ClientRpc]
    void Rpc_Send_All_My_Name(GameObject obj, string id)
    {
        Set_Client_Name(obj, id);
    }
    void Set_Client_Name(GameObject obj, string id)
    {
        obj.GetComponent<player_slot_sc>()._client_name.text = "ID: " + id;
        obj.GetComponent<player_slot_sc>().name = "player_" + id;
    }



    [Server]
    string Get_Server_Room_Name()
    {
        return _lobby_pn.GetComponent<lobby_pn_sc>()._room_name_txt.text;
    }
    [Command]
    void Srv_Set_Room_Name()
    {
        Rpc_Set_Room_Name(Get_Server_Room_Name());
    }
    [ClientRpc]
    void Rpc_Set_Room_Name(string room_name)
    {
        Set_Room_Name(room_name);
    }
    void Set_Room_Name(string room_name)
    {
        _lobby_pn.GetComponent<lobby_pn_sc>()._room_name_txt.text = room_name;
    }
}
