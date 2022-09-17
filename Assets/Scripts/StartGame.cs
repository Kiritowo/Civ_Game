using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	//This loads the second scene in the load order when it runs
	public void LoadScene(){
		SceneManager.LoadScene(1);
	}
	//When this is selected the games closes
	public void leave(){
		Application.Quit ();
	}
}