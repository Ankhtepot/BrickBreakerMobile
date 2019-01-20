using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Classes {
    public class OptionsSet {
        public int HighestLevel;
        public bool ShowHintBoards;
        public bool MusicOn;

        public OptionsSet() : this(0,true,true) {}

        public OptionsSet(int highestLevel, bool showHintBoards, bool musicOn) {
            HighestLevel = highestLevel;
            ShowHintBoards = showHintBoards;
            MusicOn = musicOn;
        }

        public override string ToString() {
            return "OptionsSet:\nHighestLevel: " + this.HighestLevel + " |:| MusicOn: " + this.MusicOn +
                " |:| ShowHintBoards: " + this.ShowHintBoards;
        }
    }
}
