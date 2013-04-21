using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fallout3HackHelper
{
    public class PossibleWord
    {
        public PossibleWord(string line)
        {
            var entries = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (entries.Length == 1) {
                _word = entries[0].Trim();
            } else if (entries.Length >= 2) {
                _word = entries[0].Trim();
                setCharacterCount(entries[1]);
            }
        }
        public void AddForMatch(PossibleWord otherPossibleWord)
        {
            if (otherPossibleWord == this) { return; }
            _possibleWords.Add(otherPossibleWord);
        }

        public void Match()
        {
            if (HasCorrectCharacterCount == false) {
                addPossibilityPoint();
                return;
            }

            if (_correctCharacterCount != _word.Length) {
                this.notPossible();
            }

            foreach (var otherPossibleWord in _possibleWords) {
                int matchCount = 0;

                if (otherPossibleWord._word.Length < _word.Length) {
                    continue;
                }

                for (int i = 0; i < _word.Length; ++i) {
                    if (_word[i] == otherPossibleWord._word[i]) {
                        matchCount++;
                    }
                }

                if (matchCount == _correctCharacterCount) {
                    otherPossibleWord.addPossibilityPoint();
                } else {
                    otherPossibleWord.notPossible();
                }
            }
        }

        public override string ToString()
        {
            return _word + "\t" + calcOwnPossibilityPercentage() + "%";
        }

        private bool HasCorrectCharacterCount
        {
            get { return _correctCharacterCountKnown; }
        }

        private void addPossibilityPoint()
        {
            if (_isPossible == false) { return; }
            _possibilityPoints++;
        }

        private void removePossibilityPoint()
        {
            if (_isPossible == false) { return; }
            _possibilityPoints--;
            if (_possibilityPoints < 0) {
                _possibilityPoints = 0;
            }
        }

        private void notPossible()
        {
            _isPossible = false;
            _possibilityPoints = 0;
        }

        private float calcOwnPossibilityPercentage()
        {
            int maxPoints = 0;
            foreach (var possibleWord in _possibleWords) {
                maxPoints += possibleWord._possibilityPoints;
            }
            maxPoints += _possibilityPoints;
            if (maxPoints == 0) { return 0; }
            return (float)_possibilityPoints * 100 / (float)maxPoints;
        }

        private void setCharacterCount(string entry)
        {
            int count = 0;

            if (int.TryParse(entry, out count) == false) {
                _correctCharacterCountKnown = false;
                _correctCharacterCount = 0;
                return;
            }

            if (count > _word.Length) {
                count = _word.Length;
            }

            _correctCharacterCount = count;
            _correctCharacterCountKnown = true;
        }

        private bool _correctCharacterCountKnown = false;
        private int _correctCharacterCount = 0;
        private bool _isPossible = true;
        private string _word = string.Empty;
        private int _possibilityPoints = 0;
        private List<PossibleWord> _possibleWords = new List<PossibleWord>();
    }
}
