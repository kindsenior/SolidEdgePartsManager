using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
        private uint m_targetThreadId;
        private uint m_mainWindowHandle;
        private string m_windowName = "Solid Edge";

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        extern static bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);


        delegate void WNDENUMPROC(IntPtr hwnd, int lParam);

        //[DllImport("user32.dll")]
        //private static extern int EnumChildWindows(IntPtr hWnd, WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern int EnumThreadWindows(IntPtr hWnd, WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        void CALLBACKFUNC(IntPtr hwnd, int lParam)
        {
            uint nullProcessId = 0;
            uint windowId = GetWindowThreadProcessId(hwnd, out nullProcessId);
            StringBuilder builder = new StringBuilder();
            GetWindowText(hwnd, builder, 100);
            if (m_mainWindowHandle != (uint)hwnd && 0 <= builder.ToString().IndexOf(m_windowName))
            {
                Console.WriteLine("   handle:" + hwnd + " param: " + lParam + " id:" + windowId + " title:" + builder.ToString());
                SetForegroundWindow(hwnd);
            }
        }

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

                Console.WriteLine("open " + filename);
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

        public void SetDialogForeground()
        {

            // search all process
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                //compare window name
                if (0 <= p.MainWindowTitle.IndexOf(m_windowName))
                {
                    Console.WriteLine("window title:" + p.MainWindowTitle + " handle:" + p.MainWindowHandle + " thread count:" + p.Threads.Count);
                    m_mainWindowHandle = (uint)p.MainWindowHandle;

                    //EnumChildWindows(p.MainWindowHandle, CALLBACKFUNC, 0);
                    EnumThreadWindows(new IntPtr(p.Threads[0].Id), CALLBACKFUNC, 0);
                }
            }
        }

        public void NotOpenAsm()
        {
            Console.WriteLine("NotOpenAsm()");

            Thread.Sleep(5000);

            Console.WriteLine();
            SetDialogForeground();
            SendKeys.SendWait("%n");
        }

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
                        Thread threadA = new Thread(new ThreadStart(NotOpenAsm));
                        threadA.Start();

                        Thread.Sleep(1000);
                        Console.WriteLine("open part");
                        part = (SolidEdgePart.PartDocument)documents.Open(filename);

                        threadA.Join();

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

            SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\pulleycover-wrist.par", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\pulleycover-wrist.par"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\common_parts\\harmonic\\CSD20\\CSD20-adapter-pin-atunyu.asm", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\common_parts\\harmonic\\CSD20\\CSD20-adapter-pin-atunyu.asm"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\arm-cable-carrier_outer.psm",propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\arm-cable-carrier_outer.psm"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\LEG\\freejoint-D40-65-encoder-base.par", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\LEG\\freejoint-D40-65-encoder-base.par"]);            
            //foreach (string key in propertySetDictionary.Keys)
            //{
            //    SetPartProperty(key, propertySetDictionary[key]);
            //}
        }


        public void CopyPartsListToClipboard(string filename, bool ConfirmUpdateFlg = true)
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

                //dft.UpdatePropertyTextCacheAndDisplay();
                //dft.UpdatePropertyTextDisplay();
                
                if (ConfirmUpdateFlg)
                {
                    MessageBox.Show("Push update button if needed");
                }
                //dft.SaveAs(filename);
                //dft.Save();

                SolidEdgeDraft.PartsLists partsLists = dft.PartsLists;
                //foreach (SolidEdgeDraft.PartsList partsList in partsLists)
                //{
                Console.WriteLine("update partslist: " + partsLists.Item(1).AssemblyFileName);
                partsLists.Item(1).Update();
                //}

                //int changedAnnotationCount = 10;
                //int detatchedAnnotationCount = 10;
                //dft.AnnotationTrackerStatistics(out changedAnnotationCount, out detatchedAnnotationCount);
                //Console.WriteLine(changedAnnotationCount + " " + detatchedAnnotationCount);

                //if (dft.Dirty)
                //{
                //Console.WriteLine("Draft is modified scince the last time it was saved");

                Console.WriteLine("save draft");
                dft.Save();
                //}

                Console.WriteLine("copy to clipboard");
                partsLists.Item(1).CopyToClipboard();

                Console.WriteLine("close draft");
                dft.Close(false);
            }
            catch (System.Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    CopyPartsListToClipboard(filename,ConfirmUpdateFlg);
                }
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
