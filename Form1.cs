using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Buscaminas02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            int rows = 9, cols = 9, mines = 10;
            switch (difficultyComboBox.SelectedItem.ToString())
            {
                case "Fácil":
                    rows = cols = 9; mines = 10; break;
                case "Medio":
                    rows = cols = 16; mines = 40; break;
                case "Difícil":
                    rows = 16; cols = 30; mines = 99; break;
            }

            var gameForm = new MinesweeperForm(rows, cols, mines);
            gameForm.Show();
        }
    }
}
