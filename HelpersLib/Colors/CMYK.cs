﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (C) 2008-2013 ShareX Developers

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using System;
using System.Drawing;

namespace HelpersLib
{
    public struct CMYK
    {
        private double cyan;
        private double magenta;
        private double yellow;
        private double key;
        private int alpha;

        public double Cyan
        {
            get
            {
                return cyan;
            }
            set
            {
                cyan = ColorHelpers.CheckColor(value);
            }
        }

        public double Cyan100
        {
            get
            {
                return cyan * 100;
            }
            set
            {
                cyan = ColorHelpers.CheckColor(value / 100);
            }
        }

        public double Magenta
        {
            get
            {
                return magenta;
            }
            set
            {
                magenta = ColorHelpers.CheckColor(value);
            }
        }

        public double Magenta100
        {
            get
            {
                return magenta * 100;
            }
            set
            {
                magenta = ColorHelpers.CheckColor(value / 100);
            }
        }

        public double Yellow
        {
            get
            {
                return yellow;
            }
            set
            {
                yellow = ColorHelpers.CheckColor(value);
            }
        }

        public double Yellow100
        {
            get
            {
                return yellow * 100;
            }
            set
            {
                yellow = ColorHelpers.CheckColor(value / 100);
            }
        }

        public double Key
        {
            get
            {
                return key;
            }
            set
            {
                key = ColorHelpers.CheckColor(value);
            }
        }

        public double Key100
        {
            get
            {
                return key * 100;
            }
            set
            {
                key = ColorHelpers.CheckColor(value / 100);
            }
        }

        public int Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = ColorHelpers.CheckColor(value);
            }
        }

        public CMYK(double cyan, double magenta, double yellow, double key, int alpha = 255)
            : this()
        {
            Cyan = cyan;
            Magenta = magenta;
            Yellow = yellow;
            Key = key;
            Alpha = alpha;
        }

        public CMYK(int cyan, int magenta, int yellow, int key, int alpha = 255)
            : this()
        {
            Cyan100 = cyan;
            Magenta100 = magenta;
            Yellow100 = yellow;
            Key100 = key;
            Alpha = alpha;
        }

        public CMYK(Color color)
        {
            this = RGBA.ToCMYK(color);
        }

        public static implicit operator CMYK(Color color)
        {
            return RGBA.ToCMYK(color);
        }

        public static implicit operator Color(CMYK color)
        {
            return color.ToColor();
        }

        public static implicit operator RGBA(CMYK color)
        {
            return color.ToColor();
        }

        public static implicit operator HSB(CMYK color)
        {
            return color.ToColor();
        }

        public static bool operator ==(CMYK left, CMYK right)
        {
            return (left.Cyan == right.Cyan) && (left.Magenta == right.Magenta) && (left.Yellow == right.Yellow) && (left.Key == right.Key);
        }

        public static bool operator !=(CMYK left, CMYK right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return String.Format("Cyan: {0:0.0}%, Magenta: {1:0.0}%, Yellow: {2:0.0}%, Key: {3:0.0}%", Cyan100, Magenta100, Yellow100, Key100);
        }

        public static Color ToColor(CMYK cmyk)
        {
            return ColorHelpers.CMYKToColor(cmyk);
        }

        public static Color ToColor(double cyan, double magenta, double yellow, double key)
        {
            return ToColor(new CMYK(cyan, magenta, yellow, key));
        }

        public Color ToColor()
        {
            return ToColor(this);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}