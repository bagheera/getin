using System.Collections.Generic;
using Iesi.Collections.Generic;
using NUnit.Framework;
using System;
using System.Drawing;
using NHibernate;
using System.IO;
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
            Assert.AreEqual(user.Id, loginid);
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



            var sessionMock = new Moq.Mock<ISession>();
            var queryMock = new Moq.Mock<IQuery>();

            sessionMock.Setup(p => p.CreateQuery("from User u where u.Id.Value = :param1")).Returns(queryMock.Object);
            queryMock.Setup(p => p.SetString("param1", user.Id.Value)).Returns(queryMock.Object);
            queryMock.Setup(p => p.List<User>()).Returns(new List<User>());
            sessionMock.Setup(p => p.Save(user));
            sessionMock.Setup(p => p.Flush());

            IUserRepository repository = new UserRepository(sessionMock.Object);

            user.Repository = repository;

            user.Register();
            sessionMock.VerifyAll();
            queryMock.VerifyAll();
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



            var sessionMock = new Moq.Mock<ISession>();
            var queryMock = new Moq.Mock<IQuery>();

            sessionMock.Setup(p => p.CreateQuery("from User u where u.Id.Value = :param1")).Returns(queryMock.Object);
            queryMock.Setup(p => p.SetString("param1", user.Id.Value)).Returns(queryMock.Object);
            queryMock.Setup(p => p.List<User>()).Returns(new List<User> { user });

            IUserRepository repository = new UserRepository(sessionMock.Object);
            user.Repository = repository;
            Assert.Throws(typeof(UserAlreadyExistsException), user.Register);
            sessionMock.VerifyAll();
            queryMock.VerifyAll();
        }

    }

    [TestFixture]
    public class UserRepositoryMappingTest : NHibernateInMemoryTestFixtureBase
    {
        private ISession session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("User.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            session = this.CreateSession();
        }

        [TearDown]
        public void TearDown()
        {
            session.Dispose();
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
            user.Register();
        }
    }
}