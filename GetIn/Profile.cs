using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn{
    public class Profile{
        private const int MAX_PROFILE_LENGTH = 102400;
        private System.String profileText;


        public Profile(){
        }

        public Profile(string givenProfile){
            ProfileText = givenProfile;
        }

        public virtual String ProfileText
        {
            get { return profileText; }
            
            set{
                if (value.Length > MAX_PROFILE_LENGTH)
                {
                    value = value.Substring(0, MAX_PROFILE_LENGTH);
                }
                profileText = value;
            }
        }

        public override string ToString(){
            return profileText;
        }

        public override bool Equals(object obj){
            return profileText == ((Profile) obj).ProfileText;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}