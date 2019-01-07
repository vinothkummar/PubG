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
       

        public async Task<IEnumerable<KilliPrinter>> GetLivePlayerKilled()
        {
            try
            {

                var liveKilledFromCache =  _cacheService.RetrieveFromCache<IEnumerable<KilliPrinter>>("LiveKilledCache");

                if (liveKilledFromCache != null && liveKilledFromCache.Count() != 0)
                {   

                    return liveKilledFromCache;
                }
            }
            catch (Exception ex)
            {

                _logger.LogInformation("LiveKilledCache exception " + ex  + Environment.NewLine);
            }
            
            

            var playerKilledOrTeamEliminatedMessages = new List<KilliPrinter>();

            var kills = await _playerKillRepository.GetLiveKilled();

            // var tournamentMatchCreatedAt =  _eventRepository.GetEventCreatedAt(matchId).Result;

            //, tournamentMatchCreatedAt

            foreach (var rule in _rules)
            {
                var output =  rule.LiveKilledOrTeamEliminiated(kills);

                if (output != null)
                {
                    playerKilledOrTeamEliminatedMessages = output.ToList();
                }
            }

            await _cacheService.SaveToCache<IEnumerable<KilliPrinter>>("LiveKilledCache", playerKilledOrTeamEliminatedMessages, 1000, 1);

            return await Task.FromResult(playerKilledOrTeamEliminatedMessages);
        }
      
    }
}
