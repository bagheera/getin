using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GetIn
{
    public class User
    {
        public User(){
        }

        public User(LoginId loginid, Name name)
        {
            Id = loginid;
            Name = name;
        }

        private int id;

        public virtual LoginId Id { get; private set; }

        public virtual Profile Profile { get; set; }

        public virtual Name Name
        {
            get;
            private set;
        }

        public virtual Image Picture { get; set; }

        public virtual Dislikes Dislikes { get; set; }

        public virtual Likes Likes { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual Location Location { get; set; }

        public virtual GetInDate DateOfBirth { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

    public class GetInDate
    {
        public DateTime Value { get; private set; }
        public GetInDate(DateTime dateTime)
        {
            Value = dateTime;
        }
    }

    public abstract class Preferences
    {
        private List<string> _prfs = new List<string>();

        public Preferences(params string[] prfs)
        {
            if (prfs == null) { throw new ArgumentNullException("prfs"); }

            _prfs.AddRange(prfs);
        }

        public void Add(params string[] prfs)
        {
            if (prfs == null) { throw new ArgumentNullException("prfs"); }

            _prfs.AddRange(prfs);
        }

        public string[] Value
        {
            get
            {
                return this._prfs.ToArray();
            }
        }
    }

    public class Likes : Preferences
    {
        public Likes(params string[] likes)
            : base(likes)
        {
        }
    }

    public class Dislikes : Preferences
    {
        public Dislikes(params string[] dlikes)
            : base(dlikes)
        {
        }
    }


    public class Name
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Name(string firstname, string lastname)
        {
            FirstName = firstname;
            LastName = lastname;
        }
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
        public LoginId(string id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is LoginId)
                return (obj as LoginId).Id == this.Id;
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public abstract class Gender
    {
    }

    public class Male : Gender
    {
        public override string ToString()
        {
            return "Male";
        }
    }

    public class Female : Gender
    {
        public override string ToString()
        {
            return "Female";
        }
    }

    public class Location
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}