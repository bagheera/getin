using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetIn
{
    public class Comment
    {
        public User Commentor { get { return commentor;} set { commentor = @value;}}
        public Profile Profile { get { return commentedOn;} set { commentedOn = @value;}}
        public CommentContent Content { get { return content; } set { content = @value; }}

        private User commentor;
        private Profile commentedOn;
        private CommentContent content;

        public Comment(User commentor, Profile commentedOn, CommentContent content){
            Commentor = commentor;
            Profile = commentedOn;
            Content = content;
            Profile.addComment(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is Comment){
                return (this.Commentor.Equals(((Comment) obj).Commentor) &&
                        this.Profile.Equals(((Comment) obj).Profile) &&
                        this.Content.Equals(((Comment) obj).Content));
            }
            else{
                return false;
            }
        }
    }

    public class CommentContent{
        private string textComment;
        public String TextComment { get { return textComment;} set{ textComment = @value;} }
        public CommentContent(String comments){
            TextComment = comments;
        }
        public override bool Equals(object obj)
        {
            if (obj is CommentContent){
                return (this.TextComment.Equals(((CommentContent) obj).TextComment));
            }
            else{
                return false;
            }
        }
    }
}
