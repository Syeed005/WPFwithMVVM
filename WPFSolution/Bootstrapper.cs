using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ValitE3DProjectCreator.ViewModels;

namespace ValitE3DProjectCreator {
    public class Bootstrapper: BootstrapperBase {
        public Bootstrapper() {
            Initialize();
        }
        protected override void OnStartup(object sender, StartupEventArgs e) {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
