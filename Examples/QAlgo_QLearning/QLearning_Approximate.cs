using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearner.QStates;

namespace QLearner.QAlgos
{
    /*
     * Approximate Q-Learning is an attempt to solve the slow-learning and memory-heavy aspect of Q-Learning by generalizing states seen to a handful of features (or characteristics).  New states can then be estimated based on similarity in feature set.
     * This particular approach of mine to the approximation allows for estimation of the features that would result from an action.  It adds another layer of uncertainty, but in some cases, it is not possible to know the exact outcomes of ones actions.
     * */
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = false)]
    public class QLearning_Approximate:QLearning
    {
        protected Dictionary<QFeature, decimal> QWeights;
        private Dictionary<QStateActionPair, Dictionary<QFeature, decimal>> featureCache;

        public override void Initialize()
        {
            QWeights = new Dictionary<QFeature, decimal>();
            random = new Random();
            featureCache = new Dictionary<QStateActionPair, Dictionary<QFeature, decimal>>();
            if (HasGUI()) RenameLearningTable("Feature", "Weight", "Last");
        }

        // Return the value of performing an action based on the features expected from the action.
        protected override decimal GetQValue(QStateActionPair p)
        {
            decimal qv = 0;

            Dictionary<QFeature, decimal> features;
            if (featureCache.ContainsKey(p)) features = featureCache[p];
            else
            {
                features = p.state.GetFeatures(p.action);
                featureCache[p] = features;
            }
            foreach (KeyValuePair<QFeature, decimal> feature in features)
            {
                if (!QWeights.ContainsKey(feature.Key)) QWeights[feature.Key] = 0;
                qv += QWeights[feature.Key] * feature.Value;
            }

            return qv;
        }

        // Update the weights of each feature based on their contribution to the reward
        protected override void QUpdate(QState currentState, QAction action, QState newState)
        {
            decimal maxQ = GetMaxValue(newState);
            QStateActionPair p = new QStateActionPair(currentState, action);
            Dictionary<QFeature, decimal> features;
            if (featureCache.ContainsKey(p)) features = featureCache[p];
            else
            {
                features = currentState.GetFeatures(action);
                featureCache[p] = features;
            }
            decimal currentQ = GetQValue(p);
            decimal reward = newState.GetValue() - currentState.GetValue();
            decimal difference = reward + discountRate * maxQ - currentQ;
            foreach (KeyValuePair<QFeature, decimal> feature in features)
            {
                try
                {
                    if (!QWeights.ContainsKey(feature.Key)) QWeights[feature.Key] = 0;
                    decimal oldWeight = QWeights[feature.Key];
                    decimal newWeight = oldWeight + learnRate * difference * feature.Value;
                    if (Math.Abs(newWeight) <= 1000000)
                    {
                        QWeights[feature.Key] = newWeight;
                    }
                    else WriteOutput("Warning: Weights diverging. Check that your features are valid and measured consistently with everything else.", true);
                    //WriteOutput("- "+feature.Key+": "+oldWeight+" => Reward "+reward+", MaxQ "+maxQ+", OldQ "+currentQ+", FValue "+feature.Value+" => "+newWeight, true);
                }
                catch (Exception e)
                {
                    WriteOutput("Exception: " + e + "\n*Check that your features are valid and measured consistently with everything else.*", true);
                    Abort();
                    break;
                }
            }
            
            // Output
            if (HasGUI())
            {
                foreach (QFeature f in features.Keys)
                {
                    UpdateLearningTable(f.ToString(), QWeights[f].ToString(), features[f]);
                }
            }
        }

        public override void Open(object o, QState initialState)
        {
            QWeights = (Dictionary<QFeature, decimal>)o;
            ClearLearningTable();

            if (HasGUI()) 
                foreach (KeyValuePair<QFeature, decimal> kv in QWeights)
                    UpdateLearningTable(kv.Key.ToString(), kv.Value.ToString(), 0);
        }

        public override object Save()
        {
            return QWeights;
        }

        public override string ToString()
        {
            return string.Join(", ", QWeights.Select(x => x.Key + "=" + x.Value.ToString("N2")));
        }
    }
}
