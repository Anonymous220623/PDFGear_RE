// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.CsvLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[Layout("CsvLayout")]
[NLog.Config.ThreadAgnostic]
[NLog.Config.ThreadSafe]
[AppDomainFixedOutput]
public class CsvLayout : LayoutWithHeaderAndFooter
{
  private string _actualColumnDelimiter;
  private string _doubleQuoteChar;
  private char[] _quotableCharacters;

  public CsvLayout()
  {
    this.Columns = (IList<CsvColumn>) new List<CsvColumn>();
    this.WithHeader = true;
    this.Delimiter = CsvColumnDelimiterMode.Auto;
    this.Quoting = CsvQuotingMode.Auto;
    this.QuoteChar = "\"";
    this.Layout = (Layout) this;
    this.Header = (Layout) new CsvLayout.CsvHeaderLayout(this);
    this.Footer = (Layout) null;
  }

  [ArrayParameter(typeof (CsvColumn), "column")]
  public IList<CsvColumn> Columns { get; private set; }

  public bool WithHeader { get; set; }

  [DefaultValue("Auto")]
  public CsvColumnDelimiterMode Delimiter { get; set; }

  [DefaultValue("Auto")]
  public CsvQuotingMode Quoting { get; set; }

  [DefaultValue("\"")]
  public string QuoteChar { get; set; }

  public string CustomColumnDelimiter { get; set; }

  protected override void InitializeLayout()
  {
    if (!this.WithHeader)
      this.Header = (Layout) null;
    base.InitializeLayout();
    switch (this.Delimiter)
    {
      case CsvColumnDelimiterMode.Auto:
        this._actualColumnDelimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        break;
      case CsvColumnDelimiterMode.Comma:
        this._actualColumnDelimiter = ",";
        break;
      case CsvColumnDelimiterMode.Semicolon:
        this._actualColumnDelimiter = ";";
        break;
      case CsvColumnDelimiterMode.Tab:
        this._actualColumnDelimiter = "\t";
        break;
      case CsvColumnDelimiterMode.Pipe:
        this._actualColumnDelimiter = "|";
        break;
      case CsvColumnDelimiterMode.Space:
        this._actualColumnDelimiter = " ";
        break;
      case CsvColumnDelimiterMode.Custom:
        this._actualColumnDelimiter = this.CustomColumnDelimiter;
        break;
    }
    this._quotableCharacters = $"{this.QuoteChar}\r\n{this._actualColumnDelimiter}".ToCharArray();
    this._doubleQuoteChar = this.QuoteChar + this.QuoteChar;
  }

  internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
  {
    this.PrecalculateBuilderInternal(logEvent, target);
  }

  protected override string GetFormattedMessage(LogEventInfo logEvent)
  {
    return this.RenderAllocateBuilder(logEvent);
  }

  protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    for (int index = 0; index < this.Columns.Count; ++index)
    {
      Layout layout = this.Columns[index].Layout;
      this.RenderColumnLayout(logEvent, layout, (CsvQuotingMode) ((int) this.Columns[index]._quoting ?? (int) this.Quoting), target, index);
    }
  }

  private void RenderColumnLayout(
    LogEventInfo logEvent,
    Layout columnLayout,
    CsvQuotingMode quoting,
    StringBuilder target,
    int i)
  {
    if (i != 0)
      target.Append(this._actualColumnDelimiter);
    if (quoting == CsvQuotingMode.All)
      target.Append(this.QuoteChar);
    int length = target.Length;
    columnLayout.RenderAppendBuilder(logEvent, target);
    if (length != target.Length && this.ColumnValueRequiresQuotes(quoting, target, length))
    {
      string str = target.ToString(length, target.Length - length);
      target.Length = length;
      if (quoting != CsvQuotingMode.All)
        target.Append(this.QuoteChar);
      target.Append(str.Replace(this.QuoteChar, this._doubleQuoteChar));
      target.Append(this.QuoteChar);
    }
    else
    {
      if (quoting != CsvQuotingMode.All)
        return;
      target.Append(this.QuoteChar);
    }
  }

  private void RenderHeader(StringBuilder sb)
  {
    LogEventInfo nullEvent = LogEventInfo.CreateNullEvent();
    for (int index = 0; index < this.Columns.Count; ++index)
    {
      CsvColumn column = this.Columns[index];
      SimpleLayout columnLayout = new SimpleLayout(new LayoutRenderer[1]
      {
        (LayoutRenderer) new LiteralLayoutRenderer(column.Name)
      }, column.Name, ConfigurationItemFactory.Default);
      columnLayout.Initialize(this.LoggingConfiguration);
      this.RenderColumnLayout(nullEvent, (Layout) columnLayout, (CsvQuotingMode) ((int) column._quoting ?? (int) this.Quoting), sb, index);
    }
  }

  private bool ColumnValueRequiresQuotes(
    CsvQuotingMode quoting,
    StringBuilder sb,
    int startPosition)
  {
    switch (quoting)
    {
      case CsvQuotingMode.All:
        return this.QuoteChar.Length == 1 ? sb.IndexOf(this.QuoteChar[0], startPosition) >= 0 : sb.IndexOfAny(this._quotableCharacters, startPosition) >= 0;
      case CsvQuotingMode.Nothing:
        return false;
      default:
        return sb.IndexOfAny(this._quotableCharacters, startPosition) >= 0;
    }
  }

  public override string ToString()
  {
    return this.ToStringWithNestedItems<CsvColumn>(this.Columns, (Func<CsvColumn, string>) (c => c.Name));
  }

  [NLog.Config.ThreadAgnostic]
  [NLog.Config.ThreadSafe]
  [AppDomainFixedOutput]
  private class CsvHeaderLayout : Layout
  {
    private readonly CsvLayout _parent;
    private string _headerOutput;

    public CsvHeaderLayout(CsvLayout parent) => this._parent = parent;

    protected override void InitializeLayout()
    {
      this._headerOutput = (string) null;
      base.InitializeLayout();
    }

    private string GetHeaderOutput()
    {
      return this._headerOutput ?? (this._headerOutput = this.BuilderHeaderOutput());
    }

    private string BuilderHeaderOutput()
    {
      StringBuilder sb = new StringBuilder();
      this._parent.RenderHeader(sb);
      return sb.ToString();
    }

    internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
    {
    }

    protected override string GetFormattedMessage(LogEventInfo logEvent) => this.GetHeaderOutput();

    protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      target.Append(this.GetHeaderOutput());
    }
  }
}
