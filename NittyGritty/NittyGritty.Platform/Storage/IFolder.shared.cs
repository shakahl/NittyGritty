﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NittyGritty.Platform.Storage
{
    public interface IFolder : IStorageItem
    {
        Task<IFile> GetFile(string name);
        Task<IFolder> GetFolder(string name);
    }
}