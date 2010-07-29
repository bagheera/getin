using System;

namespace GetIn
{
    public class Post
    {
        public virtual int Id { get; set; }
        public virtual string Content { get; set; }
        public virtual DateTime PostedDateTime { get; set; }
    }
}