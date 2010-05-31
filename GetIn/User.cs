using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn
{
    public class User
    {
        private Profile profile;

        public Profile Profile
        {
            get { return profile; }
            set { profile = value; }
        }
    }
}
