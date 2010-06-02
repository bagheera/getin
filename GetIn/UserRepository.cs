using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NHibernate;
using NHibernate.Cfg;

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
            IQuery qry = Session.CreateQuery("from User u where u.LoginId.Value = :param1").SetString("param1", loginId.Value);
            IList<User> usrs = qry.List<User>();
            return usrs;
        }

        public IList<User> LookupUsers(User user){
            if (user.Name != null && user.Name.FirstName != null){
                Session.EnableFilter("FirstNameFilter").SetParameter("firstName", user.Name.FirstName);
            } else{
                Session.DisableFilter("FirstNameFilter");
            }
            if (user.Name != null && user.Name.LastName != null){
                Session.EnableFilter("LastNameFilter").SetParameter("lastName", user.Name.LastName);
            } else{
                Session.DisableFilter("LastNameFilter");
            }
            IQuery query = Session.CreateQuery("from User");
            return query.List<User>();
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
        IList<User> FindUser(LoginId loginId);
        IList<User> LookupUsers(User user);
    }
}