using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class PlayersWatcher : MonoBehaviour
    {
        public int Players => _playerDeaths.Count;

        private readonly List<PlayerDeath> _playerDeaths = new();
        private readonly List<PlayerHudConnector> _playerHudConnectors = new();
        private readonly List<PlayerScore> _playerScores = new();

        private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService) => 
            _progressService = progressService;

        public void AddPlayer(PlayerDeath player)
        {
            _playerDeaths.Add(player);
            _playerHudConnectors.Add(player.GetComponent<PlayerHudConnector>());
            _playerScores.Add(player.GetComponent<PlayerScore>());
            player.Happened += RemovePlayer;
        }

        public List<PlayerHudConnector> GetConnectors() => 
            _playerHudConnectors;

        public void UpdateScore(int score)
        {
            foreach (var playerScore in _playerScores) 
                playerScore.UpdateScore(score/_playerScores.Count);
        }

        private void RemovePlayer(PlayerDeath player)
        {
            _playerDeaths.Remove(player);
            _playerHudConnectors.Remove(player.GetComponent<PlayerHudConnector>());
            var playerScore = player.GetComponent<PlayerScore>();
            _playerScores.Remove(playerScore);

            player.Happened -= RemovePlayer;
            if (_playerDeaths.Count <= 0)
            {
                var players = FindObjectsByType<PlayerHudConnector>(FindObjectsSortMode.None);
                foreach (var connector in players.Where(c => c.isClient)) 
                    PerformEndGameProcedure(connector);
                foreach (var connector in players.Where(c => c.isServer)) 
                    PerformEndGameProcedure(connector);
            }

            return;

            void PerformEndGameProcedure(PlayerHudConnector connector)
            {
                connector.GetComponent<PlayerScore>().RpcUpdateGlobalData(_progressService.Progress.WorldData.WaveData.Encountered,
                    GetValue(EnemyType.SmallMelee),
                    GetValue(EnemyType.BigMelee),
                    GetValue(EnemyType.Ranged));
                connector.RpcGameOver();
            }
        
            int GetValue(EnemyType type)
            {
                var result = 0;
                foreach (var pair in _progressService.Progress.WorldData.KillData.Killed.Where(x => x.Key == type))
                    result = pair.Value;
                return result;
            }
        }
    }
}