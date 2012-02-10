namespace Gi7.Client.Request.Base
{
    public interface ISingleRequest<TSource, TDestination> : IGenericRequest<TSource, TDestination>
        where TSource : class, new()
        where TDestination : class, new()
    {
        TDestination Result { get; set; }

        void SetResult(TSource result);
    }
}