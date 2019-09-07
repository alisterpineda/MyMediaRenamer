using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyMediaRenamer.Gui.Utilities.WindowService
{
    public interface IWindowService
    {
        void Show(object dataContext = null);
    }
}
