using UnityEngine;
using System.Text;
using System.Collections.Generic;
using BaxaTech.AssessmentSystem;

public class AssessmentManager : AssessmentSystem
{

    #region Constructors

    public AssessmentManager() 
    {        
    }
        
    #endregion

    #region Internal Properties

    internal float TotalScore 
    { 
        get { return mTotalScore; } 
    }

    #endregion


    #region Internal methods
	
	internal string GetResult
    {
		get
		{
			if(mResult == null)
				return "No result found";
			else
				return mResult.ToString(); 
		}
	}

    internal void CalculateResult() 
    {
        List<AssessmentRecord> feedbacks = GetRecords(FindFeedback);
        
        if (feedbacks == null)
        {
            Debug.LogError("No results found");
            return;
        }
        Debug.Log("RECORDS LENGTH " + feedbacks.Count);

		// Clear results
        mResult.Remove(0, mResult.Length);
		
        mTotalScore = 0;

        foreach (AssessmentRecord record in feedbacks) 
        {
            FeedbackRecord fbRecord = (FeedbackRecord)record;
            Quiz quiz = fbRecord.Quiz;

            mResult.Append(quiz.Question);
            mResult.Append("\n");
            mResult.Append("\n");

            mResult.Append("Your answer - ");
            mResult.Append(quiz.SelectedtAnswer);
            mResult.Append("\n");
            mResult.Append("\n");

            mTotalScore += quiz.PointsScored;
        }

        //result.Append("Points scored out of 10 - " + mTotalScore);
        mResult.Append("\n");
        mResult.Append("\n");             
       
    }
	
	
	
	// displays the assesment window and enables assesment camera
	internal void AssesmentMode()
	{
        // Remove all records
		RemoveAllRecords();
        Debug.Log("REMOVED ALL RECORDS");
        
		mAssesmentCam.enabled = true;
		mAssesmentGui.AssessmentMode = true;
		
		mMainCam = Camera.mainCamera;
		mMainCam.rect = mMiniScreen;

        if (mMainCam.GetComponent<MouseLook>())
        {            
            mMainCam.GetComponent<MouseLook>().enabled = false;
        }

        mPlayer = GameObject.Find("Player");

        if (mPlayer == null)
            return;

        mPlayer.GetComponent<CharacterMotor>().enabled = false;
        mPlayer.GetComponent<MouseLook>().enabled = false;
        mPlayer.GetComponent<FPSInputController>().enabled = false;

        //EnablePlayer.EnablePlayer(false);

	}
	
	// hides the assesment window and disables assesment camera
	internal void ScapeMode()
	{	
		mAssesmentCam.enabled = false;
        mAssesmentGui.AssessmentMode = false;
		
		mMainCam = Camera.mainCamera;
		mMainCam.rect = mFullScreen;

        if(mMainCam.GetComponent<MouseLook>())
            mMainCam.GetComponent<MouseLook>().enabled = true;

        mPlayer = GameObject.Find("Player");

        if (mPlayer == null)
            return;

        mPlayer.GetComponent<CharacterMotor>().enabled = true;
        mPlayer.GetComponent<MouseLook>().enabled = true;
        mPlayer.GetComponent<FPSInputController>().enabled = true;

	}
	
    #endregion

    #region Private members

    private void Awake() 
    {
        // Make this object persistence across all levels
        DontDestroyOnLoad(gameObject);        
		
		mAssesmentCam = GameObject.Find("AssesmentCamera").GetComponent<Camera>();		
		mAssesmentGui = GameObject.Find("AssesmentGUI").GetComponent<AssesmentGUI>();		
		mVcrScript = GameObject.Find("Systems").GetComponent<ReviewSystemVtr>();       

		ScapeMode();
    }

    private bool FindFeedback(AssessmentRecord record)
    {
        if (record.AssessorCategory.Equals("Feedback"))
        {
            Debug.Log("match found");
            return true;
        }
        else
            return false;

    }

    private void EnablePlayer(bool state) 
    {
        /*mPlayer.GetComponent<CharacterMotor>().enabled = state;
        mPlayer.GetComponent<MouseLook>().enabled = state;
        mPlayer.GetComponent<FPSInputController>().enabled = state;*/
    }
    
    #endregion

    #region Private members

    private StringBuilder mResult = new StringBuilder();
    
    private List<Quiz> mQuizes = new List<Quiz>();
    private FeedbackAssessor mFeedbackAssessor;		
	
	private Camera mAssesmentCam;
	private Camera mMainCam;
    private GameObject mPlayer;
	
	private AssesmentGUI mAssesmentGui;	
	private ReviewSystemVtr mVcrScript;
		
    private Rect mFullScreen = new Rect(0, 0, 1, 1);
	private Rect mMiniScreen = new Rect(.53f, .23f, 0.44f, 0.5f);

    private float mTotalScore = 0;
	
    #endregion
}
