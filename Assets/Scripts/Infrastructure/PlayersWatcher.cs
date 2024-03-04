using Assets.Scripts.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class PlayersWatcher : MonoBehaviour
    {
        private const string StorageTag = "Storage";
        [SerializeField] private List<PlayerDeath> _playerDeaths = new();
        [SerializeField] private List<PlayerHudConnector> _playerHudConnectors = new();
        [SerializeField] private List<PlayerShooter> _playerShooters = new();

        private DataStorage _storage;
        private WaveChanger _waveChanger;

        private void Start()
        {
            _storage = GameObject.FindWithTag(StorageTag).GetComponent<DataStorage>();
            _waveChanger = GetComponent<WaveChanger>();
        }

        public void AddPlayer(PlayerDeath player)
        {
            _playerDeaths.Add(player);
            _playerHudConnectors.Add(player.GetComponent<PlayerHudConnector>());
            _playerShooters.Add(player.GetComponent<PlayerShooter>());
            player.Happened += RemovePlayer;
        }

        public List<PlayerHudConnector> GetConnectors() => 
            _playerHudConnectors;

        public void UpdateScore(int score)
        {
            foreach (var shooter in _playerShooters) 
                shooter.PlayerDynamicData.ScoreData.UpdateScore(score/_playerShooters.Count);
        }

        private void RemovePlayer(PlayerDeath player)
        {
            _playerDeaths.Remove(player);
            _playerHudConnectors.Remove(player.GetComponent<PlayerHudConnector>());
            var shooter = player.GetComponent<PlayerShooter>();
            _storage.PlayerDynamicData = shooter.PlayerDynamicData;
            _playerShooters.Remove(shooter);

            player.Happened -= RemovePlayer;
            if (_playerDeaths.Count <= 0) 
                _waveChanger.GameOver();
        }
    }
}