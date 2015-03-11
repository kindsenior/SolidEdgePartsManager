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

        private SpreadsheetEntry m_spreadsheet;
        private SpreadsheetsService m_service; 

        private WorksheetEntry GetWorksheetEntry(string worksheetTitle)
        {
            foreach (WorksheetEntry entry in m_spreadsheet.Worksheets.Entries)
            {
                if (entry.Title.Text == worksheetTitle) return entry;
            }
            return null;
        }

        public void GetPartsPathFromGDrive(SpreadsheetsService service)
        {
            SpreadsheetQuery query = new SpreadsheetQuery();

            SpreadsheetFeed feed = service.Query(query);
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
                        service.Insert(m_spreadsheet.Worksheets, newWorksheet);
                    }
                }
            }
        }


        public void PasetToWorksheet(string worksheetTitle)
        {
            SpreadsheetQuery query = new SpreadsheetQuery();

            SpreadsheetFeed feed = m_service.Query(query);
            foreach (SpreadsheetEntry spreadsheet in feed.Entries)
            {
                if (spreadsheet.Title.Text == "JAXON2図番管理表")
                {
                    m_spreadsheet = spreadsheet;
                    Console.WriteLine(spreadsheet.Title.Text);
                    WorksheetEntry worksheet = GetWorksheetEntry(worksheetTitle);
                    if (worksheet == null)
                    {
                        WorksheetEntry newWorksheet = new WorksheetEntry();
                        newWorksheet.Title.Text = worksheetTitle;
                        newWorksheet.Cols = 8; newWorksheet.Rows = 100;
                        m_service.Insert(m_spreadsheet.Worksheets, newWorksheet);
                    }
                    worksheet = GetWorksheetEntry(worksheetTitle);
                    Console.WriteLine(worksheet.Title.Text);

                    

                    IDataObject data = Clipboard.GetDataObject();// get data from clipboard
                    if (data.GetDataPresent(DataFormats.Text))
                    {
                        string clipboardStr = (string)data.GetData(DataFormats.Text);
                        //Console.WriteLine(clipboardStr);

                        string[] rowStrs = clipboardStr.Split('\n');
                        Console.WriteLine(rowStrs.Length);

                        //string[] headRowStrs = rowStrs[0].Split('\t');
                        string[] headRowStrs = new string[8] { "itemnum", "filename", "numbers", "process", "material", "order", "type", "path" };

                        for (uint i = 0; i < headRowStrs.Length; ++i)
                        {
                            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
                            CellFeed cellFeed = m_service.Query(cellQuery);
                            CellEntry entry = new CellEntry(1, i+1, headRowStrs[i]);
                            cellFeed.Insert(entry);
                        }

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
                                    Console.Write(" "+cellStrs[i]);
                                } Console.WriteLine();
                                m_service.Insert(listFeed, newrow);
                            }
                        }

                    }                    

                    //foreach (ListEntry row in listFeed.Entries)
                    //{
                    //    // Print the first column's cell value
                    //    Console.WriteLine(row.Title.Text);
                    //    // Iterate over the remaining columns, and print each cell value
                    //    foreach (ListEntry.Custom element in row.Elements)
                    //    {
                    //        Console.Write(element.Value);
                    //        element.Value = "hoge";
                    //    }
                    //    Console.WriteLine();
                    //    //row.Update();
                    //    row.Delete();
                    //}

                }
            }
        }

    }
}
