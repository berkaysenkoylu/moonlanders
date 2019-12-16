using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject buttonPanel;
    public GameObject levelSelectionPanel;
    public Animator MenuAgentAnimatorController;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void OnPlayButton()
    {
        StartCoroutine(OnPlayButtonPressed());
        //buttonPanel.SetActive(false);
        //levelSelectionPanel.SetActive(true);
    }

    public void OnBackButton()
    {
        StartCoroutine(OnBackButtonPressed());
        //buttonPanel.SetActive(true);
        //levelSelectionPanel.SetActive(false);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    IEnumerator OnPlayButtonPressed()
    {
        MenuAgentAnimatorController.SetBool("playPressed", true);

        yield return new WaitForSeconds(1.0f);
        //buttonPanel.SetActive(false);
        //levelSelectionPanel.SetActive(true);

        MenuAgentAnimatorController.SetBool("playPressed", false);
    }

    IEnumerator OnBackButtonPressed()
    {
        MenuAgentAnimatorController.SetBool("backPressed", true);

        yield return new WaitForSeconds(1.0f);
        //buttonPanel.SetActive(true);
        //levelSelectionPanel.SetActive(false);

        MenuAgentAnimatorController.SetBool("backPressed", false);
    }
}
