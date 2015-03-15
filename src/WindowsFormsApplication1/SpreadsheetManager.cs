using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Google.GData.Client;
using Google.GData.Spreadsheets;


namespace WindowsFormsApplication1
{
    class SpreadsheetManager
    {
        public SpreadsheetManager(SpreadsheetsService service)
        {
            m_service = service;
        }

        public SpreadsheetManager(string account, string pass)
        {
            SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            service.setUserCredentials(account + "@jsk.imi.i.u-tokyo.ac.jp", pass);
            m_service = service;
            Console.WriteLine(" account: " + account + "  pass: " + pass);

            m_query = new SpreadsheetQuery();
            m_feed = null;
            try
            {
                m_feed = m_service.Query(m_query);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Spreadsheet access failed.!!\n Prease check account and password.");
                m_feedFlg = false;
            }

        }

        private SpreadsheetEntry m_spreadsheet;
        private SpreadsheetsService m_service;
        private SpreadsheetQuery m_query;
        private SpreadsheetFeed m_feed;
        public bool m_feedFlg = true;

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

        public List<Dictionary<string,string>> GetPartsProperties()
        {
            Console.WriteLine("GetPartsProperties()");

            WorksheetEntry worksheet = GetWorksheetEntry("発注リスト");

            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = m_service.Query(listQuery);
            
            Dictionary<string, int> columnHeadDic = new Dictionary<string,int>();
            columnHeadDic["part-num"] = 0;
            columnHeadDic["part-name"] = 0;
            columnHeadDic["part-path"] = 0;
            //columnHeadDic["sym-asy"] = 0;
            //columnHeadDic["num-sym"] = 0;
            //columnHeadDic["num-asy"] = 0;
            //columnHeadDic["spare-sym"] = 0;
            //columnHeadDic["spare-asy"] = 0;
            //columnHeadDic["stock-sym"] = 0;
            columnHeadDic["order-sym"] = 0;
            columnHeadDic["order-asy"] = 0;
            //columnHeadDic["maker"] = 0;
            //columnHeadDic["order-plan"] = 0;
            //columnHeadDic["order-state"] = 0;
            //columnHeadDic["surface-process"] = 0;

            //Check Column Head in Google Spreadsheet
            ListEntry secondRow = (ListEntry)listFeed.Entries[0];
            ListEntry.CustomElementCollection secondRowElements = (ListEntry.CustomElementCollection) secondRow.Elements;
            Console.WriteLine(columnHeadDic.Keys.Count.ToString());
            for (int i = 0; i < secondRowElements.Count; ++i)
            {
                string columnHead = secondRowElements[i].LocalName;
                for (int j = 0; j < columnHeadDic.Keys.Count; ++j )
                {
                    string key = columnHeadDic.Keys.ElementAt<string>(j);
                    if (key== columnHead)
                    {
                        columnHeadDic[key] = i;
                    }
                }
            }

            List<Dictionary<string, string>> propertyDictionaries = new List<Dictionary<string, string>>();
            foreach (ListEntry rowEntry in listFeed.Entries)
            {
                if (rowEntry.Elements[0].Value != "")
                {
                    Dictionary<string, string> rowData = new Dictionary<string, string>();
                    foreach (string key in columnHeadDic.Keys)
                    {
                        rowData[key] = rowEntry.Elements[columnHeadDic[key]].Value;
                    }
                    propertyDictionaries.Add(rowData);
                }
            }

            return propertyDictionaries;
        }

        public void GetPartsPathFromGDrive()
        {
            SpreadsheetQuery query = new SpreadsheetQuery();

            SpreadsheetFeed feed = m_service.Query(query);
            foreach (SpreadsheetEntry spreadsheet in feed.Entries)
            {
                if (spreadsheet.Title.Text == "JAXON2図番管理表")
                {
                    m_spreadsheet = spreadsheet;
                    Console.WriteLine(spreadsheet.Title.Text);
                    WorksheetEntry worksheet = GetWorksheetEntry("Test");
                    if (worksheet == null)
                    {
                        WorksheetEntry newWorksheet = new WorksheetEntry();
                        newWorksheet.Title.Text = "Test";
                        newWorksheet.Cols = 10; newWorksheet.Rows = 20;
                        m_service.Insert(m_spreadsheet.Worksheets, newWorksheet);
                    }
                }
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
                //ListEntry row = (ListEntry)listFeed.Entries[i];
                //row.Delete();
            }
        }

        public void PasetToWorksheet(string worksheetTitle)
        {
            Console.WriteLine("PasetToWorksheet(" + worksheetTitle + ")");

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
                    WorksheetEntry worksheet = GetWorksheetEntry(worksheetTitle);
                    if (worksheet == null)
                    {
                        Console.WriteLine("Worksheet not found. Continue without updating link data worksheet.");
                        return;
                    }
                    Console.WriteLine("Worksheet Title: " + worksheet.Title.Text);

                    ResetLinkWorksheet(worksheet);

                    IDataObject data = Clipboard.GetDataObject();// get data from clipboard
                    if (data.GetDataPresent(DataFormats.Text))
                    {
                        string clipboardStr = (string)data.GetData(DataFormats.Text);
                        //Console.WriteLine(clipboardStr);

                        string[] rowStrs = clipboardStr.Split('\n');
                        Console.WriteLine(rowStrs.Length);

                        //string[] headRowStrs = rowStrs[0].Split('\t');
                        string[] headRowStrs = new string[8] { "itemnum", "filename", "numbers", "process", "material", "order", "type", "path" };

                        //for (uint i = 0; i < headRowStrs.Length; ++i)
                        //{
                        //    Console.WriteLine(worksheet.CellFeedLink.ToString());
                        //    CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
                        //    CellFeed cellFeed = m_service.Query(cellQuery);
                        //    CellEntry entry = new CellEntry(1, i+1, headRowStrs[i]);
                        //    cellFeed.Insert(entry);
                        //}

                        AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
                        ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
                        ListFeed listFeed = m_service.Query(listQuery);

                        foreach (string rowStr in rowStrs)
                        {
                            //string pattern = @"\t+";
                            //Regex rgx = new Regex(pattern);
                            //string[] cellStrs = rgx.Split(rowStr);
                            string[] cellStrs = rowStr.Split('\t');

                            if (cellStrs.Length == headRowStrs.Length)
                            {
                                ListEntry newrow = new ListEntry();
                                for (uint i = 0; i < cellStrs.Length; ++i)
                                {
                                    newrow.Elements.Add(new ListEntry.Custom() { LocalName = headRowStrs[i], Value = cellStrs[i] });
                                }
                                m_service.Insert(listFeed, newrow);
                            }
                        }

                    }                    

                }
            }
        }

    }
}
