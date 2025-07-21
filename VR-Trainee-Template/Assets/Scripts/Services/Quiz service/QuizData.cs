using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ATG.Services.Quiz
{
    [CreateAssetMenu(menuName = "configs/quiz", fileName = "new_quiz_config")]
    public sealed class QuizData: ScriptableObject
    {
        [SerializeField] private QuizQuestionCreator[] questions;
        [SerializeField] private int neededQuestionIndex = -1;

        public IReadOnlyCollection<QuizQuestion> GetRandomQuests(int count)
        {
            HashSet<QuizQuestion> result = new HashSet<QuizQuestion>(count);
            
            var creators = GetRandomQuestionContainers(count).ToArray();

            byte counter = 1;
            
            if(neededQuestionIndex >= 0)
            {
                QuizQuestionCreator needed = questions[neededQuestionIndex];

                if(creators.Contains(needed) == false)
                {
                    int rndCreator = UnityEngine.Random.Range(0, creators.Length);
                    creators[rndCreator] = needed;
                }
            }

            foreach(var creator in creators)
            {
                result.Add(creator.Create(counter));
                counter++;
            }

            return result;
        }

        public IReadOnlyCollection<QuizQuestionCreator> GetRandomQuestionContainers(int count)
        {
            HashSet<QuizQuestionCreator> creators = new HashSet<QuizQuestionCreator>();

            while(creators.Count < count)
            {
                QuizQuestionCreator res = GetRandomQuestionContainer();

                if(creators.Contains(res) == false)
                {
                    creators.Add(res);
                }
            }

            return creators;
        }

        private QuizQuestionCreator GetRandomQuestionContainer()
        {
            int rnd = UnityEngine.Random.Range(0, questions.Length);

            return questions[rnd];
        }
    }
}