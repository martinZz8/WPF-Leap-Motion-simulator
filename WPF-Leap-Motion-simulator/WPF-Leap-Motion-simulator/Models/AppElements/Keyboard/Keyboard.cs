using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Leap_Motion_simulator.Models
{
    abstract class Keyboard
    {
        //-- Fields --
        private double _positionX;
        private double _positionY;
        private double _width;
        private double _height;
        private KeyboardTypes _type;
        private bool _isVisible;
        private List<Key> _keys;

        //-- Properties --
        public double PositionX
        {
            get
            {
                return _positionX;
            }

            set
            {
                _positionX = value;
            }
        }

        public double PositionY
        {
            get
            {
                return _positionY;
            }

            set
            {
                _positionY = value;
            }
        }

        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
            }
        }

        public KeyboardTypes Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }

            set
            {
                _isVisible = value;
            }
        }

        public List<Key> Keys
        {
            get
            {
                return _keys;
            }

            set
            {
                _keys = value;
            }
        }

        public string GetLetterKeyboardVisibilityType
        {
            get
            {
                if (_type == KeyboardTypes.LETTER && _isVisible)
                {
                    return "Visible";
                }
                    
                return "Collapsed";
            }
        }

        public string GetNumericKeyboardVisibilityType
        {
            get
            {
                if (_type == KeyboardTypes.NUMERIC && _isVisible)
                {
                    return "Visible";
                }

                return "Collapsed";
            }
        }

        //-- Properties --
        public Key PropKeyQ
        {
            get
            {
                return GetKey(KeyTypes.LETTER_Q);
            }
        }

        public Key PropKeyW
        {
            get
            {
                return GetKey(KeyTypes.LETTER_W);
            }
        }

        public Key PropKeyE
        {
            get
            {
                return GetKey(KeyTypes.LETTER_E);
            }
        }

        public Key PropKeyR
        {
            get
            {
                return GetKey(KeyTypes.LETTER_R);
            }
        }

        public Key PropKeyT
        {
            get
            {
                return GetKey(KeyTypes.LETTER_T);
            }
        }

        public Key PropKeyY
        {
            get
            {
                return GetKey(KeyTypes.LETTER_Y);
            }
        }

        public Key PropKeyU
        {
            get
            {
                return GetKey(KeyTypes.LETTER_U);
            }
        }

        public Key PropKeyI
        {
            get
            {
                return GetKey(KeyTypes.LETTER_I);
            }
        }

        public Key PropKeyO
        {
            get
            {
                return GetKey(KeyTypes.LETTER_O);
            }
        }

        public Key PropKeyP
        {
            get
            {
                return GetKey(KeyTypes.LETTER_P);
            }
        }

        public Key PropKeyA
        {
            get
            {
                return GetKey(KeyTypes.LETTER_A);
            }
        }

        public Key PropKeyS
        {
            get
            {
                return GetKey(KeyTypes.LETTER_S);
            }
        }

        public Key PropKeyD
        {
            get
            {
                return GetKey(KeyTypes.LETTER_D);
            }
        }

        public Key PropKeyF
        {
            get
            {
                return GetKey(KeyTypes.LETTER_F);
            }
        }

        public Key PropKeyG
        {
            get
            {
                return GetKey(KeyTypes.LETTER_G);
            }
        }

        public Key PropKeyH
        {
            get
            {
                return GetKey(KeyTypes.LETTER_H);
            }
        }

        public Key PropKeyJ
        {
            get
            {
                return GetKey(KeyTypes.LETTER_J);
            }
        }

        public Key PropKeyK
        {
            get
            {
                return GetKey(KeyTypes.LETTER_K);
            }
        }

        public Key PropKeyL
        {
            get
            {
                return GetKey(KeyTypes.LETTER_L);
            }
        }

        public Key PropKeyZ
        {
            get
            {
                return GetKey(KeyTypes.LETTER_Z);
            }
        }

        public Key PropKeyX
        {
            get
            {
                return GetKey(KeyTypes.LETTER_X);
            }
        }

        public Key PropKeyC
        {
            get
            {
                return GetKey(KeyTypes.LETTER_C);
            }
        }

        public Key PropKeyV
        {
            get
            {
                return GetKey(KeyTypes.LETTER_V);
            }
        }

        public Key PropKeyB
        {
            get
            {
                return GetKey(KeyTypes.LETTER_B);
            }
        }

        public Key PropKeyN
        {
            get
            {
                return GetKey(KeyTypes.LETTER_N);
            }
        }

        public Key PropKeyM
        {
            get
            {
                return GetKey(KeyTypes.LETTER_M);
            }
        }

        public Key PropKey1
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_1);
            }
        }
        public Key PropKey2
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_2);
            }
        }
        public Key PropKey3
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_3);
            }
        }
        public Key PropKey4
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_4);
            }
        }
        public Key PropKey5
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_5);
            }
        }
        public Key PropKey6
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_6);
            }
        }
        public Key PropKey7
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_7);
            }
        }
        public Key PropKey8
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_8);
            }
        }
        public Key PropKey9
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_9);
            }
        }
        public Key PropKey0
        {
            get
            {
                return GetKey(KeyTypes.NUMBER_0);
            }
        }

        public Key PropKeyAt
        {
            get
            {
                return GetKey(KeyTypes.SPECIAL_AT);
            }
        }

        public Key PropKeySpace
        {
            get
            {
                return GetKey(KeyTypes.SPECIAL_SPACE);
            }
        }

        public Key PropKeyDot
        {
            get
            {
                return GetKey(KeyTypes.SPECIAL_DOT);
            }
        }

        public Key PropKeyDelete
        {
            get
            {
                return GetKey(KeyTypes.DELETE);
            }
        }

        //-- Methods --
        public Key GetKey(KeyTypes keyType)
        {
            return Keys.Find(key => key.Type == keyType);
        }
    }
}
