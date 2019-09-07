using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyMediaRenamer.Gui
{
    public interface IWindowService
    {
        bool? Show(object dataContext);
        bool? ShowDialog(object dataContext);
    }
    public class WindowService<T> : IWindowService where T: Window 
    {
        public bool? Show(object dataContext)
        {
            throw new NotImplementedException();
        }

        public bool? ShowDialog(object dataContext)
        {
            throw new NotImplementedException();
        }
    }
}
