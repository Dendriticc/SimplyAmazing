
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    Player player;
    void Start()
    {
        while (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        player.GetComponentInParent<CharacterController>().enabled = false;
        Debug.Log("PlayerSpawner picked up a Player object");
        player.Warp(transform.position);
        Debug.Log("PlayerSpawner has warped the player");
        player.GetComponentInParent<CharacterController>().enabled = true;
    }
}
