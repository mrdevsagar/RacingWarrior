using System.Collections;
using UnityEngine;

public class Drive : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _frontTireRB;
    [SerializeField] private Rigidbody2D _backTireRB;
    [SerializeField] private Rigidbody2D _vehicleBody;
    [SerializeField] private float _speed = 150f;
    [SerializeField] private float _rotationalSpeed = 300f;

    public float _moveInput;

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _frontTireRB.AddTorque(-_moveInput * _speed * Time.deltaTime);
        _backTireRB.AddTorque(-_moveInput * _speed * Time.deltaTime);
        _vehicleBody.AddTorque(_moveInput * _rotationalSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "End")
        {
            //Call Game Over
            Debug.Log("Win");
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.GameOver();
    }
}
