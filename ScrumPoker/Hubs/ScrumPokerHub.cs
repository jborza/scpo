using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using ScrumPoker.Models;

namespace ScrumPoker.Hubs
{
    public interface IPokerClient
    {
        void RevealVotes();
        void ShowSummary(object summary);
        void UpdateStatus(Dictionary<string, int?> getVotes);
        void HideVotes();
        void HideClientVote();
    }

    //TODO use groups
    public class ScrumPokerHub : Hub<IPokerClient>
    {
        private Room GetModel(int roomId)
        {
            return RoomManager.Instance.GetRoom(roomId);
        }

        public void CastVote(int roomId, string sender, int vote)
        {
            GetModel(roomId).AcceptVote(sender, vote);
            BroadcastStatus(roomId);
            if (!GetModel(roomId).AllParticipantsHaveVoted())
                return;
            Clients.Group(roomId.ToString()).RevealVotes();
            Clients.Group(roomId.ToString()).ShowSummary(GetModel(roomId).GetSummaryVote());
        }
            
        public void AddClient(int roomId, string guid)
        {
            Groups.Add(Context.ConnectionId, roomId.ToString());
            GetModel(roomId).AddParticipant(guid);
            UserManager.Instance.userToRoomIdDictionary.Add(guid,roomId);
            BroadcastStatus(roomId);
        }

        public void DisconnectClient(int roomId, string guid)
        {
            Groups.Remove(guid, roomId.ToString());
            GetModel(roomId).RemoveParticipant(guid);            
        }

        public void BroadcastStatus(int roomId)
        {
            Clients.Group(roomId.ToString()).UpdateStatus(GetModel(roomId).GetVotes());
        }

        public void ResetRoom(int roomId)
        {
            GetModel(roomId).Reset();
            Clients.Group(roomId.ToString()).HideVotes();
            Clients.Group(roomId.ToString()).HideClientVote();
            Clients.Group(roomId.ToString()).ShowSummary("");
            BroadcastStatus(roomId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //remove the user from the list and the connected group
            var clientId = this.Context.ConnectionId;
            int roomId = UserManager.Instance.userToRoomIdDictionary[clientId];
            DisconnectClient(roomId,clientId);
            BroadcastStatus(roomId);
            return base.OnDisconnected(stopCalled);
        }
    }
}