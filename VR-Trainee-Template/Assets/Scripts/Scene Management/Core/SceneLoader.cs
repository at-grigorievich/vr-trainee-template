namespace ATG.SceneManagement
{
    public abstract class SceneLoader
    {
        private readonly ISceneManagement _sceneManager;
        
        public SceneLoader(ISceneManagement sceneManager)
        {
            _sceneManager = sceneManager;
        }

        public void Load()
        {
            SceneInfoData seleted = GetSceneInfo();
            LoadScene(seleted);
        }

        protected abstract SceneInfoData GetSceneInfo();
        protected void LoadScene(SceneInfoData sceneInfo) => 
            _sceneManager.LoadBySceneInfoAsync(sceneInfo);
    }
}