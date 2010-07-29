using System.Collections;
using System.Collections.Generic;

namespace GetIn
{
    public interface IUserPreferenceRepository
    {
        IList HottestLikes();
        IList HottestDislikes();
    }
}