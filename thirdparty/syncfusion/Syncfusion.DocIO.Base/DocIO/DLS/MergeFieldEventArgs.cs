// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MergeFieldEventArgs
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class MergeFieldEventArgs : EventArgs
{
  private IWordDocument m_doc;
  private IWMergeField m_field;
  private object m_fieldValue;
  private int m_rowIndex;
  private string m_tableName;
  private string m_groupName;
  private WTextRange m_textRange;

  public IWordDocument Document => this.m_doc;

  public string FieldName => this.m_field.FieldName;

  public object FieldValue => this.m_fieldValue;

  public string TableName => this.m_tableName;

  public string GroupName => this.m_groupName;

  public int RowIndex => this.m_rowIndex;

  public WCharacterFormat CharacterFormat => this.m_textRange.CharacterFormat;

  public string Text
  {
    get
    {
      if (this.FieldValue == null)
        return "";
      string text = this.FieldValue.ToString();
      WMergeField currentMergeField = this.CurrentMergeField as WMergeField;
      try
      {
        text = currentMergeField.UpdateTextFormat(this.FieldValue.ToString());
        string numberFormat = currentMergeField.NumberFormat;
        if (numberFormat == string.Empty)
          currentMergeField.RemoveMergeFormat(currentMergeField.FieldCode, ref numberFormat);
        if (numberFormat != string.Empty)
          text = currentMergeField.UpdateNumberFormat(text, numberFormat);
        if (currentMergeField.DateFormat != string.Empty)
        {
          if (this.FieldValue is DateTime)
          {
            DateTime fieldValue = (DateTime) this.FieldValue;
            text = currentMergeField.UpdateDateField($"\\@ {currentMergeField.DateFormat}{currentMergeField.FormattingString}", fieldValue);
          }
          else
          {
            DateTime result;
            if (DateTime.TryParse(this.FieldValue.ToString(), out result))
            {
              text = currentMergeField.UpdateDateField($"\\@ {currentMergeField.DateFormat}{currentMergeField.FormattingString}", result);
              text = currentMergeField.UpdateTextFormat(text.ToString());
            }
          }
        }
      }
      catch
      {
      }
      return text;
    }
    set
    {
      this.m_fieldValue = (object) value;
      this.m_textRange.Text = this.Text;
    }
  }

  public IWMergeField CurrentMergeField => this.m_field;

  public WTextRange TextRange => this.m_textRange;

  public MergeFieldEventArgs(
    IWordDocument doc,
    string tableName,
    int rowIndex,
    IWMergeField field,
    object value)
  {
    this.m_doc = doc;
    this.m_field = field;
    this.m_fieldValue = value;
    this.m_rowIndex = rowIndex;
    this.m_tableName = tableName;
    this.m_textRange = new WTextRange(this.m_doc);
    this.m_textRange.Text = this.Text;
    List<WCharacterFormat> characterFormatting = (this.m_field as WField).GetResultCharacterFormatting();
    if (characterFormatting.Count <= 0)
      return;
    this.m_textRange.ApplyCharacterFormat(characterFormatting[0]);
  }

  internal MergeFieldEventArgs(
    IWordDocument doc,
    string tableName,
    int rowIndex,
    IWMergeField field,
    object value,
    string groupName)
  {
    this.m_doc = doc;
    this.m_field = field;
    this.m_fieldValue = value;
    this.m_rowIndex = rowIndex;
    this.m_tableName = tableName;
    this.m_groupName = groupName;
    this.m_textRange = new WTextRange(this.m_doc);
    this.m_textRange.Text = this.Text;
    List<WCharacterFormat> characterFormatting = (this.m_field as WField).GetResultCharacterFormatting();
    if (characterFormatting.Count <= 0)
      return;
    this.m_textRange.ApplyCharacterFormat(characterFormatting[0]);
  }
}
