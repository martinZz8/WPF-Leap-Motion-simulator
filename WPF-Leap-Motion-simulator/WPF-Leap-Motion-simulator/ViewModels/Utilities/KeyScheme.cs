using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    class KeyScheme
    {
        public KeyTypes Type { get; set; }
        public string Value { get; set; }

        public static List<Key> CreateNumericKeyboardKeyScheme(
            TDOPosition keyboardPosition,
            double keyboardWidth,
            double keyboardHeight,
            double keyWidth,
            double keyHeight,
            double paddingBetween,
            double extraSpaceTop = 0
         ) {
            List<List<KeyScheme>> numericKeyScheme = new List<List<KeyScheme>>
            {
                new List<KeyScheme>
                {
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_1,
                        Value = "1"
                    },
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_2,
                        Value = "2"
                    },
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_3,
                        Value = "3"
                    }
                },
                new List<KeyScheme>
                {
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_4,
                        Value = "4"
                    },
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_5,
                        Value = "5"
                    },
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_6,
                        Value = "6"
                    }
                },
                new List<KeyScheme>
                {
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_7,
                        Value = "7"
                    },
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_8,
                        Value = "8"
                    },
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_9,
                        Value = "9"
                    }
                },
                new List<KeyScheme>
                {
                    new KeyScheme
                    {
                        Type = KeyTypes.NUMBER_0,
                        Value = "0"
                    },
                    new KeyScheme
                    {
                        Type = KeyTypes.DELETE,
                        Value = "DEL"
                    },
                }
            };

            return CalculateProperKeyScheme(
                numericKeyScheme,
                keyboardWidth,
                keyWidth,
                keyHeight,
                paddingBetween,
                extraSpaceTop
            );
        }

        public static List<Key> CreateLetterKeyboardKeyScheme(
            TDOPosition keyboardPosition,
            double keyboardWidth,
            double keyboardHeight,
            double keyWidth,
            double keyHeight,
            double paddingBetween,
            double extraSpaceTop = 0
        ) {
            List<List<KeyScheme>> letterKeyScheme = new List<List<KeyScheme>>
            {
                new List<KeyScheme>
                {
                    new KeyScheme
                    {
                        Type = KeyTypes.LETTER_Q,
                        Value = "Q"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_W,
                        Value = "W"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_E,
                        Value = "E"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_R,
                        Value = "R"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_T,
                        Value = "T"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_Y,
                        Value = "Y"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_U,
                        Value = "U"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_I,
                        Value = "I"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_O,
                        Value = "O"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_P,
                        Value = "P"
                    }
                },
                new List<KeyScheme>
                {
                    new KeyScheme {
                        Type = KeyTypes.LETTER_A,
                        Value = "A"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_S,
                        Value = "S"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_D,
                        Value = "D"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_F,
                        Value = "F"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_G,
                        Value = "G"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_H,
                        Value = "H"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_J,
                        Value = "J"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_K,
                        Value = "K"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_L,
                        Value = "L"
                    },
                    new KeyScheme {
                        Type = KeyTypes.SPECIAL_AT,
                        Value = "@"
                    }
                },
                new List<KeyScheme>
                {
                    new KeyScheme {
                        Type = KeyTypes.LETTER_Z,
                        Value = "Z"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_X,
                        Value = "X"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_C,
                        Value = "C"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_V,
                        Value = "V"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_B,
                        Value = "B"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_N,
                        Value = "N"
                    },
                    new KeyScheme {
                        Type = KeyTypes.LETTER_M,
                        Value = "M"
                    },
                    new KeyScheme {
                        Type = KeyTypes.SPECIAL_DOT,
                        Value = "."
                    },
                    new KeyScheme {
                        Type = KeyTypes.SPECIAL_SPACE,
                        Value = " "
                    },
                    new KeyScheme
                    {
                        Type = KeyTypes.DELETE,
                        Value = "DEL"
                    }
                }
            };

            return CalculateProperKeyScheme(
                letterKeyScheme,
                keyboardWidth,
                keyWidth,
                keyHeight,
                paddingBetween,
                extraSpaceTop
            );
        }

        //Calc the proper List of Keys - the proper scheme
        private static List<Key> CalculateProperKeyScheme (
            List<List<KeyScheme>> actualKeyScheme,
            double keyboardWidth,
            double keyWidth,
            double keyHeight,
            double paddingBetween,
            double extraSpaceTop
         ) {
            int outerLoopIndex = 0;
            List<Key> keysToRet = new List<Key>();

            foreach (List<KeyScheme> rowScheme in actualKeyScheme)
            {
                int numberOfItemsInRow = rowScheme.Count;
                double rowWidth = ((numberOfItemsInRow - 1) * paddingBetween) + (numberOfItemsInRow * keyWidth);
                double paddingTop = (keyHeight * outerLoopIndex) + (paddingBetween * outerLoopIndex) + extraSpaceTop;
                double basePaddingLeft = (keyboardWidth - rowWidth) / 2;

                int innerLoopIndex = 0;
                foreach (KeyScheme singleScheme in rowScheme)
                {
                    double paddingLeft = basePaddingLeft + (keyWidth * innerLoopIndex) + (paddingBetween * innerLoopIndex);

                    keysToRet.Add(new Key
                    {
                        Type = singleScheme.Type,
                        Value = singleScheme.Value,
                        Height = keyHeight,
                        Width = keyWidth,
                        PositionX = paddingLeft,
                        PositionY = paddingTop
                    });
                    innerLoopIndex++;
                }

                outerLoopIndex++;
            }

            return keysToRet;
        }
    }
}
