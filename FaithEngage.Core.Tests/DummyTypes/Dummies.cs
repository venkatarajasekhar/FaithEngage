using System;
using FaithEngage.Core.Interfaces;
using FakeItEasy;
using FaithEngage.Core.Tests;
using NUnit.Framework;

namespace FaithEngage.Core.Tests
{

    public interface IDummy
    {
        
    }

    public interface IDummy2
    {
        
    }

    public class Dummy2_NoParameters : IDummy2
    {
        
    }
        

    public class Dummy_NoParameters : IDummy
    {
        
    }

    public class Dummy_CtorThrowsException : IDummy
    {
        public Dummy_CtorThrowsException ()
        {
            throw new Exception("Dummy Exception");            
        }       
    }

    public class Dummy_OneParam : Dummy_NoParameters
    {
        public Dummy_OneParam (string parameter)
        {
            
        }
    }

    abstract public class AbstractDummy_NoParams : IDummy
    {

    }

    abstract public class AbstractDummy_OneParam : AbstractDummy_NoParams
    {
        public AbstractDummy_OneParam (string parameter)
        {
            
        }
    }

    public class Dummy_CtorHasDependencies : IDummy
    {
        public Dummy_CtorHasDependencies (IDummy2 dependency)
        {
            
        }
    }
}

