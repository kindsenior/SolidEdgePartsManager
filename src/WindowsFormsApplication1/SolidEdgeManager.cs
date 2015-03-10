using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.Reflection;
using System.Runtime.InteropServices; // for com
using IWshRuntimeLibrary; // for shortcuts, needs windows script host object model


namespace WindowsFormsApplication1
{
    class SolidEdgeManager
    {

        public void GetOccurenceFiles(string filename)
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
                Console.WriteLine("solid edge found");

                Console.WriteLine("open asm");
                //draft = (SolidEdgeDraft.DraftDocument)documents.Add("SolidEdge.DraftDocument", Missing.Value);
                asm = (SolidEdgeAssembly.AssemblyDocument)documents.Open(filename);

                SolidEdgeAssembly.Occurrences subs = asm.Occurrences;
                MessageBox.Show(subs.Count.ToString());
                for (int i = 1; i <= subs.Count; ++i)
                {
                    MessageBox.Show("sub:" + subs.Item(i).OccurrenceFileName);
                }

                Console.WriteLine("close asm");
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

    }
}
