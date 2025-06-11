// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FX_RECT
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Rectangle area in device coordination system.</summary>
[StructLayout(LayoutKind.Sequential)]
public class FX_RECT
{
  /// <summary>The x-coordinate of the left-top corner.</summary>
  public int left;
  /// <summary>The y-coordinate of the left-top corner.</summary>
  public int top;
  /// <summary>The x-coordinate of the right-bottom corner.</summary>
  public int right;
  /// <summary>The y-coordinate of the right-bottom corner.</summary>
  public int bottom;

  /// <summary>Gets the width of current rectangle</summary>
  public int Width => this.right - this.left;

  /// <summary>Gets the height of current rectangle</summary>
  public int Height => this.top - this.bottom;
}
