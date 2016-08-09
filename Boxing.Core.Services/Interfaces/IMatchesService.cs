using Boxing.Contracts;
using Boxing.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Boxing.Core.Services.Interfaces
{
    public interface IMatchesService
    {
        Task<Match> create(Match request);
        Task<IEnumerable<Match>> getMatches(int skip, int take);
        Task<Unit> makePrediction(string username, int matchId, int winner);
        Task<Unit> changePrediction(string username, int matchId, int winner); 
        Task<Match> setWinner(int matchId, int winner);
        Task<bool> cancel(int id);
        Task<IEnumerable<Match>> getOpen();
        Task<IEnumerable<Match>> search(int skip, int take, string searchString);
        int getPrediction(int matchId, string username);
    }
}