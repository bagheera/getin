using System;
using System.Collections.Generic;
using NHibernate;

namespace GetIn{
    public class GroupRepository : IGroupRepository{
        public GroupRepository(ISession session){
            
        }

        public void Create(Group group){
            
        }

        public bool Exists(Group group){
            return false;
        }
    }
}