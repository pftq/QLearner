using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearner.QStates
{
    public class QState_Example:QState
    {
        /* QState class structure
         * 
         * This file contains the general structure of a QState class.
         * To get started, rename this file and change the class declaration from 'public abstract class...' to 'public class <yourPluginName>:QState'.
         * All of the outlined functions below are required and must be defined.  Rename the "override" flag on each to "override".
         * When you are done coding, "Build" the solution in Visual Studio.  Your finished dll file will be in the bin/Release file.  You can now load this as a plugin file when running QLearner.
         */

        // Run once all time.  Instantiate variables and other one-time setups (Renaming tables, GUI, etc can go here) before any trials.
        public override void Initialize()
        {
        }

        // The first thing called from this class when trial begins.  It should return a new instance of your object with all parameters/variables initialized to create the initial/starting state.  Make sure to actually create a stand-alone instance not just pass references.
        public override QState Start()
        {
            return this;
        }

        // Displayable text to identify the state to the user
        public override string ToString()
        {
            return GetHashCode().ToString();
        }

        // Returns true if two QStates are equal.  Usually check all important variables defining your state here
        public override bool Equals(object obj)
        {
            return false;
        }

        // Returns an ideally id for hashing.  Should be fast to compute
        public override int GetHashCode()
        {
            return 0;
        }

        // Returns the worth of the current state, used to gauge progress
        // Note this is not the incremental gain between prior and current state but the total value up to this point.
        // Tip: Try to make it represent the cumulative value across time (like a score) as opposed to a discrete state value.
        public override decimal GetValue()
        {
            return 0;
        }

        // Heuristics to estimate the value of this state, if applicable (usually for pathfinding)
        // Should always underestimate the value (aka overestimate the cost from pathfinding perspective).
        public override decimal GetValueHeuristic()
        {
            return 0;
        }

        // Returns list of possible choices given the current state.
        public override QAction[] GetActions()
        {
            return new QAction[] { };
        }

        // Returns the new state you get to from here if you take a certain action
        // This should only reflect the new action taken (independent variables update) and the most immediate unconditional effects of the independent action (no adversary, random environmental change, gui updates, etc). It should be quick to calculate, so that algorithms can use this for calculating estimates of solutions (with environment unchanging).  See the next method for applying the changes by environment/adversaries and gui updates.
        // Make sure to actually return a new instance with deep-copied properties, as opposed to just passing references.
        public override QState GetNewState(QAction action)
        {
            return null;
        }

        // Called once per state/increment after GetNewState(). This should be where environment/adversaries change (dependent variables) or gui updates.  This can be slower and more thorough to calculate.  It is excluded when calculating estimates of solutions by algorithms.
        public override void Step() { }

        // Returns the other observable QStates to learn from, such as moves it could have done or what opponents did
        public override Dictionary<QStateActionPair, QState> GetObservedStates(QState prevState, QAction action)
        {
            return new Dictionary<QStateActionPair, QState>() { };
        }

        // Returns true when we reached the end point (no further actions to take)
        // There must be an end for Learning process to finish.
        // An ending can be omitted (always return false) for AWAKE mode so that QLearner is constantly solving and returning new actions back to this QState.
        public override bool IsEnd()
        {
            return true;
        }
        // Called when the trial ends
        public override void End() { }

        // Features - aka characteristics - associated with a decision (state-action) and their values.  Used to determine similarity between different state-action pairs.  These should be estimated from a prior state.
        // Leave empty to not use and QLearner will judge each state independently with Equals.
        // Tip: Feature values should be defined as 1 or 0 (on/off) or try to normalize the value out of 1 as opposed to using infinite domain.
        public override Dictionary<QFeature, decimal> GetFeatures(QAction action)
        {
            { return new Dictionary<QFeature, decimal>() { { new QFeature_StateAction(new QStateActionPair(this, action)), 1 }}; }
        }

        // Called when the user clicks on the Settings button with your plugin selected.
        // You can have this method call a new Form to create a Settings GUI as well.  An easy-to-use NewSettingsForm method is included in the API if you want to create something quick and simple.
        // If you have no settings, set the hasSettings flag to false to disable the button.
        public override bool HasSettings { get { return false; } }
        public override void Settings()
        {
            /* Example
            Dictionary<string, string> settings = new Dictionary<string, string>(){
                {"Setting1", "Value of Setting 1"},
                {"Setting2", "Value of Setting 2"}
            };
            Dictionary<string, string> newSettingsFromUser = NewSettingsForm(settings);
            */
        }


        // The below functions must be implemented before anything works.  They are called to open and save QStates as serialized objects.  Identify all core variables needed to reconstruct any unique QState instance and save it as a serializable object.  An easy object to use is the object[] type (new object[]{variable1, variable2,...}).  It is usually enough to save the variables you use in the Equals() method.  Make sure you include all settings variables as well.  The below methods should be able to reconstruct a QState exactly as it was saved.  Unlike QAlgo's Open method, this Open method should return a new object.  If you don't implement this, the user will not be able to save learned data on algorithms that reference your QState.
        public override QState Open(object o)
        {
            return this;
        }
        public override object Save()
        {
            return null;
        }
    }
}
