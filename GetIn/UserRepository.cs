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

        public IList<User> FindUser(LoginId loginId)
        {
            IQuery qry = Session.CreateQuery("from User u where u.Id.Value = :param1").SetString("param1", loginId.Value);
            IList<User> usrs = qry.List<User>();
            return usrs;
        }

        public IList<User> LookupUsers(User user){
            ICriteria aQuery = Session.CreateCriteria(typeof(GetIn.User));
            if (user.Name != null) {
                addRestriction(aQuery, "Name.FirstName", user.Name.FirstName);
                addRestriction(aQuery, "Name.LastName", user.Name.LastName);
            }
            if(user.Location != null){
                addRestriction(aQuery, "Location.Country", user.Location.Country);
                addRestriction(aQuery, "Location.City", user.Location.City);
                addRestriction(aQuery, "Location.ZipCode", user.Location.ZipCode);
            }
            return aQuery.List<User>();
        }

        private void addRestriction(ICriteria criteria, String type, String value){
            if (criteria != null && type != null && value != null){
                criteria.Add(Expression.Like(type, value));
            }
        }

        private void EnableFilter(String filterName, String paramName, String value){
            if(!String.IsNullOrEmpty(value)){
                Session.EnableFilter(filterName).SetParameter(paramName, value);
            }
        }
    }

    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(User usr)
            : base(string.Format("User {0} already exists.", usr.Id.Value))
        {
        }
    }

    public interface IUserRepository
    {
        void Save(User user);
        IList<User> FindUser(LoginId loginId);
        IList<User> LookupUsers(User user);
    }
}