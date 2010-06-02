using System;

namespace GetIn{
    public class Comment{
        private int id;

        public Comment(){
        }

        public Comment(User commentor, User commentedOn, String content){
            Commentor = commentor;
            CommentedOn = commentedOn;
            Content = content;
            CommentedOn.AddCommentToProfile(this);
        }

        public virtual User Commentor { get; set; }
        public virtual User CommentedOn { get; set; }
        public virtual String Content { get; set; }

        public override bool Equals(object obj){
            if (obj is Comment){
                return (Commentor.Equals(((Comment) obj).Commentor) &&
                        CommentedOn.Equals(((Comment) obj).CommentedOn) &&
                        Content.Equals(((Comment) obj).Content));
            }
            return false;
        }
    }
}