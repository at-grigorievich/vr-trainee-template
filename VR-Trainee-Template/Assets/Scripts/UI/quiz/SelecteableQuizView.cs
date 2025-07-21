using System;
using System.Collections.Generic;
using System.Linq;
//using ATG.Observable;
using ATG.Services.Quiz;
using TMPro;
using UnityEngine;
using VContainer;

namespace ATG.UI
{
    public readonly struct OnlyQuestionTextQuizViewData
    {
        public string Text {get;}

        public OnlyQuestionTextQuizViewData(QuizQuestion quizQuestion)
        {
            Text = quizQuestion.Text;
        }
    }

    public readonly struct SelecteableQuizViewData
    {
        public QuizQuestion Question {get;}
        public Action<QuizAnswerResult[]> Callback {get;}

        public SelecteableQuizViewData(QuizQuestion question, Action<QuizAnswerResult[]> callback)
        {
            Question = question;
            Callback = callback;
        }
    }

    public readonly struct SelecteableQuizViewResultData
    {
        public QuizQuestion Question {get;}
        public QuizAnswerResult[] Results {get;}

        public SelecteableQuizViewResultData(QuizQuestion question, QuizAnswerResult[] results)
        {
            Question = question;
            Results = results;
        }
    }

    public sealed class SelecteableQuizView : UIView
    {
        public const float AdditionalHeight = 1.25f;
        
        public const string AnswerText = "Узнать";
        public const string CloseText = "Закрыть";
        
        public const string ResultOutput = "Вы верно ответили на <color=green>{0}/{1}</color> вопросов.";

        public const string MoreAnswers = "(Выберите несколько ответов)";
        public const string OneAnswer = "(Выберите только один ответ)";

        [SerializeField] private GameObject questionPlaceholder;
        [SerializeField] private GameObject answersPlaceholder;
        [Space(5)]
        [SerializeField] private TextMeshProUGUI questionTextOutput;
        [SerializeField] private TextMeshProUGUI questionAnswerCountOutput;
        [Space(5)]
        [SerializeField] private AnswerElementCreator answerElementCreator;
        [SerializeField] private ScaleClickButton nextButton;
        [Space(10)]
        [SerializeField] private bool isHideOnAwake;

        //[Inject] private IMessageBroker _messageBroker;

        private QuizQuestion _currentQuestion;
        private List<AnswerElement> _answerInstances;

        private SelecteableQuizViewData _data;

        public override UIElementType ViewType => UIElementType.Quiz;

        private void Start()
        {
            if(isHideOnAwake == true)
                Hide();
        }

        public override void Show(object sender, object data)
        {
            base.Show(sender, data);
            
            if(data is SelecteableQuizViewData quizData)
            {
                _data = quizData;
                _currentQuestion = _data.Question;

                UpdateQuestion();
            }
            else if(data is SelecteableQuizViewResultData results)
            {
                _currentQuestion = results.Question;
                ShowReadyResultsQuestion(results.Results);
            }
            else if(data is OnlyQuestionTextQuizViewData onlyText)
            {
                ShowOnlyQuestionText(onlyText.Text);
            }
        }

        public override void Hide()
        {
            if(gameObject != null)
            {
                base.Hide();
            }

            nextButton.Hide();

            if(_answerInstances != null)
            {
                foreach(var answerElement in _answerInstances)
                {
                    answerElement.OnSelected -= OnAnswerUserSelected;
                    //answerElement.Hide();
                }
            }
        }

        private void UpdateQuestion()
        {
            questionPlaceholder.SetActive(true);
            answersPlaceholder.SetActive(true);

            UpdateQuestionTextOutput();
            UpdateAnswers();

            Action userSelectAnswer = OnUserSelectAnswerHandler;

            nextButton.UpdateText(AnswerText);
            nextButton.Show(this, userSelectAnswer);
        }

        private void ShowOnlyQuestionText(string text)
        {
            questionPlaceholder.SetActive(true);
            answersPlaceholder.SetActive(false);

            questionTextOutput.text = text;

            //Action hideQuiz = Hide;
            //hideQuiz += () => _messageBroker.Send(new CloseSelecteableQuizViewMessage(this));

            nextButton.UpdateText(CloseText);
            //nextButton.Show(this, hideQuiz);
        }

        private void ShowReadyResultsQuestion(QuizAnswerResult[] results)
        {
            questionPlaceholder.SetActive(true);
            answersPlaceholder.SetActive(true);

            UpdateQuestionTextOutput();
            UpdateAnswers(false);

            for(int i = 0; i < results.Length; i++)
            {
                bool IsSelected = results[i].AnswerVariant;

                if(IsSelected)
                {
                    _answerInstances[i].SelectBlocked();
                }
                else
                {
                    _answerInstances[i].UnselectBlocked();
                }
            }

            QuizAnswerResult[] requestResult = _currentQuestion.CheckQuestionAnswers(results).ToArray();

            ShowQuestionResult(requestResult);
        }

        private void OnUserSelectAnswerHandler()
        {
            if(Array.TrueForAll(_answerInstances.ToArray(), a => a.IsSelected == false) == true)
            {
                return;
            }

            nextButton.Hide();

            if(_answerInstances == null) return;
            
            QuizAnswerResult[] userResults = new QuizAnswerResult[_answerInstances.Count];
            
            for(int i = 0; i < _answerInstances.Count; i++)
            {
                var answer = _answerInstances[i];

                answer.OnSelected -= OnAnswerUserSelected;

                userResults[i] = answer.GetUserAnswer();
            }

            _data.Callback?.Invoke(userResults);

            QuizAnswerResult[] requestResult = _currentQuestion.CheckQuestionAnswers(userResults).ToArray();

            ShowQuestionResult(requestResult);
        }

        private void ShowQuestionResult(QuizAnswerResult[] requestResults)
        {
            int counter = 0;

            foreach(var answerInstance in _answerInstances)
            {
                answerInstance.ShowResult(requestResults[counter]);
                counter++;
            }

            Action hideQuiz = Hide;
            //hideQuiz += () => _messageBroker.Send(new CloseSelecteableQuizViewMessage(this));
        
            nextButton.UpdateText(CloseText);
            nextButton.Show(this, hideQuiz);
        }

        private void UpdateAnswers(bool allowSelected = true)
        {
            int needAnswerElements = _currentQuestion.Answers.Count;

            int trueAnswersCount = _currentQuestion.TrueAnswersCount;
        
            _answerInstances = new List<AnswerElement>(answerElementCreator.GetAnswers(needAnswerElements));

            int count = 0;
            
            questionAnswerCountOutput.text = trueAnswersCount > 1 ? MoreAnswers : OneAnswer;

            foreach(var answer in _currentQuestion.Answers)
            {
                _answerInstances[count].Show(this, answer);

                // Если можно выбрать только один верный ответ, то остальные должны автоматически отключиться
                if(trueAnswersCount == 1 && allowSelected == true)
                {
                    _answerInstances[count].OnSelected += OnAnswerUserSelected;
                }

                count++;
            }
        }

        private void UpdateQuestionTextOutput()
        {
            questionTextOutput.text = _currentQuestion.Text;
        }

        private void OnAnswerUserSelected(AnswerElement selected)
        {
            foreach(var answer in _answerInstances)
            {
                if(ReferenceEquals(answer, selected) == true) continue;

                answer.Unselect();
            }
        }
    }
}