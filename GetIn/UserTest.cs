using System.Collections.Generic;
using Iesi.Collections.Generic;
using Moq;
using NUnit.Framework;
using System;
using System.Drawing;
using NHibernate;
using System.IO;
using System.Linq;
namespace GetIn
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void ShouldBeAbleToCreateANewUserObject()
        {
            LoginId loginid = new LoginId("test@ThoughtWorks.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            User user = new User(loginid, name);
            Assert.AreEqual(user.Name, name);
            Assert.AreEqual(user.LoginId, loginid);
        }

        [Test]
        public void ShouldBeAbleToSetUserProperties()
        {
            LoginId loginid = new LoginId("test@ThoughtWorks.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            Like[] likes = new Like[]
                               {
                                   new Like() {UserId = loginid, Text = "Like1"},
                                   new Like() {UserId = loginid, Text = "Like2"},
                                   new Like() {UserId = loginid, Text = "Like3"},
                               };

            Dislike[] dlikes = new Dislike[]
                               {
                                   new Dislike() {UserId = loginid, Text = "Dislike1"},
                                   new Dislike() {UserId = loginid, Text = "Dislike2"},
                                   new Dislike() {UserId = loginid, Text = "Dislike3"},
                               };

            Image image = new Bitmap(1, 1);

            User user = new User(loginid, name)
                            {
                                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                                Location = new Location { City = "Banglore" },
                                Gender = new Gender(),
                                Likes = new HashedSet<Like>(likes),
                                Dislikes = new HashedSet<Dislike>(dlikes),
                                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                                Profile = new Profile("Big Profile")
                            };
        }

        [Test]
        public void ShouldBeAbleToRegisterUser()
        {

            LoginId loginid = new LoginId("test@ThoughtWorks.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            Image image = new Bitmap(1, 1);

            Like[] likes = new Like[]
                               {
                                   new Like() {UserId = loginid, Text = "Like1"},
                                   new Like() {UserId = loginid, Text = "Like2"},
                                   new Like() {UserId = loginid, Text = "Like3"},
                               };

            Dislike[] dlikes = new Dislike[]
                               {
                                   new Dislike() {UserId = loginid, Text = "Dislike1"},
                                   new Dislike() {UserId = loginid, Text = "Dislike2"},
                                   new Dislike() {UserId = loginid, Text = "Dislike3"},
                               };


            User user = new User(loginid, name)
                            {
                                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                                Location = new Location { City = "Banglore" },
                                Gender = new Gender(),
                                Likes = new HashedSet<Like>(likes),
                                Dislikes = new HashedSet<Dislike>(dlikes),
                                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                                Profile = new Profile("Big Profile")
                            };



            var repositoryMock = new Moq.Mock<IUserRepository>();
            repositoryMock.Setup(p => p.LookupUsers(It.IsAny<User>())).Returns(new List<User>());
            repositoryMock.Setup(p => p.Save(user));

            user.Repository = repositoryMock.Object;

            user.Register();
            repositoryMock.VerifyAll();
        }

        [Test]
        public void ShouldNotBeAbleToRegisterUserIfAlreadyExists()
        {

            LoginId loginid = new LoginId("test@ThoughtWorks.com");
            Name name = new Name("firstName", "lastName");

            var likes = new[]
                               {
                                   new Like() {UserId = loginid, Text = "Like1"},
                                   new Like() {UserId = loginid, Text = "Like2"},
                                   new Like() {UserId = loginid, Text = "Like3"},
                               };

            var dislikes = new[]
                               {
                                   new Dislike() {UserId = loginid, Text = "Dislike1"},
                                   new Dislike() {UserId = loginid, Text = "Dislike2"},
                                   new Dislike() {UserId = loginid, Text = "Dislike3"},
                               };


            User user = new User(loginid, name)
            {
                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                Location = new Location { City = "Banglore" },
                Gender = new Gender(),
                Likes = new HashedSet<Like>(likes),
                Dislikes = new HashedSet<Dislike>(dislikes),
                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                Profile = new Profile("Big Profile")
            };

            var repositoryMock = new Moq.Mock<IUserRepository>();
            repositoryMock.Setup(p => p.LookupUsers(It.IsAny<User>())).Returns(new List<User> { user });
            user.Repository = repositoryMock.Object;
            Assert.Throws(typeof(UserAlreadyExistsException), user.Register);
            repositoryMock.VerifyAll();
        }

        [Test]
        public void ShouldBeAbleToCallUserRepositoryOnLookupUsers()
        {
            User user1 = new User(new LoginId("123"), null);
            var repositoryMock = new Moq.Mock<IUserRepository>();
            repositoryMock.Setup(p => p.LookupUsers(It.IsAny<User>())).Returns(new List<User> { user1 });
            user1.Repository = repositoryMock.Object;
            IList<User> lookedupUser = user1.LookupUsers();
            Assert.AreEqual(1, lookedupUser.Count());
            repositoryMock.VerifyAll();
        }

        [Test]
        public void ShouldBeAbleToCallUserRepositoryonLookUpUsersWithAgeRestriction()
        {
            User user1 = new User(new LoginId("123"), null);
            var repositoryMock = new Moq.Mock<IUserRepository>();
            repositoryMock.Setup(p => p.LookupUsers(It.IsAny<User>(), It.IsAny<AgeRange>())).Returns(new List<User> { user1 });
            user1.Repository = repositoryMock.Object;
            IList<User> lookedupUser = user1.LookupUsers(new AgeRange());
            Assert.AreEqual(1, lookedupUser.Count());
            repositoryMock.VerifyAll();
        }

        [Test]
        public void ShouldBeAbleToInviteFriends()
        {
            var loginid = new LoginId("Manish@ThoughtWorks.com");
            var name = new Name("firstName", "lastName");
            var manish = new User(loginid, name);

            var loginid2 = new LoginId("Umar@ThoughtWorks.com");
            var name2 = new Name("firstName2", "lastName2");
            var umar = new User(loginid2, name2);

            Assert.AreEqual(0, umar.Inviters.Count);

            manish.InviteFriend(umar);

            Assert.AreEqual(1, umar.Inviters.Count);
            Assert.AreEqual(manish, umar.Inviters.ElementAt(0));
        }
        
        [Test]
        public void ShouldBeAbleToAcceptInvitation()
        {
            var loginid = new LoginId("Manish@ThoughtWorks.com");
            var name = new Name("Manish", "Manish");
            var manish = new User(loginid, name);

            var loginid2 = new LoginId("Umar@ThoughtWorks.com");
            var name2 = new Name("Umar", "Umar");
            var umar = new User(loginid2, name2);

            manish.InviteFriend(umar);

            Assert.AreEqual(1, umar.Inviters.Count);

            umar.AcceptFriendInvite(manish);

            Assert.IsTrue(!umar.Inviters.Contains(manish));
            Assert.IsTrue(umar.Friends.Contains(manish));
        }

        [Test]
        public void ShouldBeAbleToRejectInvitation()
        {
            var loginid = new LoginId("Manish@ThoughtWorks.com");
            var name = new Name("Manish", "Manish");
            var manish = new User(loginid, name);

            var loginid2 = new LoginId("Umar@ThoughtWorks.com");
            var name2 = new Name("Umar", "Umar");
            var umar = new User(loginid2, name2);

            manish.InviteFriend(umar);

            Assert.AreEqual(1, umar.Inviters.Count);

            umar.RejectFriendInvite(manish);

            Assert.AreEqual(0, umar.Inviters.Count);
            Assert.IsTrue(!umar.Friends.Contains(manish));
        }

        [Test]
        public void TwoLikessAreSameIfTextInLikesAreEqualAndIsNotDependantOnUserId(){
            Like like1 = new Like(){Text="Like1"};
            Like like2 = new Like(){Text="Like1"};

            Assert.True(like1.Equals(like2));
        }
        
        [Test]
        public void TwoDisLikesAreSameIfTextInDislikesAreEqualAndIsNotDependantOnUserId(){
            Dislike like1 = new Dislike(){Text="Dislike1"};
            Dislike like2 = new Dislike(){Text="Dislike1"};

            Assert.True(like1.Equals(like2));
        }

        [Test]
        public void UserShouldBeAbleToComputeSimilarityScoreWithAnotherUser()
        {
            LoginId loginId1 = new LoginId("test@ThoughtWorks.com");
            string firstname1 = "firstName";
            string lastname1 = "lastName";
            Name name1 = new Name(firstname1, lastname1);

            Like[] likes1 = new Like[]
                               {
                                   new Like() {UserId = loginId1, Text = "Like1"},
                                   new Like() {UserId = loginId1, Text = "Like2"},
                                   new Like() {UserId = loginId1, Text = "Like3"},
                               };

            Dislike[] dlikes1 = new Dislike[]
                                   {
                                       new Dislike() {UserId = loginId1, Text = "Dislike1"},
                                       new Dislike() {UserId = loginId1, Text = "Dislike2"},
                                       new Dislike() {UserId = loginId1, Text = "Dislike3"},
                                   };

            User currentUser = new User(loginId1, name1)
            {
                Likes = new HashedSet<Like>(likes1),
                Dislikes = new HashedSet<Dislike>(dlikes1),
            };


            LoginId loginId2 = new LoginId("anotherTest@thoughtworks.com");
            Like[] likes2 = new Like[]
                               {
                                   new Like() {UserId = loginId2, Text = "Like1"},
                                   new Like() {UserId = loginId2, Text = "Like2"},
                                   new Like() {UserId = loginId2, Text = "Like4"},
                               };
            Dislike[] dlikes2 = new Dislike[]
                                   {
                                       new Dislike() {UserId = loginId2, Text = "Dislike1"},
                                       new Dislike() {UserId = loginId2, Text = "Dislike4"},
                                       new Dislike() {UserId = loginId2, Text = "Dislike5"},
                                   };

            User anotherUser = new User(loginId2, new Name("anotherFirstName", "anotherLastName"))
            {
                Likes = new HashedSet<Like>(likes2),
                Dislikes = new HashedSet<Dislike>(dlikes2),
            };

            Assert.AreEqual(2.7d, currentUser.ComputeSimilarityScore(anotherUser));
        }
    }

    [TestFixture]
    public class UserRepositoryMappingTest : NHibernateFixtureBase
    {
        private ISession session;

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
        }

        [TearDown]
        public void TearDown()
        {
            session.Transaction.Rollback();
            session.Dispose();
        }

        [Test]
        public void ShouldBeAbleToPersistInvitersOnInvite()
        {
            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("Martin@ThoughtWorks.com");
            var name = new Name("Martin", "Fowler");
            var martin = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("Roy@ThoughtWorks.com");
            var name2 = new Name("Roy", "Singham");
            var roy = new User(loginid2, name2) { Repository = repository };

            repository.Save(roy);
            repository.Save(martin);

            martin.InviteFriend(roy);
            
            session.Flush();
            session.Evict(martin);
            session.Evict(roy);

            IList<User> users = repository.LookupUsers(roy);
            Assert.IsTrue(users[0].Inviters.Contains(martin));
        }

        [Test]
        public void ShouldBeAbleToPersistInvitersAndFriendsOnAcceptInvite()
        {
            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("Martin@ThoughtWorks.com");
            var name = new Name("Martin", "Fowler");
            var martin = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("Roy@ThoughtWorks.com");
            var name2 = new Name("Roy", "Singham");
            var roy = new User(loginid2, name2) { Repository = repository };

            repository.Save(roy);
            repository.Save(martin);

            martin.InviteFriend(roy);

            roy.AcceptFriendInvite(martin);

            session.Flush();
            session.Evict(martin);
            session.Evict(roy);

            IList<User> users = repository.LookupUsers(roy);
            Assert.IsTrue(!users[0].Inviters.Contains(martin));
            Assert.IsTrue(users[0].Friends.Contains(martin));
        }

        [Test]
        public void ShouldBeAbleToPersistInvitersAndFriendsOnRejectInvite()
        {
            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("Martin@ThoughtWorks.com");
            var name = new Name("Martin", "Fowler");
            var martin = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("Roy@ThoughtWorks.com");
            var name2 = new Name("Roy", "Singham");
            var roy = new User(loginid2, name2) { Repository = repository };

            repository.Save(roy);
            repository.Save(martin);

            martin.InviteFriend(roy);

            roy.RejectFriendInvite(martin);

            session.Flush();
            session.Evict(martin);
            session.Evict(roy);

            IList<User> users = repository.LookupUsers(roy);
            Assert.IsTrue(!users[0].Inviters.Contains(martin));
            Assert.IsTrue(!users[0].Friends.Contains(martin));
        }

        [Test]
        public void ShouldBeAbleToRegisterUser()
        {

            LoginId loginid = new LoginId("test@ThoughtWorks.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);

            Like[] likes = new Like[]
                               {
                                   new Like() {UserId = loginid, Text = "Like1"},
                                   new Like() {UserId = loginid, Text = "Like2"},
                                   new Like() {UserId = loginid, Text = "Like3"},
                               };

            Dislike[] dlikes = new Dislike[]
                                   {
                                       new Dislike() {UserId = loginid, Text = "Dislike1"},
                                       new Dislike() {UserId = loginid, Text = "Dislike2"},
                                       new Dislike() {UserId = loginid, Text = "Dislike3"},
                                   };


            Image image = new Bitmap(1, 1);

            IUserRepository repository = new UserRepository(session);
            User user = new User(loginid, name)
                            {
                                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                                Location = new Location { City = "Banglore" },
                                Gender = new Gender(),
                                Likes = new HashedSet<Like>(likes),
                                Dislikes = new HashedSet<Dislike>(dlikes),
                                Picture = new Photo { Bytes = new byte[] { 1, 2, 3, 4, 5 } },
                                Profile = new Profile("Big Profile"),
                                Repository = repository
                            };
            repository.Save(user);

            session.Flush();
            session.Evict(user);

            User savedUser = repository.LookupUsers(user)[0];


            Assert.AreEqual(savedUser.LoginId, user.LoginId);
            Assert.AreEqual(savedUser.Location.City, user.Location.City);
            Assert.AreEqual(savedUser.Location.Country, user.Location.Country);
            Assert.AreEqual(savedUser.DateOfBirth.Value, user.DateOfBirth.Value);
            Assert.AreEqual(savedUser.Picture.Bytes.Length, user.Picture.Bytes.Length);
            for (int i = 0; i < savedUser.Picture.Bytes.Length; i++)
                Assert.AreEqual(savedUser.Picture.Bytes[i], user.Picture.Bytes[i]);

            Assert.AreEqual(savedUser.Likes.Count, user.Likes.Count);
            var arraylikes1 = savedUser.Likes.ToArray();
            var arraylikes2 = user.Likes.ToArray();
            for (int i = 0; i < arraylikes1.Length; i++)
                Assert.AreEqual(arraylikes1[i].Text, arraylikes2[i].Text);

            Assert.AreEqual(savedUser.Dislikes.Count, user.Dislikes.Count);
            var arraydislikes1 = savedUser.Dislikes.ToArray();
            var arraydislikes2 = user.Dislikes.ToArray();
            for (int i = 0; i < arraydislikes1.Length; i++)
                Assert.AreEqual(arraydislikes1[i].Text, arraydislikes2[i].Text);

            Assert.AreEqual(savedUser.Name.FirstName, user.Name.FirstName);
            Assert.AreEqual(savedUser.Name.LastName, user.Name.LastName);

            Assert.AreEqual(savedUser.Gender.Code, user.Gender.Code);
        }




        [Test]
        public void onInvitationAcceptedUserShouldBePresentinFriendsList()
        {
            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("suchitP@ThoughtWorks.com");
            var name = new Name("Suchit", "Puri");
            var suchit = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("sumitg@ThoughtWorks.com");
            var name2 = new Name("Sumit", "Gupta");
            var sumit = new User(loginid2, name2) { Repository = repository };

            repository.Save(sumit);
            repository.Save(suchit);

            suchit.InviteFriend(sumit);
            sumit.AcceptFriendInvite(suchit);
            session.Flush();
            session.Evict(suchit);
            session.Evict(sumit);

            //IList<User> users = repository.LookupUsers(sumit);
            Assert.True(suchit.isFriend(sumit));
        }

        [Test]
        public void DegreeOfSeparationShouldBeOneWhenDirectFriends()
        {
            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("suchitP@ThoughtWorks.com");
            var name = new Name("Suchit", "Puri");
            var suchit = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("sumitg@ThoughtWorks.com");
            var name2 = new Name("Sumit", "Gupta");
            var sumit = new User(loginid2, name2) { Repository = repository };

            repository.Save(sumit);
            repository.Save(suchit);

            suchit.InviteFriend(sumit);
            sumit.AcceptFriendInvite(suchit);
            session.Flush();
            session.Evict(suchit);
            session.Evict(sumit);
            Assert.AreEqual(1, sumit.DegreeOfSeparation(suchit).Count);
        }
        //TODO try not to hit the database
        [Test]
        public void DegreeOfSeparationShouldBeTwoWhenUserIsAFriendOfFriend()
        {
            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("suchitP@ThoughtWorks.com");
            var name = new Name("Suchit", "Puri");
            var suchit = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("sumitg@ThoughtWorks.com");
            var name2 = new Name("Sumit", "Gupta");
            var sumit = new User(loginid2, name2) { Repository = repository };

            var loginid3 = new LoginId("manav@ThoughtWorks.com");
            var name3 = new Name("Manav", "Prasad");
            var manav = new User(loginid3, name3) { Repository = repository };

            repository.Save(sumit);
            repository.Save(suchit);
            repository.Save(manav);

            suchit.InviteFriend(sumit);
            sumit.AcceptFriendInvite(suchit);
            sumit.InviteFriend(manav);
            manav.AcceptFriendInvite(sumit);

            session.Flush();
            session.Evict(suchit);
            session.Evict(sumit);
            session.Evict(manav);
            Assert.AreEqual(2, suchit.DegreeOfSeparation(manav).Count);
        }


        [Test]
        public void DegreeOfSeparationShouldBeMinimumOfTwoPathsForTheSameFriend()
        {
            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("suchitP@ThoughtWorks.com");
            var name = new Name("Suchit", "Puri");
            var suchit = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("sumitg@ThoughtWorks.com");
            var name2 = new Name("Sumit", "Gupta");
            var sumit = new User(loginid2, name2) { Repository = repository };

            var loginid3 = new LoginId("manav@ThoughtWorks.com");
            var name3 = new Name("Manav", "Prasad");
            var manav = new User(loginid3, name3) { Repository = repository };


            repository.Save(sumit);
            repository.Save(suchit);
            repository.Save(manav);

            suchit.InviteFriend(sumit);
            sumit.AcceptFriendInvite(suchit);
            sumit.InviteFriend(manav);
            manav.AcceptFriendInvite(sumit);

            suchit.InviteFriend(manav);
            manav.AcceptFriendInvite(suchit);

            session.Flush();
            session.Evict(suchit);
            session.Evict(sumit);
            session.Evict(manav);
            Assert.AreEqual(1, suchit.DegreeOfSeparation(manav).Count);
        }


        [Test]
        public void DegreeOfSeparationShouldBeThreeWhenThreeIndirectFrindsBetweenInputUsers()
        {
            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("suchitP@ThoughtWorks.com");
            var name = new Name("Suchit", "Puri");
            var suchit = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("sumitg@ThoughtWorks.com");
            var name2 = new Name("Sumit", "Gupta");
            var sumit = new User(loginid2, name2) { Repository = repository };

            var loginid3 = new LoginId("manav@ThoughtWorks.com");
            var name3 = new Name("Manav", "Prasad");
            var manav = new User(loginid3, name3) { Repository = repository };


            var loginid4 = new LoginId("Krishna@ThoughtWorks.com");
            var name4 = new Name("Krishna", "Prasad");
            var krishna = new User(loginid4, name4) { Repository = repository };


            repository.Save(sumit);
            repository.Save(suchit);
            repository.Save(manav);
            repository.Save(krishna);

            suchit.InviteFriend(sumit);
            sumit.AcceptFriendInvite(suchit);
            sumit.InviteFriend(manav);
            manav.AcceptFriendInvite(sumit);


            manav.InviteFriend(krishna);
            krishna.AcceptFriendInvite(manav);

            session.Flush();
            session.Evict(suchit);
            session.Evict(sumit);
            session.Evict(manav);
            session.Evict(krishna);
            Assert.AreEqual(3, suchit.DegreeOfSeparation(krishna).Count);
        }


        [Test]
        public void UserShouldNotRecommendFriends(){
            User user1 = new User(new LoginId("123"), null);
            var repositoryMock = new Moq.Mock<IUserRepository>();
            User user2 = new User(new LoginId("234"), null);
            User user3 = new User(new LoginId("234"), null);
            repositoryMock.Setup(p => p.NotFriendsOf(user1)).Returns(new List<User> { user2, user3 });
            user1.Repository = repositoryMock.Object;
            Assert.AreEqual(0, user1.RecommendFriends().Count);
            repositoryMock.VerifyAll();
        }

        [Test]
        public void UserShouldRecommendOneFriend(){
            var user1 = new User(new LoginId("123"), new Name("Mark", "Twain"))
            {
                Likes = new HashedSet<Like>(new[]
                               {
                                   new Like() {UserId = new LoginId("123"), Text = "Like1"},
                                   new Like() {UserId = new LoginId("123"), Text = "Like2"},
                                   new Like() {UserId = new LoginId("123"), Text = "Like3"},
                                   new Like() {UserId = new LoginId("123"), Text = "Like4"},
                                   new Like() {UserId = new LoginId("123"), Text = "Like5"},
                               }),
                Dislikes = new HashedSet<Dislike>(new[]
                               {
                                   new Dislike() {UserId = new LoginId("123"), Text = "Dislike1"},
                                   new Dislike() {UserId = new LoginId("123"), Text = "Dislike2"},
                                   new Dislike() {UserId = new LoginId("123"), Text = "Dislike3"},
                                   new Dislike() {UserId = new LoginId("123"), Text = "Dislike4"},
                                   new Dislike() {UserId = new LoginId("123"), Text = "Dislike5"},
                               }),
            };

            var user2 = new User(new LoginId("345"), new Name("Sudhakar", "Rayavaram"))
            {
                Likes = new HashedSet<Like>(new[]
                               {
                                   new Like() {UserId = new LoginId("345"), Text = "Like6"},
                                   new Like() {UserId = new LoginId("345"), Text = "Like7"},
                                   new Like() {UserId = new LoginId("345"), Text = "Like8"},
                               }),
                Dislikes = new HashedSet<Dislike>(new[]
                               {
                                   new Dislike() {UserId = new LoginId("345"), Text = "Dislike6"},
                                   new Dislike() {UserId = new LoginId("345"), Text = "Dislike7"},
                                   new Dislike() {UserId = new LoginId("345"), Text = "Dislike8"},
                               }),
            };
            var user3 = new User(new LoginId("678"), new Name("Alex", "Anto"))
            {
                Likes = new HashedSet<Like>(new[]
                               {
                                   new Like() {UserId = new LoginId("678"), Text = "Like1"},
                                   new Like() {UserId = new LoginId("678"), Text = "Like2"},
                                   new Like() {UserId = new LoginId("678"), Text = "Like3"},
                                   new Like() {UserId = new LoginId("678"), Text = "Like4"},
                                   new Like() {UserId = new LoginId("678"), Text = "Like5"},
                               }),
                Dislikes = new HashedSet<Dislike>(new[]
                               {
                                   new Dislike() {UserId = new LoginId("678"), Text = "Dislike6"},
                                   new Dislike() {UserId = new LoginId("678"), Text = "Dislike7"},
                                   new Dislike() {UserId = new LoginId("678"), Text = "Dislike8"},
                               }),
            };
            var repositoryMock = new Moq.Mock<IUserRepository>();
            repositoryMock.Setup(p => p.NotFriendsOf(user1)).Returns(new List<User> { user2, user3 });
            user1.Repository = repositoryMock.Object;
            Assert.AreEqual(1, user1.RecommendFriends().Count);
            Assert.AreEqual(user3,user1.RecommendFriends().First());
            repositoryMock.VerifyAll();
        }
        [Test]
        public void DegreeOfSeparationShouldReturnZeroSizeListWhenDegreeOfSeparationGreaterThanThree()
        {

            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("suchitP@ThoughtWorks.com");
            var name = new Name("Suchit", "Puri");
            var suchit = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("sumitg@ThoughtWorks.com");
            var name2 = new Name("Sumit", "Gupta");
            var sumit = new User(loginid2, name2) { Repository = repository };

            var loginid3 = new LoginId("manav@ThoughtWorks.com");
            var name3 = new Name("Manav", "Prasad");
            var manav = new User(loginid3, name3) { Repository = repository };


            var loginid4 = new LoginId("Krishna@ThoughtWorks.com");
            var name4 = new Name("Krishna", "Prasad");
            var krishna = new User(loginid4, name4) { Repository = repository };

            var loginid5 = new LoginId("srinivas@ThoughtWorks.com");
            var name5 = new Name("Srinivas", "");
            var srinivas = new User(loginid5, name5) { Repository = repository };


            repository.Save(sumit);
            repository.Save(suchit);
            repository.Save(manav);
            repository.Save(krishna);
            repository.Save(srinivas);

            suchit.InviteFriend(sumit);
            sumit.AcceptFriendInvite(suchit);
            sumit.InviteFriend(manav);
            manav.AcceptFriendInvite(sumit);


            manav.InviteFriend(krishna);
            krishna.AcceptFriendInvite(manav);

            krishna.InviteFriend(srinivas);
            srinivas.AcceptFriendInvite(krishna);
            
            session.Flush();
            session.Evict(suchit);
            session.Evict(sumit);
            session.Evict(manav);
            session.Evict(krishna);
            session.Evict(srinivas);
            Assert.AreEqual(0, suchit.DegreeOfSeparation(srinivas).Count);


        }

        [Test]
        public void DegreeOfSeparationShouldReturnZeroSizeListWhenNotFriends()
        {

            IUserRepository repository = new UserRepository(session);

            var loginid = new LoginId("suchitP@ThoughtWorks.com");
            var name = new Name("Suchit", "Puri");
            var suchit = new User(loginid, name) { Repository = repository };

            var loginid2 = new LoginId("sumitg@ThoughtWorks.com");
            var name2 = new Name("Sumit", "Gupta");
            var sumit = new User(loginid2, name2) { Repository = repository };

            var loginid3 = new LoginId("manav@ThoughtWorks.com");
            var name3 = new Name("Manav", "Prasad");
            var manav = new User(loginid3, name3) { Repository = repository };


            repository.Save(sumit);
            repository.Save(suchit);
            repository.Save(manav);
          
            suchit.InviteFriend(sumit);
            sumit.AcceptFriendInvite(suchit);
          
            session.Flush();
            session.Evict(suchit);
            session.Evict(sumit);
            session.Evict(manav);
            Assert.AreEqual(0, suchit.DegreeOfSeparation(manav).Count);


        }
        
    }
}