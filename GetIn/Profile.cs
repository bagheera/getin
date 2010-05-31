using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn
{
    public class Profile
    {
        private String profile;

        public Profile(string givenProfile)
        {
            profile = givenProfile;
        }

        public override string ToString()
        {
            return profile;
        }
    }
}
