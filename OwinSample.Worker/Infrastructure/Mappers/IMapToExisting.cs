namespace OwinSample.Worker.Infrastructure.Mappers
{
    public interface IMapToExisting<TSource, TTarget>
    {
        void Map(TSource source, TTarget target);
    }
}