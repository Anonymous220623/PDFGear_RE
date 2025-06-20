﻿// Decompiled with JetBrains decompiler
// Type: FileWatcher.KnownFolders
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace FileWatcher;

public static class KnownFolders
{
  private static readonly Dictionary<KnownFolder, Guid> _guids = new Dictionary<KnownFolder, Guid>()
  {
    [KnownFolder.Contacts] = new Guid("56784854-C6CB-462B-8169-88E350ACB882"),
    [KnownFolder.Downloads] = new Guid("374DE290-123F-4565-9164-39C4925E467B"),
    [KnownFolder.Favorites] = new Guid("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
    [KnownFolder.Links] = new Guid("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
    [KnownFolder.SavedGames] = new Guid("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
    [KnownFolder.SavedSearches] = new Guid("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
  };

  public static string GetPath(KnownFolder knownFolder)
  {
    return KnownFolders.SHGetKnownFolderPath(KnownFolders._guids[knownFolder], 0U, IntPtr.Zero);
  }

  [DllImport("shell32", CharSet = CharSet.Unicode, PreserveSig = false)]
  private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken);
}
