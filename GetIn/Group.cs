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

        public virtual bool Equals(Group other){
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override bool Equals(object obj){
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Group)) return false;
            return Equals((Group) obj);
        }

        public override int GetHashCode(){
            return Id;
        }

        public static bool operator ==(Group left, Group right){
            return Equals(left, right);
        }

        public static bool operator !=(Group left, Group right){
            return !Equals(left, right);
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
