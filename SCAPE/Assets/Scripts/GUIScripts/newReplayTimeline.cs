using UnityEngine;
using System.Collections;

public class newReplayTimeline : AARRGuiProgressBar
{
	void Start()
	{
		mPosition = new Rect((3 * Screen.width) / 4 - 50, ((7 * Screen.height) / 8) - 10, 50, 20);
	}
	
    void OnGUI()
    {
        //Vector3 pos = mCamera.WorldToScreenPoint(this.transform.position);
		
		//float yCoord = mCamera.pixelHeight - pos.y;
		
        GUI.Label(new Rect(mPosition.x, mPosition.y, 146, 11), "", customStyle);
        GUI.DrawTexture(new Rect(mPosition.x + 3, mPosition.y + 3, 140 * mProgress, 7), progressTexture);
    }
	
	private Rect mPosition;
}