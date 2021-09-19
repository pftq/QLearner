using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using QLearner.Resources;

namespace QLearner.QStates
{
    public class Maze_With_No_Walls : QState
    {
        private Point self, goal;
        private int time, score=0;
        private int width = 5, height = 5, startx = 0, starty = 0, goalx = 4, goaly = 4;
        private volatile MazeGUI_Example maze;
        private ManualResetEvent wait;

        private readonly QAction UP = new QAction_String("up"), DOWN = new QAction_String("down"), LEFT = new QAction_String("left"), RIGHT = new QAction_String("right");

        public override QState Initialize()
        {
            WriteOutput("Dimensions: " + width + "x" + height, true);
            self = new Point(startx, starty);
            goal = new Point(goalx, goaly);
            time = score = 0;

            if (!HideOutput)
            {
                wait = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(CreateGUI), new object[] { width, height, self.X, self.Y, goal.X, goal.Y, this, wait });
                wait.WaitOne();
            }
            return new Maze_With_No_Walls() { width = width, height = height, startx = startx, starty = starty, goalx = goalx, goaly = goaly, maze = maze, goal = goal, self = self, score = score, time = time };
        }

        private void CreateGUI(object o)
        {
            int width = (int)((object[])o)[0];
            int height = (int)((object[])o)[1];
            int selfx = (int)((object[])o)[2];
            int selfy = (int)((object[])o)[3];
            int goalx = (int)((object[])o)[4];
            int goaly = (int)((object[])o)[5];
            Maze_With_No_Walls master = (Maze_With_No_Walls)((object[])o)[6];
            ManualResetEvent w = (ManualResetEvent)((object[])o)[7];
            master.maze = new MazeGUI_Example(width, height, selfx, selfy, goalx, goaly);
            w.Set();
            Application.Run(maze);
        }

        public override bool Equals(object obj)
        {
            return
                self == ((Maze_With_No_Walls)obj).self &&
                goal == ((Maze_With_No_Walls)obj).goal &&
                width == ((Maze_With_No_Walls)obj).width &&
                height == ((Maze_With_No_Walls)obj).height;
        }
        public override int GetHashCode()
        {
            return  (self.ToString()).GetHashCode() * 1000000 + goal.ToString().GetHashCode() * 10000 + width * 100 + height;
        }
        public override QState GetNewState(QAction action)
        {

            int newX = self.X, newY = self.Y;

            // Translate decision to new coordinate
            if (action == UP) newY--;
            else if (action == DOWN) newY++;
            else if (action == LEFT) newX--;
            else if (action == RIGHT) newX++;

            // Cannot go there
            if (newY < 0 || newX < 0 || newY >= height || newX >= width)
            {
                newX = self.X;
                newY = self.Y;
            }

            // Pass all variables to new state
            return new Maze_With_No_Walls() { maze = maze, self = new Point(newX, newY), goal = goal, goalx = goalx, goaly = goaly, time = time + 1, width = width, height = height, score = score};
        }
        public override QAction[] GetActions()
        {
            List<QAction> options = new List<QAction>();
            if (self.X > 0) options.Add(LEFT);
            if (self.X < width - 1 ) options.Add(RIGHT);
            if (self.Y > 0 ) options.Add(UP);
            if (self.Y < height - 1 ) options.Add(DOWN);
            return options.ToArray();
        }
        public override decimal GetValue()
        {
            return score;
        }
        public override bool IsEnd()
        {
            return self == goal;
        }
        public override string ToString()
        {
            return "Self: " + self;
        }
        public override void Step()
        {
            if (!HideOutput)
            {
                try
                {
                    maze.SetSelf(self.X, self.Y);
                    if (CurrentMode == AWAKEN) Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    WriteOutput("" + e, true);
                }
            }
            if (goal == self)
            {
                WriteOutput("Goal reached!");
                score += 100;
            }
            else score -= 1;
        }
        public override void End()
        {
            if (!HideOutput || maze != null)
            {
                try
                {
                    if (CurrentMode == AWAKEN) Thread.Sleep(500);
                    maze.Quit();
                }
                catch (Exception e)
                {
                    WriteOutput("" + e, true);
                }
            }
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
                {"Width", ""+width},
                {"Height", ""+height},
                {"Start-X", ""+startx},
                {"Start-Y", ""+starty},
                {"Goal-X", ""+goalx},
                {"Goal-Y", ""+goaly}
            };
            Dictionary<string, string> values = NewSettingsForm(settings);
            try
            {
                width = Convert.ToInt32(values["Width"]);
                height = Convert.ToInt32(values["Height"]);
                startx = Convert.ToInt32(values["Start-X"]);
                starty = Convert.ToInt32(values["Start-Y"]);
                goalx = Convert.ToInt32(values["Goal-X"]);
                goaly = Convert.ToInt32(values["Goal-Y"]);

                if (width < 5) width = 5;
                if (height < 5) height = 5;
                if (startx < 0 || startx >= width) startx = 0;
                if (starty < 0 || starty >= height) starty = 0;
                if (goalx < 0 || goalx >= width) goalx = 0;
                if (goaly < 0 || goaly >= height) goaly = 0;
            }
            catch (Exception)
            {
                WriteOutput("Invalid settings! Please only use integer numbers.");
            }
        }

        public override Dictionary<QFeature, decimal> GetFeatures(QAction action)
        {
            Point self = ((Maze_With_No_Walls)GetNewState(action)).self;
            QSearch qsearch = new QSearch(this);
            Maze_With_No_Walls simpleMaze = new Maze_With_No_Walls() { maze = maze, self = self, goal = goal, width = width, height = height };
            QSearchResult bestPath = qsearch.AStar(simpleMaze);

            return QFeature_String.FromStringDecimalDictionary(new Dictionary<string, decimal>() {
                //{this.GetHashCode().ToString()+"_"+action, 1}, // Identity to convert this back to QLearning
                {"Goal", goal==self? 1:0},
                {"Distance_To_Goal", bestPath==null? 1:(decimal) bestPath.Count / (decimal)(width * height)},
            });
        }

        public override decimal GetValueHeuristic()
        {
            // Manhatten distance
            return -1 * ((decimal)Math.Abs(self.X - goal.X) + Math.Abs(self.Y - goal.Y));
        }

        public override object Save()
        {
            return new object[] { self, goal, width, height };
        }

        public override QState Open(object o)
        {
            object[] oo = (object[])o;
            return new Maze_With_No_Walls() { self = (Point)oo[0], goal = (Point)oo[1], width = (int)oo[2], height = (int)oo[3] };
        }
    }
}
