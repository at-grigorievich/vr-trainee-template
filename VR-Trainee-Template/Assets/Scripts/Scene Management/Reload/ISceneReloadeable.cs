using ATG.Activator;

namespace ATG.SceneManagement
{
    public interface ISceneReloader: IActivateable
    {
        public void ReloadScene();
    }
}