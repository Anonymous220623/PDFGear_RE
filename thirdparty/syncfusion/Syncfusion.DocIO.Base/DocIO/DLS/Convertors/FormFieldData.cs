// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.FormFieldData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal class FormFieldData
{
  private string m_name;
  private string m_helpText;
  private string m_statusHelpText;
  private FormFieldType m_formFieldType;
  private bool m_bCalculateOnExit;
  private string m_marcoOnStart;
  private string m_marcoOnEnd;
  private bool m_enabled = true;
  private int m_checkboxSize;
  private CheckBoxSizeType m_checkboxSizeType;
  private string m_defaultText;
  private string m_stringFormat;
  private int m_maxLength;
  internal WDropDownCollection DropDownItems;
  private bool m_bIsListBox;
  internal int Ffres;
  internal int Ffdefres;
  internal bool m_bIsChecked;

  internal bool IsChecked
  {
    get => this.m_bIsChecked;
    set => this.m_bIsChecked = value;
  }

  internal bool IsListBox
  {
    get => this.m_bIsListBox;
    set => this.m_bIsListBox = value;
  }

  internal int MaxLength
  {
    get => this.m_maxLength;
    set => this.m_maxLength = value;
  }

  internal string StringFormat
  {
    get => this.m_stringFormat;
    set => this.m_stringFormat = value;
  }

  internal string DefaultText
  {
    get => this.m_defaultText;
    set => this.m_defaultText = value;
  }

  internal CheckBoxSizeType CheckboxSizeType
  {
    get => this.m_checkboxSizeType;
    set => this.m_checkboxSizeType = value;
  }

  internal int CheckboxSize
  {
    get => this.m_checkboxSize;
    set => this.m_checkboxSize = value;
  }

  internal bool Enabled
  {
    get => this.m_enabled;
    set => this.m_enabled = value;
  }

  internal string MacroOnExit
  {
    get => this.m_marcoOnEnd;
    set => this.m_marcoOnEnd = value;
  }

  internal string MarcoOnStart
  {
    get => this.m_marcoOnStart;
    set => this.m_marcoOnStart = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string HelpText
  {
    get => this.m_helpText;
    set => this.m_helpText = value;
  }

  internal bool CalculateOnExit
  {
    get => this.m_bCalculateOnExit;
    set => this.m_bCalculateOnExit = value;
  }

  internal string StatusHelpText
  {
    get => this.m_statusHelpText;
    set => this.m_statusHelpText = value;
  }

  internal FormFieldType FormFieldType
  {
    get => this.m_formFieldType;
    set => this.m_formFieldType = value;
  }
}
