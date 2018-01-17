using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TicTacToe
{
    public partial class TTTForm : Form
    {
        public TTTForm()
        {
            InitializeComponent();
        }

        const string USER_SYMBOL = "X";
        const string COMPUTER_SYMBOL = "O";
        const string EMPTY = "";

        const int SIZE = 5;

        // constants for the 2 diagonals
        const int TOP_LEFT_TO_BOTTOM_RIGHT = 1;
        const int TOP_RIGHT_TO_BOTTOM_LEFT = 2;

        // constants for IsWinner
        const int NONE = -1;
        const int ROW = 1;
        const int COLUMN = 2;
        const int DIAGONAL = 3;

        //My variables
        int player = 1;
        Random rand = new Random();

        // This method takes a row and column as parameters and 
        // returns a reference to a label on the form in that position
        private Label GetSquare(int row, int column)
        {
            int labelNumber = row * SIZE + column + 1;
            return (Label)(this.Controls["label" + labelNumber.ToString()]);
        }

        // This method does the "reverse" process of GetSquare
        // It takes a label on the form as it's parameter and
        // returns the row and column of that square as output parameters
        private void GetRowAndColumn(Label l, out int row, out int column)
        {
            int position = int.Parse(l.Name.Substring(5));
            row = (position - 1) / SIZE;
            column = (position - 1) % SIZE;
        }

        // This method takes a row (in the range of 0 - 4) and returns true if 
        // the row on the form contains 5 Xs or 5 Os.
        // Use it as a model for writing IsColumnWinner
        private bool IsRowWinner(int row)
        {
            Label square = GetSquare(row, 0);
            string symbol = square.Text;
            for (int col = 1; col < SIZE; col++)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }

        //* TODO:  finish all of these that return true
        private bool IsAnyRowWinner()
        {
            /*int pTotal = 0;
            int cTotal = 0;
            Label testSquare;

            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    testSquare = GetSquare(i, j);

                    if (testSquare.Text == USER_SYMBOL)
                        pTotal++;
                    else if (testSquare.Text == COMPUTER_SYMBOL)
                        cTotal++;
                }

                if (pTotal >= 5 || cTotal >= 5)
                    return true;
                else
                {
                    pTotal = 0;
                    cTotal = 0;
                }
            }
            return false;*/

            for (int i = 0; i < SIZE; i++)
            {
                if (IsRowWinner(i))
                    return true;
            }
            return false;
        }

        private bool IsColumnWinner(int col)
        {
            Label testSquare = GetSquare(0, col);
            string symbol = testSquare.Text;

            for (int i = 0; i < SIZE; i++)
            {
                testSquare = GetSquare(i, col);

                if (symbol == EMPTY || testSquare.Text != symbol)
                    return false;
            }
            return true;
        }

        private bool IsAnyColumnWinner()
        {
            /*int pTotal = 0;
            int cTotal = 0;
            Label testSquare;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    testSquare = GetSquare(j, i);

                    if (testSquare.Text == USER_SYMBOL)
                        pTotal++;
                    else if (testSquare.Text == COMPUTER_SYMBOL)
                        cTotal++;
                }

                if (pTotal >= 5 || cTotal >= 5)
                    return true;
                else
                {
                    pTotal = 0;
                    cTotal = 0;
                }
            }
            return false;*/

            for(int i = 0; i < SIZE; i++)
            {
                if (IsColumnWinner(i))
                    return true;
            }
            return false;
        }

        private bool IsDiagonal2Winner()
        {
            Label square = GetSquare(0, (SIZE - 1));
            string symbol = square.Text;
            for (int row = 1, col = SIZE - 2; row < SIZE; row++, col--)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }

        private bool IsDiagonal1Winner()
        {
            Label square = GetSquare(0, 0);
            string symbol = square.Text;
            for (int row = 1, col = 1; row < SIZE; row++, col++)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }

        private bool IsAnyDiagonalWinner()
        {
            if (IsDiagonal1Winner())
                return true;
            else if (IsDiagonal2Winner())
                return true;
            else
                return false;

        }

        private bool IsFull()
        {
            for(int i = 0; i < SIZE; i++)
            {
                for(int j = 0; j < SIZE; j++)
                {
                    Label testSquare = GetSquare(i, j);
                    if (testSquare.Text == EMPTY)
                        return false;
                }
            }
            return true;
        }

        // This method determines if any row, column or diagonal on the board is a winner.
        // It returns true or false and the output parameters will contain appropriate values
        // when the method returns true.  See constant definitions at top of form.
        private bool IsWinner(out int whichDimension, out int whichOne)
        {
            // rows
            for (int row = 0; row < SIZE; row++)
            {
                if (IsRowWinner(row))
                {
                    whichDimension = ROW;
                    whichOne = row;
                    Debug.Print("Row");
                    return true;
                }
            }
            // columns
            for (int column = 0; column < SIZE; column++)
            {
                if (IsColumnWinner(column))
                {
                    whichDimension = COLUMN;
                    whichOne = column;
                    Debug.Print("Column");
                    return true;
                }
            }
            // diagonals
            if (IsDiagonal1Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_LEFT_TO_BOTTOM_RIGHT;
                Debug.Print("Diagonal1");
                return true;
            }
            if (IsDiagonal2Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_RIGHT_TO_BOTTOM_LEFT;
                Debug.Print("Diagonal2");
                return true;
            }
            whichDimension = NONE;
            whichOne = NONE;
            Debug.Print("No Win");
            return false;
        }

        // I wrote this method to show you how to call IsWinner
        private bool IsTie()
        {
            int winningDimension, winningValue;
            return (IsFull() && !IsWinner(out winningDimension, out winningValue));
        }

        // This method takes an integer in the range 0 - 4 that represents a column
        // as it's parameter and changes the font color of that cell to red.
        private void HighlightColumn(int col)
        {
            for (int row = 0; row < SIZE; row++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method changes the font color of the top right to bottom left diagonal to red
        // I did this diagonal because it's harder than the other one
        private void HighlightDiagonal2()
        {
            for (int row = 0, col = SIZE - 1; row < SIZE; row++, col--)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method will highlight either diagonal, depending on the parameter that you pass
        private void HighlightDiagonal(int whichDiagonal)
        {
            if (whichDiagonal == TOP_LEFT_TO_BOTTOM_RIGHT)
                HighlightDiagonal1();
            else
                HighlightDiagonal2();
        }

        //* TODO:  finish these 2
        private void HighlightRow(int row)
        {
            for (int col = 0; col < SIZE; col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        private void HighlightDiagonal1()
        {
            for (int row = 0, col = 0; row < SIZE; row++, col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        //* TODO:  finish this
        private void HighlightWinner(int player, int winningDimension, int winningValue)
        {
            switch (winningDimension)
            {
                case ROW:
                    HighlightRow(winningValue);
                    resultLabel.Text = ("Player " + player + " wins!");
                    break;
                case COLUMN:
                    HighlightColumn(winningValue);
                    resultLabel.Text = ("Player " + player + " wins!");
                    break;
                case DIAGONAL:
                    HighlightDiagonal(winningValue);
                    resultLabel.Text = ("Player " + player + " wins!");
                    break;
            }
        }

        //* TODO:  finish these 2
        private void ResetSquares()
        {
            //Found a different way to solve this problem "Application.Restart()"
        }

        private void MakeComputerMove()
        {
            Label compSquare = null;

            while(true)
            {
                int row = rand.Next(5);
                int col = rand.Next(5);
                compSquare = GetSquare(row, col);
                if (compSquare.Text != USER_SYMBOL && compSquare.Text != COMPUTER_SYMBOL)
                    break;
            }

            if (!IsFull())
            {
                compSquare.Text = COMPUTER_SYMBOL;
                DisableSquare(compSquare);
            }
        }

        // Setting the enabled property changes the look and feel of the cell.
        // Instead, this code removes the event handler from each square.
        // Use it when someone wins or the board is full to prevent clicking a square.
        private void DisableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    DisableSquare(square);
                }
            }
        }

        // Inside the click event handler you have a reference to the label that was clicked
        // Use this method (and pass that label as a parameter) to disable just that one square
        private void DisableSquare(Label square)
        {
            square.Click -= new System.EventHandler(this.label_Click);
        }

        // You'll need this method to allow the user to start a new game
        private void EnableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Click += new System.EventHandler(this.label_Click);
                }
            }
        }

        //* TODO:  finish the event handlers
        private void label_Click(object sender, EventArgs e)
        {
            
            int winningDimension = NONE;
            int winningValue = NONE;

            Label clickedLabel = (Label)sender;
            if(clickedLabel != null)
            {
               if(player == 1)
                {
                    clickedLabel.Text = USER_SYMBOL;
                    DisableSquare(clickedLabel);
                }
            }

            if (IsWinner(out winningDimension, out winningValue))
            {
                HighlightWinner(player, winningDimension, winningValue);
                DisableAllSquares();
                return;
            }
            else
                player = 2;

            if (!IsFull())
            {
                MakeComputerMove();
            }
            else
            {
                resultLabel.Text = ("It's a Tie. No one wins!");
                return;
            }

            if (IsWinner(out winningDimension, out winningValue))
            {
                HighlightWinner(player, winningDimension, winningValue);
                DisableAllSquares();
                return;
            }
            else
                player = 1;
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
