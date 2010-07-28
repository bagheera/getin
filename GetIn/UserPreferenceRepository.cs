using System;
using System.Collections;
using System.Collections.Generic;
using Iesi.Collections;
using NHibernate;

namespace GetIn
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly ISession session;

        public UserPreferenceRepository(ISession session)
        {
            this.session = session;
        }

        public IList HottestLikes()
        {
            const string queryString =
                "select likes.Text, count(likes.UserId) from Like likes group by likes.Text order by count(likes.UserId) desc";
            return getHottestWithQuery(queryString);
        }

        private IList getHottestWithQuery(string queryString)
        {
            IQuery query =
                session.CreateQuery(
                    queryString);
            IList<object[]> result = query.List<Object[]>();
            return hottestThree(result);
        }

        public IList HottestDislikes()
        {
            const string queryString =
                "select dislikes.Text, count(dislikes.UserId) from Dislike dislikes group by dislikes.Text order by count(dislikes.UserId) desc";
            return getHottestWithQuery(queryString);
        }

        private static IList hottestThree(IEnumerable<object[]> inputList)
        {
            IList result = new ArrayList();
            ISet hottestTemperatures = new HashedSet();
            foreach (var row in inputList)
            {
                bool isAdded = hottestTemperatures.Add(row[1]);
                if (isAdded)
                {
                    if (hottestTemperatures.Count > 3)
                    {
                        break;
                    }
                }
                result.Add(row[0]);
            }
            return result;
        }
    }
}
