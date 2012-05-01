using UnityEngine;
using System.Collections;

public class ModeController : MonoBehaviour
{ 
	
	#region Public members
	
    /* This variable will hold the function to be called */
    public string MethodToInvoke;

    /* This is script which has the function - methodToInvoke */
    public MonoBehaviour ScriptContainer;
	
	public Runner Runner;
	
	public ReviewSystemVtr Vcr;

    public BlackOut Blackout;
	
	#endregion
    
	#region Private members

    // Use this for initialization
	private void Start () 
	{
		mPlayer = GameObject.Find("Player");
				
		mAssessmentManager = GameObject.Find("Assessment").GetComponent<AssessmentManager>();
	}

    
	
	private void OnTriggerEnter(Collider other) 
	{
		Runner.Stop();
		ScriptContainer.Invoke(MethodToInvoke, 0f);

        Blackout.StartFadeIn();
        mPlayer.GetComponent<DefaultPositionTracker>().SetDefault();		
        //mPlayer.transform.localPosition = mDefaultPosition;
		Vcr.showVcr = true;
		mAssessmentManager.AssesmentMode();	
		        
    }
    
        
    #endregion
    
    #region Private members

    private Vector3 mDefaultPosition = new Vector3(0,0,0);
	private AssessmentManager mAssessmentManager;
	private GameObject mPlayer;
	
    #endregion
}
