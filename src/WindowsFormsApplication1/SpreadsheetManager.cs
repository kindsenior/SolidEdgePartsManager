using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        newWorksheet.Cols = 30; newWorksheet.Rows = 100;
                        m_service.Insert(m_spreadsheet.Worksheets, newWorksheet);
                    }
                    worksheet = GetWorksheetEntry(worksheetTitle);
                    Console.WriteLine(worksheet.Title);

                    CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
                    CellFeed cellFeed = m_service.Query(cellQuery);
                    foreach (CellEntry cell in cellFeed.Entries)
                    {
                        if (cell.Row == (uint)1 )
                        {
                            Console.WriteLine("write to sheet");
                            cell.InputValue = "200";
                            cell.Update();
                        }
                    }
                }
            }
        }

    }
}
