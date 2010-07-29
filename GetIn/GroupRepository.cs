using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace GetIn{
    public class GroupRepository : IGroupRepository{
        public GroupRepository(ISession session){
            Session = session;
        }
        public ISession Session { get; private set; }
        public void Create(Group group){
            Session.Save(group);
        }

        public bool Exists(Group group){
            ICriteria criteria = Session.CreateCriteria(typeof (Group));
            criteria.Add(Restrictions.Eq("Name", group.Name));
            return criteria.List<Group>().Count != 0;
        }
    }
}