using UnityEngine;
using System.Collections;

public class AARRGuiElement : MonoBehaviour {
	
	public GUIStyle customStyle;
    public Texture2D disabledTexture;
    private Texture2D enabledTexture;
    private Texture2D enabledHover;
    private Texture2D enabledActive;

    protected bool mEnabled;
	protected Camera mCamera;
	
	
    protected delegate void ClickCallBackType();
    protected ClickCallBackType mClickCallback;

    public void setControlIsEnabled(bool value)
    {
        mEnabled = value;

        if (!value) 
        {
            customStyle.normal.background = disabledTexture;
            customStyle.hover.background = null;
            customStyle.active.background = null;
        }
        else
        {
            customStyle.normal.background = enabledTexture;
            customStyle.hover.background = enabledHover;
            customStyle.active.background = enabledActive;
        }
    }
     
	// Use this for initialization
	protected void Start () {

        // Save this since the hover images will change depending on enabled/disabled state
        enabledTexture = customStyle.normal.background;
        enabledHover = customStyle.hover.background;
        enabledActive = customStyle.active.background;
		
		//mCamera = GameObject.Find("Camera").GetComponent<Camera>();

        setControlIsEnabled(true);
	}
	 
	// Update is called once per frame
	void Update () {
	
	}
}
