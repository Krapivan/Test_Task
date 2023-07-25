using UnityEngine;

public class joystick_sñ : MonoBehaviour
{
    public GameObject _player;

    public GameObject _stick;
    bool _is_touch;

    public void FixedUpdate()
    {
        if (_is_touch == true)
        {
            if (Input.touchCount > 0)
            {
                Vector3 t_pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                _stick.transform.position = new Vector3(t_pos.x, t_pos.y, 0);
                Bound();
                if (Vector().x != 0 || Vector().y != 0)
                {
                    _player.GetComponent<player_sc>()._move_v = Vector();
                    _player.GetComponent<player_sc>()._direction = Direction();
                }
            }
        }
    }
    void Bound()
    {
        float dist = Mathf.Sqrt(Mathf.Pow((0 - _stick.transform.localPosition.x),2) + Mathf.Pow((0 - _stick.transform.localPosition.y), 2));
        float r = gameObject.GetComponent<RectTransform>().rect.width/2;

        if (dist > r)
        {
            if (_stick.transform.localPosition.x > r)
            {
                _stick.transform.localPosition = new Vector3(r, _stick.transform.localPosition.y, 0);
            }
            if (_stick.transform.localPosition.x < r * -1)
            {
                _stick.transform.localPosition = new Vector3(r * -1, _stick.transform.localPosition.y, 0);
            }

            if (_stick.transform.localPosition.y > r)
            {
                _stick.transform.localPosition = new Vector3(_stick.transform.localPosition.x, r, 0);
            }
            if (_stick.transform.localPosition.y < r * -1)
            {
                _stick.transform.localPosition = new Vector3(_stick.transform.localPosition.x, r * -1, 0);
            }
        }
    }
    Vector3 Vector()
    {
        var p = _stick.transform.localPosition - new Vector3(0, 0, 0);

        float x = p.x / (gameObject.GetComponent<RectTransform>().rect.width / 2), y = p.y / (gameObject.GetComponent<RectTransform>().rect.width / 2);

        x = (float)System.Math.Round(x, 1);
        y = (float)System.Math.Round(y, 1);
        Vector3 v = new Vector3(x, y, 0);

        return v;
    }
    float Direction()
    {
        var dir = _stick.transform.position - transform.position;
        var ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return ang;
    }


    public void B_Down()
    {
        _is_touch = true;
    }
    public void B_Up()
    {
        _is_touch = false;
        _stick.transform.localPosition = new Vector3(0, 0, 0);
        _player.GetComponent<player_sc>()._move_v = new Vector3(0, 0, 0);
    }
}
