using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.GData.Client;
using Google.Apis.Drive.v2;

namespace WindowsFormsApplication1
{
    class GoogleAuthorizationManager
    {
        private static string m_clientId = "1006233954353-1cauii1ksqvmip6kttlt2h9ku6hhpa4o.apps.googleusercontent.com";
        private static string m_clientSecret = "vhA55KV3ZQTVKbbKVS5z41pi";
        private static string m_redirectUri = "urn:ietf:wg:oauth:2.0:oob";
        //static  private string m_redirectUri = "http://localhost";
        private static string[] m_scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile, "https://spreadsheets.google.com/feeds" };
        private static string m_scope = DriveService.Scope.Drive + " " + DriveService.Scope.DriveFile + " " + "https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.appdata https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

        //public UserCredential userCredential;
        //public OAuth2Parameters oauth2Parameters;
        //public GOAuth2RequestFactory requestFactory;

        static public UserCredential userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = m_clientId,
                ClientSecret = m_clientSecret
            },
            m_scopes,
            "user",
            CancellationToken.None,
            new FileDataStore("SolidEdgePartsManager")// folder name
            ).Result;

        static public OAuth2Parameters oauth2Parameters = new OAuth2Parameters
        {
            ClientId = m_clientId,
            ClientSecret = m_clientSecret,
            RedirectUri = m_redirectUri,
            Scope = m_scope,
            AccessToken = userCredential.Token.AccessToken,
            RefreshToken = userCredential.Token.RefreshToken,
            TokenType = userCredential.Token.TokenType,
        };

        static public GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "SolidEdgePartsManager", oauth2Parameters);

        public GoogleAuthorizationManager()
        {
            //m_clientId = "1006233954353-1cauii1ksqvmip6kttlt2h9ku6hhpa4o.apps.googleusercontent.com";
            //m_clientSecret = "vhA55KV3ZQTVKbbKVS5z41pi";
            //m_redirectUri = "urn:ietf:wg:oauth:2.0:oob";
            //m_redirectUri = "http://localhost";
            //string[] m_scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile, "https://spreadsheets.google.com/feeds" };
            //string m_scope = DriveService.Scope.Drive + " " + DriveService.Scope.DriveFile + " " + "https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.appdata https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
            Console.WriteLine(m_scopes[0]);

            try
            {
                //userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                //    new ClientSecrets
                //    {
                //        ClientId = m_clientId,
                //        ClientSecret = m_clientSecret
                //    },
                //    m_scopes,
                //    "user1",
                //    CancellationToken.None,
                //    new FileDataStore("SolidEdgePartsManager")// folder name
                //    ).Result;

                //oauth2Parameters = new OAuth2Parameters
                //{
                //    ClientId = m_clientId,
                //    ClientSecret = m_clientSecret,
                //    RedirectUri = m_redirectUri,
                //    Scope = m_scope,
                //    AccessToken = userCredential.Token.AccessToken,
                //    RefreshToken = userCredential.Token.RefreshToken,
                //    TokenType = userCredential.Token.TokenType,
                //};

                //requestFactory = new GOAuth2RequestFactory(null, "SolidEdgePartsManager", oauth2Parameters);
            }
            //catch (AggregateException aex)
            //{
            //    MessageBox.Show(aex.Message);
            //}
            catch (SystemException ex)
            {
                MessageBox.Show("GoogleAuthorizationManager() error\n" + ex.Message);
            }

        }

    }
}
