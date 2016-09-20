using System;
namespace FaithEngage.IntegrationTests
{
    public static class TestHelpers
    {
        public static Exception TryGetException (Action action)
        {
            Exception except = null;
            try {
                action ();
            } catch (Exception ex) {
                except = ex;
            }
            return except;
        }
    }
}

