using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Models
{
    public class RoomManager
    {
        private static RoomManager instance;
        public static RoomManager Instance
        {
            get { return instance ?? (instance = new RoomManager()); }
        }

        private static Random random;
        private Dictionary<int, Room> rooms;

        public RoomManager()
        {
            rooms = new Dictionary<int, Room>();
        }

        public Room GetRoom(int id)
        {
            if (rooms.ContainsKey(id) == false)
                rooms[id] = new Room(id);
            return rooms[id];
        }

        public Room CreateRoom()
        {
            int id;
            do
            {
                id = random.Next();
            } while (rooms.ContainsKey(id) == false);
            rooms[id] = new Room(id);
            return GetRoom(id);
        }
    }
}