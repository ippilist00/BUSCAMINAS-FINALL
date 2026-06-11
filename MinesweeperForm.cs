using System;
using System.Drawing;
using System.Windows.Forms;

namespace Buscaminas02
{
    public class MinesweeperForm : Form
    {
        private int rows;
        private int cols;
        private int minesCount;
        private Button[,] buttons;
        private Cell[,] cells;

        private class Cell
        {
            public bool IsMine;
            public bool Revealed;
            public bool Flagged;
            public int AdjacentMines;
        }

        public MinesweeperForm(int rows, int cols, int mines)
        {
            this.rows = rows;
            this.cols = cols;
            this.minesCount = mines;
            this.Text = "Buscaminas";
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            this.ClientSize = new Size(cols * 28 + 20, rows * 28 + 20);
            buttons = new Button[rows, cols];
            cells = new Cell[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    cells[r, c] = new Cell();
                    var b = new Button();
                    b.Size = new Size(28, 28);
                    b.Location = new Point(10 + c * 28, 10 + r * 28);
                    b.Tag = new Point(r, c);
                    b.MouseUp += Cell_MouseUp;
                   
                    try
                    {
                        b.Font = new Font("Segoe UI Emoji", 12, FontStyle.Regular);
                    }
                    catch
                    {
                        b.Font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
                    }
                    this.Controls.Add(b);
                    buttons[r, c] = b;
                }
            }

            PlaceMines();
            CalculateAdjacents();
        }

        private void PlaceMines()
        {
            var rand = new Random();
            int placed = 0;
            while (placed < minesCount)
            {
                int r = rand.Next(rows);
                int c = rand.Next(cols);
                if (!cells[r, c].IsMine)
                {
                    cells[r, c].IsMine = true;
                    placed++;
                }
            }
        }

        private void CalculateAdjacents()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (cells[r, c].IsMine) continue;
                    int count = 0;
                    for (int dr = -1; dr <= 1; dr++)
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            int nr = r + dr, nc = c + dc;
                            if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
                                if (cells[nr, nc].IsMine) count++;
                        }
                    cells[r, c].AdjacentMines = count;
                }
            }
        }

        private void Cell_MouseUp(object sender, MouseEventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;
            var pt = (Point)b.Tag;
            int r = pt.X, c = pt.Y;
            if (e.Button == MouseButtons.Right)
            {
                ToggleFlag(r, c);
            }
            else if (e.Button == MouseButtons.Left)
            {
                RevealCell(r, c);
            }
        }

        private void ToggleFlag(int r, int c)
        {
            if (cells[r, c].Revealed) return;
            cells[r, c].Flagged = !cells[r, c].Flagged;
            buttons[r, c].Text = cells[r, c].Flagged ? "🚩" : "";
        }

        private void RevealCell(int r, int c)
        {
            if (cells[r, c].Revealed || cells[r, c].Flagged) return;
            cells[r, c].Revealed = true;
            buttons[r, c].Enabled = false;
            if (cells[r, c].IsMine)
            {
                buttons[r, c].BackColor = Color.Red;
                buttons[r, c].Text = "💣";
                GameOver(false);
                return;
            }

            int adj = cells[r, c].AdjacentMines;
            if (adj > 0)
            {
                buttons[r, c].Text = adj.ToString();
            }
            else
            {
             
                for (int dr = -1; dr <= 1; dr++)
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int nr = r + dr, nc = c + dc;
                        if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
                            if (!cells[nr, nc].Revealed)
                                RevealCell(nr, nc);
                    }
            }

            CheckWin();
        }

        private void GameOver(bool won)
        {
            
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (cells[r, c].IsMine)
                    {
                        buttons[r, c].Text = "💣";
                        buttons[r, c].BackColor = Color.LightGray;
                    }
                    if (cells[r, c].Flagged && !cells[r, c].IsMine)
                    {
                       
                        buttons[r, c].Text = "✖";
                    }
                    buttons[r, c].Enabled = false;
                }

            MessageBox.Show(won ? "¡Has ganado!" : "¡Has perdido!");
        }

        private void CheckWin()
        {
            int revealed = 0;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    if (cells[r, c].Revealed) revealed++;

            if (revealed == rows * cols - minesCount)
            {
                GameOver(true);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
          
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "MinesweeperForm";
            this.Load += new System.EventHandler(this.MinesweeperForm_Load);
            this.ResumeLayout(false);

        }

        private void MinesweeperForm_Load(object sender, EventArgs e)
        {

        }
    }
}
