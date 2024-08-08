using System.Collections;
using UnityEngine;

public class MainSceneCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _aboutUsPanal;


    public void ShowSettingsPanel()
    {
        StartCoroutine(IShowSettingsPanel());
    }

    public void HideSettingsPanel()
    {
        StartCoroutine(IHideSettingsPanel());
    }

    public void ShowAboutUssPanel()
    {
        StartCoroutine(IShowAboutUssPanel());
    }

    public void HideAboutUsPanel()
    {
        StartCoroutine(IHideAboutUsPanel());
    }

    public IEnumerator IShowSettingsPanel()
    {
        yield return new WaitForSeconds(0.2f);
        _settingsPanel.SetActive(true);
    }

    public IEnumerator IHideSettingsPanel()
    {
        yield return new WaitForSeconds(0.2f);
        _settingsPanel.SetActive(false);
    }

    public IEnumerator IShowAboutUssPanel()
    {
        yield return new WaitForSeconds(0.2f);
        _aboutUsPanal.SetActive(true);
    }

    public IEnumerator IHideAboutUsPanel()
    {
        yield return new WaitForSeconds(0.2f);
        _aboutUsPanal.SetActive(false);
    }
}
