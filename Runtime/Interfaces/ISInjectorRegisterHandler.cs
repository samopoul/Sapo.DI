namespace Sapo.DI.Runtime.Interfaces
{
    public interface ISInjectorRegisterHandler
    {
        public void OnRegister(ISInjector injector);
    }
    
    public interface ISInjectorInjectHandler
    {
        public void OnInject(ISInjector injector);
    }
}