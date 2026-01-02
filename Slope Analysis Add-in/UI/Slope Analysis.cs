using Autodesk.Revit.UI;
using Slope_Analysis_Add_in.Revit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Slope_Analysis_Add_in.UI
{
    public partial class Slope_Analysis : Form
    {
        public Slope_Analysis()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Analysis_Click(object sender, EventArgs e)
        {
            try
            {

                RvtData.start_range = double.Parse(tb_start_range.Text);
                RvtData.end_range = double.Parse(tb_end_range.Text);

                if (RvtData.start_range > RvtData.end_range)
                {
                    TaskDialog.Show("Slope Analysis", "(Start Range) cannot be greater than (End Range)");
                }
                else if (RvtData.start_range < 0 || RvtData.end_range < 0)
                {
                    TaskDialog.Show("Slope Analysis", "(Start Range) and (End Range) must be positive numbers");
                }
                else if (!int.TryParse(No_Selection.Text, out int selection) || selection == 0)
                {
                    TaskDialog.Show("Slope Analysis", "Please , You must select at least one floor to Analyse it");
                }
                else
                {
                    ExtCmd.ExtEventHan.Request = Request.Analysis;
                    ExtCmd.ExtEvent.Raise();
                }
            }
            catch
            {
                TaskDialog.Show("Slope Analysis", "You must type numbers only in Start and End Range");
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ExtCmd.ExtEventHan.Request = Request.Reset;
            ExtCmd.ExtEvent.Raise();
        }

        private void SelectFloors_Click(object sender, EventArgs e)
        {
            ExtCmd.ExtEventHan.Request = Request.SelectFloors;
            ExtCmd.ExtEvent.Raise();

        }
    }
}
