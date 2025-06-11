// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PathHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.IO;

#nullable disable
namespace NLog.Internal;

internal static class PathHelpers
{
  private static readonly char[] DirectorySeparatorChars = new char[2]
  {
    Path.DirectorySeparatorChar,
    Path.AltDirectorySeparatorChar
  };

  internal static string CombinePaths(string path, string dir, string file)
  {
    if (dir != null)
      path = Path.Combine(path, dir);
    if (file != null)
      path = Path.Combine(path, file);
    return path;
  }

  public static string TrimDirectorySeparators(string path)
  {
    string str = path?.TrimEnd(PathHelpers.DirectorySeparatorChars) ?? string.Empty;
    return str.EndsWith(":", StringComparison.Ordinal) ? path : str;
  }

  public static bool IsTempDir(string directory, string tempDir)
  {
    tempDir = PathHelpers.TrimDirectorySeparators(tempDir);
    if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(tempDir))
      return false;
    string fullPath = Path.GetFullPath(directory);
    return !string.IsNullOrEmpty(fullPath) && (fullPath.StartsWith(tempDir, StringComparison.OrdinalIgnoreCase) || tempDir.StartsWith("/tmp") && directory.StartsWith("/var/tmp/"));
  }
}
