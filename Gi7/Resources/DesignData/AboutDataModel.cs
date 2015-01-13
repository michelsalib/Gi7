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

            Zeromax = new User
            {
                Login = "Zeromax",
                Name = "Andreas Nölke",
                AvatarUrl = "https://avatars2.githubusercontent.com/u/1867177"
            };

            NPadrutt = new User
            {
                Login = "NPadrutt",
                Name = "Nino Padrutt",
                AvatarUrl = "https://avatars3.githubusercontent.com/u/1764367"
            };

            Jonnybest = new User
            {
                Login = "jonnybest",
                Name = "jonnybest",
                AvatarUrl = "https://avatars2.githubusercontent.com/u/187852"
            };

            RandomlyKnighted = new User
            {
                Login = "RandomlyKnighted",
                Name = "Tyler Hughes",
                AvatarUrl = "https://avatars0.githubusercontent.com/u/2483249"
            };
        }

        public User Michelsalib { get; set; }
        public User AlbertoMonteiro { get; set; }
        public User Zeromax { get; set; }
        public User NPadrutt { get; set; }
        public User Jonnybest { get; set; }
        public User RandomlyKnighted { get; set; }
    }
}