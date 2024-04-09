using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Common;
using DAL;
using LSExtensionControlLib;
using LSSERVICEPROVIDERLib;
using One1.Controls;

//using Utils;
//using DAL;

namespace ResultAttachments
{

    [ComVisible(true)]
    [ProgId("ResultAttachments.ResultAttachmentscls")]
    public partial class ResultAttachmentscls : UserControl, IExtensionControl
    {
        private INautilusDBConnection _ntlsCon;
        private IExtensionControlSite _site;
        private double _resultId;
        private IDataLayer _dal;
        private string _path;

        #region Ctor

        public ResultAttachmentscls()
        {
            InitializeComponent();
            this.BackColor = Color.FromName("Control");
        }
        #endregion

        #region Page Extension Interface

        public void SetReadOnly(bool readOnly) { }

        public void Internationalise() { }

        public void SetSite(object site)
        {
            if (site != null)
            {
                _site = site as IExtensionControlSite;
     

            }
        }

        public void SetupData()
        {
            bool flag;
            if (_site != null)
            {
                // Set the page name
                _site.SetPageName("מסמכים נלווים");

                // Get the record id, in this example" Product
                _site.GetDoubleValue("RESULT_ID", out _resultId, out flag);

                var show = ShowPage();
                _site.ShowPage(show);
                if (!show)
                {
                    if (_dal != null) _dal.Close();
                }
            }
        }

        public void SaveData()
        { }

        public void EnterPage() { }

        public void ExitPage() { }

        public void PreDisplay()
        {
         
            Utils.CreateConstring(_ntlsCon);
        }

        public void SetServiceProvider(object serviceProvider)
        {
            var sp = serviceProvider as NautilusServiceProvider;
            _ntlsCon = Utils.GetNtlsCon(sp);
        }

        public void SaveSettings(int hKey)
        { }

        public void RestoreSettings(int hKey)
        { }

        #endregion

        private void btnOpenDoc_click(object sender, EventArgs e)
        {
            try
            {
               // webBrowser1.Navigate(new Uri(_path));                                 
                Process p = Process.Start(_path);
            }
            catch (Exception ex)
            {
                label1.Text = "Error";
                Logger.WriteLogFile(ex);
            }
        }

        private bool ShowPage()
        {
            bool show;
            _dal = new DataLayer();
            _dal.Connect();
            var result = _dal.GetResultById((long)_resultId);
            if (result.ResultType == "F" && result.FormattedResult != null)
            {
                _path = result.FormattedResult;
                label1.Text = _path;
                show = true;
            }
            else
            {
                show = false;
            }
            _dal.Close();
            return show;
        }
    }


}

