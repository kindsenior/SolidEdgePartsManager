using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Google.GData.Client;
using Google.GData.Spreadsheets;

using Google.Apis.Auth.OAuth2;

namespace WindowsFormsApplication1
{
    class SpreadsheetManager
    {
        public SpreadsheetManager(SpreadsheetsService service)
        {
            m_service = service;
        }

        //public SpreadsheetManager(string account, string pass)
        public SpreadsheetManager()
        {
            Console.WriteLine("SpreadsheetManager()");

            //old for OAuth1
            //Console.WriteLine("SpreadsheetManager(" + account + ",<pass>)");
            //SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            //service.setUserCredentials(account + "@jsk.imi.i.u-tokyo.ac.jp", pass);
            //m_service = service;
            //Console.WriteLine(" account: " + account + "  pass: " + pass);

            //for OAuth2
            SpreadsheetsService service = new SpreadsheetsService("SolidEdgePartsManager")
            {
                Credentials = new GDataCredentials(GoogleAuthorizationManager.userCredential.Token.TokenType + " " + GoogleAuthorizationManager.userCredential.Token.AccessToken),
                RequestFactory = GoogleAuthorizationManager.requestFactory
            };
            m_service = service;

            m_query = new SpreadsheetQuery();
            m_feed = null;
            try
            {
                m_feed = m_service.Query(m_query);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("SpreadsheetManager() error\n" + ex.Message);
                m_feedFlg = false;
            }

        }

        private SpreadsheetEntry m_spreadsheet;
        private SpreadsheetsService m_service;
        private SpreadsheetQuery m_query;
        private SpreadsheetFeed m_feed;
        private WorksheetEntry m_worksheet;
        public bool m_feedFlg = true;
        private string m_clipboardStr;
        private Thread m_thread;

        private WorksheetEntry GetWorksheetEntry(string worksheetTitle)
        {
            foreach (WorksheetEntry entry in m_spreadsheet.Worksheets.Entries)
            {
                if (entry.Title.Text == worksheetTitle) return entry;
            }
            return null;
        }

        public bool SetSpreadsheetByName(string spreadsheetName)
        {
            Console.WriteLine("SetSpreadsheetByName(" + spreadsheetName + ")");

            foreach (SpreadsheetEntry spreadsheet in m_feed.Entries)
            {
                if (spreadsheet.Title.Text == spreadsheetName)
                {
                    m_spreadsheet = spreadsheet;
                    Console.WriteLine("Set to spreadsheet: "+spreadsheet.Title.Text);
                    return true;
                }
            }
            MessageBox.Show("Spreadsheet '"+ spreadsheetName +"' not found");
            return false;
        }

        public Dictionary<string,Dictionary<string,string>> GetPartsProperties()
        {
            Console.WriteLine("GetPartsProperties()");

            WorksheetEntry worksheet = GetWorksheetEntry("発注リスト");

            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = m_service.Query(listQuery);
            
            Dictionary<string, int> columnHeadDic = new Dictionary<string,int>();
            columnHeadDic["ロボット番号"] = 0;
            columnHeadDic["図番"] = 0;
            columnHeadDic["図番タイプ"] = 0;
            columnHeadDic["パス"] = 0;
            columnHeadDic["対称性"] = 0;
            //columnHeadDic["num-sym"] = 0;
            //columnHeadDic["num-asy"] = 0;
            //columnHeadDic["spare-sym"] = 0;
            //columnHeadDic["spare-asy"] = 0;
            //columnHeadDic["stock-sym"] = 0;
            columnHeadDic["個数"] = 0;
            columnHeadDic["鏡映個数"] = 0;
            columnHeadDic["発注先"] = 0;
            columnHeadDic["表面処理"] = 0;
            columnHeadDic["工程"] = 0;
            columnHeadDic["型番"] = 0;
            //columnHeadDic["材質"] = 0;

            //Check Column Head in Google Spreadsheet
            ListEntry secondRow = (ListEntry)listFeed.Entries[0];
            ListEntry.CustomElementCollection secondRowElements = (ListEntry.CustomElementCollection) secondRow.Elements;
            for (int i = 0; i < secondRowElements.Count; ++i)
            {
                string columnHead = secondRowElements[i].LocalName;
                for (int j = 0; j < columnHeadDic.Keys.Count; ++j )
                {
                    string key = columnHeadDic.Keys.ElementAt<string>(j);
                    if (key== columnHead)
                    {
                        columnHeadDic[key] = i;
                        Console.Write("  " + key + " :" + i);
                    }
                }
            } Console.WriteLine();

            //Get parts properties
            Dictionary<string, Dictionary<string,string>> propertySetDictionary = new Dictionary<string, Dictionary<string,string>>();
            foreach (ListEntry rowEntry in listFeed.Entries)
            {
                if (rowEntry.Elements[0].Value != "")
                {
                    Console.WriteLine(rowEntry.Elements[columnHeadDic["図番"]].Value);
                    Dictionary<string,string> rowData = new Dictionary<string, string>();
                    foreach (string key in columnHeadDic.Keys)
                    {
                        Console.Write(" " + key + ":" + rowEntry.Elements[columnHeadDic[key]].Value);
                        rowData[key] = rowEntry.Elements[columnHeadDic[key]].Value;
                    } Console.WriteLine();
                    //propertySetDictionary[rowEntry.Elements[columnHeadDic["ファイル名 (完全パス)"]].Value] = rowData;
                    propertySetDictionary[rowEntry.Elements[columnHeadDic["パス"]].Value] = rowData;
                }
            }

            //foreach (string key in propertySetDictionary.Keys)
            //{
            //    Console.Write(key + "    ");
            //    foreach (string subkey in propertySetDictionary[key].Keys)
            //    {
            //        Console.WriteLine(" " + subkey + ": " + propertySetDictionary[key][subkey]);
            //    } Console.WriteLine();
            //}

            resolveMirrorPartsNum(ref propertySetDictionary);

            return propertySetDictionary;
        }

        private void resolveMirrorPartsNum(ref Dictionary<string, Dictionary<string,string>> propertySetDictionary)
        {
            Console.WriteLine("resolveMirrorPartsNum()");

            foreach (System.Collections.Generic.KeyValuePair<string,Dictionary<string,string>> propertySet in propertySetDictionary)
            {
                string fileName = propertySet.Key;
                string mirFileName = System.IO.Path.GetDirectoryName(fileName) + "\\"
                        + System.IO.Path.GetFileNameWithoutExtension(fileName)
                        + "_mir" + System.IO.Path.GetExtension(fileName);
                if (propertySetDictionary.ContainsKey(mirFileName))
                {
                    Console.WriteLine("found mirror file:" + mirFileName);
                    Dictionary<string,string> mirPropertySet = propertySetDictionary[mirFileName];
                    propertySet.Value["個数"]     = (int.Parse(propertySet.Value["個数"])  + int.Parse(mirPropertySet["鏡映個数"])).ToString();
                    propertySet.Value["鏡映個数"] = (int.Parse(mirPropertySet["個数"])     + int.Parse(propertySet.Value["鏡映個数"])).ToString();
                    Console.WriteLine("num:" + propertySet.Value["個数"] + " mirNum:"+propertySet.Value["鏡映個数"]);
                }
            }
        }


        //public void GetPartsPathFromGDrive()
        //{
        //    SpreadsheetQuery query = new SpreadsheetQuery();

        //    SpreadsheetFeed feed = m_service.Query(query);
        //    foreach (SpreadsheetEntry spreadsheet in feed.Entries)
        //    {
        //        if (spreadsheet.Title.Text == "JAXON2図番管理表")
        //        {
        //            m_spreadsheet = spreadsheet;
        //            Console.WriteLine(spreadsheet.Title.Text);
        //            WorksheetEntry worksheet = GetWorksheetEntry("Test");
        //            if (worksheet == null)
        //            {
        //                WorksheetEntry newWorksheet = new WorksheetEntry();
        //                newWorksheet.Title.Text = "Test";
        //                newWorksheet.Cols = 10; newWorksheet.Rows = 20;
        //                m_service.Insert(m_spreadsheet.Worksheets, newWorksheet);
        //            }
        //        }
        //    }
        //}

        public void Join()
        {
            Console.WriteLine("Joint()");
            if (m_thread != null)
            {
                m_thread.Join();
                Console.WriteLine("Finished updating " + m_worksheet.Title.Text);
            }
            else
            {
                Console.WriteLine("thread slot is null");
            }
        }

        private void ResetLinkWorksheet(WorksheetEntry worksheet)
        {
            Console.WriteLine("ResetLinkWorksheet(" + worksheet.Title.Text + ")");

            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = m_service.Query(listQuery);
            for (int i = listFeed.Entries.Count - 1; i >= 0; --i)
            {
                listFeed.Entries[i].Delete();
            }
        }

        private void PasteToWorksheetThreaded()
        {
            Console.WriteLine("PasteToWorksheetThreaded()");

            ResetLinkWorksheet(m_worksheet);

            string[] rowStrs = m_clipboardStr.Split('\n');

            //string[] headRowStrs = rowStrs[0].Split('\t');
            string[] headRowStrs = new string[13] { "itemnum", "filename", "num", "process", "material", "robotno", "order", "type", "path", "sym", "maker", "surface", "model" };

            //for (uint i = 0; i < headRowStrs.Length; ++i)
            //{
            //    Console.WriteLine(worksheet.CellFeedLink.ToString());
            //    CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            //    CellFeed cellFeed = m_service.Query(cellQuery);
            //    CellEntry entry = new CellEntry(1, i+1, headRowStrs[i]);
            //    cellFeed.Insert(entry);
            //}

            AtomLink listFeedLink = m_worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = m_service.Query(listQuery);
            

            for (int i = 0; i < rowStrs.Length; ++i)
            {
                //string pattern = @"\t+";
                //Regex rgx = new Regex(pattern);
                //string[] cellStrs = rgx.Split(rowStr);
                string[] cellStrs = rowStrs[i].Split('\t');

                if (cellStrs.Length == headRowStrs.Length)
                {
                    ListEntry newrow = new ListEntry();
                    for (uint j = 0; j < cellStrs.Length; ++j)
                    {
                        newrow.Elements.Add(new ListEntry.Custom() { LocalName = headRowStrs[j], Value = cellStrs[j] });
                    }
                    m_service.Insert(listFeed, newrow);
                }
                else if (i != rowStrs.Length - 1)
                {
                    MessageBox.Show("Draft partslist column num is not!!" + headRowStrs.Count().ToString() + "\n Please change link draft partslist!!");
                    return;
                }
            }
        }

        public void PasteToWorksheet(string worksheetTitle, string clipboardStr)
        {
            Console.WriteLine("PasetToWorksheet(" + worksheetTitle + ")");

            m_clipboardStr = clipboardStr;

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = null;
            try
            {
                feed = m_service.Query(query);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Spreadsheet access failed.!!\n Prease check account and password.");
                return;
            }

            foreach (SpreadsheetEntry spreadsheet in feed.Entries)
            {
                if (spreadsheet.Title.Text == "JAXON2図番管理表")
                {
                    m_spreadsheet = spreadsheet;
                    Console.WriteLine("Spreadsheet Title: "+spreadsheet.Title.Text);
                    //WorksheetEntry oldWorksheet = GetWorksheetEntry(worksheetTitle);
                    //if (oldWorksheet != null) oldWorksheet.Delete();
                    //WorksheetEntry worksheet = new WorksheetEntry();
                    //worksheet.Title.Text = worksheetTitle;
                    //worksheet.Cols = 8; worksheet.Rows = 100;
                    //m_service.Insert(m_spreadsheet.Worksheets, worksheet);
                    m_worksheet = GetWorksheetEntry(worksheetTitle);
                    if (m_worksheet == null)
                    {
                        Console.WriteLine("Worksheet not found. Continue without updating link data worksheet.");
                        return;
                    }
                    Console.WriteLine("Worksheet Title: " + m_worksheet.Title.Text);

                    //m_thread = new Thread(new ThreadStart(PasteToWorksheetThreaded));
                    //m_thread.Start();
                    PasteToWorksheetThreaded();

                }
            }
        }

    }
}
