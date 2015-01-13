using System.Windows.Input;

namespace Gi7.Views
{
    public partial class HomeView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void EnterPressed(object sender, KeyEventArgs e)
        {
            // Remove this if statement and the search box will search everytime a character is typed
            // However this is not appear to be asynchronous as you have to wait for the search results
            // to appear before it will let you type another character
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }
    }
}