using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sac1Task2
{
    public partial class Form1 : Form
    {
        //get the path of Task2_Shop_Data.csv no matter the directory of the actual VS repository
        string path = Path.Combine(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 9) + "Task2_Shop_Data.csv");

        List<string> lines = new List<string>();

        float totalProfit;

        OpenFileDialog openFileDialog = new OpenFileDialog();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalcProfit_Click(object sender, EventArgs e)
        {
            //resets the table and total profit value
            dgvTable.Rows.Clear();
            totalProfit = 0;

            //paramaters making it so by default its set to the Task2_Shop_Data.csv and only accepts .csv and .txt files
            openFileDialog.Filter = " CSV | *.csv| Text File | *.txt";
            openFileDialog.InitialDirectory = Path.GetDirectoryName(path);
            openFileDialog.FileName = "Task2_Shop_Data.csv";

            //opens file dialog and checks if OK is pressed
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;

                //reads all lines from selected file
                lines = File.ReadAllLines(path).ToList();

                //removes the the first line if it contains header words
                if (lines[0].Contains("Sale price")) { lines.RemoveAt(0); }

                //loops through the lines adding them to items
                foreach (string line in lines)
                {
                    List<string> items = line.Split(',').ToList();
                    //adds the profits value to the end of items
                    items.Add(calc_profit(items[4], items[5]));

                    dgvTable.Rows.Add(items.ToArray());
                }

                //adds the little text in the bottom corner saying "Total Profit:" totalProfit
                dgvTable.Rows[dgvTable.Rows.Count - 1].Cells[5].Value = "Total Profit:";
                dgvTable.Rows[dgvTable.Rows.Count - 1].Cells[6].Value = totalProfit;
            }
        }
        //calculates and returns the profit, and adds to the total profit.
        private string calc_profit(string pp, string sp)
        {
            float fpp;
            float fsp;
            float returnVal;
            
            //if both puchasePrice and salePrice are actual numbers it calculates the profit
            if(float.TryParse(pp, out fpp) && float.TryParse(sp, out fsp))
            {
                returnVal = fsp - fpp;
            }
            //otherwise if the sale price is NA or invalid it returns a negtive puchase price as the profit
            else if(float.TryParse(pp, out fpp) && float.TryParse(sp, out fsp) != true)
            {
                returnVal = fpp * -1;
            }
            //other otherwise if both values are invalid it returns 0 as profit
            else
            {
                returnVal = 0f;
            }

            totalProfit += returnVal;
            return returnVal.ToString(); 
        }

    }
}
