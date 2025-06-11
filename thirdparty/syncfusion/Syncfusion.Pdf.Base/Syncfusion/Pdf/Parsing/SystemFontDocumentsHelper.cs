// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontDocumentsHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Reflection;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontDocumentsHelper
{
  public const char SpaceSymbol = ' ';
  public const char TabSymbol = '\t';
  public const char NewLine = '\n';
  public const char ZeroWidthSymbol = '\u200D';
  public const char LineHeightMeasureSymbol = 'X';

  public static Uri GetResourceUri(string resource)
  {
    return new Uri($"/{new AssemblyName(typeof (SystemFontDocumentsHelper).Assembly.FullName).Name};component/{resource}", UriKind.Relative);
  }

  public static Stream GetResourceStream(string resource)
  {
    return Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
  }

  public static bool IsLineBreak(char ch) => ch == '\n';

  public static bool IsTab(char ch) => ch == '\t';

  public static bool IsWhiteSpace(char ch) => ch == ' ';
}
