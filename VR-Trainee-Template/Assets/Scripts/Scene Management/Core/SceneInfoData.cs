using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ATG.SceneManagement
{
    [CreateAssetMenu(menuName = "Configs/New Scene Info", fileName = "new_scene_config")]
    public sealed class SceneInfoData : ScriptableObject
    {
#if UNITY_EDITOR
        [field: SerializeField] public SceneAsset Scene { get; private set; }
#endif
        [SerializeField] private string sceneName;

        [field: SerializeField] public bool LoadAdditive { get; private set; } = false;
        [field: SerializeField] public bool AllowUnload { get; private set; } = true;
        [field: SerializeField] public Material Skybox { get; private set; }
        
        public string SceneName => sceneName;

        public bool IsSceneHasInBuildSettings(out int sceneIndex)
        {
#if UNITY_EDITOR
            sceneName = Scene.name;

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif

            sceneIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{sceneName}.unity");


            if (sceneIndex == -1)
            {
                throw new Exception($"Scene {sceneName} does not exist in Build Settings !");
            }

            return sceneIndex >= 0;
        }

        public int? GetBuildSettingsIndex()
        {
            if (IsSceneHasInBuildSettings(out int index)) return index;
            return null;
        }

        [Button("Update")]
        public void Update()
        {
#if UNITY_EDITOR
            if(Scene == null)
                throw new NullReferenceException("Scene is not set!");
            
            sceneName = Scene.name;

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}