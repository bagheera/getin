using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                session = CreateSession();
        }

        [TearDown]
        public void TearDown()
        {
            session.Dispose();
        }
    
        [Test]
        public void ShouldBeAbleToCreateUserWithProfile()
        {
            var loginid = new LoginId("test@test.com");
            var name = new Name("firstName", "lastName");
            var profile = new Profile("Some useful profile goes here");
            new User(loginid, name) {Profile = profile};
        }

        [Test]
        public void ShouldRestrictUserProfileTextToMaxLength()
        {
            var loginid = new LoginId("test@test.com");
            var name = new Name("firstName", "lastName");

            System.String tooBigText = getBigProfileText("../../../GetIn/bigProfile.txt");
            var profile = new Profile(tooBigText);

            var user = new User(loginid, name) { Profile = profile };
            
            session.Save(user);
            
            IList<User> users=session.CreateQuery("from User").List<User>();
            Assert.AreEqual(102400, users.First().Profile.ToString().Length);
        }

        [Test]
        public void ShouldStoreUserProfileTextOfMaxLength()
        {
            var loginid = new LoginId("test@test.com");
            var name = new Name("firstName", "lastName");

            System.String tooBigText = getBigProfileText("../../../GetIn/MaxProfile.txt");
            var profile = new Profile(tooBigText);
            
            var user = new User(loginid, name) { Profile = profile };
            
            session.Save(user);
            
            IList<User> users = session.CreateQuery("from User").List<User>();
            Assert.AreEqual(profile, users.First().Profile);
        }

        private string	getBigProfileText(string fileName){
            var sr = new StreamReader(fileName);
            System.String bigText=sr.ReadToEnd();
            sr.Close();
            return bigText;
        }
    }
}