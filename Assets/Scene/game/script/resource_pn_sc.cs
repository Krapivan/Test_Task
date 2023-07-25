using UnityEngine;
using TMPro;

public class resource_pn_sc : MonoBehaviour
{
    public GameObject _player;


    private void FixedUpdate()
    {
        if (_player != null)
        {
            Resource_Load();
        }
    }


    [SerializeField] TextMeshProUGUI _my_coin;
    [SerializeField] GameObject _hp_line;
    void Resource_Load()
    {
        _my_coin.text = _player.GetComponent<player_sc>()._coin + "";
        float x_scale = (float)_player.GetComponent<player_sc>()._hp / (float)_player.GetComponent<player_sc>()._hp_mx;
        _hp_line.transform.localScale = new Vector3(x_scale, 1, 1);
    }
}
