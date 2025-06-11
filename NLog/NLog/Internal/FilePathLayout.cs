// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FilePathLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal class FilePathLayout : IRenderable
{
  private static readonly char[] DirectorySeparatorChars = new char[2]
  {
    Path.DirectorySeparatorChar,
    Path.AltDirectorySeparatorChar
  };
  private static readonly HashSet<char> InvalidFileNameChars = new HashSet<char>((IEnumerable<char>) Path.GetInvalidFileNameChars());
  private readonly Layout _layout;
  private readonly FilePathKind _filePathKind;
  private readonly string _baseDir;
  private readonly string _cleanedFixedResult;
  private readonly bool _cleanupInvalidChars;
  private string _cachedPrevRawFileName;
  private string _cachedPrevCleanFileName;

  public FilePathLayout(Layout layout, bool cleanupInvalidChars, FilePathKind filePathKind)
  {
    this._layout = layout;
    this._filePathKind = filePathKind;
    this._cleanupInvalidChars = cleanupInvalidChars;
    if (this._layout == null)
    {
      this._filePathKind = FilePathKind.Unknown;
    }
    else
    {
      if (cleanupInvalidChars || this._filePathKind == FilePathKind.Unknown)
      {
        this._cleanedFixedResult = FilePathLayout.CreateCleanedFixedResult(cleanupInvalidChars, layout);
        this._filePathKind = FilePathLayout.DetectKind(layout, this._filePathKind);
      }
      if (this._filePathKind != FilePathKind.Relative)
        return;
      this._baseDir = LogFactory.CurrentAppDomain.BaseDirectory;
    }
  }

  private static FilePathKind DetectKind(Layout layout, FilePathKind currentFilePathKind)
  {
    if (!(layout is SimpleLayout pathLayout))
      return FilePathKind.Unknown;
    return currentFilePathKind == FilePathKind.Unknown ? FilePathLayout.DetectFilePathKind(pathLayout) : currentFilePathKind;
  }

  private static string CreateCleanedFixedResult(bool cleanupInvalidChars, Layout layout)
  {
    if (!(layout is SimpleLayout simpleLayout) || !simpleLayout.IsFixedText)
      return (string) null;
    string filePath = simpleLayout.FixedText;
    if (cleanupInvalidChars)
      filePath = FilePathLayout.CleanupInvalidFilePath(filePath);
    return filePath;
  }

  public Layout GetLayout() => this._layout;

  private string GetRenderedFileName(LogEventInfo logEvent, StringBuilder reusableBuilder = null)
  {
    if (this._cleanedFixedResult != null)
      return this._cleanedFixedResult;
    if (this._layout == null)
      return (string) null;
    if (reusableBuilder == null)
      return this._layout.Render(logEvent);
    object obj;
    if ((!this._layout.ThreadAgnostic || this._layout.MutableUnsafe) && logEvent.TryGetCachedLayoutValue(this._layout, out obj))
      return obj?.ToString() ?? string.Empty;
    this._layout.RenderAppendBuilder(logEvent, reusableBuilder);
    if (this._cachedPrevRawFileName != null && reusableBuilder.EqualTo(this._cachedPrevRawFileName))
      return this._cachedPrevRawFileName;
    this._cachedPrevRawFileName = reusableBuilder.ToString();
    this._cachedPrevCleanFileName = (string) null;
    return this._cachedPrevRawFileName;
  }

  private string GetCleanFileName(string rawFileName)
  {
    string cleanFileName = rawFileName;
    if (this._cleanupInvalidChars && this._cleanedFixedResult == null)
      cleanFileName = FilePathLayout.CleanupInvalidFilePath(rawFileName);
    if (this._filePathKind == FilePathKind.Absolute)
      return cleanFileName;
    return this._filePathKind == FilePathKind.Relative && this._baseDir != null ? Path.Combine(this._baseDir, cleanFileName) : Path.GetFullPath(cleanFileName);
  }

  public string Render(LogEventInfo logEvent) => this.RenderWithBuilder(logEvent);

  internal string RenderWithBuilder(LogEventInfo logEvent, StringBuilder reusableBuilder = null)
  {
    string renderedFileName = this.GetRenderedFileName(logEvent, reusableBuilder);
    if (string.IsNullOrEmpty(renderedFileName) || (!this._cleanupInvalidChars || this._cleanedFixedResult != null) && this._filePathKind == FilePathKind.Absolute)
      return renderedFileName;
    if (string.Equals(this._cachedPrevRawFileName, renderedFileName, StringComparison.Ordinal) && this._cachedPrevCleanFileName != null)
      return this._cachedPrevCleanFileName;
    string cleanFileName = this.GetCleanFileName(renderedFileName);
    this._cachedPrevCleanFileName = cleanFileName;
    this._cachedPrevRawFileName = renderedFileName;
    return cleanFileName;
  }

  internal static FilePathKind DetectFilePathKind(Layout pathLayout)
  {
    return pathLayout is SimpleLayout pathLayout1 ? FilePathLayout.DetectFilePathKind(pathLayout1) : FilePathKind.Unknown;
  }

  private static FilePathKind DetectFilePathKind(SimpleLayout pathLayout)
  {
    bool isFixedText = pathLayout.IsFixedText;
    return FilePathLayout.DetectFilePathKind(isFixedText ? pathLayout.FixedText : pathLayout.Text, isFixedText);
  }

  internal static FilePathKind DetectFilePathKind(string path, bool isFixedText = true)
  {
    if (!string.IsNullOrEmpty(path))
    {
      path = path.TrimStart();
      int length = path.Length;
      if (length >= 1)
      {
        char firstChar = path[0];
        if (FilePathLayout.IsAbsoluteStartChar(firstChar))
          return FilePathKind.Absolute;
        if (firstChar == '.')
          return FilePathKind.Relative;
        if (length >= 2)
        {
          char ch = path[1];
          if ((int) Path.VolumeSeparatorChar != (int) Path.DirectorySeparatorChar && (int) ch == (int) Path.VolumeSeparatorChar)
            return FilePathKind.Absolute;
        }
        return FilePathLayout.IsLayoutRenderer(path, isFixedText) ? FilePathKind.Unknown : FilePathKind.Relative;
      }
    }
    return FilePathKind.Unknown;
  }

  private static bool IsLayoutRenderer(string path, bool isFixedText)
  {
    return !isFixedText && path.StartsWith("${", StringComparison.OrdinalIgnoreCase);
  }

  private static bool IsAbsoluteStartChar(char firstChar)
  {
    return (int) firstChar == (int) Path.DirectorySeparatorChar || (int) firstChar == (int) Path.AltDirectorySeparatorChar;
  }

  private static string CleanupInvalidFilePath(string filePath)
  {
    if (StringHelpers.IsNullOrWhiteSpace(filePath))
      return filePath;
    int num = filePath.LastIndexOfAny(FilePathLayout.DirectorySeparatorChars);
    char[] chArray = (char[]) null;
    for (int index = num + 1; index < filePath.Length; ++index)
    {
      if (FilePathLayout.InvalidFileNameChars.Contains(filePath[index]))
      {
        if (chArray == null)
          chArray = filePath.Substring(num + 1).ToCharArray();
        chArray[index - (num + 1)] = '_';
      }
    }
    return chArray != null ? Path.Combine(num > 0 ? filePath.Substring(0, num + 1) : string.Empty, new string(chArray)) : filePath;
  }
}
