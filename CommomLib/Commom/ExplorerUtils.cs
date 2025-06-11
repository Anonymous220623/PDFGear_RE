// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.ExplorerUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Commom;

public static class ExplorerUtils
{
  private static Func<ShellObject, IntPtr> shellObjectPIDLGetter;

  public static async Task<bool> ShowInExplorerAsync(
    this DirectoryInfo directoryInfo,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    return directoryInfo != null && await ExplorerUtils.OpenFolderAsync(directoryInfo.FullName, cancellationToken).ConfigureAwait(false);
  }

  public static async Task<bool> ShowInExplorerAsync(
    this DirectoryInfo directoryInfo,
    string[] selectedFiles,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    return directoryInfo != null && await ExplorerUtils.OpenFolderAsync(directoryInfo.FullName, selectedFiles, cancellationToken).ConfigureAwait(false);
  }

  public static async Task<bool> ShowInExplorerAsync(
    this FileInfo fileInfo,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (fileInfo == null)
      return false;
    return await ExplorerUtils.OpenFolderAsync(fileInfo.DirectoryName, new string[1]
    {
      fileInfo.FullName
    }, cancellationToken).ConfigureAwait(false);
  }

  public static async Task<bool> SelectItemInExplorerAsync(
    string path,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrEmpty(path))
      return false;
    path = path.Trim();
    FileSystemInfo fileInfoCore = ExplorerUtils.CreateFileInfoCore(path);
    if (fileInfoCore == null)
      return false;
    return await ExplorerUtils.OpenFolderAsync(ExplorerUtils.GetFileInfoParentPath(fileInfoCore), new string[1]
    {
      fileInfoCore.FullName
    }, cancellationToken).ConfigureAwait(false);
  }

  public static async Task<bool> OpenFolderAsync(
    string folderPath,
    CancellationToken cancellationToken)
  {
    return await ExplorerUtils.OpenFolderAsync(folderPath, (string[]) null, cancellationToken);
  }

  public static async Task<bool> OpenFolderAsync(
    string folderPath,
    string[] selectedItems,
    CancellationToken cancellationToken)
  {
    List<ShellObject> list = new List<ShellObject>();
    List<IntPtr> pidls = new List<IntPtr>();
    ShellFolder folder = (ShellFolder) null;
    try
    {
      folder = await ExplorerUtils.TryGetFolderAsync(folderPath, cancellationToken);
      if ((ShellObject) folder == (ShellObject) null)
        return false;
      IntPtr folderPidl = ExplorerUtils.GetShellObjectPIDL((ShellObject) folder);
      if (selectedItems != null && selectedItems.Length != 0)
      {
        string str1 = folderPath.ToLowerInvariant().Trim();
        if (!str1.EndsWith("\\"))
        {
          string str2 = str1 + "\\";
        }
        List<FileSystemInfo> list1 = ((IEnumerable<string>) selectedItems).Select<string, FileSystemInfo>((Func<string, FileSystemInfo>) (c => ExplorerUtils.TryCreateFileInfo(c, folderPath))).Where<FileSystemInfo>((Func<FileSystemInfo, bool>) (c => c != null)).ToList<FileSystemInfo>();
        if (list1.Count > 0)
        {
          foreach (FileSystemInfo fileInfo in list1)
          {
            ShellObject shellObjectAsync = await ExplorerUtils.ConvertToShellObjectAsync(fileInfo, cancellationToken);
            if (shellObjectAsync != (ShellObject) null)
            {
              list.Add(shellObjectAsync);
              pidls.Add(ExplorerUtils.GetShellObjectPIDL(shellObjectAsync));
            }
          }
        }
      }
      try
      {
        ExplorerUtils.SHOpenFolderAndSelectItems(folderPidl, pidls.ToArray(), 0);
        return true;
      }
      catch
      {
      }
      return false;
    }
    finally
    {
      if ((ShellObject) folder != (ShellObject) null)
      {
        try
        {
          folder?.Dispose();
        }
        catch
        {
        }
      }
      foreach (ShellObject shellObject in list)
      {
        try
        {
          shellObject?.Dispose();
        }
        catch
        {
        }
      }
    }
  }

  private static FileSystemInfo CreateFileInfoCore(string fileName)
  {
    try
    {
      FileInfo fileInfoCore = new FileInfo(fileName);
      if (fileInfoCore.Exists)
        return (FileSystemInfo) fileInfoCore;
    }
    catch
    {
    }
    try
    {
      DirectoryInfo fileInfoCore = new DirectoryInfo(fileName);
      if (fileInfoCore.Exists)
        return (FileSystemInfo) fileInfoCore;
    }
    catch
    {
    }
    return (FileSystemInfo) null;
  }

  private static string GetFileInfoParentPath(FileSystemInfo fileInfo)
  {
    switch (fileInfo)
    {
      case FileInfo fileInfo1:
        return fileInfo1.DirectoryName;
      case DirectoryInfo directoryInfo:
        return directoryInfo.Parent?.FullName;
      default:
        return (string) null;
    }
  }

  private static FileSystemInfo TryCreateFileInfo(string fileName, string folder)
  {
    folder = folder.Trim();
    string lowerInvariant = folder.ToLowerInvariant();
    FileSystemInfo fileInfoCore = ExplorerUtils.CreateFileInfoCore(fileName);
    if (fileInfoCore != null)
      return ExplorerUtils.GetFileInfoParentPath(fileInfoCore)?.ToLowerInvariant() == lowerInvariant ? fileInfoCore : (FileSystemInfo) null;
    FileInfo fileInfo = new FileInfo(Path.Combine(folder, fileName));
    if (fileInfo == null)
      return (FileSystemInfo) null;
    return ExplorerUtils.GetFileInfoParentPath((FileSystemInfo) fileInfo)?.ToLowerInvariant() == lowerInvariant ? (FileSystemInfo) fileInfo : (FileSystemInfo) null;
  }

  private static async Task<ShellObject> ConvertToShellObjectAsync(
    FileSystemInfo fileInfo,
    CancellationToken cancellationToken)
  {
    switch (fileInfo)
    {
      case FileInfo fileInfo1:
        return (ShellObject) await ExplorerUtils.TryGetFileAsync(fileInfo1.FullName, cancellationToken).ConfigureAwait(false);
      case DirectoryInfo directoryInfo:
        return (ShellObject) await ExplorerUtils.TryGetFolderAsync(directoryInfo.FullName, cancellationToken).ConfigureAwait(false);
      default:
        return (ShellObject) null;
    }
  }

  private static Task<ShellFolder> TryGetFolderAsync(
    string folderPath,
    CancellationToken cancellationToken)
  {
    if (cancellationToken.IsCancellationRequested)
      return Task.FromCanceled<ShellFolder>(cancellationToken);
    if (string.IsNullOrEmpty(folderPath))
      return Task.FromResult<ShellFolder>((ShellFolder) null);
    folderPath = folderPath.Trim();
    if (!Directory.Exists(folderPath))
      return Task.FromResult<ShellFolder>((ShellFolder) null);
    try
    {
      return Task.FromResult<ShellFolder>((ShellFolder) ShellFileSystemFolder.FromFolderPath(folderPath));
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    return Task.FromResult<ShellFolder>((ShellFolder) null);
  }

  private static Task<ShellFile> TryGetFileAsync(
    string filePath,
    CancellationToken cancellationToken)
  {
    if (cancellationToken.IsCancellationRequested)
      return Task.FromCanceled<ShellFile>(cancellationToken);
    if (string.IsNullOrEmpty(filePath))
      return Task.FromResult<ShellFile>((ShellFile) null);
    filePath = filePath.Trim();
    if (!File.Exists(filePath))
      return Task.FromResult<ShellFile>((ShellFile) null);
    try
    {
      return Task.FromResult<ShellFile>(ShellFile.FromFilePath(filePath));
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    return Task.FromResult<ShellFile>((ShellFile) null);
  }

  [DllImport("shell32.dll", EntryPoint = "SHOpenFolderAndSelectItems")]
  private static extern int SHOpenFolderAndSelectItemsNative(
    [In] IntPtr pidlFolder,
    uint cidl,
    [MarshalAs(UnmanagedType.LPArray), In, Optional] IntPtr[] apidl,
    int dwFlags);

  private static void SHOpenFolderAndSelectItems(IntPtr pidlFolder, IntPtr[] apidl, int dwFlags)
  {
    uint length = apidl != null ? (uint) apidl.Length : 0U;
    Marshal.ThrowExceptionForHR(ExplorerUtils.SHOpenFolderAndSelectItemsNative(pidlFolder, length, apidl, dwFlags));
  }

  private static IntPtr GetShellObjectPIDL(ShellObject shellObj)
  {
    if (shellObj == (ShellObject) null)
      return IntPtr.Zero;
    if (ExplorerUtils.shellObjectPIDLGetter == null)
    {
      Func<ShellObject, object> func = TypeHelper.CreateFieldOrPropertyGetter<ShellObject>("PIDL", BindingFlags.Instance | BindingFlags.NonPublic);
      if (func != null)
        ExplorerUtils.shellObjectPIDLGetter = (Func<ShellObject, IntPtr>) (c => (IntPtr) func(c));
    }
    if (ExplorerUtils.shellObjectPIDLGetter == null)
      ExplorerUtils.shellObjectPIDLGetter = (Func<ShellObject, IntPtr>) (_ => IntPtr.Zero);
    return ExplorerUtils.shellObjectPIDLGetter(shellObj);
  }
}
