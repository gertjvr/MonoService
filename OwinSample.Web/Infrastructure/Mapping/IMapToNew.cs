using System.Threading.Tasks;

namespace OwinSample.Web.Infrastructure.Mapping
{
    public interface IMapToNew<TSource, TTarget>
    {
        Task<TTarget> Map(TSource source);
    }
}