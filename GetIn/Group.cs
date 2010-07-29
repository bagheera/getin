using System;
using System.Collections.Generic;

namespace GetIn{
    public class Group{
        public Group(string name){
            Name = name;
        }

        public Group(){
        }

        public virtual string Name { get; set; }
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual GroupPosts MessageList { get; set; }

        public virtual void post(Post post1)
        {
            // throw new NotImplementedException();
        }
    }

    public class GroupPosts
    {
        public virtual IList<Post> groupPostList { get; set; } 
    }

    public class GroupAlreadyExistsException : Exception{
        public GroupAlreadyExistsException(Group group)
            : base(string.Format("Group {0} already exists.", group.Name)){
        }
    }
}
