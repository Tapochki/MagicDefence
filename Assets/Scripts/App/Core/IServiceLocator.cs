namespace GrandDevs.AppName
{
    public interface IServiceLocator
    {
        T GetService<T>();
        void Update();
    }
}