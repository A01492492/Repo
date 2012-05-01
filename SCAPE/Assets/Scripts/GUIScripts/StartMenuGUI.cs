using UnityEngine;
using System.Collections;

/// <summary>
/// This class displays the initial splash screen to the user. It also displays a 
/// "Play" and "Quit" button. 
/// 
/// Note - "Quit button is not working right now"
/// </summary>
public class StartMenuGUI : MonoBehaviour
{
    // Represents the background image
    public Texture2D backdrop;
    
    // Represents the gui skin to be used
    public GUISkin guiSkin;
    
    /// <summary>
    /// This function handles gui events. It creates buttons and 
    /// adds the bacckground image
    /// </summary>
    void OnGUI() 
    {
        // Log error and returnn if no skin is found
        if (guiSkin == null)
        {
            Debug.LogError("No GUI skin found for start menu");
            return;
        }

        GUI.skin = guiSkin;
        
        GUIStyle backgroundStyle = new GUIStyle();

        // add the background image
        backgroundStyle.normal.background = backdrop;

        // Creates a background window and adds the background style (created above) to it.
        //GUI.Label(new Rect((Screen.width - (Screen.height * 2)) * ASPECTRATIO, 0, Screen.height * 2, Screen.height), "", backgroundStyle);

        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", backgroundStyle);

        // Creates the play button
        if(GUI.Button(new Rect((Screen.width / 2) - 70, Screen.height - 120, 140, 50),"Play"))
        {
            isLoading = true;

            Application.LoadLevel("LevelOne");
        }

        // Check if user is playing in a web browser
        bool isWebPlayer = (Application.platform == RuntimePlatform.OSXWebPlayer ||
                            Application.platform == RuntimePlatform.WindowsWebPlayer);

        // Displays exit button is web browser is not being used to play the game
        if (!isWebPlayer)
        {
            if (GUI.Button(new Rect((Screen.width / 2) - 70, Screen.height - 60, 140, 50), "Quit"))
            {
                Application.Quit();
            }
        }

        // Displays a text "Loading" while the game is loading in the background
        if (isLoading)
        {
            GUI.Label(new Rect((Screen.width / 2) - 110, (Screen.height / 2) - 60, 400, 70), "Loading...", "mainMenuTitle");
        }

    }

    // Represents the standard aspect ration
    private const float ASPECTRATIO = 0.75f;

    // Flag representing if the game is being loaded or not
    private bool isLoading = false;
    
}
