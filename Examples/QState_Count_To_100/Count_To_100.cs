using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearner.QStates
{
    public class Count_To_100:QState
    {
        private int value, downIncrement, upIncrement;
        private const int CountTo = 100; // The ending count
        private readonly QAction UP = new QAction_String("up"), DOWN = new QAction_String("down");

        public override QState Initialize()
        {
            value = 0;
            upIncrement = downIncrement = 1;
            return new Count_To_100() { value = value, downIncrement = downIncrement, upIncrement = upIncrement };
        }
        public override bool Equals(object obj)
        {
            return value==((Count_To_100)obj).value;
        }
        public override int GetHashCode()
        {
            return value;
        }
        public override QState GetNewState(QAction action)
        {
            return new Count_To_100() { value = (action == UP ? this.value + this.upIncrement : this.value - this.downIncrement), upIncrement = this.upIncrement, downIncrement = this.downIncrement };
        }
        public override QAction[] GetActions()
        {
            return new QAction[] { UP, DOWN };
        }
        public override decimal GetValue()
        {
            return -1*Math.Abs(value-CountTo); // The farther you're away, the worse the value
        }
        public override bool IsEnd()
        {
            return value == CountTo;
        }
        public override string ToString()
        {
            return "" + value.ToString("D8");
        }
        public override bool HasSettings
        {
            get
            {
                return true;
            }
        }
        public override void Settings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>(){
                {"Upward Increment", ""+upIncrement},
                {"Downward Increment", ""+downIncrement}
            };
            Dictionary<string, string> values = NewSettingsForm(settings);
            try
            {
                upIncrement = Convert.ToInt32(values["Upward Increment"]);
                downIncrement = Convert.ToInt32(values["Downward Increment"]);
            }
            catch (Exception)
            {
                WriteOutput("Invalid settings! Please only use numbers for increment values.");
            }
        }

        public override Dictionary<QFeature, decimal> GetFeatures(QAction action)
        {
            int newVal = value;
            if (action == UP) newVal++;
            else newVal--;
            return QFeature_String.FromStringDecimalDictionary(new Dictionary<string, decimal>() {
                //{value.ToString()+"_"+action, 1}, // Identity for sanity check
                //{"Distance", (decimal)Math.Abs(end-newVal)}
                {"Distance_Change", Math.Abs(CountTo-newVal)-Math.Abs(CountTo-value)}
            });
        }

        public override QState Open(object o)
        {
            object[] a = (object[])o;
            return new Count_To_100() { value = (int)a[0], upIncrement = (int)a[1], downIncrement = (int)a[2] };
        }

        public override object Save()
        {
            return new object[] { value, upIncrement, downIncrement };
        }

    }
}
