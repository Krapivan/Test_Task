using UnityEngine;
using Mirror;
using TMPro;

public class offline_ui_sc : MonoBehaviour
{
    [SerializeField] NetworkManager _net_manager;
    public TMP_InputField _room_name_field;
    public TMP_InputField _server_id_field;

    public void Create_Room_B()
    {
        if (_room_name_field.text.Length > 2)
        {
            NetworkManager.singleton.StartHost();
        }
    }
    public void Con_Room_B()
    {
        if (_room_name_field.text.Length > 2)
        {
            _net_manager.networkAddress = _server_id_field.text;
            Debug.Log(_net_manager.networkAddress);
            NetworkManager.singleton.StartClient();
        }
    }
}
