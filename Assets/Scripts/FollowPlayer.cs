using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    private int distanceBack = 15;

    private void Start()
    {
        transform.position = new Vector3(
            player.position.x, 
            player.position.y, 
            player.position.z - distanceBack
        );
    }

    private void Update()
    {
        transform.position = new Vector3(
            player.position.x, 
            player.position.y, 
            player.position.z - distanceBack
        );
    }
}
