using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileFinder
{
    class PathFinder
    {
        private string Path;
        private string FileName;
        private AppPathFinder App;

        public PathFinder(AppPathFinder _App, string _Path, string _FileName)
        {
            this.Path = _Path;
            this.FileName = _FileName;
            this.App = _App;
        }

        public void Find()
        {
            App.UpdateStatus(AppStatus.Done);
        }
    }
}
