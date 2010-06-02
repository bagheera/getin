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
    public class CommentsTest
    {
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

            CommentContent content = new CommentContent("This is what I am going to comment");

            Comment comment = new Comment(user1, profile, content);
            Assert.AreEqual(comment.Commentor, user1);
            Assert.AreEqual(comment.Profile, profile);
            Assert.AreEqual(comment, profile.getLatestComment());
        }
    }
}
