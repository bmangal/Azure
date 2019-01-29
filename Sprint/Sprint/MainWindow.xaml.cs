using SprintLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sprint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SprintCalculator _sprintCalc = new SprintCalculator(11);
        List<Issue> _issues = new List<Issue>();
        public MainWindow()
        {
            InitializeComponent();
            this.InitIssuesData();
        }

        private void InitIssuesData()
        {
            _issues.Add(new Issue() { Name = "Issue 1", Cost = 1, WinFactor = 2 });
            _issues.Add(new Issue() { Name = "Issue 2", Cost = 3, WinFactor = 3 });
            _issues.Add(new Issue() { Name = "Issue 3", Cost = 3, WinFactor = 3 });
            _issues.Add(new Issue() { Name = "Issue 4", Cost = 4, WinFactor = 7 });
            _issues.Add(new Issue() { Name = "Issue 5", Cost = 5, WinFactor = 10 });

            //this.dataGridIssues.Items.Clear();
            foreach (Issue issue in _issues)
            {
                this.dataGridIssues.Items.Add(issue);
            }

        }
       
        private void buttonAddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = this.textBoxName.Text;
                int cost = int.Parse(this.textBoxCost.Text);
                int winFactor = int.Parse(this.textBoxWinFactor.Text);


                Issue issue = new Issue() { Name = name, Cost = cost, WinFactor = winFactor };
                this._issues.Add(issue);
                this.dataGridIssues.Items.Add(issue);
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter correct input.");
            }
        }

        private void CalculateSprints()
        {
            this.dataGridIterations.Items.Clear();

            Dictionary<int, List<Issue>> iterations = this._sprintCalc.ArrangeIssues(this._issues);

            foreach (KeyValuePair<int, List<Issue>> iteration in iterations)
            {
                foreach (var issue in iteration.Value)
                {
                    this.dataGridIterations.Items.Add(new SprintIssue(iteration.Key + 1, issue));
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.CalculateSprints();
            this.dataGridIterations.Visibility = Visibility.Visible;
        }


    }
}
