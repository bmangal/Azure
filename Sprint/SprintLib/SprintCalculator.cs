using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintLib
{
    public class SprintCalculator
    {
        private const int WINMULTIPLIER = 1;
        private const int COSTMULTIPLIER = 1;

        public SprintCalculator(int capacity)
        {
            this.SprintCostCapactiy = capacity;
        }

        private int _sprintCostCapacity = 11;
        public int SprintCostCapactiy
        {
            get { return this._sprintCostCapacity; }
            set
            {
                this._sprintCostCapacity = (value > 0) ? value : 0;
            }
        }

        public Dictionary<int, List<Issue>> ArrangeIssues(List<Issue> issuesToArrange)
        {
            Dictionary<int, List<Issue>> sprints = new Dictionary<int, List<Issue>>();

            // Keeping original collection intact
            List<Issue> issues = issuesToArrange.ToList();

            if (issues != null)
            {
                while (issues.Count > 0)
                {
                    List<Issue> issuesInNextSprint = this.GetIssuesInOneSprint(issues);
                    // Adding issues to next spring
                    sprints[sprints.Count] = issuesInNextSprint;
                    // From original issues list removing the issues already added to sprint.
                    issues.RemoveAll(o => issuesInNextSprint.Contains(o));
                }
            }

            return sprints;
        }

        private List<Issue> GetIssuesInOneSprint(List<Issue> issues)
        {
            List<Issue> selectedIssues = new List<Issue>();

            if (issues != null)
            {
                // We want to address the Issues with high Win Factor and low cost first
                var orderedIssues = issues.OrderByDescending(o => o.WinFactor - o.Cost);

                //Using Multiplier of 1 currently
                //var orderedIssues = issues.OrderByDescending(o => (o.WinFactor * WINMULTIPLIER) - (o.Cost * COSTMULTIPLIER));

                int totalCost = 0;
                foreach (Issue issue in orderedIssues)
                {
                    // Add issues within sprint capacity
                    if ( (issue.Cost + totalCost) <= this.SprintCostCapactiy)
                    {
                        selectedIssues.Add(issue);
                        totalCost += issue.Cost;
                    }

                    // Iteration is full, can not add any more issues
                    if (totalCost == this.SprintCostCapactiy)
                        break;
                }
            }

            return selectedIssues;
        }

    }
}
