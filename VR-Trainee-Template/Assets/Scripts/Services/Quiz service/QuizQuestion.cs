using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ATG.Services.Quiz
{
    [Serializable]
    public sealed class QuizQuestionCreator
    {
        [SerializeField, TextArea] private string questionText;
        [SerializeField] private float points = 1f;
        [SerializeField] private bool withAnswers = true; // очков за верный ответ
        [Space(10)]
        [SerializeField] private QuizAnswerCreator[] answers;

        public QuizQuestion Create(byte index)
        {
            HashSet<QuizAnswer> answersSet = new();

            for(byte i = 0; i < answers.Length; i++)
            {
                answersSet.Add(answers[i].Create(i));
            }

            return new QuizQuestion(index, questionText, answersSet, points, withAnswers);
        }

        public QuizQuestion Create() => Create(0);
    }
        
    public readonly struct QuizQuestion
    {
        public byte Index {get;}
        public string Text {get;}
        public float Points {get;} // очков за верный ответ
        
        public readonly bool WithAnswers;

        public HashSet<QuizAnswer> Answers {get;}

        public int TrueAnswersCount => Answers.Count(answer => answer.IsTrue == true);

        public QuizQuestion(byte index, string text, HashSet<QuizAnswer> quizAnswers, float points, bool withAnswers)
        {
            Index = index;
            Text = text;

            Answers = quizAnswers;

            Points = points;

            WithAnswers = withAnswers;
        }

        public static QuizQuestion None => new QuizQuestion(0, string.Empty, null, 0, false);
    }
}