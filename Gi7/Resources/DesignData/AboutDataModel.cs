using Gi7.Client.Model;

namespace Gi7.Resources.DesignData
{
    public class AboutDataModel
    {
        public AboutDataModel()
        {
            Michelsalib = new User
            {
                Login = "Michelsalib",
                Name = "Michel Salib",
                AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317"
            };

            AlbertoMonteiro = new User
            {
                Login = "AlbertoMonteiro",
                Name = "Alberto Monteiro",
                AvatarUrl = "http://pt.gravatar.com/userimage/11089328/847bb84cca2c25b56dacf1bafc6107ae"
            };
        }

        public User Michelsalib { get; set; }
        public User AlbertoMonteiro { get; set; }
    }
}