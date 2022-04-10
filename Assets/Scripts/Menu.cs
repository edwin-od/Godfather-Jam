using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;

    [SerializeField]
    private GameObject tutoPanel;

    [SerializeField]
    private GameObject creditPanel;

    [SerializeField]
    private GameObject backgroundTitle;

    private bool mainpanelActiveSelf = true;
    private bool creditpanelActiveSelf = false;
    private bool backgroundTitleActiveSelf = true;

    void Start()
    {
        if (!mainPanel.activeSelf)
        {
            mainPanel.SetActive(true);
        }

        if (creditPanel.activeSelf)
        {
            creditPanel.SetActive(false);
        }
        tutoPanel.SetActive(false);
    }

    public void LaunchGame()
    {
        SceneManager.LoadScene(1);
        SoundManager.Get.Play(Sound.soundNames.MenuClick);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisplayCredit()
    {
        mainpanelActiveSelf = !mainpanelActiveSelf;
        creditpanelActiveSelf = !creditpanelActiveSelf;

        backgroundTitleActiveSelf = !backgroundTitleActiveSelf;

        mainPanel.SetActive(mainpanelActiveSelf);
        creditPanel.SetActive(creditpanelActiveSelf);
        backgroundTitle.SetActive(backgroundTitleActiveSelf);

        SoundManager.Get.Play(Sound.soundNames.MenuClick);
    }

    public void DisplayTutorial() {
        tutoPanel.SetActive(true);
    }

    public void CloseTutorial() {
        tutoPanel.SetActive(false);
    }

}
