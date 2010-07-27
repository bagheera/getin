using System;
using System.Collections.Generic;
using System.Linq;
using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Mapping;


namespace GetIn{
    //Comment for a git commit and cruise build test 
    public class User{
        private const int MAXIMUM_DEGREE_OF_SEPARATION = 3;
        
        public User(){
            UserProfileComments = new UserProfileComments();
            Friends = new HashedSet<User>();
            Inviters = new HashedSet<User>();
            Likes = new HashedSet<Like>();
            Dislikes = new HashedSet<Dislike>();
        }

        public User(LoginId loginid, Name name) : this(){
            LoginId = loginid;
            Name = name;
        }

        public virtual ISet<User> Friends { get; set; }

        public virtual ISet<User> Inviters { get; set; }

        public virtual IUserRepository Repository { get; set; }

        public virtual int Id { get; set; }

        public virtual LoginId LoginId { get; private set; }

        public virtual Profile Profile { get; set; }

        public virtual Name Name { get; set; }

        public virtual Photo Picture { get; set; }

        public virtual ISet<Dislike> Dislikes { get; set; }

        public virtual ISet<Like> Likes { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual Location Location { get; set; }

        public virtual GetInDate DateOfBirth { get; set; }

        public virtual void AddCommentToProfile(Comment comment){
            UserProfileComments.Add(comment);
        }

        public virtual ISet<Comment> GetAllProfileComments(){
            return UserProfileComments.List;
        }

        public virtual Comment GetLatestProfileComment(){
            return UserProfileComments.GetLastComment();
        }

        public virtual UserProfileComments UserProfileComments { get; set; }

        public override bool Equals(object obj){
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (User)) return false;
            return Equals((User) obj);
        }

        public virtual bool Equals(User other){
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override int GetHashCode(){
            return Id.GetHashCode();
        }

        public virtual void Register(){
            var usrToChkUnique = new User(this.LoginId, new Name());
            var usrs = Repository.LookupUsers(usrToChkUnique);
            if (usrs.Count != 0){
                throw new UserAlreadyExistsException(this);
            }
            Repository.Save(this);
        }

        public virtual void InviteFriend(User u){
            if (!Friends.Contains(u)){
                u.Inviters.Add(this);
            }
        }

        public virtual void AcceptFriendInvite(User friend){
            if (Inviters.Contains(friend)){
                Friends.Add(friend);
                friend.Friends.Add(this);
                Inviters.Remove(friend);
            }
        }

        public virtual void RejectFriendInvite(User u){
            if (Inviters.Contains(u)){
                Inviters.Remove(u);
            }
        }

        public virtual IList<User> LookupUsers(){
            return Repository.LookupUsers(this);
        }

        public virtual IList<User> LookupUsers(AgeRange ageRange){
            return Repository.LookupUsers(this, ageRange);
        }


        public virtual IList<User> DegreeOfSeparation(User indirectFriend){
            Dictionary<User, User> friendChain = new Dictionary<User, User>();
            Queue<User> userQueue = new Queue<User>();
            IList<User> path = new List<User>();
            userQueue.Enqueue(this);
            int friendChainLength = 0;
            while (userQueue.Count() != 0){
                User user = userQueue.Dequeue();

                if (friendChainLength++ > MAXIMUM_DEGREE_OF_SEPARATION)
                {
                    return path;
                }
                foreach (User friend in user.Friends){
                    if (!friendChain.ContainsKey(friend)){
                        friendChain.Add(friend, user);
                        if (friend.Equals(indirectFriend)){
                            break;
                        }
                        userQueue.Enqueue(friend);
                    }
                }
            }

            User tempFriend = indirectFriend;

            while (!tempFriend.Equals(this)){
                if (friendChain.ContainsKey(tempFriend)){
                    path.Add(tempFriend);
                    tempFriend = friendChain[tempFriend];
                }
            }
            return path;
        }

        public virtual bool isFriend(User other){
            return Friends.Contains(other);
        }

        public virtual double ComputeSimilarityScore(User user){
            double similarity = 0d;
            foreach (Like like in Likes){
                if (user.Likes.Contains(like))
                    similarity += 1;
            }

            foreach (Dislike disLike in Dislikes){
                if (user.Dislikes.Contains(disLike))
                    similarity += 0.7d;
            }

            return similarity;
        }

        public virtual IList<User> RecommendFriends(){
            IList<User> users = Repository.NotFriendsOf(this);
            IList<User> recommendedFriends = new List<User>();
            foreach (var user in users){
                if(ComputeSimilarityScore(user) >= 5){
                    recommendedFriends.Add(user);
                }
            }
            return recommendedFriends;
        }
    }


    public class Photo{
        public byte[] Bytes { get; set; }
    }

    public class GetInDate{
        public GetInDate(){
        }

        public GetInDate(DateTime dateTime){
            Value = dateTime;
        }

        public virtual DateTime Value { get; set; }

        public GetInDate Subtract(int years){
            return new GetInDate(Value.Subtract(new TimeSpan(365*years, 0, 0, 0)));
        }

        public override bool Equals(object obj){
            return base.Equals(obj);
        }

        public bool Equals(GetInDate other){
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Value.Equals(Value);
        }

        public override int GetHashCode(){
            return Value.GetHashCode();
        }
    }

    public class Like{
        private int id;
        public virtual LoginId UserId { get; set; }
        public virtual string Text { get; set; }

        public virtual bool Equals(Like other){
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Text, Text);
        }

        public override bool Equals(object obj){
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Like)) return false;
            return Equals((Like) obj);
        }

        public override int GetHashCode(){
            return (Text != null ? Text.GetHashCode() : 0);
        }
    }

    public class Dislike{
        private int id;
        public virtual LoginId UserId { get; set; }
        public virtual string Text { get; set; }

        public virtual bool Equals(Dislike other){
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Text, Text);
        }

        public override bool Equals(object obj){
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Dislike)) return false;
            return Equals((Dislike) obj);
        }

        public override int GetHashCode(){
            return (Text != null ? Text.GetHashCode() : 0);
        }

        public static bool operator ==(Dislike left, Dislike right){
            return Equals(left, right);
        }

        public static bool operator !=(Dislike left, Dislike right){
            return !Equals(left, right);
        }
    }

    public class Name{
        public Name(){
        }

        public Name(string firstname, string lastname){
            FirstName = firstname;
            LastName = lastname;
        }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public override bool Equals(object obj){
            if (obj is Name){
                var name = obj as Name;
                return name.FirstName.Equals(this.FirstName, StringComparison.OrdinalIgnoreCase) &&
                       name.LastName.Equals(this.LastName, StringComparison.OrdinalIgnoreCase);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode(){
            return (this.FirstName.ToLower() + this.LastName.ToLower()).GetHashCode();
        }
    }

    public class LoginId{
        public string Id { get; private set; }

        public LoginId(){
        }

        public LoginId(string id){
            Value = id;
        }


        public virtual string Value { get; set; }

        public override bool Equals(object obj){
            if (obj is LoginId)
                return (obj as LoginId).Value == this.Value;
            return base.Equals(obj);
        }

        public override int GetHashCode(){
            return this.Value.GetHashCode();
        }
    }

    public class Gender{
        private char gcode = 'M';

        public Gender(){
        }

        public Gender(char code){
            Code = code;
        }


        public char Code{
            get { return gcode; }
            set{
                if (gcode != 'M' && gcode != 'F'){
                    throw new ArgumentException("Invalid gender code.");
                }

                gcode = value;
            }
        }

        public override string ToString(){
            return Code == 'M' ? "Male" : "Female";
        }

        public override bool Equals(object obj){
            return base.Equals(obj);
        }


        public bool Equals(Gender other){
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Code == Code;
        }

        public override int GetHashCode(){
            return Code.GetHashCode();
        }
    }

    public class Location{
        public virtual string City { get; set; }
        public virtual string Country { get; set; }
        public virtual string ZipCode { get; set; }
    }

    public class UserProfileComments{
        public virtual ISet<Comment> List { get; set; }

        public UserProfileComments(){
            List = new HashedSet<Comment>();
        }

        public Comment GetLastComment(){
            return (List.OrderByDescending(p => p.CommentDate.Value)).FirstOrDefault();
        }

        public void Add(Comment comment){
            List.Add(comment);
        }
    }

    public class AgeRange{
        public int From { get; set; }
        public int To { get; set; }
    }
}