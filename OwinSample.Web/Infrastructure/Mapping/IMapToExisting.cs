namespace OwinSample.Web.Infrastructure.Mapping
{
    public interface IMapToExisting<TSource, TTarget>
    {
        void Map(TSource source, TTarget target);
    }
}