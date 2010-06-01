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

        public String ProfileText
        {
            get { return profileText; }
            set { profileText = value; }
        }

        public override string ToString()
        {
            return profileText;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
