using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using NHibernate;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class ProfileTest : NHibernateInMemoryTestFixtureBase
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
        public void ShouldBeAbleToCreateProfile()
        {
            Profile profile = new Profile("Some useful profile goes here");
        }

        [Test]
        public void ShouldBeAbleToCreateUserWithProfile()
        {
            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);
            Profile profile = new Profile("Some useful profile goes here");
            User user = new User(loginid, name) {Profile = profile};
            Assert.AreEqual(profile.ToString(), user.Profile.ToString());
        }

        [Test]
        public void ShouldBeAbleToSaveUserWithProfile()
        {
            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            Profile profile = new Profile("Some useful profile goes here");
            User user = new User(loginid, name) { Profile = profile };
            session.Save(user);
            IList<User> users=session.CreateQuery("from User").List<User>();
            Assert.AreEqual(profile, users.First().Profile);
        }

    }
}