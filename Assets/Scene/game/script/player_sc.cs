using UnityEngine;
using Mirror;
using TMPro;

public class player_sc : NetworkBehaviour
{
    [SerializeField] GameObject _joystick, _shot_b, _resource_pn, _game_manager;
    [SerializeField] Rigidbody2D _rb;
    public TextMeshProUGUI _name_txt;
    public int _hp, _coin, _hp_mx;
    public bool _is_die;



    private void Start()
    {
        Start_Setting();
        Find_UI();
    }
    private void FixedUpdate()
    {
        Move();
        Wall();
        Camera_Follow();
        Send_All_My_Name();
    }


    //general
    void Find_UI()
    {
        if (isLocalPlayer)
        {
            _joystick = GameObject.Find("joystick");
            _joystick.GetComponent<joystick_sñ>()._player = gameObject;
            _shot_b = GameObject.Find("shot_b");
            _shot_b.GetComponent<shot_b_sc>()._player = gameObject;
            _resource_pn = GameObject.Find("resource_pn");
            _resource_pn.GetComponent<resource_pn_sc>()._player = gameObject;
            _game_manager = GameObject.Find("game_manager");
            Req_Add_To_Live();
        }
    }
    void Start_Setting()
    {
        _rb = GetComponent<Rigidbody2D>();
        _is_die = false;
        _hp = _hp_mx = 100;
        _coin = 0;
        _direction = 0f;
        _speed = 5f;
    }
    void Camera_Follow()
    {
        if (isLocalPlayer)
        {
            Camera.main.transform.localPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.localPosition.z);
        }
    }


    //name
    void Send_All_My_Name()
    {
        string id = netId.ToString();
        if (isServer)
        {
            Serv_Send_All_My_Name(gameObject, id);
        }
        else if (!isServer)
        {
            Cmd_Send_All_My_Name(gameObject, id);
        }
    }
    [Command]
    void Cmd_Send_All_My_Name(GameObject obj, string id)
    {
        Rpc_Send_All_My_Name(obj, id);
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
        obj.GetComponent<player_sc>()._name_txt.text = "ID: " + id;
        obj.GetComponent<player_sc>().name = "player_" + id;
    }


    //move
    public Vector3 _move_v;
    public float _direction;
    [SerializeField] float _speed;
    [SerializeField] float _mx_pos;
    void Move()
    {
        transform.rotation = Quaternion.Euler(0, 0, _direction);
        _rb.velocity = _move_v.normalized * _speed;
    }
    void Wall()
    {
        if (transform.position.x > _mx_pos)
        {
            transform.position = new Vector3(_mx_pos, transform.position.y, transform.position.z);
        }
        if (transform.position.y > _mx_pos)
        {
            transform.position = new Vector3(transform.position.x, _mx_pos, transform.position.z);
        }
    }


    //shot
    [SerializeField] GameObject _bullet;
    [SerializeField] Transform _bullet_spawn_point;
    public void Shot()
    {
        Vector2 pos = _bullet_spawn_point.position;
        Cmd_Spawn_Bullet(pos, _direction);
    }
    [Command]
    void Cmd_Spawn_Bullet(Vector2 pos, float ang)
    {
        Srv_Spawn_Bullet(pos, ang);
    }
    [Server]
    void Srv_Spawn_Bullet(Vector2 pos, float ang)
    {
        GameObject bullet = Instantiate(_bullet, pos, Quaternion.Euler(0, 0, ang));
        NetworkServer.Spawn(bullet);
    }
    public void Bullet_Take_Damage(GameObject bullet ,int dmg)
    {
        GetComponent<Animation>().Play("take_dmg");
        if (_hp - dmg > 0)
        {
            _hp -= dmg;
        }
        else if (_hp - dmg <= 0)
        {
            _hp = 0;
            Die();
        }
        Destroy(bullet);
    }
    void Die()
    {
        gameObject.transform.position = new Vector3(0, 0, 1000);
        _is_die = true;
        Req_Remove_From_Live();
    }


    //live managment
    void Req_Add_To_Live()
    {
        Cmd_Req_Add_To_Live();
    }
    [Command]
    void Cmd_Req_Add_To_Live()
    {
        Rpc_Req_Add_To_Live(gameObject);
    }
    [ClientRpc]
    void Rpc_Req_Add_To_Live(GameObject player)
    {
        Add_To_Live(player);
    }
    void Add_To_Live(GameObject player)
    {
        _game_manager = GameObject.Find("game_manager");
        _game_manager.GetComponent<game_manager_sc>()._live_player.Add(player);
    }

    void Req_Remove_From_Live()
    {
        Cmd_Req_Remove_From_Live();
    }
    [Command]
    void Cmd_Req_Remove_From_Live()
    {
        Rpc_Req_Remove_From_Live(gameObject);
    }
    [ClientRpc]
    void Rpc_Req_Remove_From_Live(GameObject player)
    {
        Remove_From_Live(player);
    }
    void Remove_From_Live(GameObject player)
    {
        _game_manager = GameObject.Find("game_manager");
        _game_manager.GetComponent<game_manager_sc>()._live_player.Remove(player);
    }


    //coin
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Coin")
        {
            Destroy(collider.gameObject);
            _coin += 1;
            if (isServer)
            {
                Srv_Call_Coin(gameObject, _coin);
            }
            else if (!isServer)
            {
                Cmd_Call_Coin(gameObject, _coin);
            }
        }
    }
    [Command]
    void Cmd_Call_Coin(GameObject player, int coin_count)
    {
        Srv_Call_Coin(player, coin_count);
    }
    [Server]
    void Srv_Call_Coin(GameObject player, int coin_count)
    {
        Rpc_Call_Coin(player, coin_count);
    }
    [ClientRpc]
    void Rpc_Call_Coin(GameObject player, int coin_count)
    {
        Obj_Set_Coin(player, coin_count);
    }
    void Obj_Set_Coin(GameObject player, int coin_count)
    {
        player.GetComponent<player_sc>()._coin = coin_count;
    }
}
