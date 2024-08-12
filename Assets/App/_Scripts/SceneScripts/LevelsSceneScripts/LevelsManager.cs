using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private GameObject _warnningPanal;

    private void Start()
    {
        _warnningPanal.SetActive(false);
    }
    public void ShowWarningScreen()
    {
        _warnningPanal.SetActive(true);
    }

    public void HideWarningScreen()
    {
        _warnningPanal.SetActive(false);
    }
}
