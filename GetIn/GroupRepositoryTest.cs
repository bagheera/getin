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
            InitalizeSessionFactory(new FileInfo("User.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            session = this.CreateSession();
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
        public void ShouldReturnFalseIfNoGroupsArePresentInRepository(){
            Assert.False(groupRepository.Exists(new Group("test")));
        }
    }
}
