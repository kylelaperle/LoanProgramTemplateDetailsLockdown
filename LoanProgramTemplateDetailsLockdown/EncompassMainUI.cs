using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoanProgramTemplateDetailsLockdown
{
    public static class EncompassMainUI
    {
        public static event EncompassFormOpenedHandler FormOpened;
        private static Dictionary<Form, IntPtr> _OpenForms;
        private static System.Timers.Timer mainUITimer = null;
        private static string source = "LoanProgramTemplateDetailsLockdownPlugin.EncompassMainUI";

        public static Form MainUI
        {
            get
            {
                return Application.OpenForms[0];
            }
        }

        static EncompassMainUI()
        {
            try
            {
                try
                {
                    ApplicationLog.WriteDebug(source, "Setting up EncompassMainUI");
                    _OpenForms = new Dictionary<Form, IntPtr>();
                }
                catch (Exception ex)
                {
                    ApplicationLog.WriteError(source, ex.ToString());
                }


                try
                {
                    Application.OpenForms[0].Deactivate += EncompassMainUI_Deactivate;
                    ApplicationLog.WriteDebug(source, "Subscribed to Deactivate");
                }
                catch (Exception ex)
                {
                    ApplicationLog.WriteError(source, ex.ToString());
                }


                try
                {
                    mainUITimer = new System.Timers.Timer(300);
                    mainUITimer.Elapsed += OnTimer;
                    mainUITimer.AutoReset = false;
                    mainUITimer.SynchronizingObject = Application.OpenForms[0];
                    mainUITimer.Enabled = true;
                    ApplicationLog.WriteDebug(source, "Timer enabled");
                }
                catch (Exception ex)
                {
                    ApplicationLog.WriteError(source, ex.ToString());
                }

                try
                {
                    CheckAndAdd();
                    ApplicationLog.WriteDebug(source, "Finished setup");
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

        private static void EncompassMainUI_Deactivate(object sender, EventArgs e)
        {
            try
            {
                CheckAndAdd();
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private static void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                CheckAndAdd();
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }

            try
            {
                CheckDictionary();
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }

            try
            {
                if (mainUITimer != null)
                {
                    mainUITimer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private static void CheckDictionary()
        {
            try
            {
                if (_OpenForms != null)
                {
                    foreach (var s in _OpenForms.Where(p => p.Key == null || p.Key.IsDisposed).ToList())
                    {
                        try
                        {
                            _OpenForms.Remove(s.Key);
                        }
                        catch (Exception ex)
                        {
                            ApplicationLog.WriteError(source, ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private static void CheckAndAdd()
        {
            try
            {
                if (Application.OpenForms != null && _OpenForms != null)
                {
                    foreach (Form _form in Application.OpenForms)
                    {
                        if (_form != null && _form.IsDisposed == false)
                        {
                            if (!_OpenForms.Keys.Contains(_form))
                            {
                                AddNewForm(_form);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private static void AddNewForm(Form _form)
        {
            try
            {
                if (_form != null && _form.IsDisposed == false)
                {
                    _form.FormClosing += _form_FormClosing;
                    _OpenForms.Add(_form, _form.Handle);
                    FormOpenEventTrigger(_form);
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private static void _form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    Form _form = (Form)sender;
                    if (_form != null && _form.IsDisposed == false)
                    {
                        if (_OpenForms != null && _OpenForms.Keys.Contains(_form))
                        {
                            _OpenForms.Remove(_form);
                        }
                        _form.FormClosing -= _form_FormClosing;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }

        private static void FormOpenEventTrigger(Form _form)
        {
            try
            {
                if (_form != null && _form.IsDisposed == false)
                {
                    EncompassFormOpenedEventArgs eventArgs = new EncompassFormOpenedEventArgs(_form);
                    FormOpened?.Invoke(null, eventArgs);
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(source, ex.ToString());
            }
        }
    }
}
