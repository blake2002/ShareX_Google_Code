﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (C) 2012 ShareX Developers

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UploadersLib.HelperClasses;

namespace UploadersLib.TextUploaders
{
    public sealed class Paste_ee : TextUploader
    {
        public string APIKey { get; private set; }

        public Paste_ee()
        {
            APIKey = "public";
        }

        public Paste_ee(string apiKey)
        {
            APIKey = apiKey;
        }

        public override UploadResult UploadText(string text)
        {
            UploadResult ur = new UploadResult();

            if (!string.IsNullOrEmpty(text))
            {
                if (string.IsNullOrEmpty(APIKey))
                {
                    APIKey = "public";
                }

                Dictionary<string, string> arguments = new Dictionary<string, string>();
                arguments.Add("key", APIKey);
                arguments.Add("description", string.Empty);
                arguments.Add("paste", text);
                arguments.Add("format", "simple");
                arguments.Add("return", "link");

                ur.Source = SendPostRequest("http://paste.ee/api", arguments, ResponseType.Text);

                if (!string.IsNullOrEmpty(ur.Source) && ur.Source.StartsWith("error"))
                {
                    Errors.Add(ur.Source);
                }
                else
                {
                    ur.URL = ur.Source;
                }
            }

            return ur;
        }
    }
}