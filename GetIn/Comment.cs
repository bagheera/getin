using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn
{
    public class Comment
    {
        public virtual User Commentor { get; set; }
        public virtual User CommentedOn { get; set; }
        //public String Content { get; set; }
        public virtual String Content { get; set; }

        private int id;

        public Comment(){
        }

        public Comment(User commentor, User commentedOn, String content){
            Commentor = commentor;
            CommentedOn = commentedOn;
            Content = content;
            CommentedOn.AddCommentToProfile(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is Comment){
                return (this.Commentor.Equals(((Comment) obj).Commentor) &&
                        this.CommentedOn.Equals(((Comment) obj).CommentedOn) &&
                        this.Content.Equals(((Comment) obj).Content));
            }
            else{
                return false;
            }
        }
    }

//    public class CommentContent{
//        public System.String TextComment { get; set; }
//
//        public CommentContent(){
//            
//        }
//
//        public CommentContent(System.String comments){
//            TextComment = comments;
//        }
//        public override bool Equals(object obj)
//        {
//            if (obj is CommentContent){
//                return (this.TextComment.Equals(((CommentContent) obj).TextComment));
//            }
//            else{
//                return false;
//            }
//        }
//    }
}
