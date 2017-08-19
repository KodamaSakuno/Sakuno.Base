namespace Sakuno.Collections
{
    public interface IProjector<TSource, TDestination>
    {
        TDestination Project(TSource source);
    }
}
