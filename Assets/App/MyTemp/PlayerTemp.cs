using System.Collections;
using UnityEngine;

public class PlayerTemp : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null && collision.gameObject.tag =="Ground") {
            //Call Game Over
            Debug.Log("game over");

            StartCoroutine(EndGame());
            
        }
    }

    private IEnumerator EndGame()
    {
       yield return new WaitForSeconds(1f);
        LevelManager.Instance.GameOver();
    }
}
