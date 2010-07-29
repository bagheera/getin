using System.Collections.Generic;

namespace GetIn{
    public interface IGroupRepository{
        void Create(Group group);
        bool Exists(Group group);
    }
}