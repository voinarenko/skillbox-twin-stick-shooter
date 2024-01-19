using System.Linq;
using Assets.Scripts.Logic;
using Assets.Scripts.Logic.EnemySpawners;
using Assets.Scripts.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint";
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var levelData = (LevelStaticData)target;
            if (GUILayout.Button("Collect"))
            {
                levelData.EnemySpawners = FindObjectsByType<SpawnMarker>(FindObjectsSortMode.None)
                    .Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.transform.position))
                    .ToList();
                levelData.LevelKey = SceneManager.GetActiveScene().name;
                levelData.InitialPlayerPosition = GameObject.FindWithTag(InitialPointTag).transform.position;
            }
            EditorUtility.SetDirty(target);
        }
    }
}