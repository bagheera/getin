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
    public class UserRepositoryTest : NHibernateInMemoryTestFixtureBase {

        private ISession session;
        private IUserRepository usrRep;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("User.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            session = this.CreateSession();
            usrRep = new UserRepository(session);
            LookUsersSetUp();
        }

        private void LookUsersSetUp(){
            User user1 = new User(new LoginId("123"), new Name("Mark", "Twain"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                Location = new Location { City = "Banglore" },
                Gender = new Gender(),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Big Profile"),
            };
            usrRep.Save(user1);
            User user2 = new User(new LoginId("345"), new Name("Sudhakar", "Rayavaram"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-28)),
                Location = new Location { City = "Banglore" },
                Gender = new Gender(),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Short and sweet profile"),
            };
            usrRep.Save(user2);
            User user3 = new User(new LoginId("678"), new Name("Alex", "Anto"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-26)),
                Location = new Location { City = "Banglore" },
                Gender = new Gender(),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Big Profile"),
            };
            usrRep.Save(user3);            
        }

        [Test]
        public void LookupUsersBasedOnName(){
            User usr1 = new User(null,new Name("Mark", null));
            IList<User> results1 = usrRep.LookupUsers(usr1);
            Assert.AreEqual("123", results1[0].LoginId.Value);

            User usr2 = new User(null,new Name(null, "anto"));
            IList<User> results2 = usrRep.LookupUsers(usr2);
            Assert.AreEqual("678", results2[0].LoginId.Value);
        }

        /*[Test]
        public void LookUpUserBasedOnGender(){
            User usr1 = new User(null, new Name(null, null));
            usr1.Gender = new Gender();
        }*/
        
        /*[Test]
        public void LookupUsersForBlankCriteriaShouldReturnZeroResults(){
            IUserRepository usrRep = new UserRepository(CreateSession());
            IList<User> selectedUsers = usrRep.LookupUsers(null);
            Assert.AreEqual(0,selectedUsers.Count);
        }*/
    }
}
