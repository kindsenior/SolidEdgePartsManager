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

        //public List<SolidEdgeAssembly.AssemblyDocument> GetOccurenceFiles(string filename)
        public List<string> GetOccurenceFiles(string filename)
        {
            Console.WriteLine("GetOccurenceFiles(" + filename + ")");

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeAssembly.AssemblyDocument asm = null;
            //SolidEdgeDraft.DraftDocument draft = null;
            //bool IsOverwrite = CheckboxIsOverwrite.Checked;

            //List<SolidEdgeAssembly.AssemblyDocument> occurenceDocuments = new List<SolidEdgeAssembly.AssemblyDocument>();
            List<string> occurenceFiles = new List<string>();
            //SolidEdgeAssembly.Occurrences occurences = new SolidEdgeAssembly.Occurrences;

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
                
                Console.WriteLine(asm.Occurrences.Count.ToString());
                for (int i = 1; i <= asm.Occurrences.Count; ++i)
                //for (int i = 1; i <= 1; ++i)
                {
                    Console.WriteLine("  sub:" + asm.Occurrences.Item(i).OccurrenceFileName);
                    occurenceFiles.Add(asm.Occurrences.Item(i).OccurrenceFileName);
                    //occurenceDocuments.Add(asm.Occurrences.Item(i).OccurrenceFileName);
                }

                Console.WriteLine("close asm");
                asm.Close(false);
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

            return occurenceFiles;
            //return occurenceDocuments;
        }

        public void GetProperties(string filename)
        {
            Console.WriteLine("GetProperties(" + filename + ")");
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument part = null;

            try
            {
                //check if the file exists
                if (!System.IO.File.Exists(filename))
                    throw (new System.Exception("file not found: " + filename));

                //check if the file ext is dft
                if (System.IO.Path.GetExtension(filename) != ".par")
                    throw (new System.Exception("This is not a Part file: " + filename));

                //connect to solidedge instance
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                documents = application.Documents;
                Console.WriteLine("solid edge found");

                Console.WriteLine("open part");
                part = (SolidEdgePart.PartDocument)documents.Open(filename);
                
                SolidEdgeFramework.PropertySets properties = part.Properties;
                Console.WriteLine(properties.Count.ToString());
                for (int i = 1; i <= properties.Count; ++i )
                {
                    Console.WriteLine(properties.Item(i).Name);
                    if (properties.Item(i).Name == "Custom")
                    {
                        for (int j = 1; j <= properties.Item(i).Count; ++j)
                        {
                            Console.WriteLine(" " +properties.Item(i).Item(j).Name+" "+properties.Item(i).Item(j).get_Value().ToString());
                        }
                    }
                }

                Console.WriteLine("close part");
                part.Close(false);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (part != null)
                {
                    Marshal.ReleaseComObject(part);
                    part = null;
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

        }

        public void CopyPartsListToClipboard(string filename)
        {
            Console.WriteLine("CopyPartsListToClipboard(" + filename + ")");

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeDraft.DraftDocument dft = null;

            try
            {
                //check if the file exists
                if (!System.IO.File.Exists(filename))
                    throw (new System.Exception("file not found: " + filename));

                //check if the file ext is dft
                if (System.IO.Path.GetExtension(filename) != ".dft")
                    throw (new System.Exception("This is not a Draft file: " + filename));

                //connect to solidedge instance
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                documents = application.Documents;
                Console.WriteLine("solid edge found");

                Console.WriteLine("open draft");
                dft = (SolidEdgeDraft.DraftDocument)documents.Open(filename);

                dft.UpdatePropertyTextCacheAndDisplay();
                MessageBox.Show("Push update button if needed");
                dft.SaveAs(filename);

                SolidEdgeDraft.PartsLists partsLists = dft.PartsLists;
                Console.WriteLine(partsLists.Count.ToString());
                foreach(SolidEdgeDraft.PartsList partsList in partsLists )
                {
                    Console.WriteLine(partsList.AssemblyFileName);
                    partsList.CopyToClipboard();
                }

                Console.WriteLine("close draft");
                dft.Close(false);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dft != null)
                {
                    Marshal.ReleaseComObject(dft);
                    dft = null;
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

        }

    }
}
