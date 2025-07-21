using System;
using System.Collections.Generic;
using ATG.Activator;

namespace ATG.Services.Quiz
{
    public interface IQuizService
    {
        int CurrentQuestIndex { get;}
        int TotalQuestCount {get;}
        int TrueQuestionsCount {get;}
        public int TotalTrueQuestionsCount {get;}

        event Action OnCompleteQuest;

        bool TryGetNextQuestion(out QuizQuestion question);
        IReadOnlyCollection<QuizAnswerResult> CheckQuestionAnswers(IReadOnlyCollection<QuizAnswerResult> userResult);

        float CalculateGraduate();
    }
}