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
    public class CommentsTest : NHibernateInMemoryTestFixtureBase
    {
        private ISession session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("Comment.hbm.xml"), new FileInfo("User.hbm.xml"));
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
        public void UserShouldBeAbleToCommentOnAProfile(){
            LoginId loginid1 = new LoginId("testcomments@test.com");
            string firstname1 = "firstName1";
            string lastname1 = "lastName1";
            Name name1 = new Name(firstname1, lastname1);
            User user1 = new User(loginid1, name1);

            LoginId loginid2 = new LoginId("testprofile@test.com");
            string firstname2 = "firstName2";
            string lastname2 = "lastName2";
            Name name2 = new Name(firstname2, lastname2);
            Profile profile = new Profile("This is the profile on which user1 will comment");
            User user2 = new User(loginid2, name2) { Profile = profile };

            String content = "This is what I am going to comment";

            Comment comment = new Comment(user1, user2, content);
            Assert.AreEqual(comment.Commentor, user1);
            Assert.AreEqual(comment.CommentedOn, user2);
            Assert.AreEqual(comment, user2.GetLatestProfileComment());
        }

        [Test]
        public void UserShouldBeAbleToSaveComments(){
            LoginId loginid1 = new LoginId("testcomments@test.com");
            string firstname1 = "firstName1";
            string lastname1 = "lastName1";
            Name name1 = new Name(firstname1, lastname1);
            User user1 = new User(loginid1, name1);
            session.Save(user1);
            LoginId loginid2 = new LoginId("testprofile@test.com");
            string firstname2 = "firstName2";
            string lastname2 = "lastName2";
            Name name2 = new Name(firstname2, lastname2);
            Profile profile = new Profile("This is the profile on which user1 will comment");
            User user2 = new User(loginid2, name2) { Profile = profile };

            String content = "This is what I am going to comment";

            Comment comment = new Comment(user1, user2, content);
            session.Save(user2);
            session.Flush();
            IList<User> users = session.CreateQuery("from User U where U.Id = :id").SetParameter("id", loginid2).List<User>();
            Assert.AreEqual(content, users.First().GetLatestProfileComment());
        }
    }
}
