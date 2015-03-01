using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Models
{
    public class Room
    {
        private HashSet<string> participants;
        private Dictionary<string, int?> votes;
        public int ID { get; private set; }

        public Room(int id)
        {
            this.ID = id;
            participants = new HashSet<string>();
            Reset();
        }

        public void Reset()
        {
            votes = new Dictionary<string, int?>();
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

        public void AcceptVote(string participantId, int vote)
        {
            votes[participantId] = vote;
        }

        public int[] AllowedVotes
        {
            get { return new int[] { 1, 2, 3, 5, 8, 13, 21, 34, 55 }; }
        }

        private Dictionary<string, int> ValidVotes()
        {
            return votes.Where(p => p.Value != null).ToDictionary(k => k.Key, v => v.Value.Value);
        }

        public bool AllParticipantsHaveVoted()
        {
            return votes.All(p => p.Value != null);
        }

        public int GetSummaryVote()
        {
            return (int)Math.Round(ValidVotes().Average(p => p.Value));
        }

        public Dictionary<string,int?> GetVotes()
        {
            return votes;
        }
    }
}