
namespace Gi7.Service.Request.Base
{
    public abstract class GithubSingleRequest<T> : IGithubSingleRequest<T>
        where T : new ()
    {
        public string Uri
        {
            get;
            protected set;
        }

        public OverrideSettings OverrideSettings
        {
            get;
            protected set;
        }
    }
}
