using NUnit.Framework;
using IntroSE.Kanban.Backend.BusinessLayer;
namespace TestKanban
{
    public class Tests
    {
        private UserController UC;
        private User value;
        private string expectedReg;
        private User user;
        
        [SetUp]
        public void Setup()
        {
            expectedReg ="exp@gmail.com";
            UC = new UserController();
            user = UC.Register("user@gmail.com", "Aa123123");
        }
        //Register Tests
        /// <summary>
        /// tests that register creates a user properly
        /// </summary>
        [Test]
        public void TestGoodRegister()
        {
            value=UC.Register("exp@gmail.com", "Aa123123");
            Assert.AreEqual(expectedReg, value.Email);
        }
        /// <summary>
        /// tests that register with null mail throws exception
        /// </summary>
        [Test]
        public void TestNULLRegister()
        {
            Assert.Throws<System.Exception>(() => UC.Register(null,"Aa123123"));
        }
        /// <summary>
        /// tests that register with mail that already registered throws exception
        /// </summary>
        [Test]
        public void TestExistRegister()
        {
            Assert.Throws<System.Exception>(() => UC.Register("user@gmail.com", "Aa123123"));
        }

        //Login Tests
        /// <summary>
        /// tests that login logs the correct user
        /// </summary>
        [Test]
        public void TestGoodLogin()
        {
            value = UC.Login("user@gmail.com", "Aa123123");
            Assert.AreEqual(UC.ActiveUser, value);
        }
        /// <summary>
        /// tests that login with wrong pass throws exception
        /// </summary>
        [Test]
        public void TestWrongPassLogin()
        {
            Assert.Throws<System.Exception>(() => UC.Login(user.Email, "Worng123"));
        }
        /// <summary>
        /// tests that login after user already logged in
        /// </summary>
        [Test]
        public void TestAlreadyLogged()
        {
            UC.Login("user@gmail.com", "Aa123123");
            Assert.Throws<System.Exception>(() => UC.Login("user@gmail.com", "Aa123123"));
        }
        /// <summary>
        /// tests that login with mail that doesn't exist in the system throws exception
        /// </summary>
        [Test]
        public void TestWrongMailLogin()
        {
            Assert.Throws<System.Exception>(() => UC.Login("Wrong", "Aa123123"));
        }
        
        //Logout Tests
        /// <summary>
        /// tests that logout when logged in works properly
        /// </summary>
        [Test]
        public void TestGoodLogout()
        {
            UC.Login("user@gmail.com", "Aa123123");
            UC.Logout();
            Assert.AreEqual(UC.ActiveUser, null);
        }
        /// <summary>
        /// tests that logout when not logged in throws exception
        /// </summary>
        [Test]
        public void TestNotLoggedLogout()
        {
            Assert.Throws<System.Exception>(() => UC.Logout());
        }
        
    }
}