using System;
using System.Collections.Generic;

namespace GetIn{
    public class Group{
        public virtual string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual GroupPosts MessageList { get; set; }

        public void	 post(Post post1)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupPosts
    {
        public virtual IList<Post> groupPostList { get; set; } 
    }
}