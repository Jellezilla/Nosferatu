using UnityEngine;
using System.Collections;

public class RestartLevel : MonoBehaviour {

    private GameObject player;

    private bool confirmationPrompt;
    // Use this for initialization
    void Start () {
        confirmationPrompt = false;
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.R))
        {
            confirmationPrompt = !confirmationPrompt;
        }
        if(player.transform.position.z > 890.0f)
        {
            confirmationPrompt = true;
        } 
	}

    void OnGUI()
    {
        Rect windowRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200);

        if(GUI.Button(new Rect(10, 10, 100, 25), "(R)estart"))
        {
            confirmationPrompt = true;
        }

        if (confirmationPrompt)
        {
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "Confirmation");
                        
        }
    }
    void DoMyWindow(int windowID)
    {
        
          GUI.Label(new Rect(10, 20, 150, 25), "Would you like to restart?");
        if(GUI.Button(new Rect(40, 100, 100, 25), "Yes")) {
            Application.LoadLevel(Application.loadedLevel);
        }
        if (GUI.Button(new Rect(160, 100, 100, 25), "No"))
        {
            confirmationPrompt = !confirmationPrompt;
        }
    }
}
