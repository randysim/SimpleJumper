using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    public PlayerMovement movement;
    public string statName;
    public float newValue;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            movement[statName] = newValue;
            Destroy(this.gameObject); // destroy after colliding
        }
    }
}
