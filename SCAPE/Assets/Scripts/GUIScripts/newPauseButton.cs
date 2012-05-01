using UnityEngine;
using System.Collections;

public class newPauseButton : AARRGuiElement
{
    private void Start()
    {
        //mClickCallback = new ClickCallBackType(GameObject.Find("Runner").GetComponent<PlayerDemoRunner>().OnPauseClicked);
        base.Start();
    }


    void OnGUI()
    {
        //Vector3 pos = mCamera.WorldToScreenPoint(this.transform.position);
		
        // Make a button that uses the "toggle" GUIStyle
        if (GUI.Button(new Rect((3 * Screen.width) / 4, (7 * Screen.height) / 8, 50, 50), "", customStyle) && mEnabled)
            mClickCallback();
    }
}
