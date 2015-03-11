using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                        newWorksheet.Cols = 8; newWorksheet.Rows = 20;
                        m_service.Insert(m_spreadsheet.Worksheets, newWorksheet);
                    }
                    worksheet = GetWorksheetEntry(worksheetTitle);
                    Console.WriteLine(worksheet.Title.Text);

                    CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
                    CellFeed cellFeed = m_service.Query(cellQuery);
                    CellEntry entry = new CellEntry(1, 3, "c");
                    cellFeed.Insert(entry);

                    AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
                    ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
                    ListFeed listFeed = m_service.Query(listQuery);

                    IDataObject data = Clipboard.GetDataObject();
                    foreach (string fmt in data.GetFormats())
                    {
                        Console.WriteLine(fmt);
                    }

                    //ListEntry newrow = new ListEntry();
                    //newrow.Elements.Add(new ListEntry.Custom() { LocalName = "a", Value = "Joe" });
                    //newrow.Elements.Add(new ListEntry.Custom() { LocalName = "b", Value = "Smith" });
                    //m_service.Insert(listFeed, newrow);

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
