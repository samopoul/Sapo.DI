using Sapo.DI.Runtime.Attributes;

namespace Sapo.DI.Tests.Runtime.TestData
{
    internal interface IServiceA
    {
            
    }
    
    internal interface IServiceB
    {
            
    }
    
    internal class ServiceA : IServiceA
    {
            
    }
    
    internal class ServiceB : IServiceB
    {
            
    }
    
    internal class ServiceAWithDependencyToB : IServiceA
    {
        [SInject] public IServiceB ServiceB;
    }
    
    internal class ServiceBWithDependencyToA : IServiceB
    {
        [SInject] public IServiceA ServiceA;
    }
    
    internal class ServiceAWithDependencyToA : IServiceA
    {
        [SInject] public IServiceA ServiceA;
    }
    
}