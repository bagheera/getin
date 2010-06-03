using System.Collections.Generic;
using Iesi.Collections.Generic;
using Moq;
using NUnit.Framework;
using System;
using System.Drawing;
using NHibernate;
using System.IO;
using System.Linq;
namespace GetIn
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void ShouldBeAbleToCreateANewUserObject()
        {
            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            User user = new User(loginid, name);
            Assert.AreEqual(user.Name, name);
            Assert.AreEqual(user.LoginId, loginid);
        }

        [Test]
        public void ShouldBeAbleToSetUserProperties()
        {
            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            Like[] likes = new Like[]
                               {
                                   new Like() {UserId = loginid, Text = "Like1"},
                                   new Like() {UserId = loginid, Text = "Like2"},
                                   new Like() {UserId = loginid, Text = "Like3"},
                               };

            Dislike[] dlikes = new Dislike[]
                               {
                                   new Dislike() {UserId = loginid, Text = "Dislike1"},
                                   new Dislike() {UserId = loginid, Text = "Dislike2"},
                                   new Dislike() {UserId = loginid, Text = "Dislike3"},
                               };

            Image image = new Bitmap(1, 1);

            User user = new User(loginid, name)
                            {
                                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                                Location = new Location { City = "Banglore" },
                                Gender = new Gender(),
                                Likes = new HashedSet<Like>(likes),
                                Dislikes = new HashedSet<Dislike>(dlikes),
                                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                                Profile = new Profile("Big Profile")
                            };
        }

        [Test]
        public void ShouldBeAbleToRegisterUser()
        {

            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            Image image = new Bitmap(1, 1);

            Like[] likes = new Like[]
                               {
                                   new Like() {UserId = loginid, Text = "Like1"},
                                   new Like() {UserId = loginid, Text = "Like2"},
                                   new Like() {UserId = loginid, Text = "Like3"},
                               };

            Dislike[] dlikes = new Dislike[]
                               {
                                   new Dislike() {UserId = loginid, Text = "Dislike1"},
                                   new Dislike() {UserId = loginid, Text = "Dislike2"},
                                   new Dislike() {UserId = loginid, Text = "Dislike3"},
                               };


            User user = new User(loginid, name)
                            {
                                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                                Location = new Location { City = "Banglore" },
                                Gender = new Gender(),
                                Likes = new HashedSet<Like>(likes),
                                Dislikes = new HashedSet<Dislike>(dlikes),
                                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                                Profile = new Profile("Big Profile")
                            };



            var repositoryMock = new Moq.Mock<IUserRepository>();
            repositoryMock.Setup(p => p.LookupUsers(It.IsAny<User>())).Returns(new List<User>());
            repositoryMock.Setup(p => p.Save(user));

            user.Repository = repositoryMock.Object;

            user.Register();
            repositoryMock.VerifyAll();
        }

        [Test]
        public void ShouldNotBeAbleToRegisterUserIfAlreadyExists()
        {

            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            Like[] likes = new Like[]
                               {
                                   new Like() {UserId = loginid, Text = "Like1"},
                                   new Like() {UserId = loginid, Text = "Like2"},
                                   new Like() {UserId = loginid, Text = "Like3"},
                               };

            Dislike[] dlikes = new Dislike[]
                               {
                                   new Dislike() {UserId = loginid, Text = "Dislike1"},
                                   new Dislike() {UserId = loginid, Text = "Dislike2"},
                                   new Dislike() {UserId = loginid, Text = "Dislike3"},
                               };

            Image image = new Bitmap(1, 1);

            User user = new User(loginid, name)
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                Location = new Location { City = "Banglore" },
                Gender = new Gender(),
                Likes = new HashedSet<Like>(likes),
                Dislikes = new HashedSet<Dislike>(dlikes),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Big Profile")
            };

            var repositoryMock = new Moq.Mock<IUserRepository>();
            repositoryMock.Setup(p => p.LookupUsers(It.IsAny<User>())).Returns(new List<User> { user });
            user.Repository = repositoryMock.Object;
            Assert.Throws(typeof(UserAlreadyExistsException), user.Register);
            repositoryMock.VerifyAll();
        }

    }

    [TestFixture]
    public class UserRepositoryMappingTest : NHibernateInMemoryTestFixtureBase
    {
        private ISession session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("User.hbm.xml"), new FileInfo("Comment.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            session = this.CreateSession();
            session.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            session.Transaction.Rollback();
            session.Dispose();
        }

        [Test]
        public void ShouldBeAbleToInviteFriends()
        {
            var loginid = new LoginId("test@test.com");
            var name = new Name("firstName", "lastName");
            var user = new User(loginid, name);

            var loginid2 = new LoginId("test2@test.com");
            var name2 = new Name("firstName2", "lastName2");
            var user2 = new User(loginid2, name2);

            session.Save(user2);
            user.InviteFriend(user2);
            session.Save(user);
            session.Flush();

            IList<User> users = session.CreateQuery("from User u where u.LoginId.Value='test2@test.com'").List<User>();
            Assert.AreEqual(1, users[0].Inviters.Count);
            Assert.AreEqual(user, users[0].Inviters.ElementAt(0));

            var loginid3 = new LoginId("test3@test.com");
            var name3 = new Name("firstName3", "lastName3");
            var user3 = new User(loginid3, name3);

            session.Save(user3);
            user.InviteFriend(user3);
            session.Save(user);
            session.Flush();

            users = session.CreateQuery("from User u where u.LoginId.Value='test3@test.com'").List<User>();
            Assert.AreEqual(1, users[0].Inviters.Count);
            Assert.AreEqual(user, users[0].Inviters.ElementAt(0));
        }

        [Test]
        public void ShouldBeAbleToAcceptFriends()
        {
            var loginid = new LoginId("Martin@ThoughtWorks.com");
            var name = new Name("Martin", "Fowler");
            var martin = new User(loginid, name);

            var loginid2 = new LoginId("Roy@ThoughtWorks.com");
            var name2 = new Name("Roy", "Singham");
            var roy = new User(loginid2, name2);

            session.Save(roy);
            martin.InviteFriend(roy);
            session.Save(martin);
            session.Flush();

            IList<User> users = session.CreateQuery("from User u where u.LoginId.Value='Roy@ThoughtWorks.com'").List<User>();
            Assert.AreEqual(1, users[0].Inviters.Count);
            Assert.AreEqual(martin, users[0].Inviters.ElementAt(0));

            roy.AcceptFriendInvite(martin);

            users = session.CreateQuery("from User u where u.LoginId.Value='Roy@ThoughtWorks.com'").List<User>();
            Assert.AreEqual(0, users[0].Inviters.Count);
            Assert.IsTrue(users[0].Friends.Contains(martin));
        }

        [Test]
        public void ShouldBeAbleToRejectFriends()
        {
            var loginid = new LoginId("Martin@ThoughtWorks.com");
            var name = new Name("Martin", "Fowler");
            var martin = new User(loginid, name);

            var loginid2 = new LoginId("Roy@ThoughtWorks.com");
            var name2 = new Name("Roy", "Singham");
            var roy = new User(loginid2, name2);

            session.Save(roy);
            martin.InviteFriend(roy);
            session.Save(martin);
            session.Flush();

            IList<User> users = session.CreateQuery("from User u where u.LoginId.Value='Roy@ThoughtWorks.com'").List<User>();
            Assert.AreEqual(1, users[0].Inviters.Count);
            Assert.AreEqual(martin, users[0].Inviters.ElementAt(0));

            roy.RejectFriendInvite(martin);

            users = session.CreateQuery("from User u where u.LoginId.Value='Roy@ThoughtWorks.com'").List<User>();
            Assert.AreEqual(0, users[0].Inviters.Count);
            Assert.IsTrue(!users[0].Friends.Contains(martin));
        }

        [Test]
        public void ShouldBeAbleToRegisterUser()
        {

            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            Like[] likes = new Like[]
                               {
                                   new Like() {UserId = loginid, Text = "Like1"},
                                   new Like() {UserId = loginid, Text = "Like2"},
                                   new Like() {UserId = loginid, Text = "Like3"},
                               };

            Dislike[] dlikes = new Dislike[]
                                   {
                                       new Dislike() {UserId = loginid, Text = "Dislike1"},
                                       new Dislike() {UserId = loginid, Text = "Dislike2"},
                                       new Dislike() {UserId = loginid, Text = "Dislike3"},
                                   };


            Image image = new Bitmap(1, 1);

            IUserRepository repository = new UserRepository(session);
            User user = new User(loginid, name)
                            {
                                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                                Location = new Location { City = "Banglore" },
                                Gender = new Gender(),
                                Likes = new HashedSet<Like>(likes),
                                Dislikes = new HashedSet<Dislike>(dlikes),
                                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                                Profile = new Profile("Big Profile"),
                                Repository = repository
                            };
            repository.Save(user);
            session.Evict(user);

            User savedUser = repository.LookupUsers(user)[0];


            Assert.AreEqual(savedUser.LoginId, user.LoginId);
            Assert.AreEqual(savedUser.Location.City, user.Location.City);
            Assert.AreEqual(savedUser.Location.Country, user.Location.Country);
            Assert.AreEqual(savedUser.DateOfBirth.Value, user.DateOfBirth.Value);
            Assert.AreEqual(savedUser.Picture.Bytes.Length, user.Picture.Bytes.Length);
            for (int i = 0; i < savedUser.Picture.Bytes.Length; i++)
                Assert.AreEqual(savedUser.Picture.Bytes[i], user.Picture.Bytes[i]);

            Assert.AreEqual(savedUser.Likes.Count, user.Likes.Count);
            var arraylikes1 = savedUser.Likes.ToArray();
            var arraylikes2 = user.Likes.ToArray();
            for (int i = 0; i < arraylikes1.Length; i++)
                Assert.AreEqual(arraylikes1[i].Text, arraylikes2[i].Text);

            Assert.AreEqual(savedUser.Dislikes.Count, user.Dislikes.Count);
            var arraydislikes1 = savedUser.Dislikes.ToArray();
            var arraydislikes2 = user.Dislikes.ToArray();
            for (int i = 0; i < arraydislikes1.Length; i++)
                Assert.AreEqual(arraydislikes1[i].Text, arraydislikes2[i].Text);

            Assert.AreEqual(savedUser.Name.FirstName, user.Name.FirstName);
            Assert.AreEqual(savedUser.Name.LastName, user.Name.LastName);

            Assert.AreEqual(savedUser.Gender.Code, user.Gender.Code);
        }
    }
}