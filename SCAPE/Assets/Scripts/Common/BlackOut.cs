using UnityEngine;
using System.Collections;

public class BlackOut : MonoBehaviour
{
    #region Public Members

    public Texture2D BlackoutTexture;
    public float BlackoutSpeed = 0.005f;
    private int GuiDepth = 1;

    #endregion

    #region internal Methods

    internal void StartFadeIn()
    {
        mAlpha = 1;
        //FADE_DIRECTION = -1;
        mBlackout = true;
    }

    internal void StopFadeIn()
    {
        mBlackout = false;
    }


    #endregion

    #region Private constants

    private const float FADE_DIRECTION = -1;
    private const float SLOW_DOWN_FACTOR = .005f;

    #endregion

    #region Private Methods

    private void OnGUI() 
    {
        if (mBlackout)
        {
            mAlpha += FADE_DIRECTION * BlackoutSpeed * SLOW_DOWN_FACTOR;
            mAlpha = Mathf.Clamp01(mAlpha);
            mAlphaColor.a = mAlpha;

            GUI.color = mAlphaColor;
            GUI.depth = GuiDepth;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BlackoutTexture);
        }
    }

    // Use this for initialization
	private void Start () 
    {
        mAlpha = 1;  
	}

    #endregion

    #region Private members
    
    private float mAlpha = 1.0f;    

    private bool mBlackout = false;
    
    private Color mAlphaColor = new Color();

    #endregion
}
