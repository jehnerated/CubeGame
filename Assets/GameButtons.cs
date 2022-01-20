using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtons : MonoBehaviour
{
    public GameObject highlightBox;
    public AudioSource buttonSound;
    public void HintButton()
    {
        buttonSound.Play();
        Animator hintAnimation = highlightBox.GetComponent<Animator>();
        highlightBox.SetActive(true);
        hintAnimation.SetBool("Idle", true);
    }
    public void homePress()
    {
        SceneManager.LoadScene(0);
        buttonSound.Play();
    }
    public void agianPress()
    {
        SceneManager.LoadScene(1);
        buttonSound.Play();
    }
}
