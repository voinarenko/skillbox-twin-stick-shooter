using System;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class PlayersWatcher : MonoBehaviour
    {
        private WaveChanger _waveChanger;
        [SerializeField] private List<PlayerDeath> _playerDeaths = new();
        [SerializeField] private List<PlayerHudConnector> _playerHudConnectors = new();

        private Action _changed;

        private void Start()
        {
            _waveChanger = GetComponent<WaveChanger>();
            _changed += OnPlayersChanged;
        }

        public void AddPlayer(PlayerDeath player)
        {
            _playerDeaths.Add(player);
            _playerHudConnectors.Add(player.GetComponent<PlayerHudConnector>());
            _changed?.Invoke();
            player.Happened += RemovePlayer;
        }

        public List<PlayerHudConnector> GetConnectors() => 
            _playerHudConnectors;

        private void RemovePlayer(PlayerDeath player)
        {
            _playerDeaths.Remove(player);
            _playerHudConnectors.Remove(player.GetComponent<PlayerHudConnector>());
            _changed?.Invoke();
            player.Happened -= RemovePlayer;
        }

        private void OnPlayersChanged()
        {
            if (_playerDeaths.Count <= 0) _waveChanger.GameOver();
        }
    }
}