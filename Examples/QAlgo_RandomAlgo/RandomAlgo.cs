using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;

namespace QLearner.QAlgos
{
    public class RandomAlgo:QAlgo
    {
        // This algo just makes random moves.  It won't even learn, but it'll give you an idea of how to create your own.

        private Random r;

        // This should instantiate everything needed before trials begin.  If called multiple times, it functions as a reset.
        public override void  Initialize()
        {
            r = new Random();
        }

        // This runs a single trial/instance of the QState problem.  QLearner will automatically run many times for learning or once to apply what has been learned.
        // Must return the final state
        public override QState Run(QState currentState, int trialNum, decimal learn, decimal discount, decimal explore)
        {
             while (!currentState.IsEnd() && currentState.GetActions().Length > 0 && isRunning)
             {
                 QAction action = currentState.GetActions().ElementAt(r.Next(currentState.GetActions().Length));
                QState newState = currentState.GetNewState(action);
                newState.Inherit(currentState);
                newState.Step();
                WriteOutput((CurrentMode == LEARN ? "Trial " + trialNum + ", " : "") + ": '" + action + "' @ " + currentState.ToString());
                currentState = newState;
            }
            return currentState;
        }

        // This is called at the end of all trials and processes, usually as clean up or GUI output
        public override void End()
        {
            WriteOutput("Done!");
        }

    }
}
