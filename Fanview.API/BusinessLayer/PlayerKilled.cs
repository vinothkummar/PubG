using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanview.API.BusinessLayer.Contracts;
using Fanview.API.Repository.Interface;
using Fanview.API.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Fanview.API.Services.Interface;

namespace Fanview.API.BusinessLayer
{
    public class PlayerKilled : IPlayerKilled
    {
       
        List<IKillingRule> _rules = new List<IKillingRule>();
        private IPlayerKillRepository _playerKillRepository;       
        private ILogger<PlayerKilled> _logger;        
        private ITeamRepository _teamRepository;
        private ITeamPlayerRepository _playerRepository;
        private IEventRepository _eventRepository;
        private ICacheService _cacheService;
        private IAssetsRepository _assetsRepository;

        public PlayerKilled(IPlayerKillRepository playerKillRepository,                           
                            ILogger<PlayerKilled> logger,                           
                            ITeamPlayerRepository playerRepository,
                            IEventRepository eventRepository,
                            ITeamRepository teamRepository,
                            IAssetsRepository assetsRepository,
                            ICacheService cacheService                           
                            )
        {
            _playerKillRepository = playerKillRepository;
            _logger = logger;           
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;            
            _eventRepository = eventRepository;
            _assetsRepository = assetsRepository;
            _cacheService = cacheService;



            _rules.Add(new IndividualPlayerKilled(_teamRepository, _assetsRepository, playerRepository));
           
        }

        public IEnumerable<string> GetLast4PlayerKilledText(string matchId)
        {

            var playerKilledOrTeamEliminatedMessages = new List<string>();

            var kills =  _playerKillRepository.GetLast4PlayerKilled(matchId).Result;

            foreach (var rule in _rules)
            {
                var output = rule.PlayerKilledOrTeamEliminiatedText(kills);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages.AddRange(output);
                }
            }


            return playerKilledOrTeamEliminatedMessages;
        }

        public IEnumerable<string> GetPlayerKilledText(string matchId)
        {
            _logger.LogInformation("GetPlayedKilledText Business Layer Function call started" + Environment.NewLine);

            try
            {
                var playerKilledOrTeamEliminatedMessages = new List<string>();

                var kills = _playerKillRepository.GetPlayerKilled(matchId).Result;

                foreach (var rule in _rules)
                {
                    var output = rule.PlayerKilledOrTeamEliminiatedText(kills);

                    if (output != null)
                    {
                        playerKilledOrTeamEliminatedMessages = output.ToList();
                    }
                }

                _logger.LogInformation("GetPlayedKilledText Business Layer Function call completed" + Environment.NewLine);

                return playerKilledOrTeamEliminatedMessages;

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetPlayerKilledText");

                throw;
            }
           
        }

       

        public async Task<IEnumerable<KilliPrinter>> GetLivePlayerKilled(int matchId)
        {            
            var liveKilledFromCache = await _cacheService.RetrieveFromCache<IEnumerable<KilliPrinter>>("LiveKilledCache");
            
            if (liveKilledFromCache != null && liveKilledFromCache.Count() != 0 )
            {
                _logger.LogInformation("GetLiveKilled returned from LiveKilledCache " + Environment.NewLine);

                return liveKilledFromCache;
            }

            var playerKilledOrTeamEliminatedMessages = new List<KilliPrinter>();

            var kills = _playerKillRepository.GetLiveKilled(matchId).Result;

            var tournamentMatchCreatedAt =  _eventRepository.GetEventCreatedAt(matchId).Result;

            foreach (var rule in _rules)
            {
                var output =  rule.LiveKilledOrTeamEliminiated(kills, tournamentMatchCreatedAt);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages = output.ToList();
                }
            }

            await _cacheService.SaveToCache<IEnumerable<KilliPrinter>>("LiveKilledCache", playerKilledOrTeamEliminatedMessages, 30, 5);

            return await Task.FromResult(playerKilledOrTeamEliminatedMessages);
        }
      
    }
}
