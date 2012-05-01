using UnityEngine;
using System.Collections.Generic;
using System.Text;
using BaxaTech.AssessmentSystem;

public class Assesment
{
    // TODO HOW TO PERSIST
    public Assesment() { }

    #region Public methods

    public Quiz GetQuiz(int index)
    {
        if(index < 0 || index > mQuizes.Count)
        {
            Debug.LogError("Error in retrieving quiz. Invalid quiz index");
            return null;
        }

        return mQuizes[index];
    }

    public void LoadLevel(int level)
    {
        ClearQuizes();
        
        //Add quizes based on level
        AddQuizQuestions();
    }

    internal void SaveQuiz(Quiz quiz)
    {
        if (quiz == null)
        {
            Debug.LogError("Error in saving the answer. Answer is null ");
            return;
        }

    }

    #endregion

    #region Private methods

    private void ClearQuizes()
    {
        if (mQuizes == null)
        {
            Debug.LogError("Error. Unable to clear earlier quizes");
            return;
        }

        mQuizes.Clear();

        Debug.Log("Successfully cleared previous quizes");
    }

    private void AddQuizQuestions()
    {
        List<Answer> answerChoices1 = new List<Answer>() { new Answer("The liquid becomes a solid because the molecules are moving more slowly", 
                                                                      "Loss. Observe the globe more closely.", 
                                                                      1.0f), 
                                                          new Answer("The liquid releases gas as the molecules move quickly and have energy to escape.",
                                                                     "Observant Human. Consider cooking with grease or water with boiling loss. ", 
                                                                     2.0f),
                                                          new Answer("The liquid becomes plasma as the molecules have enough energy to glow", 
                                                                     "Loss. No observable light emission.", 
                                                                     0.0f),
                                                          new Answer("The liquid remains unchanged as a liquid.", 
                                                                     "Loss. Observe the globe more closely.", 
                                                                     0.0f)
                                                        };

        Quiz quiz1 = new Quiz("Question 1 - Engage the explanation that describes what happens when you push the red button.", answerChoices1, 0);
        AddQuiz(quiz1);


        List<Answer> answerChoices2 = new List<Answer>() { new Answer("The blue button aligns the molecules to become more dense, the red button spreads " + "\n" + 
                                                                      "the molecules to less density.", 
                                                                      "Loss. The reason for the density changes remains ambiguous." + "\n" + 
                                                                      "What do the buttons do to create those changes?", 
                                                                      1.0f), 
                                                          new Answer("The blue button aligns the molecules to decrease density, the red button expands the " +"\n"+
                                                                     "molecules and creates greater density. ",
                                                                     "Loss. Access the definition of density and observe the globe again.", 
                                                                     0.0f),
                                                          new Answer("The density of the materials remains the same from red to blue button.", 
                                                                     "Loss. More observation is needed of the molecules.", 
                                                                     0.0f),
                                                          new Answer("The blue button removes energy, causing the molecules to align in lesser density, " + "\n" + 
                                                                     "the red button adds energy to allow molecules to escape from liquid phase.", 
                                                                     "Observant human. Application of heat energy dramatically changes how dense the molecules are.", 
                                                                     2.0f)
                                                        };




        Quiz quiz2 = new Quiz("Question 2 - Engage the best description of molecular density for the liquid.", answerChoices2, 0);
        AddQuiz(quiz2);


        List<Answer> answerChoices3 = new List<Answer>() { new Answer("The less dense gas molecules escape causing the dense liquid to fall.",
                                                                      "Loss. Dismiss factors of density.", 
                                                                      0.0f), 
                                                          new Answer("The molecules change to a different chemical in the air and falls back to the water.",
                                                                     "Loss. Observe no chemical change in globe.", 
                                                                     0.0f),
                                                          new Answer("Heat energy is released causing the molecules to slow and get closer returning" + "\n" + 
                                                                     "to the liquid.", 
                                                                     "Observant human. Similar to your ice water glass on a hot day leaving a water ring on a table.", 
                                                                     2.0f),
                                                          new Answer("The container directs the flow of energy to fall back into a new container.", 
                                                                     "Loss. The container bears little effect.", 
                                                                     1.0f)
                                                        };

        Quiz quiz3 = new Quiz("Question 3 - Engage the factor that explains the process of gas to liquid drops, your condensation.", answerChoices3, 0);
        AddQuiz(quiz3);


        List<Answer> answerChoices4 = new List<Answer>() { new Answer("Heat energy caused the molecules to move faster and farther apart until some " + "\n" + 
                                                                      "molecules have enough energy to escape as a gas.",
                                                                      "Observant human. Ponder why a fog appears on a pond some mornings.", 
                                                                      2.0f),
                                                          new Answer("The liquid changed into a gas when heated because it expands and the container" + "\n" + 
                                                                     "can no longer hold it.",
                                                                     "Loss.  The container bears little effect.", 
                                                                     1.0f),
                                                          new Answer("The molecules change to a different chemical that is a gas and escapes from the water.", 
                                                                     "Loss. Observe no molecular change in globe.", 
                                                                     0.0f),
                                                          new Answer("when the liquid was heated, molecules of gas are dissolved and rise and escape " + "\n" +
                                                                      "because they are less dense.", 
                                                                     "Loss. Dismiss factors of density", 
                                                                     0.0f)
                                                        };


        Quiz quiz4 = new Quiz("Question 4 - Engage the explanation why the liquid changed to a gas.", answerChoices4, 0);
        AddQuiz(quiz4);

    }

    private bool AddQuiz(Quiz quiz)
    {
        if(quiz == null)
        {
            Debug.LogError("Error in adding quiz. Quiz is null");
            return false;
        }

        if(mQuizes.Contains(quiz))
        {
            Debug.Log("Failed to add quiz. Quiz already exists ");
            return false;
        }

        mQuizes.Add(quiz);

        return true;
    }

    private bool RemoveQuiz(Quiz quiz)
    {
        return false;
    }

    private bool RemoveQuiz(int quizId)
    {
        return false;
    }
    #endregion

    #region Private members

    private static Assesment mInstance; 
    private static List<Quiz> mQuizes = new List<Quiz>();

    #endregion
}
