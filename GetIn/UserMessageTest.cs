using System;
using Moq;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class UserMessageTest
    {
        [Test]
        public void UserCanPostToSubscribedGroup()
        {
            Mock<Group> mockGroup = new Mock<Group>();
            Post post = new Post() {Content = "Some stupid content", PostedDateTime = new DateTime(2010, 01, 01)};

            Group group = mockGroup.Object;
            mockGroup.Setup(foo => foo.post(post));

            LoginId loginid = new LoginId("test@ThoughtWorks.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            User footballFan = new User(loginid, name);


            footballFan.Groups.Add(group);

            try
            {
                footballFan.PostToGroup(group, post);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed");
            }
        }

        [Test]
        public void UserCannotPostToUnsubscribedGroup()
        {
            Mock<Group> mockGroup = new Mock<Group>();
            Post post = new Post() {Content = "Some stupid content", PostedDateTime = new DateTime(2010, 01, 01)};

            Group group = mockGroup.Object;
            mockGroup.Setup(foo => foo.post(post));

            LoginId loginid = new LoginId("test@ThoughtWorks.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            User footballFan = new User(loginid, name);


            try
            {
                footballFan.PostToGroup(group, post);
                Assert.Fail("Failed");
            }
            catch (Exception ex)
            {
            }
        }

        [Test]
        public void GroupCanListAllMessagsPostedToIt()
        {
//            Assert.AreEqual("expected", "actual");
        }
    }
}