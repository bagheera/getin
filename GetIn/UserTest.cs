using NUnit.Framework;
using System;
using System.Drawing;
namespace GetIn
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void ShouldBeAbleToCreateANewUserObject()
        {
            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname,lastname);
            
            User user = new User(loginid, name);
            Assert.AreEqual(user.Name, name);
            Assert.AreEqual(user.Id, loginid);
        }

        [Test]
        public void ShouldBeAbleToSetUserProperties(){
            LoginId loginid = new LoginId("test@test.com");
            string firstname = "firstName";
            string lastname = "lastName";
            Name name = new Name(firstname, lastname);
            string[] likes = new string[] {"Like1", "Like2", "Like3"};
            string[] dlikes = new string[] {"Dislike1", "Dislike2", "Dislike3"};
            Image image = new Bitmap(1, 1);

            User user = new User(loginid, name)
                            {
                                DateOfBirth = new GetInDate(DateTime.Today.AddYears(-25)),
                                Location = new Location {City = "Banglore"},
                                Gender = new Male(),
                                Likes = new Likes(likes),
                                Dislikes = new Dislikes(dlikes),
                                Picture= image ,
                                Profile = new Profile("Big Profile")
                            };
        }


    }

    
}
