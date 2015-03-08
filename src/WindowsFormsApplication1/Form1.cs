using System;
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

        private void button1_Click(object sender, EventArgs e)
        {
            GetPartsPathFromGDrive();
            
            MessageBox.Show(TextboxDestAsm.Text);

            //if (!IsSolidEdge()) return;
            //getChildFile(TextboxDestAsm.Text);
        }

        private void getChildFile(string filename)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeAssembly.AssemblyDocument asm = null;
            //SolidEdgeDraft.DraftDocument draft = null;
            //bool IsOverwrite = CheckboxIsOverwrite.Checked;
            bool ret = false;

            try
            {
                //check if the file exists
                if (!System.IO.File.Exists(filename))
                    throw (new System.Exception("file not found: " + filename));

                //check if the file ext is dft
                if (System.IO.Path.GetExtension(filename) != ".asm")
                    throw (new System.Exception("This is not a Assembly file: " + filename));

                //connect to solidedge instance
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                documents = application.Documents;
                if (debug) MessageBox.Show("solid edge found");

                if (debug) MessageBox.Show("open asm");
                //draft = (SolidEdgeDraft.DraftDocument)documents.Add("SolidEdge.DraftDocument", Missing.Value);
                asm = (SolidEdgeAssembly.AssemblyDocument)documents.Open(filename);

                SolidEdgeAssembly.Occurrences subs = asm.Occurrences;
                MessageBox.Show(subs.Count.ToString());
                for (int i = 1; i <= subs.Count; ++i)
                {
                    MessageBox.Show( "sub:" + subs.Item(i).OccurrenceFileName );
                }
                
                if (debug) MessageBox.Show("close draft");
                asm.Close(false);

                ret = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (asm != null)
                {
                    Marshal.ReleaseComObject(asm);
                    asm = null;
                }
                if (documents != null)
                {
                    Marshal.ReleaseComObject(documents);
                    documents = null;
                }
                if (application != null)
                {
                    Marshal.ReleaseComObject(application);
                    application = null;
                }
            }

            return;
        }


        private void GetPartsPathFromGDrive()
        {
            string CLIENT_ID = "1006233954353-1cauii1ksqvmip6kttlt2h9ku6hhpa4o.apps.googleusercontent.com";
            string CLIENT_SECRET = "vhA55KV3ZQTVKbbKVS5z41pi";
            
            //string SCOPE = "https://www.googleapis.com/auth/drive";
            string SCOPE = "https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.appdata";
            
            //string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
            string REDIRECT_URI = "http://localhost";

            //string REDIRECT_URI = "http://www.example.com/oauth2callback";
            //string REDIRECT_URI = "https://code.google.com/apis/console";

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
            Console.WriteLine(" AuthorizationCode(AccessCode): "+parameters.AccessCode);

            //string redirectUrl = "http://www.example.com/oauth2callback?code=SOME-RETURNED-VALUE";
            //string redirectUrl = "https://code.google.com/apis/console?code=SOME-RETURNED-VALUE";
            //Uri uri = new Uri(redirectUrl);
            //Console.WriteLine(uri.Query.ToString());
            //OAuthUtil.GetAccessToken(uri.Query, parameters);

            string accessToken = "ya29.MAHytisyJiuz-nz8IPriQ-872u8hy-Xe0wsrRjgw9FdqufZmCHQPypBnSls5UR-uF9GNFy9ShchNRA";
            string refreshToken = "1/nrxh65IqcBQS6erUO2hj3wRKJVyAWlGz_C2IWej68dg";
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

            SpreadsheetQuery query = new SpreadsheetQuery();
            //SpreadsheetFeed feed = service.Query(query);
            //foreach (SpreadsheetEntry entry in feed.Entries)
            //{
            //    // Print the title of this spreadsheet to the screen
            //    Console.WriteLine(entry.Title.Text);
            //}

        }

    }
}
