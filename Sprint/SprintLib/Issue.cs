using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintLib
{
    public class Issue
    {
        public Issue()
        {

        }

        public Issue(Issue source)
        {
            this.Name = source.Name;
            this.Cost = source.Cost;
            this.WinFactor = source.WinFactor;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value ?? string.Empty;
            }
        }

        private int _cost;
        public int Cost
        {
            get { return this._cost; }
            set
            {
                this._cost = (value > 0) ? value : 0;
            }
        }

        private int _winFactor;
        public int WinFactor
        {
            get { return this._winFactor; }
            set
            {
                this._winFactor = (value > 0) ? value : 0;
            }
        }

        public override string ToString()
        {
            return string.Format($"{Name}: Cost = {Cost}, Win Factor = {WinFactor}");
        }
    }
}
