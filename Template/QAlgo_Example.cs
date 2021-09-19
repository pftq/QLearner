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
         * All of the outlined functions below are required and must be defined.  Rename the "virtual" flag on each to "override".
         * When you are done coding, "Build" the solution in Visual Studio.  Your finished dll file will be in the bin/Release file.  You can now load this as a plugin file when running QLearner.
         */

        // This should instantiate everything needed before trials begin.  If called multiple times, it functions as a reset.
        public override void Initialize()
        {
        }

        // This runs a single trial/instance of the QState problem.  QLearner will automatically run many times for learning or once to apply what has been learned.
        // Must return the final state
        public override QState Run(QState currentState, int trialNum, decimal learn, decimal discount, decimal explore)
        {
            /* Should follow the following format:
             while (!currentState.IsEnd() && currentState.GetActions().Length > 0 && isRunning)
             {
                *Pick an action for the current state*
                ...
                QState newState = currentState.GetNewState(a);
                newState.Inherit(currentState);
                newState.Step();
                ...
                *Learn from the reward/cost of the decision*
                ...
                currentState = newState;
            }
             * */
            return currentState;
        }

        // This is called at the end of all trials and processes, usually as clean up or GUI output
        public override void End()
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

    }
}
