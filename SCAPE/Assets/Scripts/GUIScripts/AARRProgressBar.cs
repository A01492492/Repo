using UnityEngine;
using System.Collections;

public class AARRGuiProgressBar : AARRGuiElement{

    public Texture2D progressTexture;
    protected float mProgress;
    protected int mInitialTextureWidth;

	// Use this for initialization
	protected void Start () {
        mProgress = 0;
        mInitialTextureWidth = progressTexture.width;

        base.Start();
    }

    public void setProgressValue(float progress)
    {
        mProgress = progress;
    }
}
