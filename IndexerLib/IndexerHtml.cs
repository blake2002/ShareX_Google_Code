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

using HelpersLib;
using IndexerLib.Properties;
using System.IO;
using System.Text;

namespace IndexerLib
{
    public class IndexerHtml : Indexer
    {
        public override string Index(string folderPath, IndexerSettings config)
        {
            string index = base.Index(folderPath, config);

            StringBuilder sbIndexHtml = new StringBuilder();

            sbIndexHtml.AppendLine(Resources.doctype_xhtml);
            sbIndexHtml.AppendLine(HtmlHelper.GetCssStyle(config.CssFilePath));
            sbIndexHtml.AppendLine(HtmlHelper.Tag("title", "Index for " + Path.GetFileName(folderPath)));
            sbIndexHtml.AppendLine(HtmlHelper.EndTag("head"));
            sbIndexHtml.AppendLine(HtmlHelper.StartTag("body"));
            sbIndexHtml.AppendLine(index);
            sbIndexHtml.AppendLine(HtmlHelper.EndTag("body"));
            sbIndexHtml.AppendLine();
            sbIndexHtml.AppendLine(HtmlHelper.EndTag("html"));

            return sbIndexHtml.ToString().Trim();
        }

        protected override void IndexFolder(FolderInfo dir, int level)
        {
            sbIndex.AppendLine(GetFolderNameRow(dir, level));

            string divClass = level > 0 ? "FolderBorder" : "MainFolderBorder";
            sbIndex.AppendLine(HtmlHelper.StartTag("div", "", "class=\"" + divClass + "\""));

            if (dir.Files.Count > 0)
            {
                sbIndex.AppendLine(HtmlHelper.StartTag("ul"));

                foreach (FileInfo fi in dir.Files)
                {
                    sbIndex.AppendLine(GetFileNameRow(fi, level));
                }

                sbIndex.AppendLine(HtmlHelper.EndTag("ul"));
            }

            foreach (FolderInfo subdir in dir.Folders)
            {
                sbIndex.AppendLine();
                IndexFolder(subdir, level + 1);
            }

            sbIndex.AppendLine(HtmlHelper.EndTag("div"));
        }

        protected override string GetFolderNameRow(FolderInfo dir, int level)
        {
            int heading = (level + 1).Between(1, 6);

            string size = string.Empty;

            if (dir.Size > 0)
            {
                size = "  " + HtmlHelper.Tag("span", Helpers.ProperFileSize(dir.Size, "", true), "", "class=\"foldersize\"");
            }

            return HtmlHelper.StartTag("h" + heading) + Helpers.HtmlEncode(dir.FolderName) + size + HtmlHelper.EndTag("h" + heading);
        }

        protected override string GetFileNameRow(FileInfo fi, int level)
        {
            string size = " " + HtmlHelper.Tag("span", Helpers.ProperFileSize(fi.Length, "", true), "", "class=\"filesize\"");

            return HtmlHelper.StartTag("li") + Helpers.HtmlEncode(fi.Name) + size + HtmlHelper.EndTag("li");
        }
    }
}