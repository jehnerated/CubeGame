using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MediumButton : StateMachineBehaviour
{
    public GameObject transPanel;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject InstTransPanel = (GameObject)Instantiate(transPanel);
        GameObject sceneCanvas = GameObject.Find("Canvas");
        InstTransPanel.transform.SetParent(sceneCanvas.transform, false);
        Animator Transani = InstTransPanel.GetComponent<Animator>();
        Transani.SetBool("Exit", true);
    }
}
