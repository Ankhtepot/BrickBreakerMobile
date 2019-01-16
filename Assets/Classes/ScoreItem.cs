using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Classes {
    public class ScoreItem {
        public DateTime Date;
        public int Score;
        public int HighestLevel;
        public bool isNewRecord;

        public ScoreItem(int score, int hl) {
            Date = DateTime.Now;
            Score = score;
            HighestLevel = hl;
            isNewRecord = true;
        }

        //Parameterless constructor for ES3
        public ScoreItem() {
            isNewRecord = false;
        }
    }
}
