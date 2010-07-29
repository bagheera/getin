using System.IO;
using NHibernate;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class GroupRepositoryTest : NHibernateFixtureBase
    {
        private ISession session;
        private IGroupRepository groupRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("Comment.hbm.xml"), new FileInfo("User.hbm.xml"), new FileInfo("Group.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            session = CreateSession();
            session.BeginTransaction();
            groupRepository = new GroupRepository(session);
        }

        [TearDown]
        public void TearDown()
        {
            session.Transaction.Rollback();
            session.Dispose();
        }

        [Test]
        public void ShouldReturnFalseIfGroupIsNotPresent(){
            Assert.False(groupRepository.Exists(new Group("test")));
        }

        [Test]
        public void ShouldBeAbleToCreateGroup(){
            var group = new Group("test");
            groupRepository.Create(group);
            Assert.True(group.Id > 0);
            Assert.True(groupRepository.Exists(group));
        }
    }
}
