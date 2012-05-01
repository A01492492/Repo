using UnityEngine;
using System.Collections.Generic;
using System;

public class Quiz
{

    #region Public methods

    public Quiz(string question, List<Answer> answerChoices, int correctAnswerIndex)
    {
        if(!AreAnswerChoicesValid(answerChoices))
            throw new ArgumentException("Error - Invalid answer choices. Must have only 4 choices");

        if(!IsAnswerIndexValid(correctAnswerIndex))
            throw new ArgumentException("Error - Invalid correct answer choice index. Must be less than 3");

        this.mQuestion = question;
        this.mAnswerChoices = answerChoices;
        this.mCorrectAnswerIndex = correctAnswerIndex;
        this.mDefaultQuestion = "No question found";
    }

    public string[] GetAnswerChoicesArray()
    {
        if (mAnswerChoices == null || mAnswerChoices.Count < 1)
            return null;

        string[] answers = new string[4];
        
        for (int i = 0; i < mAnswerChoices.Count; i++)
            answers[i] = mAnswerChoices[i].AnswerDescription;

        return answers;
    }
    
    #endregion    

    #region Internal properties

    internal string Question 
    {
        get 
        {
            if (mQuestion == null)
                return mDefaultQuestion;

            return mQuestion; 
        }
    }

    internal int SelectedAnswerIndex
    {
        get { return mSelectedAnswerIndex; }
        set { mSelectedAnswerIndex = value; }
    }

    internal string SelectedtAnswer
    {
        get
        {
            if (mSelectedAnswerIndex == -1)
                return "No answer";

            return mAnswerChoices[mSelectedAnswerIndex].AnswerDescription;
        }
        
    }

    internal string CorrectAnswer
    {
        get 
        {
            if (mCorrectAnswerIndex == -1)
                return "No answer provided";

            return mAnswerChoices[mCorrectAnswerIndex].AnswerDescription;
        }
        
    }

    internal float PointsScored
    {
        get
        {
            if (mSelectedAnswerIndex == -1)
                return 0;

            return mAnswerChoices[mSelectedAnswerIndex].Score;
        }

    }

    internal string Feedback
    {
        get
        {
            if (mSelectedAnswerIndex == -1)
                return "No feedback";

            return mAnswerChoices[mSelectedAnswerIndex].Feedback;
        }        
    }

    #endregion

    #region Private methods

    private bool AreAnswerChoicesValid(List<Answer> answerChoices)
    {
        if(answerChoices == null || answerChoices.Count != 4)
            return false;

        return true;
    } 

    private bool IsAnswerIndexValid(int correctAnswerIndex)
    {
        if(correctAnswerIndex < 0 || correctAnswerIndex > 3)
            return false;

        return true;
    }

    #endregion

    #region Private members

    private List<Answer> mAnswerChoices;

    private string mQuestion;
    private string mDefaultQuestion;
    
    private int mSelectedAnswerIndex = -1;
    private int mCorrectAnswerIndex = -1;


    #endregion
}
