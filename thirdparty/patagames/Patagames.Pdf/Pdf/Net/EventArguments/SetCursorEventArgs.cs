// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.SetCursorEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfForms.SetCursor" /> event
/// </summary>
public class SetCursorEventArgs : EventArgs
{
  /// <summary>Gets the cursor type</summary>
  public CursorTypes Cursor { get; private set; }

  /// <summary>Construct SetCursorEventArgs object</summary>
  /// <param name="cursor">The cursor type</param>
  public SetCursorEventArgs(CursorTypes cursor) => this.Cursor = cursor;
}
