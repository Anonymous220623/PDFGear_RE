// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedFileLinkAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedFileLinkAnnotation : PdfLoadedStyledAnnotation
{
  private int[] destinationArray;
  private PdfCrossTable m_crossTable;
  private PdfLaunchAction m_action;
  private PdfArray m_destination;

  public string FileName
  {
    get => this.ObtainFileName();
    set
    {
      PdfDictionary dictionary = this.Dictionary;
      if (!this.Dictionary.ContainsKey("A"))
        return;
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject((this.m_crossTable.GetObject(this.Dictionary["A"]) as PdfDictionary)["F"]) as PdfDictionary;
      pdfDictionary.SetString("F", value);
      if (pdfDictionary.ContainsKey("UF"))
        pdfDictionary.SetString("UF", value);
      this.Dictionary.Modify();
    }
  }

  private PdfArray Destination
  {
    get => this.m_destination;
    set => this.m_destination = value;
  }

  public int[] DestinationArray
  {
    get => this.ObtainDestination();
    set
    {
      if (value == null)
        throw new ArgumentNullException("DestinationPageNumber");
      if (value == this.destinationArray)
        return;
      this.destinationArray = value;
      this.Destination.Clear();
      this.Destination.Add((IPdfPrimitive) new PdfNumber(value[0] - 1));
      this.Destination.Add((IPdfPrimitive) new PdfName("XYZ"));
      this.Destination.Add((IPdfPrimitive) new PdfNull());
      this.Destination.Add((IPdfPrimitive) new PdfNumber(value[1]));
      this.Destination.Add((IPdfPrimitive) new PdfNumber(value[2]));
      PdfDictionary dictionary = this.Dictionary;
      if (!this.Dictionary.ContainsKey("A"))
        return;
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["A"]) as PdfDictionary;
      pdfDictionary.Remove("D");
      pdfDictionary.SetProperty("D", (IPdfPrimitive) this.Destination);
      this.Dictionary.Modify();
    }
  }

  private string ObtainFileName()
  {
    string empty = string.Empty;
    if (this.Dictionary.ContainsKey("A"))
    {
      PdfDictionary pdfDictionary1 = this.m_crossTable.GetObject(this.Dictionary["A"]) as PdfDictionary;
      PdfString pdfString = (PdfString) null;
      if (PdfCrossTable.Dereference(pdfDictionary1["F"]) is PdfString)
        pdfString = PdfCrossTable.Dereference(pdfDictionary1["F"]) as PdfString;
      else if (this.m_crossTable.GetObject(pdfDictionary1["F"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("F"))
        pdfString = pdfDictionary2["F"] as PdfString;
      else if (pdfDictionary2 != null && pdfDictionary2.ContainsKey("UF"))
        pdfString = pdfDictionary2["UF"] as PdfString;
      empty = pdfString.Value.ToString();
    }
    return empty;
  }

  internal PdfLoadedFileLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string filename)
    : base(dictionary, crossTable)
  {
    if (filename == null)
      throw new ArgumentNullException(nameof (filename));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
    this.m_action = new PdfLaunchAction(filename, true);
  }

  internal PdfLoadedFileLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    PdfArray destination,
    RectangleF rectangle,
    string filename)
    : base(dictionary, crossTable)
  {
    if (filename == null)
      throw new ArgumentNullException(nameof (filename));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
    this.Destination = destination;
  }

  private int[] ObtainDestination()
  {
    int[] destination = (int[]) null;
    if (this.Dictionary.ContainsKey("A"))
    {
      PdfArray pdfArray = PdfCrossTable.Dereference((this.m_crossTable.GetObject(this.Dictionary["A"]) as PdfDictionary)["D"]) as PdfArray;
      int index = 0;
      destination = new int[pdfArray.Count - 1];
      foreach (object obj in pdfArray)
      {
        if (obj is PdfNumber)
        {
          if (index == 0)
          {
            destination[index] = (obj as PdfNumber).IntValue + 1;
            ++index;
          }
          else
          {
            destination[index] = (obj as PdfNumber).IntValue;
            ++index;
          }
        }
        else if (obj is PdfNull)
        {
          destination[index] = 0;
          ++index;
        }
      }
    }
    return destination;
  }
}
