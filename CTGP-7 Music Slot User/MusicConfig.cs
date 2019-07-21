using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CTGP_7_Music_Slot_User
{
    class MusicConfigEntry
    {
        [Browsable(false)]
        NameList info;

        [Browsable(false)]
        public string szsNameInner;

        public string CourseName
        {
            get
            {
                string ret = info.GetHumanName(szsNameInner);
                if (ret == null) ret = "";
                return ret;
            }
            set
            {
                szsNameInner = info.GetSzsName(value);
            }
        }
        public string MusicName { get; set; }

        [Browsable(false)]
        public string MusicModeInner;

        public string MusicMode
        {
            get
            {
                switch (MusicModeInner)
                {
                    case "MULTI_WATER":
                        return "Multi Channel Water";
                    case "MULTI_AREA":
                        return "Multi Channel Area";
                    default:
                        return "Single Channel";
                }
            }
            set
            {
                switch (value)
                {
                    case "Multi Channel Water":
                        MusicModeInner = "MULTI_WATER";
                        break;
                    case "Multi Channel Area":
                        MusicModeInner = "MULTI_AREA";
                        break;
                    default:
                        MusicModeInner = "SINGLE";
                        break;
                }
            }
        }

        [Browsable(false)]
        public byte NormalBPMInner;
        public string NormalBPM
        {
            get
            {
                return NormalBPMInner.ToString();
            }
            set
            {
                NormalBPMInner = Byte.Parse(value);
            }
        }

        [Browsable(false)]
        public byte FastBPMInner;
        public string FastBPM
        {
            get
            {
                return FastBPMInner.ToString();
            }
            set
            {
                FastBPMInner = Byte.Parse(value);
            }
        }

        [Browsable(false)]
        public uint NormalOffsetInner;
        public string NormalOffset
        {
            get
            {
                return NormalOffsetInner.ToString();
            }
            set
            {
                NormalOffsetInner = UInt32.Parse(value);
            }
        }

        [Browsable(false)]
        public uint FastOffsetInner;
        public string FastOffset
        {
            get
            {
                return FastOffsetInner.ToString();
            }
            set
            {
                FastOffsetInner = UInt32.Parse(value);
            }
        }

        public MusicConfigEntry(NameList list, string courseName, string musicName, string musicMode, byte normalBPM, byte fastBPM, uint normalOffset, uint fastOffset) 
        {
            info = list;
            szsNameInner = courseName;
            MusicName = musicName;
            MusicModeInner = musicMode;
            NormalBPMInner = normalBPM;
            FastBPMInner = fastBPM;
            NormalOffsetInner = normalOffset;
            FastOffsetInner = fastOffset;
        }

        public MusicConfigEntry(NameList list)
        {
            info = list;
            CourseName = "";
            MusicName = "";
            MusicMode = "";
            NormalBPMInner = 0;
            FastBPMInner = 0;
            NormalOffsetInner = 0;
            FastOffsetInner = 0;
        }
    }
    class MusicConfigManager
    {
        public BindingList<MusicConfigEntry> entries;
        public MusicConfigManager(NameList list, string fileName)
        {
            entries = new BindingList<MusicConfigEntry>();
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue;
                string[] elements = line.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                if (elements.Length != 7) continue;
                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i] = elements[i].Trim();
                }
                try
                {
                    entries.Add(new MusicConfigEntry(list, elements[0], elements[1], elements[2], Byte.Parse(elements[3]), Byte.Parse(elements[5]), UInt32.Parse(elements[4]), UInt32.Parse(elements[6])));
                } catch (Exception e)
                {
                    continue;
                }
            }
        }
        public MusicConfigManager()
        {
            entries = new BindingList<MusicConfigEntry>();
        }
        public void Write(string fileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            System.IO.StreamWriter file = new System.IO.StreamWriter(fs);
            string defaultText = "#-------------------------------------------------------------------------------------------------------------------#\n# This is the custom music configuration file, you can find more info here: https://ctgp7.page.link/MusicSlotConfig #\n#-------------------------------------------------------------------------------------------------------------------#\n";
            file.WriteLine(defaultText);
            foreach(var entry in entries)
            {
                if (entry.CourseName == "") continue;
                file.WriteLine(entry.szsNameInner + " :: " + entry.MusicName + " :: " + entry.MusicModeInner + " :: " + entry.NormalBPMInner + " :: " + entry.NormalOffsetInner + " :: " + entry.FastBPMInner + " :: " + entry.FastOffsetInner);
            }
            file.Flush();
            file.Close();
        }
    }
}
