// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class Pdf3DAnnotation : PdfFileAnnotation
{
  private Pdf3DActivation m_activation;
  private Pdf3DBase m_u3d;
  private PdfTemplate m_apperance;

  public Pdf3DViewCollection Views => this.m_u3d.Stream.Views;

  public int DefaultView
  {
    get => this.m_u3d.Stream.DefaultView;
    set => this.m_u3d.Stream.DefaultView = value;
  }

  public Pdf3DAnnotationType Type
  {
    get => this.m_u3d.Stream.Type;
    set => this.m_u3d.Stream.Type = value;
  }

  public string OnInstantiate
  {
    get => this.m_u3d.Stream.OnInstantiate;
    set => this.m_u3d.Stream.OnInstantiate = value;
  }

  public Pdf3DActivation Activation
  {
    get => this.m_activation;
    set => this.m_activation = value;
  }

  public override string FileName
  {
    get => this.m_u3d.FileName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (FileName));
        case "":
          throw new ArgumentException("FileName can't be empty");
        default:
          if (this.m_u3d != null)
          {
            if (!(this.m_u3d.FileName != value))
              break;
            this.m_u3d.FileName = value;
            break;
          }
          this.m_u3d = new Pdf3DBase(value);
          break;
      }
    }
  }

  public Pdf3DAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  public Pdf3DAnnotation(RectangleF rectangle, string fileName)
    : base(rectangle)
  {
    this.m_u3d = fileName != null ? new Pdf3DBase(fileName) : throw new ArgumentNullException(nameof (fileName));
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("3D"));
  }

  protected override void Save()
  {
    base.Save();
    this.Dictionary.SetProperty("3DD", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_u3d));
    if (this.m_activation != null)
      this.Dictionary["3DA"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_activation);
    if (this.m_apperance == null)
      return;
    this.Dictionary["AP /N"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_apperance);
  }
}
