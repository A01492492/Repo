using UnityEngine;
using System.Collections;

/// <summary>
/// Script for rendering the 2d background via GL commands
/// </summary>
public class GLBackground : MonoBehaviour 
{
	public Material background;
	
	void OnPostRender()
	{
		// render the backgroun
		GL.PushMatrix();
		GL.LoadOrtho();
		background.SetPass(0);
		GL.Begin(GL.QUADS);
			GL.TexCoord2(0, 0);
			GL.Vertex3(0, 0, 0);

			GL.TexCoord2(0, 1);
			GL.Vertex3(0, 1, 0);

			GL.TexCoord2(1, 1);
			GL.Vertex3(1, 1, 0);

			GL.TexCoord2(1, 0);
			GL.Vertex3(1, 0, 0);
		GL.End();		
		GL.PopMatrix();
	}

    private void Awake() 
    {
        // add dont destroy
        DontDestroyOnLoad(gameObject);
    }
	
}