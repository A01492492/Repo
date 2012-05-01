using UnityEngine;
using System.Collections;

public class LevelResetter : MonoBehaviour
{
	
	#region Public members
	
    /* This variable will hold the function to be called */
    
    public string methodToInvoke;

    /* This is script which has the function - methodToInvoke */
    public MonoBehaviour scriptContainer;
	
	public Runner runner;
	
	public ReviewSystemVtr vcr;
	
	#endregion
	
	#region Private methods

	// Use this for initialization
	private void Start ()
    {
        if (scriptContainer == null)
        {
            Debug.LogError("No script container found. Please add script which contains the method");
            return;
        }
		
		if(runner == null || vcr == null)
		{
			Debug.LogError("No Runner or vcr found");
            return;
		}
		
        if (methodToInvoke == null)
        {
            Debug.LogError("No method found. Please add the method to invoke");
            return;
        }	
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered in Level Resetter");
		runner.Stop();
		vcr.showVcr = true;
        scriptContainer.Invoke(methodToInvoke, 0f);
    }
	
	#endregion
		
}
