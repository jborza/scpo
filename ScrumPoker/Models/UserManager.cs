using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Models
{
    public class UserManager
    {
        private static UserManager instance;
        public static UserManager Instance
        {
            get { return instance ?? (instance = new UserManager()); }
        }

        public Dictionary<string, int> userToRoomIdDictionary;

        public UserManager()
        {
            userToRoomIdDictionary=new Dictionary<string, int>();
        }

        //TODO encapsulate
    }
}