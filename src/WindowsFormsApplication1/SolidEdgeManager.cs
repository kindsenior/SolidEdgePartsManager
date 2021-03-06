﻿using System;
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
    enum ProcessType { Add, Open, Skip, Exception };

    class SolidEdgeManager
    {
        //private uint m_targetThreadId;
        private uint m_mainWindowHandle;
        static private string m_windowName = "Solid Edge";
        private uint m_retryCount = 0;
        private SolidEdgeFramework.PropertySets m_propertySets;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //[DllImport("user32.dll")]
        //extern static bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern bool BringWindowToTop(IntPtr hWnd);

        delegate void WNDENUMPROC(IntPtr hwnd, int lParam);

        //[DllImport("user32.dll")]
        //private static extern int EnumChildWindows(IntPtr hWnd, WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern int EnumThreadWindows(IntPtr hWnd, WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        
        //[DllImport("kernel32.dll")]
        //static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        void SetDialogForegroundAndEnterAltN(IntPtr hwnd, int lParam)
        {
            StringBuilder builder = new StringBuilder(100);
            if (GetWindowText(hwnd, builder, builder.Capacity) != 0)
            {
                if (m_mainWindowHandle != (uint)hwnd && 0 <= builder.ToString().IndexOf(m_windowName))
                {
                    Console.WriteLine("   handle:" + hwnd + " param: " + lParam + " title:" + builder.ToString());
                    SetForegroundWindow(hwnd);
                    SendKeys.SendWait("%n");
                }
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

        public void NotOpenAsm()
        {
            Console.WriteLine("NotOpenAsm()");

            //wait for open part file
            Thread.Sleep(1000);

            // search all process
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                //compare to window name:"Solid Edge"
                if ( 0 <= p.MainWindowTitle.IndexOf(m_windowName))
                {
                    Console.WriteLine("window title:" + p.MainWindowTitle + " handle:" + p.MainWindowHandle + " thread count:" + p.Threads.Count);
                    m_mainWindowHandle = (uint)p.MainWindowHandle;
                    //set dialog foreground and enter Alt+N
                    //EnumChildWindows(p.MainWindowHandle, CALLBACKFUNC, 0);
                    EnumThreadWindows(new IntPtr(p.Threads[0].Id), SetDialogForegroundAndEnterAltN, 0);
                }
            }
        }

        //asmとparがカウント(Add)か展開(Open)かスキップ(Skip)かを判断する
        private ProcessType GetProcessType_impl(Dictionary<string, string> dummyDictionary)
        {
            foreach (SolidEdgeFramework.Properties propertySet in m_propertySets)
            {
                Console.WriteLine("propertySet.Name:" + propertySet.Name);
                if (propertySet.Name == "Custom")
                {
                    string keyName = "ProcessType";
                    foreach (SolidEdgeFramework.Property property in propertySet)
                    {
                        if (property.Name == keyName)
                        {
                            Console.WriteLine("  found ProcessType property");
                            return (ProcessType) Enum.Parse(typeof(ProcessType), property.get_Value());
                        }
                    }
                    //ProcessTypeがなかった時の処理
                    ProcessTypeForm processTypeForm = new ProcessTypeForm();
                    if (processTypeForm.ShowDialog() == DialogResult.OK)
                    {
                        ProcessType processType = (ProcessType) processTypeForm.ComboBoxProcessType.SelectedValue;
                        Console.WriteLine("  add ProcessType:" + processType.ToString("g"));
                        propertySet.Add(keyName, processType.ToString("g"));
                        propertySet.Save();
                        return processType;
                    }
                    else
                    {
                        Console.WriteLine("  skipped or closed ProcessTypeForm");
                        return ProcessType.Exception;
                    }
                }
            }

            return ProcessType.Exception;
        }

        private ProcessType SetPartProparty_impl(Dictionary<string, string> inputPropertySet)
        {
            foreach (SolidEdgeFramework.Properties propertySet in m_propertySets)
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
            return ProcessType.Exception;
        }
        
        //SolidEdgeファイルを開く前後の共通処理
        private ProcessType ManipulateProperty(string filename, Dictionary<string,string> inputPropertySet,
            Func<Dictionary<string,string>, ProcessType> func, bool autoRetryFlg = false)
        {
            Console.WriteLine("ManipulateProperty(" + filename + ",Dictionary)");
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument part = null;
            SolidEdgePart.SheetMetalDocument psm = null;
            SolidEdgeAssembly.AssemblyDocument asm = null;
            ProcessType processType;

            try
            {
                //check if the file exists
                if (!System.IO.File.Exists(filename))
                    throw (new System.Exception("file not found: " + filename));

                //connect to solidedge instance
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                documents = application.Documents;
                Console.WriteLine("solid edge found");

                Thread threadA = new Thread(new ThreadStart(NotOpenAsm));
                threadA.Start();

                //開く&プロパティ取得
                m_propertySets = null;
                switch (System.IO.Path.GetExtension(filename))
                {
                    case ".par":
                        Console.WriteLine("open part");
                        part = (SolidEdgePart.PartDocument)documents.Open(filename);
                        threadA.Join();
                        m_propertySets = part.Properties;
                        break;
                    case ".psm":
                        Console.WriteLine("open psm");
                        psm = (SolidEdgePart.SheetMetalDocument)documents.Open(filename);
                        threadA.Join();
                        m_propertySets = psm.Properties;
                        break;
                    case ".asm":
                        Console.WriteLine("open asm");
                        asm = (SolidEdgeAssembly.AssemblyDocument)documents.Open(filename);
                        threadA.Join();
                        m_propertySets = asm.Properties;
                        break;
                    default:
                        MessageBox.Show("!!Bad Extention Type: " + System.IO.Path.GetExtension(filename) + "!!");
                        return ProcessType.Exception;
                }

                //プロパティ処理
                processType = func(inputPropertySet);

                //保存&閉じる
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
                        return ProcessType.Exception;
                }

            }
            catch (System.Exception ex)
            {
                processType = ProcessType.Exception;
                if ( autoRetryFlg && m_retryCount <= 2||
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Retry )
                {
                    ++m_retryCount;
                    if (autoRetryFlg) System.Threading.Thread.Sleep(1000);
                    processType = ManipulateProperty(filename, inputPropertySet, func);
                }
            }
            finally
            {
                m_retryCount = 0;
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

            return processType;
        }

        private ProcessType GetProcessType(string filename, bool autoRetryFlg = false)
        {
            Dictionary<string, string> dummyDictionary = new Dictionary<string, string>();
            return ManipulateProperty(filename, dummyDictionary, GetProcessType_impl, autoRetryFlg);
        }

        public void createPartsListDictionary(ref Dictionary<string, string> partsListDictionary, string fileName, bool autoRetryFlg = false)
        {
            Console.WriteLine("createPartsListDictionary() file:" + fileName);

            string fileBaseName = System.IO.Path.GetFileNameWithoutExtension(fileName);
            ProcessType processType = GetProcessType(fileName, autoRetryFlg);
            switch (processType)
            {
                case ProcessType.Add:// link level
                    {
                        Console.WriteLine("  add: " + fileBaseName);
                        string dftname = System.IO.Path.GetDirectoryName(fileName) + "\\dft\\" + System.IO.Path.GetFileNameWithoutExtension(fileName) + ".dft";
                        string partsListStr;
                        GetPartsListAsString(dftname, out partsListStr);
                        //link nameは一意でなければならない
                        partsListDictionary.Add(System.IO.Path.GetFileNameWithoutExtension(fileName), partsListStr);
                        //spreadsheetManager.PasetToWorksheet(System.IO.Path.GetFileNameWithoutExtension(dftname), partsListStr);
                    }
                    break;
                case ProcessType.Open:// limb level
                    {
                        Console.WriteLine("  open " + fileBaseName);
                        List<string> childFileNames = GetOccurenceFiles(fileName);
                        foreach (string childFileName in childFileNames)
                        {// link
                            Console.WriteLine("  child name: " + System.IO.Path.GetFileNameWithoutExtension(childFileName));
                            createPartsListDictionary(ref partsListDictionary, childFileName, autoRetryFlg);
                        }
                    }
                    break;
                case ProcessType.Skip:
                    Console.WriteLine("  skip " + fileBaseName);
                    break;
                case ProcessType.Exception:
                    Console.WriteLine("  exception " + fileBaseName);
                    break;
                default:
                    MessageBox.Show("No such ProcessType:" + processType.ToString("g"));// enumを文字列に変換
                    break;
            }
        }

        public void SetPartProperty(string filename, Dictionary<string, string> inputPropertySet, bool autoRetryFlg = false)
        {
            ManipulateProperty(filename, inputPropertySet, SetPartProparty_impl, autoRetryFlg);
        }

        public void SetPartsProperties(Dictionary<string, Dictionary<string,string>> propertySetDictionary, bool autoRetryFlg = false)
        {
            Console.WriteLine("SetAllPartsProperties()");

            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\pulleycover-wrist.par", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\pulleycover-wrist.par"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\common_parts\\harmonic\\CSD20\\CSD20-adapter-pin-atunyu.asm", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\common_parts\\harmonic\\CSD20\\CSD20-adapter-pin-atunyu.asm"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\arm-cable-carrier_outer.psm",propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\ARM\\arm-cable-carrier_outer.psm"]);
            //SetPartProperty("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\LEG\\freejoint-D40-65-encoder-base.par", propertySetDictionary["\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\LEG\\freejoint-D40-65-encoder-base.par"]);            
            foreach (string key in propertySetDictionary.Keys)
            {
                SetPartProperty(key, propertySetDictionary[key], autoRetryFlg);
            }
        }


        public void GetPartsListAsString(string filename, out string clipboardStr, bool ConfirmUpdateFlg = false)
        {
            Console.WriteLine("CopyPartsListToClipboard(" + filename + ")");

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeDraft.DraftDocument dft = null;
            clipboardStr = "";

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

                Console.WriteLine("store data from clipboard to string");
                IDataObject data = Clipboard.GetDataObject();// get data from clipboard
                if (data.GetDataPresent(DataFormats.Text))
                {
                    clipboardStr = (string)data.GetData(DataFormats.Text);
                    Console.WriteLine(clipboardStr);
                }

                Console.WriteLine("close draft");
                dft.Close(false);
            }
            catch (System.Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    GetPartsListAsString(filename,out clipboardStr,ConfirmUpdateFlg);
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
