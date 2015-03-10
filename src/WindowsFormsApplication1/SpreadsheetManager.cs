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
        private SpreadsheetEntry m_spreadsheet;

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
            Console.WriteLine("hoge");

        }

    }
}
