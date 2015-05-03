using System.Threading.Tasks;

namespace OwinSample.Worker.Infrastructure.Mappers
{
    public interface IMapToNew<TSource, TTarget>
    {
        Task<TTarget> Map(TSource source);
    }
}