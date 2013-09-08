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
using ShareX.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UploadersLib;

namespace ShareX
{
    public partial class AfterUploadForm : Form
    {
        public TaskInfo Info { get; private set; }

        private UploadInfoParser parser = new UploadInfoParser();

        public AfterUploadForm(TaskInfo info)
        {
            InitializeComponent();
            Icon = Resources.ShareXIcon;
            Info = info;
            tmrClose.Start();

            bool isFileExist = !string.IsNullOrEmpty(info.FilePath) && File.Exists(info.FilePath);

            if (info.DataType == EDataType.Image)
            {
                if (isFileExist)
                {
                    pbPreview.LoadImageFromFile(info.FilePath);
                }
                else
                {
                    pbPreview.LoadImageFromURL(info.Result.URL);
                }
            }

            Text = "ShareX - " + (isFileExist ? info.FilePath : info.FileName);

            foreach (LinkFormatEnum type in Enum.GetValues(typeof(LinkFormatEnum)))
            {
                if (!Helpers.IsImageFile(Info.Result.URL) && type != LinkFormatEnum.URL && type != LinkFormatEnum.LocalFilePath && type != LinkFormatEnum.LocalFilePathUri)
                    continue;

                AddTreeNode(type.GetDescription(), GetUrlByType(type));
            }

            if (Helpers.IsImageFile(Info.Result.URL))
            {
                foreach (ClipboardFormat cf in Program.Settings.ClipboardContentFormats)
                {
                    AddTreeNode(cf.Description, parser.Parse(Info, cf.Format));
                }
            }

            tvMain.ExpandAll();
        }

        private void AddTreeNode(string description, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                TreeNode tnUrl = new TreeNode(description);
                tnUrl.Nodes.Add(text);
                tvMain.Nodes.Add(tnUrl);
            }
        }

        private void tvMain_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string text = null;

            if (e.Node != null)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    text = e.Node.FirstNode.Text;
                }
                else
                {
                    text = e.Node.Text;
                }
            }

            if (!string.IsNullOrEmpty(text))
            {
                ClipboardHelper.CopyText(text);
            }
        }

        private void tmrClose_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCopyImage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Info.FilePath) && Helpers.IsImageFile(Info.FilePath) && File.Exists(Info.FilePath))
            {
                ClipboardHelper.CopyImageFromFile(Info.FilePath);
            }
        }

        private void btnCopyLink_Click(object sender, EventArgs e)
        {
            string url = null;

            if (tvMain.SelectedNode != null)
            {
                url = tvMain.SelectedNode.Text;
            }
            else if (tvMain.Nodes.Count > 0 && tvMain.Nodes[0].Nodes.Count > 0)
            {
                url = tvMain.Nodes[0].Nodes[0].Text;
            }

            if (!string.IsNullOrEmpty(url))
            {
                ClipboardHelper.CopyText(url);
            }
        }

        private void btnOpenLink_Click(object sender, EventArgs e)
        {
            string url = Info.Result.URL;

            if (!string.IsNullOrEmpty(url))
            {
                Helpers.LoadBrowserAsync(url);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Info.FilePath) && File.Exists(Info.FilePath))
            {
                Helpers.LoadBrowserAsync(Info.FilePath);
            }
        }

        private void btnFolderOpen_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Info.FilePath) && File.Exists(Info.FilePath))
            {
                Helpers.OpenFolderWithFile(Info.FilePath);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region TaskInfo helper methods

        public string GetUrlByType(LinkFormatEnum type)
        {
            switch (type)
            {
                case LinkFormatEnum.URL:
                    return Info.Result.URL;
                case LinkFormatEnum.ShortenedURL:
                    return Info.Result.ShortenedURL;
                case LinkFormatEnum.ForumImage:
                    return parser.Parse(Info, UploadInfoParser.ForumImage);
                case LinkFormatEnum.HTMLImage:
                    return parser.Parse(Info, UploadInfoParser.HTMLImage);
                case LinkFormatEnum.WikiImage:
                    return parser.Parse(Info, UploadInfoParser.WikiImage);
                case LinkFormatEnum.ForumLinkedImage:
                    return parser.Parse(Info, UploadInfoParser.ForumLinkedImage);
                case LinkFormatEnum.HTMLLinkedImage:
                    return parser.Parse(Info, UploadInfoParser.HTMLLinkedImage);
                case LinkFormatEnum.WikiLinkedImage:
                    return parser.Parse(Info, UploadInfoParser.WikiLinkedImage);
                case LinkFormatEnum.ThumbnailURL:
                    return Info.Result.ThumbnailURL;
                case LinkFormatEnum.LocalFilePath:
                    return Info.FilePath;
                case LinkFormatEnum.LocalFilePathUri:
                    return GetLocalFilePathAsUri(Info.FilePath);
            }

            return Info.Result.URL;
        }

        public string GetLocalFilePathAsUri(string fp)
        {
            if (!string.IsNullOrEmpty(fp) && File.Exists(fp))
            {
                try
                {
                    return new Uri(fp).AbsoluteUri;
                }
                catch (Exception ex)
                {
                    DebugHelper.WriteException(ex);
                }
            }

            return string.Empty;
        }

        #endregion TaskInfo helper methods
    }
}