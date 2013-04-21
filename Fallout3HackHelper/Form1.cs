using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fallout3HackHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtWords_TextChanged(object sender, EventArgs e)
        {
            var cursorPos = txtWords.SelectionStart;
            txtWords.Text = txtWords.Text.ToUpperInvariant();
            txtWords.SelectionStart = cursorPos;
            var lines = txtWords.Text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            _possibleWords.Clear();

            foreach (var line in lines) {
                _possibleWords.Add(new PossibleWord(line));
            }

            foreach (var possibleWord in _possibleWords) {
                foreach (var otherPossibleWord in _possibleWords) {
                    possibleWord.AddForMatch(otherPossibleWord);
                }
            }

            foreach (var possibleWord in _possibleWords) {
                possibleWord.Match();
            }

            lstPossibleWords.Items.Clear();
            lstPossibleWords.Items.AddRange(_possibleWords.ToArray());
        }

        private List<PossibleWord> _possibleWords = new List<PossibleWord>();
    }
}
