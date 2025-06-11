// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ExternNameImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class ExternNameImpl : CommonObject, INameIndexChangedEventProvider, ICloneParent
{
  private ExternNameRecord m_name;
  private int m_iIndex;
  private ExternWorkbookImpl m_externBook;
  private string m_refersTo;
  public int sheetId;

  [CLSCompliant(false)]
  public ExternNameImpl(IApplication application, object parent, ExternNameRecord name, int index)
    : base(application, parent)
  {
    this.m_name = name;
    this.m_iIndex = index;
    this.SetParents();
  }

  private void SetParents()
  {
    this.m_externBook = this.FindParent(typeof (ExternWorkbookImpl)) as ExternWorkbookImpl;
    if (this.m_externBook == null)
      throw new ArgumentNullException("Can't find parent extern workbook");
  }

  public int Index
  {
    get => this.m_iIndex;
    set
    {
      if (value == this.m_iIndex)
        return;
      int iIndex = this.m_iIndex;
      this.m_iIndex = value;
      this.RaiseIndexChangedEvent(new NameIndexChangedEventArgs(iIndex, this.m_iIndex));
    }
  }

  public string Name => this.m_name.Name;

  public int BookIndex => this.m_externBook.Index;

  internal ExternNameRecord Record => this.m_name;

  internal string RefersTo
  {
    get => this.m_refersTo;
    set => this.m_refersTo = value;
  }

  private void RaiseIndexChangedEvent(NameIndexChangedEventArgs args)
  {
    if (this.NameIndexChanged == null)
      return;
    this.NameIndexChanged((object) this, args);
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_name);
  }

  public object Clone(object parent)
  {
    ExternNameImpl externNameImpl = (ExternNameImpl) this.MemberwiseClone();
    externNameImpl.SetParent(parent);
    externNameImpl.SetParents();
    this.m_name = (ExternNameRecord) CloneUtils.CloneCloneable((ICloneable) this.m_name);
    return (object) externNameImpl;
  }

  public event NameImpl.NameIndexChangedEventHandler NameIndexChanged;
}
