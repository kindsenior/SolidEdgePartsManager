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

        //public void GetPartsProperties(string filename)
        //{
        //    Console.WriteLine("GetPartsProperties(" + filename + ")");
        //    SolidEdgeFramework.Application application = null;
        //    SolidEdgeFramework.Documents documents = null;
        //    SolidEdgePart.PartDocument part = null;

        //    try
        //    {
        //        //check if the file exists
        //        if (!System.IO.File.Exists(filename))
        //            throw (new System.Exception("file not found: " + filename));

        //        //check if the file ext is dft
        //        if (System.IO.Path.GetExtension(filename) != ".par")
        //            throw (new System.Exception("This is not a Part file: " + filename));

        //        //connect to solidedge instance
        //        application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
        //        documents = application.Documents;
        //        Console.WriteLine("solid edge found");

        //        Console.WriteLine("open part");
        //        part = (SolidEdgePart.PartDocument)documents.Open(filename);
                
        //        SolidEdgeFramework.PropertySets properties = part.Properties;
        //        Console.WriteLine(properties.Count.ToString());
        //        for (int i = 1; i <= properties.Count; ++i )
        //        {
        //            Console.WriteLine(properties.Item(i).Name);
        //            if (properties.Item(i).Name == "Custom")
        //            {
        //                for (int j = 1; j <= properties.Item(i).Count; ++j)
        //                {
        //                    Console.WriteLine(" " +properties.Item(i).Item(j).Name+" "+properties.Item(i).Item(j).get_Value().ToString());
        //                }
        //            }
        //        }

        //        Console.WriteLine("close part");
        //        part.Close(false);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        if (part != null)
        //        {
        //            Marshal.ReleaseComObject(part);
        //            part = null;
        //        }
        //        if (documents != null)
        //        {
        //            Marshal.ReleaseComObject(documents);
        //            documents = null;
        //        }
        //        if (application != null)
        //        {
        //            Marshal.ReleaseComObject(application);
        //            application = null;
        //        }
        //    }

        //}

        public void SetPartProperty(string filename, Dictionary<string,string> inputPropertySet)
        {
            Console.WriteLine("SetPartsProperty(" + filename + ",Dictionary)");
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument part = null;
            SolidEdgePart.SheetMetalDocument psm = null;
            SolidEdgeAssembly.AssemblyDocument asm = null;

            try
            {
                //check if the file exists
                if (!System.IO.File.Exists(filename))
                    throw (new System.Exception("file not found: " + filename));

                //connect to solidedge instance
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                documents = application.Documents;
                Console.WriteLine("solid edge found");

                SolidEdgeFramework.PropertySets propertySets = null;
                switch (System.IO.Path.GetExtension(filename))
                {
                    case ".par":
                        Console.WriteLine("open part");
                        part = (SolidEdgePart.PartDocument)documents.Open(filename);
                        propertySets = part.Properties;
                        break;
                    case ".psm":
                        Console.WriteLine("open psm");
                        psm = (SolidEdgePart.SheetMetalDocument)documents.Open(filename);
                        propertySets = psm.Properties;
                        break;
                    case ".asm":
                        Console.WriteLine("open asm");
                        asm = (SolidEdgeAssembly.AssemblyDocument)documents.Open(filename);
                        propertySets = asm.Properties;
                        break;
                    default:
                        MessageBox.Show("!!Bad Extention Type: " + System.IO.Path.GetExtension(filename) + "!!");
                        return;
                }

                foreach( SolidEdgeFramework.Properties propertySet in propertySets)
                {
                    if (propertySet.Name == "Custom")
                    {
                        foreach (string key in inputPropertySet.Keys)
                        {
                            bool keyFoundFlg = false;
                            foreach (SolidEdgeFramework.Property property in propertySet)
                            {
                                if (key == property.Name)
                                {
                                    property.set_Value(inputPropertySet[key]);
                                    keyFoundFlg = true;
                                }
                            }
                            if (!keyFoundFlg)
                            {
                                Console.WriteLine("  add propetry: " + key);
                                propertySet.Add(key, inputPropertySet[key]);
                            }
                        }
                        propertySet.Save();
                    }
                }

                switch (System.IO.Path.GetExtension(filename))
                {
                    case ".par":
                        Console.WriteLine("save par");
                        part.Save();
                        Console.WriteLine("close part");
                        part.Close(false);
                        break;
                    case ".psm":
                        Console.WriteLine("save psm");
                        psm.Save();
                        Console.WriteLine("close psm");
                        psm.Close(false);
                        break;
                    case ".asm":
                        Console.WriteLine("save asm");
                        asm.Save();
                        Console.WriteLine("close asm");
                        asm.Close(false);
                        break;
                    default:
                        MessageBox.Show("!!Bad Extention Type: " + System.IO.Path.GetExtension(filename) + "!!");
                        return;
                }

            }
            catch (System.Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    SetPartProperty(filename, inputPropertySet);
                }
            }
            finally
            {
                if (part != null)
                {
                    Marshal.ReleaseComObject(part);
                    part = null;
                }
                if (psm != null)
                {
                    Marshal.ReleaseComObject(psm);
                    psm = null;
                }
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

        }


        public void SetPartsProperties(Dictionary<string, Dictionary<string,string>> propertySetDictionary)
        {
            Console.WriteLine("SetAllPartsProperties()");

            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\CHEST\\spine-center.par", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\CHEST\\spine-center.par"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\common_parts\\harmonic\\CSD20\\CSD20-adapter-pin-atunyu.asm", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\common_parts\\harmonic\\CSD20\\CSD20-adapter-pin-atunyu.asm"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\arm-cable-carrier_outer.psm",propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\arm-cable-carrier_outer.psm"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\LEG\\freejoint-D40-65-encoder-base.par", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\LEG\\freejoint-D40-65-encoder-base.par"]);            
            foreach (string key in propertySetDictionary.Keys)
            {
                SetPartProperty(key, propertySetDictionary[key]);
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
                //dft.SaveAs(filename);
                dft.Save();

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
