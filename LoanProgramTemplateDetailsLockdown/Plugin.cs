using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using frm = System.Windows.Forms;

namespace LoanProgramTemplateDetailsLockdown
{
    [Plugin]
    public class Plugin
    {
        private frm.Form _LoanProgramSelectForm;
        private Loan _loan;
        private Persona _SuperAdmin;
        private static string source = "LoanProgramTemplateDetailsLockdownPlugin";
        
        public Plugin()
        {
            EncompassApplication.Login += EncompassApplication_Login;
            EncompassApplication.LoanOpened += EncompassApplication_LoanOpened;
            EncompassApplication.LoanClosing += EncompassApplication_LoanClosing;
            EncompassMainUI.FormOpened += EncompassMainUI_FormOpened;
        }

        private void EncompassApplication_Login(object sender, EventArgs e)
        {
            try
            {
                SetupRights();
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private void SetupRights()
        {
            _SuperAdmin = EncompassApplication.Session.Users.Personas.GetPersonaByName("Super Administrator");
        }

        private void EncompassApplication_LoanOpened(object sender, EventArgs e)
        {
            try
            {
                _loan = EncompassApplication.CurrentLoan;
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private void EncompassMainUI_FormOpened(object sender, EncompassFormOpenedEventArgs e)
        {
            try
            {
                if (e.OpenedForm != null && !e.OpenedForm.IsDisposed)
                {
                    if (e.OpenedForm.Name == "LoanProgramSelect")
                    {
                        LoanProgramSelectActivated(e.OpenedForm);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }
        private void LoanProgramSelectActivated(frm.Form form)
        {
            if (_LoanProgramSelectForm == null)
            {
                _LoanProgramSelectForm = form;
                _LoanProgramSelectForm.FormClosing += _LoanProgramSelectForm_FormClosing;

                try
                {
                    frm.Panel panelRight = (frm.Panel)_LoanProgramSelectForm.Controls.Find("panelRight", true)[0];
                    panelRight.Enabled = false;


                    frm.CheckBox chkAppendLP = (frm.CheckBox)_LoanProgramSelectForm.Controls.Find("chkAppendLP", true)[0];
                    chkAppendLP.Enabled = false;
                    chkAppendLP.Checked = true;

                    frm.CheckBox chkAppendCC = (frm.CheckBox)_LoanProgramSelectForm.Controls.Find("chkAppendCC", true)[0];
                    chkAppendCC.Enabled = false;

                    try
                    {
                        EllieMae.EMLite.UI.GroupContainer groupDetail = (EllieMae.EMLite.UI.GroupContainer)_LoanProgramSelectForm.Controls.Find("groupDetail", true)[0];
                        groupDetail.Text = "Detail - DISABLED!";
                        groupDetail.HeaderForeColor = System.Drawing.Color.Red;
                    }
                    catch (Exception ex)
                    {
                        ApplicationLog.WriteError(source, ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    ApplicationLog.WriteError(source, ex.ToString());
                }
            }

        }

        private void _LoanProgramSelectForm_FormClosing(object sender, frm.FormClosingEventArgs e)
        {
            try
            {
                if (_LoanProgramSelectForm != null)
                {
                    _LoanProgramSelectForm.FormClosing -= _LoanProgramSelectForm_FormClosing;
                }
                _LoanProgramSelectForm = null;

            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private void EncompassApplication_LoanClosing(object sender, EventArgs e)
        {
            _loan = null;
        }     
    }
}
