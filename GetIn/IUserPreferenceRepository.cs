using System.Collections;

namespace GetIn
{
    public interface IUserPreferenceRepository
    {
        IList HottestLikes();
        IList HottestDislikes();
    }
}