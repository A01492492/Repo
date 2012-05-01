/**
*	Copyright (c) 2011, Baxajaunak Technology, L.L.C.
*	All Rights Reserved. 
*/

using UnityEngine;
using System.Collections.Generic;
using BaxaTech.AssessmentSystem;

//#if BUILD_WITH_NAMESPACES
//namespace BaxaTech.AssessmentSystem
//{
//#endif

/// <summary>
///		IAssessor component that defines the default assessment performed for the Completion 
///		category. Assessment is only performed once, and is awarded the greatest possible points.
/// </summary>
/// <remarks>
///		This assessor class generates records of type PointRangeRecord
/// </remarks>
public class FeedbackAssessor : IAssessor
{
    #region Constructor

    /// <summary>Default constructor</summary>
    FeedbackAssessor()
    {
        Category = "Feedback";     
    }

    #endregion

    #region Properties

    public Quiz Quiz
    {
        get { return m_quiz; }
        set { m_quiz = value; }
    }

    #endregion

    #region Overrides

    /// <summary>
    ///		Performs assessment of the instance
    /// </summary>
    /// <param name="timestamp">
    ///		Timestamp to use when recording assessment events
    /// </param>
    /// <returns>
    ///		Whether or not the assessment was actually performed.
    /// </returns>
    protected override bool PerformAssessmentImpl(float timestamp)
    {
        if (mAssesmentManager == null)
        {
            Debug.LogError("Failed to save the record. Assesssment Manager is null");
            return false;
        }
        
        // add the selected answer and correct answer to records
        mAssesmentManager.AddRecord(new FeedbackRecord(this, timestamp)
        {
            Quiz = m_quiz
        });       

        return true;
    }

    #endregion
	
	#region Private methods
		
	private void Awake()
	{
		if(GameObject.Find("Assessment") != null)
			mAssesmentManager = GameObject.Find("Assessment").GetComponent<AssessmentManager>();
	}
	
	#endregion

    #region Data

    private Quiz m_quiz;
    private AssessmentManager mAssesmentManager;
    #endregion
}

//#if BUILD_WITH_NAMESPACES
//}// end AssessmentSystem
//#endif