using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Gi7.Client.Model;

namespace Gi7.Utils.ViewModels
{
    public class CommitFile : ViewModelBase
    {
        private ObservableCollection<CommitLine> _lines;
        public ObservableCollection<CommitLine> Lines
        {
            get { return _lines; }
            set { _lines = value; }
        }

        private File _file;
        public File File
        {
            get { return _file; }
            set { _file = value; }
        }
        
    }
}
