using UnityEngine;

using System.Collections;



/// <summary>
/// This class displays the initial splash screen to the user. It also displays a 
/// "Play" and "Quit" button. 
/// 
/// Note - "Quit button is not working right now"
/// </summary>
public class BackgroundGUI : MonoBehaviour
{

    // Represents the background image
    public Texture2D backdrop;
    public Texture2D logo;

    // Represents the gui skin to be used
    public GUISkin guiSkin;

    

    /// <summary>
    /// This function creates the background image and logo
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
        GUIStyle logoStyle = new GUIStyle();

        // add the background image
        backgroundStyle.normal.background = backdrop;
        logoStyle.normal.background = logo;
        
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", backgroundStyle);

        GUI.Label(new Rect((Screen.width / 2) - (Screen.width / 4), 0, Screen.width / 2, Screen.height / 5), "", logoStyle);
    }



    // Represents the standard aspect ration
    private const float ASPECTRATIO = 0.75f;


    // Flag representing if the game is being loaded or not
    private bool isLoading = false;



}

