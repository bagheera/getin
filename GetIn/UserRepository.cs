using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

namespace GetIn
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(ISession session)
        {
            Session = session;
        }
        public ISession Session { get; private set; }

        public void Save(User user)
        {
            Session.Save(user);
            Session.Flush();
        }

        public IList<User> LookupUsers(User user){
            ICriteria aQuery = Session.CreateCriteria(typeof(GetIn.User));
            if(user.LoginId != null){
                addRestriction(aQuery, "LoginId", user.LoginId ,true);
            }
            if (user.Name != null) {
                addRestriction(aQuery, "Name.FirstName", user.Name.FirstName,false);
                addRestriction(aQuery, "Name.LastName", user.Name.LastName,false);
            }
            if(user.Location != null){
                addRestriction(aQuery, "Location.Country", user.Location.Country,false);
                addRestriction(aQuery, "Location.City", user.Location.City, false);
                addRestriction(aQuery, "Location.ZipCode", user.Location.ZipCode, false);
            }
            return aQuery.List<User>();
        }

        private void addRestriction(ICriteria criteria, String type, Object value,Boolean exactMatch){
            if (criteria != null && type != null && value != null){
                if (!exactMatch){
                    criteria.Add(Expression.Like(type, value));
                } else{
                    criteria.Add(Expression.Eq(type, value));
                }
            }
        }
    }

    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(User usr)
            : base(string.Format("User {0} already exists.", usr.LoginId.Value))
        {
        }
    }

    public interface IUserRepository
    {
        void Save(User user);
        IList<User> LookupUsers(User user);
    }
}