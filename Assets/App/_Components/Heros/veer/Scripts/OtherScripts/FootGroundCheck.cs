using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FootGroundCheck : MonoBehaviour
{
    private enum FootCollider {  LeftFoot, BottomCenter, RightFoot }

    [SerializeField]
    private Player player;

    [SerializeField]
    private FootCollider SelectedCollider;



    private HashSet<Collider2D> Colliders = new HashSet<Collider2D>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add the collider to the set when it enters
        Colliders.Add(collision);
        if(SelectedCollider == FootCollider.LeftFoot)
        {
            player.LeftFootGrounded = true;
        }
        else if (SelectedCollider == FootCollider.BottomCenter)
        {
            player.CenterGrounded = true;
        }
        else if (SelectedCollider == FootCollider.RightFoot)
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
            if (SelectedCollider == FootCollider.LeftFoot)
            {
                player.LeftFootGrounded = false;
            }
            else if (SelectedCollider == FootCollider.BottomCenter)
            {
                player.CenterGrounded = false;
            }
            else if (SelectedCollider == FootCollider.RightFoot)
            {
                player.RightFootGrounded = false;
            }

        }
    }

}


