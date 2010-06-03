﻿using System;
using System.Collections.Generic;
using NHibernate;
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
            ICriteria lookupCriteria = Session.CreateCriteria(typeof(User));
            if (user.LoginId != null)
            {
                AddRestriction(lookupCriteria, "LoginId", user.LoginId, true);
            }
            if (user.Name != null)
            {
                AddRestriction(lookupCriteria, "Name.FirstName", user.Name.FirstName, false);
                AddRestriction(lookupCriteria, "Name.LastName", user.Name.LastName, false);
            }
            if (user.Location != null)
            {
                AddRestriction(lookupCriteria, "Location.Country", user.Location.Country, false);
                AddRestriction(lookupCriteria, "Location.City", user.Location.City, false);
                AddRestriction(lookupCriteria, "Location.ZipCode", user.Location.ZipCode, false);
            }
            AddRestriction(lookupCriteria, "Gender", user.Gender, true);
            if (user.Profile != null && !String.IsNullOrEmpty(user.Profile.ProfileText)){
                Profile wildCardProfile = new Profile("%" + user.Profile.ProfileText + "%");
                AddRestriction(lookupCriteria, "Profile", wildCardProfile, false);
            }

            return lookupCriteria;
        }

        public IList<User> LookupUsers(User user){
            var lookupCriteria = BuildLookupCriteria(user);
            return lookupCriteria.List<User>();
        }

        public IList<User> LookupUsers(User user, AgeRange ageRange){
            var lookupCriteria = BuildLookupCriteria(user);
            if(ageRange != null){
                var beginDate = new GetInDate(DateTime.Now);
                beginDate.Subtract(ageRange.To);
                var endDate = new GetInDate(DateTime.Now);
                endDate.Subtract(ageRange.From);
                lookupCriteria.Add(Restrictions.Between("DateOfBirth", beginDate, endDate));
            }
            return lookupCriteria.List<User>();
        }

        private void AddRestriction(ICriteria criteria, String type, Object value,Boolean exactMatch){
            if (criteria != null && type != null && value != null){
                if (exactMatch){
                    criteria.Add(Restrictions.Eq(type, value));
                } else{
                    criteria.Add(Restrictions.Like(type, value));
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