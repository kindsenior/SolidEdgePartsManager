using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace WindowsFormsApplication1
{
    public class CustomedSettings : ApplicationSettingsBase
    {

        [UserScopedSetting()]
        public HashSet<String> DestAsmHashSet
        {
            get
            {
                return (HashSet<String>)this["DestAsmHashSet"];
            }
            set
            {
                this["DestAsmHashSet"] = value;
            }
        }

        [UserScopedSetting()]
        public int SelectedAsmIdx
        {
            get
            {
                return (int)this["SelectedAsmIdx"];
            }
            set
            {
                this["SelectedAsmIdx"] = value;
            }
        }

        [UserScopedSetting()]
        public HashSet<SheetData>DestSheetDataHashSet
        {
            get
            {
                return (HashSet<SheetData>)this["DestSheetDataHashSet"];
            }
            set
            {
                this["DestSheetDataHashSet"] = value;
            }
        }

        [UserScopedSetting()]
        public int SelectedSheetIdx
        {
            get
            {
                return (int)this["SelectedSheetIdx"];
            }
            set
            {
                this["SelectedSheetIdx"] = value;
            }
        }
    }
}
