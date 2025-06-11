// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.BrowseFileEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for display File Browse Dialog from SDK
/// </summary>
public class BrowseFileEventArgs : EventArgs
{
  /// <summary>
  /// The application must set this property to a user-selected file
  /// </summary>
  public string FilePath { get; set; }
}
