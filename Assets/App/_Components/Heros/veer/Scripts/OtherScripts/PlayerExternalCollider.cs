// Ignore Spelling: Collider

using UnityEngine;

public class PlayerExternalCollider : MonoBehaviour
{
    private GameObject CurrentRightSlope;

    private GameObject CurrentLeftSlope;

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
                if (player.DisabledSlopeObjects != null && !player.DisabledSlopeObjects.Contains(CurrentLeftSlope))
                {
                    player.DisableCollisionForPlayer(CurrentLeftSlope.GetComponent<Collider2D>());
                    player.DisabledSlopeObjects.Add(CurrentLeftSlope);
                }
            }


    }
}
