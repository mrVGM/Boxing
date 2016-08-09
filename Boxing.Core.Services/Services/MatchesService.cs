using Boxing.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxing.Contracts.Dto;
using Boxing.Contracts;
using Boxing.Core.Sql;
using Boxing.Core.Sql.Entities;
using Boxing.Core.Services.Exceptions;

namespace Boxing.Core.Services.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly BoxingContext _db;

        public MatchesService(BoxingContext db)
        {
            _db = db;
        }

        public async Task<bool> cancel(int id)
        {
            MatchEntity match = _db.Matches.Find(id);
            if (match == null)
            {
                throw new NotFoundException();
            }
            else
            {
                if (match.isClosed)
                {
                    return false;
                }
                else
                {
                    _db.deletePredictionsAsync(id);
                    
                    return true;
                }
            }
        }

        public async Task<Unit> changePrediction(string username, int matchId, int winner)
        {
            PredictionEntity prediction = _db.Predictions.Where(x => x.match.id == matchId && x.user.username == username).FirstOrDefault();
            if (prediction == null)
            {
                throw new NotFoundException();
            }
            else if (prediction.match.isClosed)
            {
                throw new ForbiddenException();
            }
            else
            {
                prediction.winner = winner;
                return Unit.Value;
            }
        }

        public async Task<Match> create(Match request)
        {
            MatchEntity match = new MatchEntity()
            {
                boxer1 = request.boxer1,
                boxer2 = request.boxer2,
                dateOfMatch = request.dateOfMatch,
                description = request.description,
                place = request.place,
                isClosed = false,
                winner = -1
            };
            
            try
            {
                match = _db.Matches.Add(match);
                _db.SaveChanges();
                
                return new Match {
                    id = match.id,
                    boxer1 = match.boxer1,
                    boxer2 = match.boxer2,
                    place = match.place,
                    dateOfMatch = match.dateOfMatch,
                    description = match.description,
                };
            }
            catch(Exception e)
            {
                throw new BadRequestException();
            }
        }

        public async Task<IEnumerable<Match>> getMatches(int skip, int take)
        {
            try {
                var matches = _db.Matches.OrderByDescending(e => e.dateOfMatch).Skip(skip).Take(take).ToArray();
                Match[] res = new Match[matches.Length];

                for (int i = 0; i < matches.Length; i++)
                {
                    var cur = matches[i];
                    if (cur.isClosed)
                    {
                        res[i] = new Match
                        {
                            id = cur.id,
                            boxer1 = cur.boxer1,
                            boxer2 = cur.boxer2,
                            place = cur.place,
                            dateOfMatch = cur.dateOfMatch,
                            description = cur.description,
                            winner = cur.winner,
                            hasFinished = true
                        };
                    }
                    else
                    {
                        res[i] = new Match
                        {
                            id = cur.id,
                            boxer1 = cur.boxer1,
                            boxer2 = cur.boxer2,
                            place = cur.place,
                            dateOfMatch = cur.dateOfMatch,
                            description = cur.description,
                            winner = -1,
                            hasFinished = false
                        };
                    }

                }
                return res;
            }
            catch (Exception e)
            {
                throw new BadRequestException();
            }
        }

        public async Task<IEnumerable<Match>> getOpen()
        {
            return _db.Matches.Where(x => !x.isClosed)
                      .OrderByDescending(x => x.dateOfMatch)
                      .Select(x => new Match
                      {
                          id = x.id,
                          boxer1 = x.boxer1,
                          boxer2 = x.boxer2,
                          place = x.place,
                          dateOfMatch = x.dateOfMatch,
                          description = x.description,
                          winner = -1,
                          hasFinished = false
                      });
        }

        public int getPrediction(int matchId, string username)
        {
            var prediction = _db.Predictions.Include("user").Include("match").Where(x => x.match.id == matchId && x.user.username.Equals(username)).FirstOrDefault();
            if (prediction == null)
            {
                return -1;
            }
            return prediction.winner;
        }

        public async Task<Unit> makePrediction(string username, int matchId, int winner)
        {
            MatchEntity match = _db.Matches.Find(matchId);
            UserEntity user = _db.Users.Find(username);

            if (match == null || user == null)
            {
                throw new NotFoundException();
            }
            else if (match.isClosed)
            {
                throw new BadRequestException();
            }
            else
            {
                PredictionEntity pred = _db.Predictions.Where(x => x.match.id == matchId && x.user.username.Equals(username)).FirstOrDefault();
                if (pred != null)
                {
                    pred.winner = winner;
                }
                else
                {
                    pred = new PredictionEntity();

                    pred.match = match;
                    pred.user = user;
                    pred.winner = winner;
                    user.predictions.Add(pred);

                    _db.Predictions.Add(pred);
                }
                _db.SaveChanges();

                return Unit.Value;
            }
            
        }

        public async Task<IEnumerable<Match>> search(int skip, int take, string searchString)
        {
            MatchEntity [] matches = _db.Matches.Where(x => x.boxer1 == searchString || x.boxer2 == searchString || x.place == searchString)
                       .OrderByDescending(x => x.dateOfMatch).Skip(skip).Take(take).ToArray();

            Match[] resp = new Match[matches.Length];
            for (int i = 0; i < matches.Length; i++)
            {
                MatchEntity cur = matches[i];

                if (cur.isClosed)
                {
                    resp[i] = new Match
                    {
                        id = cur.id,
                        boxer1 = cur.boxer1,
                        boxer2 = cur.boxer2,
                        dateOfMatch = cur.dateOfMatch,
                        place = cur.place,
                        description = cur.description,
                        winner = cur.winner,
                        hasFinished = true
                    };
                }
                else
                {
                    resp[i] = new Match
                    {
                        id = cur.id,
                        boxer1 = cur.boxer1,
                        boxer2 = cur.boxer2,
                        dateOfMatch = cur.dateOfMatch,
                        place = cur.place,
                        description = cur.description,
                        winner = -1,
                        hasFinished = false
                    };
                }
                
            }
            return resp;
        }

        public async Task<Match> setWinner(int matchId, int winner)
        {
            MatchEntity match = _db.Matches.Find(matchId);

            if (match == null)
            {
                throw new NotFoundException();
            }
            else
            {
                match.isClosed = true;
                match.winner = winner;

                _db.SaveChanges();

                _db.updateRatinsAsync(matchId, winner);
                
                return new Match
                {
                    id = match.id,
                    boxer1 = match.boxer1,
                    boxer2 = match.boxer2,
                    place = match.place,
                    dateOfMatch = match.dateOfMatch,
                    description = match.description,
                    winner = match.winner,
                    hasFinished = true
                };
            }
        }
    }
}
