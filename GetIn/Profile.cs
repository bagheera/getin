using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn
{
    public class Profile
    {
        private String profileText;
        private ProfileComments commentList = new ProfileComments();

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
