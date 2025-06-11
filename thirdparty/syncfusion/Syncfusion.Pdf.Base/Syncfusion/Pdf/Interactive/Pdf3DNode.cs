// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DNode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class Pdf3DNode : IPdfWrapper
{
  private bool m_visible;
  private string m_name;
  private float m_opacity;
  private float[] m_matrix;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public bool Visible
  {
    get => this.m_visible;
    set => this.m_visible = value;
  }

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public float Opacity
  {
    get => this.m_opacity;
    set => this.m_opacity = value;
  }

  public float[] Matrix
  {
    get => this.m_matrix;
    set
    {
      this.m_matrix = value;
      if (this.m_matrix != null && this.m_matrix.Length < 12)
        throw new ArgumentOutOfRangeException("Matrix.Length", "Matrix array must have at least 12 elements.");
    }
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  public Pdf3DNode() => this.Initialize();

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("3DNode"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected virtual void Save()
  {
    this.Dictionary.SetProperty("O", (IPdfPrimitive) new PdfNumber(this.m_opacity / 100f));
    this.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfBoolean(this.m_visible));
    if (this.m_name != null && this.m_name.Length > 0)
      this.Dictionary.SetProperty("N", (IPdfPrimitive) new PdfName(this.m_name));
    if (this.m_matrix == null || this.m_matrix.Length < 12)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendFormat("[{0:0.####} {1:0.####} {2:0.####} {3:0.####} {4:0.####} {5:0.####} {6:0.####} {7:0.####} {8:0.####} {9:0.####} {10:0.####} {11:0.####}]\n", (object) this.m_matrix[0], (object) this.m_matrix[1], (object) this.m_matrix[2], (object) this.m_matrix[3], (object) this.m_matrix[4], (object) this.m_matrix[5], (object) this.m_matrix[6], (object) this.m_matrix[7], (object) this.m_matrix[8], (object) this.m_matrix[9], (object) this.m_matrix[10], (object) this.m_matrix[11]);
    this.Dictionary.SetProperty("M", (IPdfPrimitive) new PdfName(stringBuilder.ToString()));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
