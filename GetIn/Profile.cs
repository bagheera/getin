using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn{
    public class Profile{
        private const int MAX_PROFILE_LENGTH = 102400;
        private String profileText;

        public Profile(){
        }

        public Profile(string givenProfile){
            if (givenProfile.Length > MAX_PROFILE_LENGTH){
                givenProfile = givenProfile.Substring(0, MAX_PROFILE_LENGTH);
            }
            profileText = givenProfile;
        }

        public String ProfileText{
            get { return profileText; }
            set { profileText = value; }
        }

        public override string ToString(){
            return profileText;
        }

        public override bool Equals(object obj){
            return base.Equals(obj);
        }
    }
}