using UnityEngine;
using System.Collections.Generic;
using BaxaTech.ReviewSystem;

/// <summary>
///		Provides Vtr-like controls in the ReviewSystem
/// </summary>
public class ReviewSystemVtr : MonoBehaviour 
{
		
	public GUISkin GuiSkin;
	public Runner runner;
	public bool recording = false;	
	public bool showVcr = false;
	
		
	/// <summary> Called whenever a change occurs in the user interface </summary>
	public void OnGUI()
	{
		
		
		GUI.skin = GuiSkin;		
		

		//runner.BeginGroupOperations();
		runner.RefreshGroups();
		
		if(showVcr)
		{
			VcrWindowRect = GUI.Window(8080, VcrWindowRect, ShowVcrWindow, "");
		}			
		
		GUI.TextField(GetPixelRect(TimeLabelRect), string.Format("{0:0.00}/{1:0.00}", runner.CurrentTime, runner.Length));
		
		if(!recording)
		{
            runner.RefreshBuffer();
			runner.Record();
			recording = true;
		}
		
		//runner.EndGroupOperations();
		
	}
						
	private void ShowVcrWindow(int windowID)
	{
		float newTime = GUI.HorizontalSlider(GetPixelRect(SliderRect), runner.CurrentTime, 0, runner.Length);
		
		if (newTime != runner.CurrentTime)
		{
			//runner.ReplayingPaused(newTime);
			runner.JumpTo(newTime);
			return;
		}
			
		
		GUILayout.BeginHorizontal();
		
		if (GUI.Button(GetPixelRect(BeginRect), "|<"))
		{
			//runner.Begin();
			runner.Start();
			return;
		}
		
		if(GUI.Button(GetPixelRect(PlayRect), "|>"))
		{
			runner.Play();
			return;
		}
		
		if(GUI.Button(GetPixelRect(PauseRect), "||"))
			runner.Pause();
		
		GUILayout.EndHorizontal();
		
		
	}


	/// <summary>
	///		Calculate the pixel placement of a rectangle
	/// </summary>
	/// <param name="unitRect">
	///		Rectangle describing a region on the screen in the range [0, 1] x [0, 1]
	/// </param>
	/// <returns>
	///		Rectangle describing a region on the screen in the range [0, Screen.height] x [0, Screen.height]
	/// </returns>
	private static Rect GetPixelRect(Rect unitRect)
	{
		return new Rect(Screen.width * unitRect.xMin, Screen.height * unitRect.yMin,
			Screen.width * unitRect.width, Screen.height * unitRect.height);
	}

	
	
	private Rect VcrWindowRect = GetPixelRect(new Rect(0.6f, 0.8f, 0.35f, 0.15f));
	//private Rect SliderRect = new Rect(0.6f, 0.8f, 0.35f, 0.05f);
	private Rect SliderRect = new Rect(0.0f, 0.02f, 0.35f, 0.05f);
	
	private Rect BeginRect = new Rect(0.1f, 0.08f, 0.05f, 0.05f);
	private Rect PlayRect = new Rect(0.15f, 0.08f, 0.05f, 0.05f);
	private Rect PauseRect = new Rect(0.2f, 0.08f, 0.05f, 0.05f);
	
	/*private Rect RewindRect = new Rect(0.66f, 0.85f, 0.05f, 0.05f);
	private Rect FFwdRect = new Rect(0.84f, 0.85f, 0.05f, 0.05f);
	private Rect EndRect = new Rect(0.9f, 0.85f, 0.05f, 0.05f);*/
	
	
	private Rect TimeLabelRect = new Rect(0.05f, 0f, 0.15f, 0.05f);
	/*private Rect StopRect = new Rect(0.37f, 0.8f, 0.08f, 0.05f);
	private Rect RecordRect = new Rect(0.46f, 0.8f, 0.08f, 0.05f);
	private Rect ClearRect = new Rect(0.55f, 0.8f, 0.08f, 0.05f);*/  

	//private Rect StopRect = new Rect(0.37f, 0.8f, 0.08f, 0.05f);
	//private Rect RecordRect = new Rect(0.46f, 0.8f, 0.08f, 0.05f);
	//private Rect ClearRect = new Rect(0.55f, 0.8f, 0.08f, 0.05f);

	//private List<ReviewGroup> m_groups = new List<ReviewGroup>();
	
}
