using System;
using System.Collections.Generic;
using System.Linq;

namespace ATG.Services.Quiz
{
    public static class QuizQuestionExtensions
    {
        public static IReadOnlyCollection<QuizAnswerResult> CheckQuestionAnswers(this QuizQuestion quest, IReadOnlyCollection<QuizAnswerResult> userResults)
        {
            if(userResults.Count != quest.Answers.Count)
                throw new ArgumentOutOfRangeException("difference between user results count and question answers count");

            QuizAnswerResult[] userResultsArr = userResults.ToArray();
            QuizAnswerResult[] resposeResults = new QuizAnswerResult[quest.Answers.Count];

            float rateByOne = 1f / quest.TrueAnswersCount;
            float rateForQuestion = 0f;

            foreach(var answer in quest.Answers)
            {
                int index = answer.Index;

                var userAnswer = userResultsArr[index];

                rateForQuestion += userAnswer.AnswerVariant == true && answer.IsTrue ? rateByOne : 0;

                resposeResults[index] = new QuizAnswerResult(index, answer.IsTrue);
            }

            return resposeResults;
        }
    }
}