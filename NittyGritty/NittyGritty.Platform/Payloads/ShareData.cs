﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NittyGritty.Platform.Payloads
{
    public class ShareData : ObservableObject
    {

        private string _text;

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        private string _html;

        public string Html
        {
            get { return _html; }
            set { Set(ref _html, value); }
        }

        private string _rtf;

        public string Rtf
        {
            get { return _rtf; }
            set { Set(ref _rtf, value); }
        }

        private Uri _appLink;

        public Uri AppLink
        {
            get { return _appLink; }
            set { Set(ref _appLink, value); }
        }

        private Uri _webLink;

        public Uri WebLink
        {
            get { return _webLink; }
            set { Set(ref _webLink, value); }
        }

        private Stream _bitmap;

        public Stream Bitmap
        {
            get { return _bitmap; }
            set { Set(ref _bitmap, value); }
        }

        private IReadOnlyDictionary<string, object> _customData;

        public IReadOnlyDictionary<string, object> CustomData
        {
            get { return _customData; }
            set { Set(ref _customData, value); }
        }

        private IReadOnlyCollection<Stream> _files;

        public IReadOnlyCollection<Stream> Files
        {
            get { return _files; }
            set { Set(ref _files, value); }
        }

    }
}