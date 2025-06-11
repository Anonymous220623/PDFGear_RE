// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.MacroSheet
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class MacroSheet
{
  private bool m_value;
  private Stream m_preservedStream;
  private string m_sheetName;
  private WorksheetDataHolder holder;

  internal MacroSheet(string sheetName, bool value)
  {
    this.m_sheetName = sheetName;
    this.m_value = value;
  }

  internal bool Value => this.m_value;

  internal Stream PreservedStream
  {
    get => this.m_preservedStream;
    set => this.m_preservedStream = value;
  }

  internal string SheetName => this.m_sheetName;

  internal WorksheetDataHolder DataHolder
  {
    get => this.holder;
    set => this.holder = value;
  }
}
