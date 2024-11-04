// Ignore Spelling: Collider

using UnityEngine;

public class PlayerExternalCollider : MonoBehaviour
{
    public GameObject CurrentRightSlope;

    public GameObject CurrentLeftSlope;

    public GameObject CurrentRightSlopePlatform;

    public GameObject CurrentLeftSlopePlatform;

    [SerializeField]
    private  Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SlopeRight"))
        {
            if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(collision.gameObject))
            {
                CurrentRightSlope = collision.gameObject;
            }
            else
            {
                CurrentRightSlope = null;
            }
        }

        if (collision.gameObject.CompareTag("SlopeLeft"))
        {
            if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(collision.gameObject))
            {
                CurrentLeftSlope = collision.gameObject;
            }
            else
            {
                CurrentLeftSlope = null;
            }
        }

        if (collision.gameObject.CompareTag("SlopeRightPlatform"))
        {
            if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(collision.gameObject))
            {
                CurrentRightSlopePlatform = collision.gameObject;
            }
            else
            {
                CurrentRightSlopePlatform = null;
            }
        }

        if (collision.gameObject.CompareTag("SlopeLeftPlatform"))
        {
            if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(collision.gameObject))
            {
                CurrentLeftSlopePlatform = collision.gameObject;
            }
            else
            {
                CurrentLeftSlopePlatform = null;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player.DisabledSlopeObjects != null && player.DisabledSlopeObjects.Contains(collision.gameObject))
        {
            player.DisabledSlopeObjects.Remove(collision.gameObject);
            player.EnableCollisionForPlayer(collision);
        }

        if (!(CurrentRightSlope == null) && CurrentRightSlope == collision.gameObject)
        {
            CurrentRightSlope = null;
        }

        if (!(CurrentLeftSlope == null) && CurrentLeftSlope == collision.gameObject)
        {
            CurrentLeftSlope = null;
        }

        if (!(CurrentRightSlopePlatform == null) && CurrentRightSlopePlatform == collision.gameObject)
        {
            CurrentRightSlopePlatform = null;
        }

        if (!(CurrentLeftSlopePlatform == null) && CurrentLeftSlopePlatform == collision.gameObject)
        {
            CurrentLeftSlopePlatform = null;
        }
    }

    private void Update()
    {

            if (CurrentRightSlope != null && player.input.MoveInput.x>0 && player.input.MoveInput.y < 0)
            {
            
                if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(CurrentRightSlope))
                {
                    player.DisableCollisionForPlayer(CurrentRightSlope.GetComponent<Collider2D>());
                    player.DisabledSlopeObjects.Add(CurrentRightSlope);
                }
            }

            if (CurrentLeftSlope != null  && player.input.MoveInput.x < 0 && player.input.MoveInput.y < 0)
            {
            /*Debug.Log("need to penetrate");*/
                if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(CurrentLeftSlope))
                {
                    player.DisableCollisionForPlayer(CurrentLeftSlope.GetComponent<Collider2D>());
                    player.DisabledSlopeObjects.Add(CurrentLeftSlope);
                }
            }


        if (CurrentRightSlopePlatform != null && player.input.MoveInput.x < 0 && player.input.MoveInput.y < 0)
        {
            if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(CurrentRightSlopePlatform))
            {
                player.DisableCollisionForPlayer(CurrentRightSlopePlatform.GetComponent<Collider2D>());
                player.DisabledSlopeObjects.Add(CurrentRightSlopePlatform);
            }
        }

        if (CurrentLeftSlopePlatform != null && player.input.MoveInput.x > 0 && player.input.MoveInput.y < 0)
        {
            if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(CurrentLeftSlopePlatform))
            {
                player.DisableCollisionForPlayer(CurrentLeftSlopePlatform.GetComponent<Collider2D>());
                player.DisabledSlopeObjects.Add(CurrentLeftSlopePlatform);
            }
        }

    }
}
