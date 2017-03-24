using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoanProgramTemplateDetailsLockdown
{
    public class EncompassFormOpenedEventArgs : EventArgs
    {
        private Form _Form;

        public Form OpenedForm
        {
            get { return _Form; }
            private set { _Form = value; }
        }

        public EncompassFormOpenedEventArgs(Form frm)
        {
            _Form = frm;
        }
        public EncompassFormOpenedEventArgs()
        {

        }
    }
}
