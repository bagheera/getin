using System;
using System.Collections;
using System.IO;
using NHibernate;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class UserPreferenceRepositoryTest : NHibernateFixtureBase
    {
        private ISession session;
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
            usrRep = new UserRepository(session);
            repositorySetUp();
        }

        private void repositorySetUp()
        {
            user1 = new User(new LoginId("123"), new Name("Mark", "Twain"))
                        {
                            DateOfBirth = new GetInDate(DateTime.Today.AddYears(-19)),
                            Location = new Location {City = "Banglore", Country = "India"},
                            Gender = new Gender('F'),
                            Picture = new Photo {Bytes = new byte[] {1, 2, 3, 4, 5}},
                            Profile = new Profile("Big Profile"),
                        };

            user2 = new User(new LoginId("345"), new Name("Sudhakar", "Rayavaram"))
                        {
                            DateOfBirth = new GetInDate(DateTime.Today.AddYears(-28)),
                            Location = new Location {City = "Seattle", Country = "USA"},
                            Gender = new Gender(),
                            Picture = new Photo {Bytes = new byte[] {1, 2, 3, 4, 5}},
                            Profile = new Profile("Short and sweet crap"),
                        };

            user3 = new User(new LoginId("678"), new Name("Alex", "Anto"))
                        {
                            DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                            Location = new Location {City = "Banglore", Country = "India"},
                            Gender = new Gender(),
                            Picture = new Photo {Bytes = new byte[] {1, 2, 3, 4, 5}},
                            Profile = new Profile("Some profile information which is useless!!! \n Some more crap!"),
                        };

            user4 = new User(new LoginId("202"), new Name("Alex", "Anto"))
                        {
                            DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                            Location = new Location {City = "Banglore", Country = "India"},
                            Gender = new Gender(),
                            Picture = new Photo {Bytes = new byte[] {1, 2, 3, 4, 5}},
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
        public void UserPreferenceRepositoryShouldReturnTopThreeLikes()
        {
            const string hottest = "applesMostCommon";
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = hottest});
            const string secondHottest = "orangesSecondMostCommon";
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = secondHottest});
            const string thirdHottest = "plumsThirdMostCommon";
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = thirdHottest});
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "pineapples"});
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "bananas"});

            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = hottest});
            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = secondHottest});
            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = thirdHottest});

            user3.Likes.Add(new Like {UserId = user3.LoginId, Text = hottest});
            user3.Likes.Add(new Like {UserId = user3.LoginId, Text = secondHottest});

            user4.Likes.Add(new Like {UserId = user4.LoginId, Text = hottest});

            registerUsers();

            IList likesList = new UserPreferenceRepository(session).HottestLikes();
            Assert.NotNull(likesList);
            Assert.AreEqual(3, likesList.Count);
            Assert.AreEqual(likesList[0], hottest);
            Assert.AreEqual(likesList[1], secondHottest);
            Assert.AreEqual(likesList[2], thirdHottest);
        }


        [Test]
        public void UserPreferenceRepositoryShouldReturnTopThreeDislikes()
        {
            const string hottest = "applesMostCommon";
            user1.Dislikes.Add(new Dislike {UserId = user1.LoginId, Text = hottest});
            const string secondHottest = "orangesSecondMostCommon";
            user1.Dislikes.Add(new Dislike {UserId = user1.LoginId, Text = secondHottest});
            const string thirdHottest = "plumsThirdMostCommon";
            user1.Dislikes.Add(new Dislike {UserId = user1.LoginId, Text = thirdHottest});
            user1.Dislikes.Add(new Dislike {UserId = user1.LoginId, Text = "pineapples"});
            user1.Dislikes.Add(new Dislike {UserId = user1.LoginId, Text = "bananas"});

            user2.Dislikes.Add(new Dislike {UserId = user2.LoginId, Text = hottest});
            user2.Dislikes.Add(new Dislike {UserId = user2.LoginId, Text = secondHottest});
            user2.Dislikes.Add(new Dislike {UserId = user2.LoginId, Text = thirdHottest});

            user3.Dislikes.Add(new Dislike {UserId = user3.LoginId, Text = hottest});
            user3.Dislikes.Add(new Dislike {UserId = user3.LoginId, Text = secondHottest});

            user4.Dislikes.Add(new Dislike {UserId = user4.LoginId, Text = hottest});

            registerUsers();

            IList dislikes = new UserPreferenceRepository(session).HottestDislikes();
            Assert.NotNull(dislikes);
            Assert.AreEqual(3, dislikes.Count);
            Assert.AreEqual(dislikes[0], hottest);
            Assert.AreEqual(dislikes[1], secondHottest);
            Assert.AreEqual(dislikes[2], thirdHottest);
        }


        [Test]
        public void WhenTieThenTopThreeCountsOfLikesAreReturned()
        {
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "rank1"});
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "anotherRank1"});
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "rank2"});
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "rank3"});
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "rank4"});

            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = "rank1"});
            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = "anotherRank1"});
            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = "rank2"});
            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = "rank3"});

            user3.Likes.Add(new Like {UserId = user3.LoginId, Text = "rank1"});
            user3.Likes.Add(new Like {UserId = user3.LoginId, Text = "anotherRank1"});
            user3.Likes.Add(new Like {UserId = user3.LoginId, Text = "rank2"});

            user4.Likes.Add(new Like {UserId = user4.LoginId, Text = "rank1"});
            user4.Likes.Add(new Like {UserId = user4.LoginId, Text = "anotherRank1"});

            registerUsers();

            IList likesList = new UserPreferenceRepository(session).HottestLikes();
            Assert.NotNull(likesList);
            Assert.AreEqual(4, likesList.Count);
            Assert.AreEqual(likesList[2], "rank2");
            Assert.AreEqual(likesList[3], "rank3");
        }

        [Test]
        public void WhenOnlyTwoLikesArePopularThenReturnJustTheTwo()
        {
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "rank1"});
            user1.Likes.Add(new Like {UserId = user1.LoginId, Text = "anotherRank1"});

            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = "rank1"});
            user2.Likes.Add(new Like {UserId = user2.LoginId, Text = "anotherRank1"});

            user3.Likes.Add(new Like {UserId = user3.LoginId, Text = "rank1"});
            user3.Likes.Add(new Like {UserId = user3.LoginId, Text = "anotherRank1"});

            user4.Likes.Add(new Like {UserId = user4.LoginId, Text = "rank1"});

            registerUsers();

            IList likesList = new UserPreferenceRepository(session).HottestLikes();
            Assert.NotNull(likesList);
            Assert.AreEqual(2, likesList.Count);
        }

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
}
