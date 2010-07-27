using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections;
using NHibernate;

namespace GetIn
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ISession session;

        public LikeRepository(ISession session)
        {
            this.session = session;
        }

        public IList HottestLikes()
        {
            IQuery query =
                session.CreateQuery(
                    "select likes.Text, count(likes.UserId) from Like likes group by likes.Text order by count(likes.UserId) desc");
            IList result = query.List();
            return hottestThree(result);
        }

        private IList hottestThree(IList inputList)
        {
            IList result = new ArrayList();
            ISet hottestTemperatures = new HashedSet();
            foreach (Object[] row in inputList)
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
