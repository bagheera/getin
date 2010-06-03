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

        private ICriteria BuildLookupCriteria(User user){
            ICriteria lookupCriteria = Session.CreateCriteria(typeof(GetIn.User));
            if (user.LoginId != null)
            {
                addRestriction(lookupCriteria, "LoginId", user.LoginId, true);
            }
            if (user.Name != null)
            {
                addRestriction(lookupCriteria, "Name.FirstName", user.Name.FirstName, false);
                addRestriction(lookupCriteria, "Name.LastName", user.Name.LastName, false);
            }
            if (user.Location != null)
            {
                addRestriction(lookupCriteria, "Location.Country", user.Location.Country, false);
                addRestriction(lookupCriteria, "Location.City", user.Location.City, false);
                addRestriction(lookupCriteria, "Location.ZipCode", user.Location.ZipCode, false);
            }
            addRestriction(lookupCriteria, "Gender", user.Gender, true);
            if (user.Profile != null && !String.IsNullOrEmpty(user.Profile.ProfileText)){
                Profile wildCardProfile = new Profile("%" + user.Profile.ProfileText + "%");
                addRestriction(lookupCriteria, "Profile", wildCardProfile, false);
            }

            return lookupCriteria;
        }

        public IList<User> LookupUsers(User user){
            ICriteria lookupCriteria = BuildLookupCriteria(user);
            return lookupCriteria.List<User>();
        }

        public IList<User> LookupUsers(User user, AgeRange ageRange){
            ICriteria aQuery = BuildLookupCriteria(user);
            if(ageRange != null){
                DateTime fromYear = DateTime.Now.Subtract(new TimeSpan(365*ageRange.To,0,0,0));
                DateTime toYear = DateTime.Now.Subtract(new TimeSpan(365*ageRange.From,0,0,0));
                aQuery.Add(Expression.Between("DateOfBirth", new GetInDate(fromYear), new GetInDate(toYear)));

                //Year - ageRange.From;
//                int toYear = DateTime.Now.Year - ageRange.To;

//                aQuery.Add(Expression.Between("(day(current_date()) - day(DateOfBirth.Value) )", ageRange.From, ageRange.To));
                //aQuery.Add()
            }
            return aQuery.List<User>();
        }

        private void addRestriction(ICriteria criteria, String type, Object value,Boolean exactMatch){
            if (criteria != null && type != null && value != null){
                if (exactMatch){
                    criteria.Add(Expression.Eq(type, value));
                } else{
                    criteria.Add(Expression.Like(type, value));
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
        IList<User> LookupUsers(User user,AgeRange ageRange);

    }
}