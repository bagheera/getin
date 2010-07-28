using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class GroupTest
    {
        [Test]
        public void CanCreateNewGroupWithIdAndName(){
            new Group {Id = 10, Name = "nature-lovers", Description = "Those who love nature can join the group"};
        }
    }
}
