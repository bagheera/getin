using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class UserRepositoryTest : NHibernateFixtureBase {

        private ISession session;
        private IUserRepository usrRep;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("User.hbm.xml"), new FileInfo("Comment.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            session = this.CreateSession();
            session.BeginTransaction();
            usrRep = new UserRepository(session);
            LookUsersSetUp();
        }

        [TearDown]
        public void TearDown(){
            session.Transaction.Rollback();
            session.Dispose();
        }

        private void LookUsersSetUp(){
            User user1 = new User(new LoginId("123"), new Name("Mark", "Twain"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-19)),
                Location = new Location { City = "Banglore", Country = "India"},
                Gender = new Gender('F'),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Big Profile"),
                Likes = new HashedSet<Like>(new[]
                               {
                                   new Like() {UserId = new LoginId("123"), Text = "Like1"},
                                   new Like() {UserId = new LoginId("123"), Text = "Like2"},
                                   new Like() {UserId = new LoginId("123"), Text = "Like3"},
                               }),
                Dislikes = new HashedSet<Dislike>(new[]
                               {
                                   new Dislike() {UserId = new LoginId("123"), Text = "Dislike1"},
                                   new Dislike() {UserId = new LoginId("123"), Text = "Dislike2"},
                                   new Dislike() {UserId = new LoginId("123"), Text = "Dislike3"},
                               }),
            };
            usrRep.Save(user1);
            User user2 = new User(new LoginId("345"), new Name("Sudhakar", "Rayavaram"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-28)),
                Location = new Location { City = "Seattle", Country = "USA"},
                Gender = new Gender(),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Short and sweet crap"),
                Likes = new HashedSet<Like>(new[]
                               {
                                   new Like() {UserId = new LoginId("345"), Text = "Like3"},
                                   new Like() {UserId = new LoginId("345"), Text = "Like4"},
                                   new Like() {UserId = new LoginId("345"), Text = "Like5"},
                               }),
                Dislikes = new HashedSet<Dislike>(new[]
                               {
                                   new Dislike() {UserId = new LoginId("345"), Text = "Dislike3"},
                                   new Dislike() {UserId = new LoginId("345"), Text = "Dislike4"},
                                   new Dislike() {UserId = new LoginId("345"), Text = "Dislike5"},
                               }),
            };
            usrRep.Save(user2);
            User user3 = new User(new LoginId("678"), new Name("Alex", "Anto"))
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                Location = new Location { City = "Banglore", Country = "India"},
                Gender = new Gender(),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Some profile information which is useless!!! \n Some more crap!"),
                Likes = new HashedSet<Like>(new[]
                               {
                                   new Like() {UserId = new LoginId("678"), Text = "Like1"},
                                   new Like() {UserId = new LoginId("678"), Text = "Like4"},
                                   new Like() {UserId = new LoginId("678"), Text = "Like2"},
                               }),
                Dislikes = new HashedSet<Dislike>(new[]
                               {
                                   new Dislike() {UserId = new LoginId("678"), Text = "Dislike1"},
                                   new Dislike() {UserId = new LoginId("678"), Text = "Dislike4"},
                                   new Dislike() {UserId = new LoginId("678"), Text = "Dislike3"},
                               }),
            };
            usrRep.Save(user3);
            user1.Friends.Add(user2);
            user1.Friends.Add(user3);
            user2.Friends.Add(user3);
//            user3.Friends.Add(user1);
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

        [Test]
        public void LookupUserBasedOnLocation(){
            User usr1 = new User(null, new Name(null, null));
            usr1.Location = new Location {City = "Banglore"};
            IList<User> results1 = usrRep.LookupUsers(usr1);
            Assert.AreEqual(2, results1.Count());

            User usr2 = new User(null, new Name(null, null));
            usr2.Location = new Location {Country = "USA"};
            IList<User> results2 = usrRep.LookupUsers(usr2);
            Assert.AreEqual(1, results2.Count());

            User usr3 = new User(null, new Name(null, null));
            usr3.Location = new Location {Country = "Canada"};
            IList<User> results3 = usrRep.LookupUsers(usr3);
            Assert.AreEqual(0, results3.Count());
        }

        [Test]
        public void LookUpBasedOnId(){
            User usr1 = new User(new LoginId("123"), new Name(null, null));
            IList<User> results2 = usrRep.LookupUsers(usr1);
            Assert.AreEqual(1, results2.Count());
        }

        [Test]
        public void LookupBasedOnGender(){
            User usr1 = new User(null, new Name(null, null));
            usr1.Gender = new Gender('F');
            IList<User> results = usrRep.LookupUsers(usr1);
            Assert.AreEqual(1,results.Count);
        }

        [Test]
        public void LookupBasedOnAge(){
            User usr = new User(null, new Name(null,null));
            usr.DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25));
            IList<User> results = usrRep.LookupUsers(usr, new AgeRange{From = 20, To = 29});
            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public void LookupBasedOnProfile(){
            User usr = new User(null, new Name(null, null));
            usr.Profile = new Profile("crap");
            IList<User> results = usrRep.LookupUsers(usr);
            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public void LookupBasedOnAgeAndName(){
            User usr = new User(null, new Name(null, null));
            usr.DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25));
            usr.Name = new Name{FirstName = "Alex"};
            IList<User> results = usrRep.LookupUsers(usr);
            Assert.AreEqual(1, results.Count);
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
