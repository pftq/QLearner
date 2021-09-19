using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLearner.QStates;
using QLearner.QAlgos;
using System.Runtime.Serialization;

namespace QLearner
{
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    [System.Reflection.ObfuscationAttribute(Feature = "properties renaming")]
    public abstract class QAPI
    {
        /* ********************* QLearner API Reference ************************* 
         * Definitions of numerous functions you can use to make it easier to code your plugins.  Do not include this file with your project.  This is just for reference.*/

        // Returns either the constants LEARN or AWAKE so you can vary your plugin depending on the mode.
        protected bool CurrentMode { get { return agent==null? AWAKEN:agent.CurrentMode; } }
        protected static bool LEARN { get { return QLearner.LEARN; } }
        protected static bool AWAKEN { get { return QLearner.AWAKEN; } }
        protected bool HideOutput { get { return main==null || main.OutputHidden; } }

        // Detect whether the current run has been aborted or is still running
        protected bool isRunning { get { return (agent==null? true:agent.isRunning); } }

        // Output text to the main GUI
        // Can be toggled off by the Hide Output checkbox unless you set the force flag to true
        protected void WriteOutput(string text = "", bool force = false) { if (agent != null) agent.WriteOutput(text, force); }

        // Send popup box that requires OK to be clicked
        protected void popup(string text = "") { 
            if(main!=null) main.popup(text); 
        }

        // Emergency abort button in case you need it
        protected void Abort() { if(main!=null) main.Abort(); }

        // Create a basic Settings GUI with a single method call in case you don't want to make your own
        // Accepts Dictionary of keys being the setting and the values being the current setting values.
        // Returns a Dictionary with the keys being the setting and the values being the setting values.
        protected Dictionary<string, string> NewSettingsForm(Dictionary<string, string> SettingNames)
        {
            if(main==null) return null;
            else return main.NewSettingsForm(SettingNames);
        }

        // Add or update the QValues table
        protected void RenameLearningTable(string s, string a, string qv)
        {
            if(main!=null) main.RenameLearningTable(s, a, qv);
        }
        protected void UpdateLearningTable(string s, string a, decimal qv)
        {
            if(main!=null) main.UpdateLearningTable(s, a, qv);
        }
        protected void UpdateLearningTable(QState s, QAction a, decimal qv)
        {
            if(main!=null) main.UpdateLearningTable(s, a, qv);
        }
        protected void ClearLearningTable()
        {
            if (main != null) main.ClearLearningTable();
        }
        /* *********** End of functions for you to use.  Below is just implementation stuff. ******** */
        private QLearner main=null;
        private QAgent agent=null;
        public void setQLearner(QLearner q) { main = q; }
        public void setQAgent(QAgent q) { agent = q; }
        public void Inherit(QAPI q) { main = q.main; agent = q.agent; }
    }
}
