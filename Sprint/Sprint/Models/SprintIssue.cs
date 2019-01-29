using SprintLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint
{
    class SprintIssue : Issue
    {
        public SprintIssue(int sprint, Issue issue) 
            : base (issue)
        {
            this.Sprint = sprint;
        }

        public int Sprint { get; set; }
    }
}
