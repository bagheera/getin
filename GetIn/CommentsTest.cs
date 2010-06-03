using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class CommentsTest : NHibernateFixtureBase
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
            session.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            session.Transaction.Rollback();
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
            Name name1 = new Name("firstName1", "lastName1");
            User user1 = new User(loginid1, name1);
            IUserRepository repository = new UserRepository(session);
            repository.Save(user1);
            LoginId loginid2 = new LoginId("testprofile@test.com");
            Name name2 = new Name("firstName2", "lastName2");
            Profile profile = new Profile("This is the profile on which user1 will comment");
            User user2 = new User(loginid2, name2) { Profile = profile };

            new Comment(user1, user2, "This is what I am going to comment");
            repository.Save(user2);
            User interestedUser = null;
            IList<User> users = session.CreateQuery("from User").List<User>();
            foreach (User user in users){
                if (user.LoginId.Value == loginid2.Value){
                    interestedUser = user;
                    break;
                }
            }
            if (interestedUser != null){
                Assert.AreEqual("This is what I am going to comment", interestedUser.GetLatestProfileComment().Content);
                Assert.AreEqual(user1,interestedUser.GetLatestProfileComment().Commentor);
            }
            else{
                Assert.Fail();
            }
        }

        [Test]
        public void ShouldBeAbleToGetTheLatestComment(){
            LoginId loginid1 = new LoginId("testlatestcomments@test.com");
            Name name1 = new Name("firstName1", "lastName1");
            User user1 = new User(loginid1, name1);
            user1.AddCommentToProfile(new Comment(user1,user1,"This is the first comment",new GetInDate(new DateTime(2010,01,01))));
            user1.AddCommentToProfile(new Comment(user1,user1,"This is the second comment",new GetInDate(new DateTime(2010,02,03))));
            user1.AddCommentToProfile(new Comment(user1,user1,"This is the third comment",new GetInDate(new DateTime(2010,01,02))));
            Assert.AreEqual("This is the second comment",user1.GetLatestProfileComment().Content);
        }

        [Test]
        public void ShouldBeAbleToGetAllCommentsOnAProfile(){
            LoginId loginid1 = new LoginId("testlatestcomments@test.com");
            Name name1 = new Name("firstName1", "lastName1");
            User user1 = new User(loginid1, name1);
            user1.AddCommentToProfile(new Comment(user1, user1, "This is the first comment", new GetInDate(new DateTime(2010, 01, 01))));
            user1.AddCommentToProfile(new Comment(user1, user1, "This is the second comment", new GetInDate(new DateTime(2010, 02, 03))));
            user1.AddCommentToProfile(new Comment(user1, user1, "This is the third comment", new GetInDate(new DateTime(2010, 01, 02))));
            user1.AddCommentToProfile(new Comment(user1, user1, "This is the fourth comment", new GetInDate(new DateTime(2010, 04, 02))));
            Assert.AreEqual(4, user1.GetAllProfileComments().Count);
        }
    }
}
