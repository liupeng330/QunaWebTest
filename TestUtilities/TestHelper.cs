namespace WebTest.TestUtilities
{
    public class TestHelper
    {
        public TestBase Test { get; private set; }

        public virtual void OnHelperCreation(TestBase test)
        {
            this.Test = test;
        }

        public virtual void OnTestCleanup() { }
    }
}
