using Mirror;
using UnityEngine;

public class bullet_sc : NetworkBehaviour
{
    public Rigidbody2D _rb;

    [SerializeField] float _shot_range_now, _shot_range_mx;
    [SerializeField] float _bullet_speed;
    [SerializeField] bool _can_move;
    [SerializeField] int _dmg;


    private void Start()
    {
        Setting();
    }
    private void FixedUpdate()
    {
        Move();
    }
    void Setting()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shot_range_now = 0f;
        _shot_range_mx = 100f;
        _bullet_speed = 10f;
        _dmg = 10;
        _can_move = true;
    }

    void Move()
    {
        if (_can_move == true)
        {
            _rb.velocity = transform.right * _bullet_speed;
            _shot_range_now += 1f;
            if (_shot_range_now >= _shot_range_mx)
            {
                _can_move = false;
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<player_sc>().Bullet_Take_Damage(gameObject, _dmg);
        }
    }
}
