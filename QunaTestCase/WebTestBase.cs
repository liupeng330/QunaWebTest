using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebTest.TestUtilities;
using Quna.WebTest.TestFramework;

namespace WebTest.TestCases
{
    public class QunaWebTestBase : TestBase
    {
        protected WebTestHelper WebTestHelper;

        public override void OnTestInitialize()
        {
            base.OnTestInitialize();
            WebTestHelper = Get<WebTestHelper>();
        }
    }
}
