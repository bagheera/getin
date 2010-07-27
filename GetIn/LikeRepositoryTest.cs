using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using NHibernate;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class LikeRepositoryTest : NHibernateFixtureBase
    {

        private ISession session;
        private ILikeRepository likeRepo;
        private IUserRepository usrRep;
        private User user1;
        private User user3;
        private User user2;
        private User user4;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("User.hbm.xml"), new FileInfo("Comment.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            session = CreateSession();
            session.BeginTransaction();
            likeRepo = new LikeRepository(session);
            usrRep = new UserRepository(session);
            repositorySetUp();
        }

        private void repositorySetUp()
        {

            

            user1 = new User(new LoginId("123"), new Name("Mark", "Twain"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-19)),
                Location = new Location { City = "Banglore", Country = "India" },
                Gender = new Gender('F'),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Big Profile"),
            };
           
            user2 = new User(new LoginId("345"), new Name("Sudhakar", "Rayavaram"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-28)),
                Location = new Location { City = "Seattle", Country = "USA" },
                Gender = new Gender(),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Short and sweet crap"),
            };
            
            user3 = new User(new LoginId("678"), new Name("Alex", "Anto"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                Location = new Location { City = "Banglore", Country = "India" },
                Gender = new Gender(),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Some profile information which is useless!!! \n Some more crap!"),
            };            
            
            user4 = new User(new LoginId("202"), new Name("Alex", "Anto"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                Location = new Location { City = "Banglore", Country = "India" },
                Gender = new Gender(),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Some profile information which is useless!!! \n Some more crap!"),
            };
            
            
        }



        [TearDown]
        public void TearDown()
        {
            session.Transaction.Rollback();
            session.Dispose();
        }

        [Test]
        public void LikeRepositoryShouldReturnTopThreeLikes()
        {
            
            //Add some likes
            user1.Likes.Add(new Like() {UserId = user1.LoginId, Text = "applesMostCommon"});
            user1.Likes.Add(new Like() {UserId = user1.LoginId, Text = "orangesSecondMostCommon"});
            user1.Likes.Add(new Like() {UserId = user1.LoginId, Text = "plumsThirdMostCommon"});
            user1.Likes.Add(new Like() {UserId = user1.LoginId, Text = "pineapples"});
            user1.Likes.Add(new Like() {UserId = user1.LoginId, Text = "bananas"});

            user2.Likes.Add(new Like() { UserId = user2.LoginId, Text = "applesMostCommon" });
            user2.Likes.Add(new Like() { UserId = user2.LoginId, Text = "orangesSecondMostCommon" });
            user2.Likes.Add(new Like() { UserId = user2.LoginId, Text = "plumsThirdMostCommon" });

            user3.Likes.Add(new Like() { UserId = user3.LoginId, Text = "applesMostCommon" });
            user3.Likes.Add(new Like() { UserId = user3.LoginId, Text = "orangesSecondMostCommon" });

            user4.Likes.Add(new Like() { UserId = user4.LoginId, Text = "applesMostCommon" });

            registerUsers();

            IList likesList = new LikeRepository(session).TopLikes();
            Assert.NotNull(likesList);
            Assert.AreEqual(3, likesList.Count);
            Assert.AreEqual(likesList[0], "applesMostCommon");
            Assert.AreEqual(likesList[1], "orangesSecondMostCommon");
            Assert.AreEqual(likesList[2], "plumsThirdMostCommon");
        }

//        [Test]
//        public void WhenTieThenTopThreeCountsOfLikesAreReturned()
//        {
//            //Add some likes
//            user1.Likes.Add(new Like() { UserId = user1.LoginId, Text = "rank1" });
//            user1.Likes.Add(new Like() { UserId = user1.LoginId, Text = "anotherRank1" });
//            user1.Likes.Add(new Like() { UserId = user1.LoginId, Text = "rank2" });
//            user1.Likes.Add(new Like() { UserId = user1.LoginId, Text = "rank3" });
//            user1.Likes.Add(new Like() { UserId = user1.LoginId, Text = "rank4" });
//
//            user2.Likes.Add(new Like() { UserId = user2.LoginId, Text = "rank1" });
//            user2.Likes.Add(new Like() { UserId = user2.LoginId, Text = "anotherRank1" });
//            user2.Likes.Add(new Like() { UserId = user2.LoginId, Text = "rank2" });
//            user2.Likes.Add(new Like() { UserId = user2.LoginId, Text = "rank3" });
//
//            user3.Likes.Add(new Like() { UserId = user3.LoginId, Text = "rank1" });
//            user3.Likes.Add(new Like() { UserId = user3.LoginId, Text = "anotherRank1" });
//            user3.Likes.Add(new Like() { UserId = user3.LoginId, Text = "rank2" });
//
//            user4.Likes.Add(new Like() { UserId = user4.LoginId, Text = "rank1" });
//            user4.Likes.Add(new Like() { UserId = user4.LoginId, Text = "anotherRank1" });
//
//            registerUsers();
//
//            IList likesList = new LikeRepository(session).TopLikes();
//            Assert.NotNull(likesList);
//            Assert.AreEqual(4, likesList.Count);
//            Assert.AreEqual(likesList[2], "rank2");
//            Assert.AreEqual(likesList[3], "rank3");
//        }

        private void registerUsers()
        {
            user1.Repository = usrRep;
            user2.Repository = usrRep;
            user3.Repository = usrRep;
            user4.Repository = usrRep;

            user1.Register();
            user2.Register();
            user3.Register();
            user4.Register();
        }
    }

    public interface ILikeRepository
    {
        IList TopLikes();
    }

    public class LikeRepository : ILikeRepository
    {
        private readonly ISession session;

        public LikeRepository(ISession session)
        {
            this.session = session;
        }

        public IList TopLikes()
        {
//            IQuery query = session.CreateQuery("select likes.Text, count(likes.UserId) from Like likes group by likes.Text order by count(likes.UserId) desc");
            IQuery query = session.CreateQuery("select likes.Text from Like likes group by likes.Text order by count(likes.UserId) desc");
            query.SetMaxResults(3);
            IList result = query.List();
//            return hottestThree(result);
            return result;
        }

//        private IList hottestThree(IList result)
//        {
//            IList result = new IList();
//            int currentRank = 0;
//            int currentRankTemperature = 0;
//        }
    }
}
