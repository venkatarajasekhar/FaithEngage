﻿using System;
namespace FaithEngage.Plugins.Tests
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

