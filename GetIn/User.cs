﻿using System;
using System.Drawing;
using System.Linq;
using Iesi.Collections.Generic;
using NHibernate.Mapping;

namespace GetIn
{
    public class User
    {
        public User(){
            UserProfileComments = new UserProfileComments();
            Friends = new HashedSet<User>();
        }

        public User(LoginId loginid, Name name)
        {
            UserProfileComments = new UserProfileComments();
            LoginId = loginid;
            Name = name;
            Friends = new HashedSet<User>();
        }


        public virtual ISet<User> Friends { get; set; }

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

        public virtual void AddCommentToProfile(Comment comment)
        {
            UserProfileComments.Add(comment);
        }
        
        public virtual ISet<Comment> GetAllProfileComments(){
            return UserProfileComments.List;
        }
        public virtual Comment GetLatestProfileComment()
        {
            return UserProfileComments.GetLastComment();
        }

        public virtual UserProfileComments UserProfileComments { get; set; }
        //public virtual ISet<Comment> CommentList { get; set; }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ((User)other).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public virtual void Register()
        {
            var usrs = Repository.FindUser(this.LoginId);
            if (usrs.Count != 0)
            {
                throw new UserAlreadyExistsException(this);
            }
            Repository.Save(this);
        }

        public virtual void	 AddFriend(User friend){
            Friends.Add	(friend);
        }
    }

    public class Photo
    {
        public byte[] Bytes { get; set; }
    }

    public class GetInDate
    {
        public GetInDate()
        {
        }

        public GetInDate(DateTime dateTime)
        {
            Value = dateTime;
        }

        public virtual DateTime Value { get; set; }
    }

    public class Like
    {
        private int id;
        public virtual LoginId UserId { get; set; }
        public virtual string Text { get; set; }
    }

    public class Dislike
    {
        private int id;
        public virtual LoginId UserId { get; set; }
        public virtual string Text { get; set; }
    }

    public class Name
    {
        public Name()
        {
        }

        public Name(string firstname, string lastname)
        {
            FirstName = firstname;
            LastName = lastname;
        }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Name)
            {
                Name name = obj as Name;
                return name.FirstName.Equals(this.FirstName, StringComparison.OrdinalIgnoreCase) &&
                       name.LastName.Equals(this.LastName, StringComparison.OrdinalIgnoreCase);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.FirstName.ToLower() + this.LastName.ToLower()).GetHashCode();
        }
    }

    public class LoginId
    {

        public string Id { get; private set; }
        public LoginId(){
        }

        public LoginId(string id)
        {
            Value = id;
        }

        public virtual string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is LoginId)
                return (obj as LoginId).Value == this.Value;
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }

    public class Gender
    {
        private char gcode = 'M';

        public Gender(){ }
        public Gender(char code) {
            Code = code; }


        public char Code
        {
            get { return gcode; }
            set
            {
                if (gcode != 'M' && gcode != 'F')
                {
                    throw new ArgumentException("Invalid gender code.");
                }

                gcode = value;
            }
        }

        public override string ToString()
        {
            return Code == 'M' ? "Male" : "Female";
        }

        public override bool Equals(object obj){
            return base.Equals(obj);
        }


        public bool Equals(Gender other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Code == Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }

    public class Location
    {
        public virtual string City { get; set; }
        public virtual string Country { get; set; }
        public virtual string ZipCode { get; set; }
    }

    public class UserProfileComments
    {
        //private ISet<Comment> listOfComments;
        public virtual ISet<Comment> List
        {
            get; set;
        }

        public UserProfileComments()
        {
            List = new HashedSet<Comment>();
        }

        public Comment GetLastComment()
        {
            return (List.OrderByDescending(p => p.CommentDate)).FirstOrDefault();
        }

        public void Add(Comment comment)
        {
            List.Add(comment);
        }
    }
}