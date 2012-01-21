using Gi7.Model;

namespace Gi7.Resources.DesignData
{
    public class AboutDataModel
    {
		public User Michelsalib { get; set; }
		public User AlbertoMonteiro { get; set; }
		
		public AboutDataModel()
		{
			Michelsalib = new User();
			Michelsalib.Login = "Michelsalib";
			Michelsalib.Name = "Michel Salib";
			Michelsalib.AvatarUrl = @"https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317";
			
			AlbertoMonteiro = new User();
			AlbertoMonteiro.Login = "AlbertoMonteiro";
			AlbertoMonteiro.Name = "Alberto Monteiro";
			AlbertoMonteiro.AvatarUrl = @"http://pt.gravatar.com/userimage/11089328/847bb84cca2c25b56dacf1bafc6107ae.jpeg";
		}
    }
}