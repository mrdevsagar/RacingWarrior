using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FootGroundCheck : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private bool _isLefFool;

    private HashSet<Collider2D> Colliders = new HashSet<Collider2D>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add the collider to the set when it enters
        Colliders.Add(collision);
        if(_isLefFool)
        {
            player.LeftFootGrounded = true;
        } else
        {
            player.RightFootGrounded = true;
        }
        // Set to true when at least one collider is touching
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Remove the collider from the set when it exits
        Colliders.Remove(collision);

        // Check if there are no grounded colliders left
        if (Colliders.Count == 0)
        {
            if (_isLefFool)
            {
                player.LeftFootGrounded = false; // Set to false if no colliders are touching
            }
            else
            {
                player.RightFootGrounded = false;
            }

        }
    }

}
