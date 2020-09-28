using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Models
{
    public class Room
    {
        private HashSet<string> participants;
        private Dictionary<string, decimal?> votes;
        public int ID { get; private set; }

        public Room(int id)
        {
            this.ID = id;
            participants = new HashSet<string>();
            Reset();
        }

        public void Reset()
        {
            votes = new Dictionary<string, decimal?>();
            foreach (var participant in participants)
            {
                votes[participant] = null;
            }
        }

        public void AddParticipant(string id)
        {
            participants.Add(id);
            votes[id] = null;
        }

        public void RemoveParticipant(string id)
        {
            participants.Remove(id);
            votes.Remove(id);
        }

        public void AcceptVote(string participantId, decimal vote)
        {
            votes[participantId] = vote;
        }

        public decimal[] AllowedVotes
        {
            get { return new decimal[] { 0.5m, 1, 2, 3, 5, 8, 13, 21, 34, 55 }; }
        }

        private Dictionary<string, decimal> ValidVotes()
        {
            return votes.Where(p => p.Value != null).ToDictionary(k => k.Key, v => v.Value.Value);
        }

        public bool AllParticipantsHaveVoted()
        {
            return votes.All(p => p.Value != null);
        }

        private decimal RoundToNearest(decimal value)
        {
            return Math.Round(value * 10, MidpointRounding.AwayFromZero) / 10;
        }

        public decimal GetSummaryVote()
        {
            var average = ValidVotes().Average(p => p.Value);
            return RoundToNearest(average);
        }

        public Dictionary<string,decimal?> GetVotes()
        {
            return votes;
        }
    }
}