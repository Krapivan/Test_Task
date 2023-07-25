using UnityEngine;

public class shot_b_sc : MonoBehaviour
{
    public GameObject _player;


    public void Shot_B()
    {
        if (_player.GetComponent<player_sc>()._is_die != true)
        {
            _player.GetComponent<player_sc>().Shot();
        }
    }
}
