using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public GameObject newGamePanel;
    public AudioSource buttonSound;

    public void newGamePress()
    {
        buttonSound.Play();
        Animator newGameAnimator = newGamePanel.GetComponent<Animator>();

        newGamePanel.SetActive(true);
        newGameAnimator.SetBool("NewGame", true);
    }

    public void EasyPress()
    {
        buttonSound.Play();
        PlayerPrefs.SetInt("Grid Size", 5);
    }

    public void MediumPress()
    {
        buttonSound.Play();
        PlayerPrefs.SetInt("Grid Size", 7);
    }

    public void HardPress()
    {
        buttonSound.Play();
        PlayerPrefs.SetInt("Grid Size", 10);
    }
}
