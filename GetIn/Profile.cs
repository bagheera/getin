using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn
{
    public class Profile
    {
        private String profileText;

        public Profile(){
            
        }
        public Profile(string givenProfile)
        {
            profileText = givenProfile;
        }

        public virtual String ProfileText
        {
            get { return profileText; }
            set { profileText = value; }
        }

        public override string ToString()
        {
            return profileText;
        }
    }
}
