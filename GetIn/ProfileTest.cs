using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GetIn
{
    [TestFixture]
    public class ProfileTest
    {
        [Test]
        public void ShouldBeAbleToCreateProfile(){
            Profile profile = new Profile("Some useful profile goes here");
            Assert.AreEqual("Some useful profile goes here", profile.ToString());
        }

        [Test]
        public void ShouldBeAbleToCreateUserWithProfile(){
            Profile profile = new Profile("Some useful profile goes here");
            User user = new User {Profile = profile};
            Assert.AreEqual(profile.ToString(),user.Profile.ToString());
        }
    }

   


}
