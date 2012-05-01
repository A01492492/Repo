using UnityEngine;
using System.Collections;

/// <summary>
/// This script changes the mouse pointer from arrow to hand when the mouse
/// hovers over a clickable object
/// </summary>
public class MouseToHand : MonoBehaviour 
{
    // Represents the mouse hand image
    public Texture cursor;

    // Set mouse over flag to true on mouse enter
	void OnMouseEnter() 
	{
        mouseOver = true;          
    }


    void OnGUI() 
    {
        // Check if the mouse over is true and cursor is not null. If true display hand instead of arrow
        if (mouseOver && cursor != null)
        {
            // hide the arrow mouse pointer
            Screen.showCursor = false;

            // Display the hand pointer
            GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, cursor.width, cursor.height), cursor);
        }
    }

    // Return to mouse pointer on mouse exit
	void OnMouseExit() 
	{
        mouseOver = false;

        Screen.showCursor = true;        
    }	

    private bool mouseOver = false;   
}
