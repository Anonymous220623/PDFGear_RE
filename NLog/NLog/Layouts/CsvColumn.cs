// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.CsvColumn
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;

#nullable disable
namespace NLog.Layouts;

[NLogConfigurationItem]
public class CsvColumn
{
  internal CsvQuotingMode? _quoting;

  public CsvColumn()
    : this((string) null, (Layout) null)
  {
  }

  public CsvColumn(string name, Layout layout)
  {
    this.Name = name;
    this.Layout = layout;
  }

  public string Name { get; set; }

  [RequiredParameter]
  public Layout Layout { get; set; }

  public CsvQuotingMode Quoting
  {
    get => this._quoting ?? CsvQuotingMode.Auto;
    set => this._quoting = new CsvQuotingMode?(value);
  }
}
