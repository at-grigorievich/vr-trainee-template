using ATG.Services.Quiz;
using EntryPoints;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scopes
{
    public class OnlyQuizSceneScope: LifetimeScope
    {
        [SerializeField] private QuizServiceCreator quizServiceCreator;

        protected override void Configure(IContainerBuilder builder)
        {
            quizServiceCreator.Create(builder);

            builder.RegisterEntryPoint<OnlyQuizSceneEntryPoint>();
        }
    }
}