using System;
using System.Collections.Generic;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class PlayersWatcher : MonoBehaviour
    {
        private WaveChanger _waveChanger;
        private readonly List<PlayerDeath> _players = new();

        private Action _changed;

        private void Start()
        {
            _waveChanger = GetComponent<WaveChanger>();
            _changed += OnPlayersChanged;
        }

        public void AddPlayer(PlayerDeath player)
        {
            _players.Add(player);
            _changed?.Invoke();
            player.Happened += RemovePlayer;
        }

        private void RemovePlayer(PlayerDeath player)
        {
            _players.Remove(player);
            _changed?.Invoke();
            player.Happened -= RemovePlayer;
        }

        private void OnPlayersChanged()
        {
            if (_players.Count <= 0) _waveChanger.GameOver();
        }
    }
}