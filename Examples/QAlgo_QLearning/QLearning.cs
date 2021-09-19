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
     * Q-Learning is a basic state-based learning algorithm.  It essentially memorizes the consequences of every state, leading to its drawback of being too memory intensive for most applications.
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class QLearning:QAlgo
    {
        protected Dictionary<QStateActionPair, decimal> QValues;
        protected Random random;

        public override void Initialize()
        {
            QValues = new Dictionary<QStateActionPair, decimal>();
            random = new Random();
            if(HasGUI()) RenameLearningTable("State", "Action", "QValue");
        }

        public override QAction GetAction(QState currentState)
        {
            if (exploreRate > 0 && (decimal)random.NextDouble() <= exploreRate)
            {
                return GetRandomAction(currentState);
            }
            else
            {
                return GetBestAction(currentState);
            }
        }

        public override void Learn(QState prevState, QAction action, QState newState)
        {
            QUpdate(prevState, action, newState);
        }

        // Return the value of performing an action at given state or 0 if not done before
        protected virtual decimal GetQValue(QStateActionPair p)
        {
            if (!QValues.ContainsKey(p)) return 0;
            else return QValues[p];
        }

        // Return list of all possible states that can result from an action taken from the current state
        protected virtual List<QStateActionPair> GetOutcomes(QState state)
        {
            return state.GetActions().Select(x => new QStateActionPair(state, x)).ToList() ;
        }

        // Return the value of the best action taken at given state, else 0 if not known
        protected virtual decimal GetMaxValue(QState state)
        {
            return GetOutcomes(state).Select(x => GetQValue(x)).Max();
        }

        // Return best action to take from current state (best QValue or random if tied)
        protected virtual QAction GetBestAction(QState state)
        {
            decimal maxVal = GetMaxValue(state);
            IEnumerable<QAction> s = GetOutcomes(state).Where(x => GetQValue(x) == maxVal).Select(x => x.action);
            int n = s.Count();
            if (n == 1) return s.First();
            else
            {
                return s.ElementAt(random.Next(n));
            }
        }

        // Return a random action for exploration
        protected virtual QAction GetRandomAction(QState state)
        {
            QAction[] actions = state.GetActions();
            return actions.ElementAt(random.Next(actions.Length));
        }

        // Learn from recent action by comparing state values
        protected virtual void QUpdate(QState currentState, QAction action, QState newState)
        {
            decimal reward = newState.GetValue() - currentState.GetValue();
            QStateActionPair p = new QStateActionPair(currentState, action);
            decimal maxQ = GetMaxValue(newState);
            QValues[p] = (1 - learnRate) * GetQValue(p) + learnRate * (reward + discountRate * maxQ);
            if (HasGUI()) UpdateLearningTable(currentState, action, QValues[p]);
        }

        public override void Open(object o, QState initialState)
        {
            QValues.Clear();
            ClearLearningTable();

            foreach (KeyValuePair<object[], decimal> kv in ((Dictionary<object[], decimal>)o))
            {
                QValues.Add(new QStateActionPair(initialState.Open(kv.Key[0]), (QAction)kv.Key[1]), kv.Value);
            }

            if (HasGUI()) 
                foreach (KeyValuePair<QStateActionPair, decimal> kv in QValues)
                {
                    UpdateLearningTable(kv.Key.state, kv.Key.action, kv.Value);
                }
        }

        public override object Save()
        {
            Dictionary<object[], decimal> obj = new Dictionary<object[], decimal>();
            foreach (KeyValuePair<QStateActionPair, decimal> kv in QValues)
            {
                object state = kv.Key.state.Save();
                if (state == null) return null;
                obj.Add(new object[] { kv.Key.state.Save(), kv.Key.action }, kv.Value);
            }
            return obj;
        }

        public override string ToString()
        {
            return string.Join(", ", QValues.Select(x => x.Key + "=" + x.Value.ToString("N2")));
        }
    }
}
