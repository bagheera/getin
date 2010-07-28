using System.Collections.Generic;

namespace GetIn{
    public interface IGroupRepository{
        void Create(Group group);
        IList<Group> LookupGroup(Group group);
    }
}