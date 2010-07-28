using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace GetIn
{
    class UserMessageTest
    {
        [Test]
        public void UserCanPostToSubscribedGroup()
        {
            Mock<Group> mockGroup = new Mock<Group>();
            Post post = new Post() {Content = "Some stupid content", PostedDateTime = new DateTime(2010, 01, 01)};
            mockGroup.Setup(foo => foo.post(post));

            LoginId loginid = new LoginId("test@ThoughtWorks.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            User footballFan = new User(loginid, name);
            //footballFan.

        }

        [Test]
        public void UserCannotPostToUnsubscribedGroup()
        {
//            Assert.AreEqual("expected", "actual");
        }

        [Test]
        public void GroupCanListAllMessagsPostedToIt()
        {
//            Assert.AreEqual("expected", "actual");
        }
    }
}
