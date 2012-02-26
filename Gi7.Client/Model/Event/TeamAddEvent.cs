
namespace Gi7.Client.Model.Event
{
    public class TeamAddEvent : Event
    {
        public Team Team { get; set; }

        public User User { get; set; }

        public Repository Repo { get; set; }
    }
}
