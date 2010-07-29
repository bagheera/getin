using System;
using System.Collections.Generic;
using System.IO;
using NHibernate;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class CommentsTest : NHibernateFixtureBase
    {
        private ISession _session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("Comment.hbm.xml"), new FileInfo("User.hbm.xml"), new FileInfo("Group.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            _session = CreateSession();
            _session.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            _session.Transaction.Rollback();
            _session.Dispose();
        }
    
        [Test]
        public void UserShouldBeAbleToCommentOnAProfile(){
            var user1 = new User(new LoginId("testcomments@test.com"), new Name("firstName1", "lastName1"));
            var user2 = new User(new LoginId("testprofile@test.com"), new Name("firstName2", "lastName2")) { Profile = new Profile("This is the profile on which user1 will comment") };
            var comment = new Comment(user1, user2, "This is what I am going to comment");
            Assert.AreEqual(comment.Commentor, user1);
            Assert.AreEqual(comment.CommentedOn, user2);
            Assert.AreEqual(comment, user2.GetLatestProfileComment());
        }

        [Test]
        public void UserShouldBeAbleToSaveComments(){
            IUserRepository repository = new UserRepository(_session);
            repository.Save(new User(new LoginId("testcomments@test.com"), new Name("firstName1", "lastName1")));
            var user2 = new User(new LoginId("testprofile@test.com"), new Name("firstName2", "lastName2")) { Profile = new Profile("This is the profile on which user1 will comment") };
            new Comment(new User(new LoginId("testcomments@test.com"), new Name("firstName1", "lastName1")), user2, "This is what I am going to comment");
            repository.Save(user2);
            User interestedUser = null;
            IList<User> users = _session.CreateQuery("from User").List<User>();
            foreach (User user in users){
                if (user.LoginId.Value == new LoginId("testprofile@test.com").Value){
                    interestedUser = user;
                    break;
                }
            }
            if (interestedUser != null){
                Assert.AreEqual("This is what I am going to comment", interestedUser.GetLatestProfileComment().Content);
                Assert.AreEqual(new User(new LoginId("testcomments@test.com"), new Name("firstName1", "lastName1")),interestedUser.GetLatestProfileComment().Commentor);
            }
            else{
                Assert.Fail();
            }
        }

        [Test]
        public void ShouldBeAbleToGetTheLatestComment(){
            var user1 = new User(new LoginId("testlatestcomments@test.com"), new Name("firstName1", "lastName1"));
            user1.AddCommentToProfile(new Comment(user1,user1,"This is the first comment",new GetInDate(new DateTime(2010,01,01))));
            user1.AddCommentToProfile(new Comment(user1,user1,"This is the second comment",new GetInDate(new DateTime(2010,02,03))));
            user1.AddCommentToProfile(new Comment(user1,user1,"This is the third comment",new GetInDate(new DateTime(2010,01,02))));
            Assert.AreEqual("This is the second comment",user1.GetLatestProfileComment().Content);
        }

        [Test]
        public void ShouldBeAbleToGetAllCommentsOnAProfile(){
            var user1 = new User(new LoginId("testlatestcomments@test.com"), new Name("firstName1", "lastName1"));
            user1.AddCommentToProfile(new Comment(user1, user1, "This is the first comment", new GetInDate(new DateTime(2010, 01, 01))));
            user1.AddCommentToProfile(new Comment(user1, user1, "This is the second comment", new GetInDate(new DateTime(2010, 02, 03))));
            user1.AddCommentToProfile(new Comment(user1, user1, "This is the third comment", new GetInDate(new DateTime(2010, 01, 02))));
            user1.AddCommentToProfile(new Comment(user1, user1, "This is the fourth comment", new GetInDate(new DateTime(2010, 04, 02))));
            Assert.AreEqual(4, user1.GetAllProfileComments().Count);
        }
    }
}
