﻿using System;
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
    public class ProfileTest : NHibernateInMemoryTestFixtureBase
    {
        private ISession session;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitalizeSessionFactory(new FileInfo("User.hbm.xml"));
        }

        [SetUp]
        public void SetUp()
        {
            session = this.CreateSession();
        }

        [TearDown]
        public void TearDown()
        {
            session.Dispose();
        }
        [Test]
        public void ShouldBeAbleToCreateProfile(){
            Profile profile = new Profile("Some useful profile goes here");
        }

        [Test]
        public void ShouldBeAbleToCreateUserWithProfile(){
            Profile profile = new Profile("Some useful profile goes here");
            User user = new User {Profile = profile};
        }

        [Test]
        public void ShouldBeAbleToSaveUserWithProfile()
        {
            Profile profile = new Profile("Some useful profile goes here");
            User user = new User { Profile = profile };
            session.Save(user);
            IList<User> users=session.CreateQuery("from User").List<User>();
            Assert.AreEqual(profile, users.First().Profile);
        }

    }

   


}
