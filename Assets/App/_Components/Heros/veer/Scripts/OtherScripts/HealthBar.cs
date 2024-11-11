using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _timeToDrain = 0.5f;
    [SerializeField] private Gradient _healthBarGradient;

    private Color _newHealBarColor;

    private Image _image;


    private float _target = 1f;


    private Coroutine _drainHealtBarCoroutine;

    private void Start()
    {
        _image = GetComponent<Image>();

        _image.color = _healthBarGradient.Evaluate(_target);
        CheckHelathBarGradientAmount();
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _target = currentHealth / maxHealth;

        _drainHealtBarCoroutine = StartCoroutine(DrainHealthBar());
        CheckHelathBarGradientAmount();
    }

    private IEnumerator DrainHealthBar()
    {
        float fillAmount =  _image.fillAmount;
        Color currentColor = _image.color;

        float elapsedTime = 0f;
        
        while (elapsedTime < _timeToDrain)
        {
            elapsedTime += Time.deltaTime;

            _image.fillAmount = Mathf.Lerp(fillAmount, _target,(elapsedTime/_timeToDrain));

            _image.color = Color.Lerp(currentColor, _newHealBarColor,(elapsedTime/_timeToDrain));

            yield return null;
        }
    }

    private void CheckHelathBarGradientAmount()
    {
        _newHealBarColor = _healthBarGradient.Evaluate(_target);
    }
}
