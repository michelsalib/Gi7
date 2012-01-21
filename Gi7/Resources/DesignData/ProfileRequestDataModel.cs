using Gi7.Model;

namespace Gi7.Resources.DesignData
{
    public class ProfileRequestDataModel
    {
        public ProfileRequestDataModel()
        {
            User = new User
            {
                Name = "Alberto Monteiro",
                Login = "AlbertoMonteiro",
                Location = "Brazil",
                AvatarUrl = @"https://pt.gravatar.com/userimage/11089328/847bb84cca2c25b56dacf1bafc6107ae.jpg?size=200",
                Blog = @"http://blog.albertomonteiro.net"
            };
        }

        public User User { get; set; }
    }
}