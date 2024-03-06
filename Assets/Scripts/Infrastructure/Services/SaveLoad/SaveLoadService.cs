using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.Parameters;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string SettingsKey = "Settings";

        private readonly ISettingsService _settingsService;

        public SaveLoadService(ISettingsService settingsService) => 
            _settingsService = settingsService;

        public void SaveSettings() => 
            PlayerPrefs.SetString(SettingsKey, _settingsService.Settings.ToJson());

        public Settings LoadSettings() =>
            PlayerPrefs.GetString(SettingsKey)?.ToDeserialized<Settings>();
    }
}