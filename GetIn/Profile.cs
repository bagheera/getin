using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn{
    public class Profile{
        private const int MAX_PROFILE_LENGTH = 102400;
        private String profileText;
        private ProfileComments commentList = new ProfileComments();

        public Profile(){
        }

        public Profile(string givenProfile){
            ProfileText = givenProfile;
        }

        public String ProfileText{
            
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

        public void addComment(Comment comment){
            CommentList.add(comment);
        }

        public Comment getLatestComment(){
            return CommentList.getLastComment();
        }

        public ProfileComments	 CommentList{
            get { return commentList; }
            set { throw new NotImplementedException(); }
        }
    }

    public class ProfileComments{
        private List<Comment> listOfComments;
        public List<Comment> List 
        { 
            get{ return listOfComments;} 
            set {listOfComments.Concat(@value);}
        }

        public ProfileComments(){
            listOfComments = new List<Comment>();
        }

        public Comment getLastComment(){
            return List.LastOrDefault();
        }

        public void add(Comment comment){
            List.Add(comment);
        }
    }
}