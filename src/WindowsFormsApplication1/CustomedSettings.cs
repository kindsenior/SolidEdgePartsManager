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
        public CustomedSettings()
        {
            DestSheetDataHashSet = new HashSet<SheetData>();
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
    }
}
