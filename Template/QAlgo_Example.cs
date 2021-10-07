using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;

namespace QLearner.QAlgos
{
    public class QAlgo_Example:QAlgo
    {
        /* QAlgo class structure
        * 
        * This file contains the general structure of a QAlgo class.
        * To get started, rename this file and change the class declaration from 'public abstract class...' to 'public class <yourPluginName>:QAlgo'.
        * All of the outlined functions below are required and must be defined.  Rename the "override" flag on each to "override".
        * When you are done coding, "Build" the solution in Visual Studio.  Your finished dll file will be in the bin/Release file.  You can now load this as a plugin file when running QLearner.
        */

        // Set to true if you design this algo to be threadsafe - all class variables should be updated with a lock to avoid multiple writes concurrently.
        public override bool multithreaded { get { return false; } }

        // Run once all time.  Instantiate variables and other one-time setups (Renaming tables, GUI, etc can go here) before any trials.  Should reset learned memory.
        public override void Initialize()
        {
            //RenameLearningTable("State", "Action", "QValue");
        }

        // Run once at the beginning of each trial for lookups and whatnot
        public override void Start()
        {

        }

        // Get an action given a state, taking into account whether it should be using best action or exploring
        public override QAction GetAction(QState currentState)
        {
            return null;
        }

        // Learn from the action taken to get from current to new state, taking into account learning and discount rates.  
        // Call UpdateLearningTable(QState/string, QAction/string, decimal value) with each value you want to update to the table.
        public override void Learn(QState prevState, QAction action, QState newState)
        {
        }

        // Call UpdateLearningTable for each value you want to show in the UI table.  PopulateTable is called after all trials are done or when HideOutput is toggled.
        public override void PopulateTable()
        {

        }


        // The below functions are called to open and save files holding what has been learned.  QLearner will handle the file write/read operations and receive or pass and general object to the QAlgo.  Make sure what you pass in Save is a serializable object.  If what you save includes QState, use the Open from the initialState to rebuild the state.
        public override void Open(object o, QState initialState)
        {

        }
        // If you need to store QStates, use the QState's Save() function to output a serializable object.  Note that it is up to the discretion of the person who programmed the QState to implement this and will be null if undefined.  Make sure to handle the null case
        public override object Save()
        {
            return null;
        }

        // ToString should output what the algo has learned in a human readable format if possible
        public override string ToString()
        {
            return "";
        }

    }
}
