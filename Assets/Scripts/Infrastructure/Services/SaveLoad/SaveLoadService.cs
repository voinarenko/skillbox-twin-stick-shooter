using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.Parameters;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private const string SettingsKey = "Settings";

        private readonly IPersistentProgressService _progressService;
        private readonly ISettingsService _settingsService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory, ISettingsService settingsService)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
            _settingsService = settingsService;
        }

        //public void SaveProgress()
        //{
        //    foreach (var progressWriter in _gameFactory.ProgressWriters)
        //        progressWriter.UpdateProgress(_progressService.Progress);

        //    PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
        //}

        //public PlayerProgress LoadProgress() => 
        //    PlayerPrefs.GetString(ProgressKey)?
        //        .ToDeserialized<PlayerProgress>();

        public void SaveSettings() => 
            PlayerPrefs.SetString(SettingsKey, _settingsService.Settings.ToJson());

        public Settings LoadSettings() =>
            PlayerPrefs.GetString(SettingsKey)?.ToDeserialized<Settings>();
    }
}