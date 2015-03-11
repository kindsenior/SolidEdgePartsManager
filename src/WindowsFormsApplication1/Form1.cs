﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;
using System.Runtime.InteropServices; // for com
using IWshRuntimeLibrary; // for shortcuts, needs windows script host object model

//using System.Xml;
//using System.IO;
using Google.GData.Client;
using Google.GData.Spreadsheets;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool debug;

        public Form1()
        {
            InitializeComponent();
#if DEBUG
            debug = true;
#else
            debug = false;
#endif
        }


        private string ResolveShorcut(string filename)
        {
            //WshShellClass shell = new WshShellClass(); // WshShellClassの型でエラーが出る
            //IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(filename);
            //return shortcut.TargetPath;
            return "hoge";
        }

        private bool IsSolidEdge()
        {
            SolidEdgeFramework.Application app = null;
            bool ret = false;

            try
            {
                //connect to solidedge instance
                app = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                ret = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("SolidEdgeが見つかりません");
            }
            finally
            {
                if (app != null)
                {
                    Marshal.ReleaseComObject(app);
                    app = null;
                }
            }
            return ret;
        }

        private void TextboxDestAsm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
            {
                if (debug) MessageBox.Show(s[0]);
                string ext = System.IO.Path.GetExtension(s[0]);
                if (ext == ".asm")
                {
                    TextboxDestAsm.Text = s[0];
                }
                else if (ext == ".lnk")
                {
                    string filename = ResolveShorcut(s[0]);
                    if (System.IO.Path.GetExtension(filename) == ".asm")
                    {
                        TextboxDestAsm.Text = s[0];
                    }
                }
            }
        }

        private void TextboxDestAsm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ButtonLoadDestAsm_Click(object sender, EventArgs e)
        {
            // OpenFileDialog の新しいインスタンスを生成する (デザイナから追加している場合は必要ない)
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Asmを追加";
            dialog.Filter = "asmファル|*.asm";
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TextboxDestAsm.Text = dialog.FileName;
            }
            dialog.Dispose();
        }

        private void ButtonUpdateAllPartsNumber_Click(object sender, EventArgs e)
        {
            Console.WriteLine("UpdateAllPartsNumber()");
            if(debug) MessageBox.Show("open asm: " + TextboxDestAsm.Text);

            if (!IsSolidEdge()) return;

            RefreshAccessToken();

            SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            service.setUserCredentials(TextboxImiAccount.Text+"@jsk.imi.i.u-tokyo.ac.jp", TextBoxImiPass.Text);
            Console.WriteLine(" account: " + TextboxImiAccount.Text + "  pass: " + TextBoxImiPass.Text);

            SpreadsheetManager spreadsheetManager = new SpreadsheetManager(service);
            //spreadsheetManager.GetPartsPathFromGDrive(service);

            SolidEdgeManager solidedgeManager = new SolidEdgeManager();

            TextboxDestAsm.Text = "\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\Arm-limb.asm";
            List<string> occurrenceFiles = solidedgeManager.GetOccurenceFiles(TextboxDestAsm.Text);
            Console.WriteLine(occurrenceFiles.Count.ToString());
            foreach (String occurrenceFile in occurrenceFiles)
            {// limb
                Console.WriteLine(occurrenceFile);

                //solidedgeManager.GetProperties(occurrenceFile);

                string dftname = System.IO.Path.GetDirectoryName(occurrenceFile) + "\\dft\\" + System.IO.Path.GetFileNameWithoutExtension(occurrenceFile)+".dft";
                solidedgeManager.CopyPartsListToClipboard(dftname);
                spreadsheetManager.PasetToWorksheet(System.IO.Path.GetFileNameWithoutExtension(dftname));

                //GetOccurenceFiles();
                //for(){// link
                //createWorksheet()
                //GeneratePartsList()
                //writePartsList()
                //}
            }


        }

        private bool RefreshAccessToken()
        {
            Console.WriteLine("RefreshAccessToken()");

            string CLIENT_ID = "1006233954353-1cauii1ksqvmip6kttlt2h9ku6hhpa4o.apps.googleusercontent.com";
            string CLIENT_SECRET = "vhA55KV3ZQTVKbbKVS5z41pi";

            string SCOPE = "https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.appdata https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

            //string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
            string REDIRECT_URI = "http://localhost";

            OAuth2Parameters parameters = new OAuth2Parameters();
            parameters.ClientId = CLIENT_ID;
            parameters.ClientSecret = CLIENT_SECRET;
            parameters.RedirectUri = REDIRECT_URI;
            parameters.Scope = SCOPE;

            //string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            //Console.WriteLine(authorizationUrl);
            //Console.WriteLine("Please visit the URL above to authorize your OAuth request token.  Once that is complete, type in your access code to continue...");
            //parameters.AccessCode = Console.ReadLine();
            parameters.AccessCode = "4/deIE1CH-cRFBTSdrsk4u78d0dYulzBzP5-XzSw2z9L4.Ys4jiJaGcykSBrG_bnfDxpJ2zoLzlwI";
            Console.WriteLine(" AuthorizationCode(AccessCode): " + parameters.AccessCode);

            //string redirectUrl = "http://www.example.com/oauth2callback?code=SOME-RETURNED-VALUE";
            //string redirectUrl = "https://code.google.com/apis/console?code=SOME-RETURNED-VALUE";
            //Uri uri = new Uri(redirectUrl);
            //Console.WriteLine(uri.Query.ToString());
            //OAuthUtil.GetAccessToken(uri.Query, parameters);

            string accessToken = "ya29.MAHytisyJiuz-nz8IPriQ-872u8hy-Xe0wsrRjgw9FdqufZmCHQPypBnSls5UR-uF9GNFy9ShchNRA";
            string refreshToken = "1/nrxh65IqcBQS6erUO2hj3wRKJVyAWlGz_C2IWej68dg"; // リフレッシュトークンは変わらない?
            parameters.AccessToken = accessToken;
            parameters.RefreshToken = refreshToken;
            OAuthUtil.RefreshAccessToken(parameters);

            //OAuthUtil.GetAccessToken(parameters);
            accessToken = parameters.AccessToken;
            Console.WriteLine(" Access Token: " + accessToken);
            refreshToken = parameters.RefreshToken;
            Console.WriteLine(" Refresh Token: " + refreshToken);

            GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", parameters);
            SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            service.RequestFactory = requestFactory;

            return true;
        }

    }
}
