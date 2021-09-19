using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;
using System.ComponentModel;

namespace QLearner.QAlgos
{
    /*
     * An enhanced version of Q-Learning I created that learns from both its own actions as well as observations of what it could have done and what its opponents are doing.  This is an iteration towards the more comprehensive AI I want to build that I outlined on my site:
     * http://www.pftq.com/blabberbox/?blogcat=writings;page=Creating_Sentient_Artificial_Intelligence
     * -pftq
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class QLearning_Observant:QLearning_Approximate
    {
        public override void Learn(QState prevState, QAction action, QState newState)
        {
            QUpdate(prevState, action, newState);
            foreach (KeyValuePair<QStateActionPair, QState> kv in newState.GetObservedStates(prevState, action))
            {
                QState observedPriorState = kv.Key.state;
                QAction observedAction = kv.Key.action;
                QState observedState = kv.Value;

                QUpdate(observedPriorState, observedAction, observedState);

                if (!HideOutput)
                {
                    decimal observedR = observedState.GetValue() - observedPriorState.GetValue();
                    WriteOutput("Observed: '" + observedAction + "' @ " + observedPriorState.ToString() + " | Gain " + Math.Round(observedR, 4));
                }
            }
        }
    }
}
