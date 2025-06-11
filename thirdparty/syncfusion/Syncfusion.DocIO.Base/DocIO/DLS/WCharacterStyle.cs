// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WCharacterStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WCharacterStyle : Style, IWCharacterStyle, IStyle
{
  public WCharacterStyle BaseStyle => base.BaseStyle as WCharacterStyle;

  public override StyleType StyleType => StyleType.CharacterStyle;

  public WCharacterStyle(WordDocument doc)
    : base(doc)
  {
    this.m_chFormat = new WCharacterFormat((IWordDocument) this.Document);
    this.m_chFormat.SetOwner((OwnerHolder) this);
    if (!doc.CreateBaseStyle)
      return;
    doc.CreateBaseStyle = false;
    this.ApplyBaseStyle(BuiltinStyle.DefaultParagraphFont);
    doc.CreateBaseStyle = true;
  }

  public override IStyle Clone() => (IStyle) this.CloneImpl();
}
