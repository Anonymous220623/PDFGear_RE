// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ChartRichTextString
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class ChartRichTextString : 
  CommonWrapper,
  IOfficeChartRichTextString,
  IParentApplication,
  IOptimizedUpdate
{
  private string m_text;
  protected WorkbookImpl m_book;
  private bool m_bIsReadOnly;
  private object m_parent;
  private ChartTextAreaImpl m_textArea;

  public ChartAlrunsRecord.TRuns[] FormattingRuns
  {
    get
    {
      return this.TextArea != null && this.TextArea.ChartAlRuns != null ? this.TextArea.ChartAlRuns.Runs : (ChartAlrunsRecord.TRuns[]) null;
    }
  }

  private ChartTextAreaImpl TextArea
  {
    get => this.m_textArea;
    set => this.m_textArea = value;
  }

  public string Text
  {
    get
    {
      if (this.Parent is ChartTextAreaImpl parent)
        this.m_text = parent.Text;
      return this.m_text;
    }
  }

  public object Parent => this.m_parent;

  public IApplication Application => this.m_book.Application;

  public ChartRichTextString(IApplication application, object parent)
  {
    this.m_parent = parent != null ? parent : throw new ArgumentNullException(nameof (parent));
    this.SetParents();
    if (!(this.m_parent is ChartTextAreaImpl))
      return;
    this.m_textArea = this.m_parent as ChartTextAreaImpl;
  }

  public ChartRichTextString(IApplication application, object parent, bool isReadOnly)
    : this(application, parent, isReadOnly, false)
  {
  }

  public ChartRichTextString(
    IApplication application,
    object parent,
    bool isReadOnly,
    bool bCreateText)
    : this(application, parent)
  {
    this.m_bIsReadOnly = isReadOnly;
    if (!bCreateText)
      return;
    this.m_text = (string) new TextWithFormat();
  }

  public ChartRichTextString(IApplication application, object parent, TextWithFormat text)
    : this(application, parent)
  {
    this.m_text = (string) text;
  }

  protected virtual void SetParents()
  {
    this.m_book = CommonObject.FindParent(this.m_parent, typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  public void SetFont(int iStartPos, int iEndPos, IOfficeFont font)
  {
    this.BeginUpdate();
    ushort fontIndex = (ushort) this.AddFont(font);
    this.TextArea = this.Parent as ChartTextAreaImpl;
    if (this.TextArea == null)
      throw new ArgumentNullException("textArea");
    if (this.TextArea.Text == null)
      throw new ArgumentNullException("Does not support rich-text for empty string");
    if (this.TextArea.Text != null && this.TextArea.Text.Length > 0 && this.TextArea.ChartAlRuns != null)
    {
      List<ChartAlrunsRecord.TRuns> trunsList = new List<ChartAlrunsRecord.TRuns>((IEnumerable<ChartAlrunsRecord.TRuns>) this.TextArea.ChartAlRuns.Runs);
      bool flag = false;
      for (int index = 0; index < trunsList.Count; ++index)
      {
        if ((int) trunsList[index].FirstCharIndex == iStartPos || flag && (int) trunsList[index].FirstCharIndex <= iEndPos)
        {
          trunsList[index].FontIndex = fontIndex;
          this.TextArea.ChartAlRuns.Runs = trunsList.ToArray();
          flag = true;
        }
      }
      if (!flag)
      {
        if (iStartPos > 0)
        {
          for (int firstChar = 0; firstChar < iStartPos; ++firstChar)
          {
            trunsList.Add(new ChartAlrunsRecord.TRuns((ushort) firstChar, (ushort) 0));
            this.TextArea.ChartAlRuns.Runs = trunsList.ToArray();
          }
        }
        for (int firstChar = iStartPos; firstChar <= iEndPos; ++firstChar)
        {
          trunsList.Add(new ChartAlrunsRecord.TRuns((ushort) firstChar, fontIndex));
          this.TextArea.ChartAlRuns.Runs = trunsList.ToArray();
        }
        for (int firstChar = iEndPos + 1; firstChar <= this.TextArea.Text.Length; ++firstChar)
        {
          trunsList.Add(new ChartAlrunsRecord.TRuns((ushort) firstChar, (ushort) 0));
          this.TextArea.ChartAlRuns.Runs = trunsList.ToArray();
        }
      }
      this.TextArea.m_chartText.IsAutoColor = false;
    }
    this.EndUpdate();
  }

  public IOfficeFont GetFont(ChartAlrunsRecord.TRuns tRuns)
  {
    FontsCollection innerFonts = this.m_book.InnerFonts;
    IOfficeFont font = (IOfficeFont) null;
    foreach (FontImpl fontImpl in (CollectionBase<FontImpl>) innerFonts)
    {
      if (fontImpl.Index == (int) tRuns.FontIndex)
        font = (IOfficeFont) fontImpl;
    }
    return font;
  }

  protected virtual int AddFont(IOfficeFont font)
  {
    return (this.m_book.InnerFonts.Add((IOfficeFont) ((IInternalFont) font).Font) as FontImpl).Index;
  }
}
