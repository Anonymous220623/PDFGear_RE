// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfDestination
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the explicit and named destination.</summary>
public class PdfDestination
{
  private PdfDocument _doc;
  private string _name;
  private int _pageIndex;
  private IntPtr _handle = IntPtr.Zero;
  private PdfTypeArray _array;
  private DestinationTypes _destinationType;
  private float? _left;
  private float? _top;
  private float? _right;
  private float? _bottom;
  private float? _zoom;

  /// <summary>Gets the destionation's array</summary>
  public PdfTypeArray Array
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeArray) null;
      if (this._array == null || this._array.IsDisposed || this._array.Handle != this.Handle)
        this._array = PdfTypeArray.Create(this.Handle);
      return this._array;
    }
  }

  /// <summary>
  /// Gets the Pdfium SDK handle that the destination is bound to
  /// </summary>
  public IntPtr Handle => this._handle;

  /// <summary>
  /// Gets the name of the named destination. This value is null for explicit destination
  /// </summary>
  public string Name
  {
    get => this._name;
    internal set => this._name = value;
  }

  /// <summary>
  /// Gets or sets the page index(zero based for current document) in current or remote document which indicates the named destination
  /// </summary>
  public int PageIndex
  {
    get => this._pageIndex;
    set
    {
      if (this._pageIndex == value)
        return;
      this._pageIndex = value;
      this.UpdateDestinationParams();
    }
  }

  /// <summary>Gets or sets type of destination.</summary>
  public DestinationTypes DestinationType
  {
    get => this._destinationType;
    set
    {
      if (this._destinationType == value)
        return;
      this._destinationType = value;
      this.UpdateDestinationParams();
    }
  }

  /// <summary>Gets or sets Left parameter of destination.</summary>
  public float? Left
  {
    get => this._left;
    set
    {
      float? left = this._left;
      float? nullable = value;
      if ((double) left.GetValueOrDefault() == (double) nullable.GetValueOrDefault() & left.HasValue == nullable.HasValue)
        return;
      this._left = value;
      this.UpdateDestinationParams();
    }
  }

  /// <summary>Gets or sets Top parameter of destination.</summary>
  public float? Top
  {
    get => this._top;
    set
    {
      float? top = this._top;
      float? nullable = value;
      if ((double) top.GetValueOrDefault() == (double) nullable.GetValueOrDefault() & top.HasValue == nullable.HasValue)
        return;
      this._top = value;
      this.UpdateDestinationParams();
    }
  }

  /// <summary>Gets or sets Right parameter of destination.</summary>
  public float? Right
  {
    get => this._right;
    set
    {
      float? right = this._right;
      float? nullable = value;
      if ((double) right.GetValueOrDefault() == (double) nullable.GetValueOrDefault() & right.HasValue == nullable.HasValue)
        return;
      this._right = value;
      this.UpdateDestinationParams();
    }
  }

  /// <summary>Gets or sets Bottom parameter of destination.</summary>
  public float? Bottom
  {
    get => this._bottom;
    set
    {
      float? bottom = this._bottom;
      float? nullable = value;
      if ((double) bottom.GetValueOrDefault() == (double) nullable.GetValueOrDefault() & bottom.HasValue == nullable.HasValue)
        return;
      this._bottom = value;
      this.UpdateDestinationParams();
    }
  }

  /// <summary>Gets or sets Zoom parameter of destination.</summary>
  public float? Zoom
  {
    get => this._zoom;
    set
    {
      float? zoom = this._zoom;
      float? nullable = value;
      if ((double) zoom.GetValueOrDefault() == (double) nullable.GetValueOrDefault() & zoom.HasValue == nullable.HasValue)
        return;
      this._zoom = value;
      this.UpdateDestinationParams();
    }
  }

  /// <summary>
  /// Initializes a new instance of the PdfDestination class.
  /// </summary>
  /// <param name="document">Document which contains this collection of destinations.</param>
  /// <param name="destHandle">Pdfium SDK handle that the destination is bound to.</param>
  /// <param name="name">The name for the named Destination; null if the destination is not a named destination.</param>
  internal PdfDestination(PdfDocument document, IntPtr destHandle, string name = null)
  {
    this._doc = document;
    this._handle = destHandle;
    this._name = name;
    this._pageIndex = (int) Pdfium.FPDFDest_GetPageIndex(this._doc == null ? IntPtr.Zero : this._doc.Handle, this._handle);
    this.FillDestinationParams();
  }

  /// <summary>
  /// Creates new <see cref="T:Patagames.Pdf.Net.PdfDestination" />
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote destination.</param>
  /// <param name="name">The name of destination in remote document. If specified, all other parameters of the destination are ignored.</param>
  public PdfDestination(PdfDocument document, string name = null)
  {
    this._doc = document;
    this._handle = Pdfium.FPDFARRAY_Create();
    if (this._handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    this._name = name;
    this._pageIndex = 0;
    this._destinationType = DestinationTypes.Fit;
    this.UpdateDestinationParams();
  }

  /// <summary>
  /// Create new destination that display the page designated by <paramref name="pageIndex" />,
  /// with the coordinates (<paramref name="left" />, <paramref name="top" />) positioned at the upper-left corner of the window
  /// and the contents of the page magnified by the factor <paramref name="zoom" />
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote document.</param>
  /// <param name="pageIndex">The zero-based index of the page to be displayed by this destination.</param>
  /// <param name="left">The x coordinate.</param>
  /// <param name="top">The y coordinate.</param>
  /// <param name="zoom">The zoom factor</param>
  /// <returns>An instance of the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> class initialized with the specified parameters.</returns>
  /// <remarks>
  /// A null value for any of the parameters <paramref name="left" />, <paramref name="top" />, or <paramref name="zoom" /> specifies
  /// that the current value of that parameter is to be retained unchanged. A zoom value of 0 has the same meaning as a null value.
  /// </remarks>
  public static PdfDestination CreateXYZ(
    PdfDocument document,
    int pageIndex,
    float? left = null,
    float? top = null,
    float? zoom = null)
  {
    return new PdfDestination(document)
    {
      DestinationType = DestinationTypes.XYZ,
      PageIndex = pageIndex,
      Left = left,
      Top = top,
      Zoom = zoom
    };
  }

  /// <summary>
  /// Create new destination that display the page designated by <paramref name="pageIndex" />,
  /// with its contents magnified just enough to fit the entire page within the window both horizontally and vertically.
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote document.</param>
  /// <param name="pageIndex">The zero-based index of the page to be displayed by this destination.</param>
  /// <returns>An instance of the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> class initialized with the specified parameters.</returns>
  /// <remarks>
  /// If the required horizontal and vertical magnification factors are different, use
  /// the smaller of the two, centering the page within the window in the other dimension.
  /// </remarks>
  public static PdfDestination CreateFit(PdfDocument document, int pageIndex)
  {
    return new PdfDestination(document)
    {
      DestinationType = DestinationTypes.Fit,
      PageIndex = pageIndex
    };
  }

  /// <summary>
  /// Create new destination that display the page designated by <paramref name="pageIndex" />,
  /// with the vertical coordinate <paramref name="top" /> positioned at the top edge of the window
  /// and the contents of the page magnified just enough to fit the entire width of the page within the window.
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote document.</param>
  /// <param name="pageIndex">The zero-based index of the page to be displayed by this destination.</param>
  /// <param name="top">The y coordinate.</param>
  /// <returns>An instance of the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> class initialized with the specified parameters.</returns>
  /// <remarks>
  /// A null value for <paramref name="top" /> specifies  that the current value of that parameter is to be retained unchanged.
  /// </remarks>
  public static PdfDestination CreateFitH(PdfDocument document, int pageIndex, float? top = null)
  {
    return new PdfDestination(document)
    {
      DestinationType = DestinationTypes.FitH,
      PageIndex = pageIndex,
      Top = top
    };
  }

  /// <summary>
  /// Create new destination that display the page designated by <paramref name="pageIndex" />,
  /// with the horizontal coordinate <paramref name="left" /> positioned at the left edge of the window
  /// and the contents of the page magnified just enough to fit the entire height of the page within the window.
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote document.</param>
  /// <param name="pageIndex">The zero-based index of the page to be displayed by this destination.</param>
  /// <param name="left">The y coordinate.</param>
  /// <returns>An instance of the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> class initialized with the specified parameters.</returns>
  /// <remarks>
  /// A null value for <paramref name="left" /> specifies  that the current value of that parameter is to be retained unchanged.
  /// </remarks>
  public static PdfDestination CreateFitV(PdfDocument document, int pageIndex, float? left = null)
  {
    return new PdfDestination(document)
    {
      DestinationType = DestinationTypes.FitV,
      PageIndex = pageIndex,
      Left = left
    };
  }

  /// <summary>
  /// Create new destination that display the page designated by <paramref name="pageIndex" />,
  /// with its contents magnified just enough to fit the rectangle specified by the coordinates
  /// <paramref name="left" />, <paramref name="bottom" />, <paramref name="right" />, and <paramref name="top" />
  /// entirely within the window both horizontally and vertically.
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote document.</param>
  /// <param name="pageIndex">The zero-based index of the page to be displayed by this destination.</param>
  /// <param name="left">The x-coordinate of the left edge.</param>
  /// <param name="bottom">The y-coordinate of the bottom edge.</param>
  /// <param name="right">The x-coordinate of the right edge.</param>
  /// <param name="top">The y-coordinate of the top edge.</param>
  /// <returns>An instance of the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> class initialized with the specified parameters.</returns>
  /// <remarks>
  /// <para>
  /// If the required horizontal and vertical magnification factors are different, use the smaller
  /// the two, centering the rectangle within the window in the other dimension.
  /// </para>
  /// <para>
  /// A null value for any of the parameters may result in unpredictable behavior.
  /// </para>
  /// </remarks>
  public static PdfDestination CreateFitR(
    PdfDocument document,
    int pageIndex,
    float? left = null,
    float? bottom = null,
    float? right = null,
    float? top = null)
  {
    return new PdfDestination(document)
    {
      DestinationType = DestinationTypes.FitR,
      PageIndex = pageIndex,
      Left = left,
      Top = top,
      Right = right,
      Bottom = bottom
    };
  }

  /// <summary>
  /// Create new destination that display the page designated by <paramref name="pageIndex" />, with its contents magnified
  /// just enough to fit its bounding box entirely within the window both horizontally and vertically.
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote document.</param>
  /// <param name="pageIndex">The zero-based index of the page to be displayed by this destination.</param>
  /// <returns>An instance of the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> class initialized with the specified parameters.</returns>
  /// <remarks>
  /// If the required horizontal and vertical magnification factors are different, use the smaller of the two,
  /// centering the bounding box within the window in the other dimension.
  /// </remarks>
  public static PdfDestination CreateFitB(PdfDocument document, int pageIndex)
  {
    return new PdfDestination(document)
    {
      DestinationType = DestinationTypes.FitB,
      PageIndex = pageIndex
    };
  }

  /// <summary>
  /// Create new destination that display the page designated by <paramref name="pageIndex" />, with the vertical coordinate
  /// <paramref name="top" /> positioned at the top edge of the window and the contents of the page
  /// magnified just enough to fit the entire width of its bounding box within the window.
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote document.</param>
  /// <param name="pageIndex">The zero-based index of the page to be displayed by this destination.</param>
  /// <param name="top">The y coordinate.</param>
  /// <returns>An instance of the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> class initialized with the specified parameters.</returns>
  /// <remarks>
  /// A null value for <paramref name="top" /> specifies  that the current value of that parameter is to be retained unchanged.
  /// </remarks>
  public static PdfDestination CreateFitBH(PdfDocument document, int pageIndex, float? top = null)
  {
    return new PdfDestination(document)
    {
      DestinationType = DestinationTypes.FitBH,
      PageIndex = pageIndex,
      Top = top
    };
  }

  /// <summary>
  /// Create new destination that display the page designated by <paramref name="pageIndex" />,
  /// with the horizontal coordinate <paramref name="left" /> positioned at the left edge of the window and the contents of the page
  /// magnified just enough to fit the entire height of its bounding box within the window.
  /// </summary>
  /// <param name="document">Document which contains this destination. Null for remote document.</param>
  /// <param name="pageIndex">The zero-based index of the page to be displayed by this destination.</param>
  /// <param name="left">The y coordinate.</param>
  /// <returns>An instance of the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> class initialized with the specified parameters.</returns>
  /// <remarks>
  /// A null value for <paramref name="left" /> specifies  that the current value of that parameter is to be retained unchanged.
  /// </remarks>
  public static PdfDestination CreateFitBV(PdfDocument document, int pageIndex, float? left = null)
  {
    return new PdfDestination(document)
    {
      DestinationType = DestinationTypes.FitBV,
      PageIndex = pageIndex,
      Left = left
    };
  }

  private void UpdateDestinationParams()
  {
    if (this._handle == IntPtr.Zero)
      return;
    string enumDescription = Pdfium.GetEnumDescription((Enum) this._destinationType);
    PdfTypeArray arr = PdfTypeArray.Create(this._handle);
    arr.Clear();
    if (this._doc != null)
      arr.AddIndirect(PdfIndirectList.FromPdfDocument(this._doc), (PdfTypeBase) this._doc.Pages[this._pageIndex].Dictionary);
    else
      arr.Add((PdfTypeBase) PdfTypeNumber.Create(this._pageIndex));
    arr.Add((PdfTypeBase) PdfTypeName.Create(enumDescription));
    switch (this._destinationType)
    {
      case DestinationTypes.XYZ:
        this.AddVal(arr, this._left);
        this.AddVal(arr, this._top);
        this.AddVal(arr, this._zoom);
        break;
      case DestinationTypes.FitH:
        this.AddVal(arr, this._top);
        break;
      case DestinationTypes.FitV:
        this.AddVal(arr, this._left);
        break;
      case DestinationTypes.FitR:
        this.AddVal(arr, this._left);
        this.AddVal(arr, this._bottom);
        this.AddVal(arr, this._right);
        this.AddVal(arr, this._top);
        break;
      case DestinationTypes.FitBH:
        this.AddVal(arr, this._top);
        break;
      case DestinationTypes.FitBV:
        this.AddVal(arr, this._left);
        break;
    }
  }

  private void FillDestinationParams()
  {
    this._left = this._top = this._right = this._bottom = this._zoom = new float?();
    this._destinationType = DestinationTypes.XYZ;
    PdfTypeArray arr = PdfTypeArray.Create(this._handle);
    if (arr.Count > 1 && arr[1].Is<PdfTypeName>())
      Pdfium.GetEnumDescription<DestinationTypes>(arr[1].As<PdfTypeName>().Value, out this._destinationType);
    switch (this._destinationType)
    {
      case DestinationTypes.XYZ:
        this._left = this.GetVal(arr, 2);
        this._top = this.GetVal(arr, 3);
        this._zoom = this.GetVal(arr, 4);
        break;
      case DestinationTypes.FitH:
        this._top = this.GetVal(arr, 2);
        break;
      case DestinationTypes.FitV:
        this._left = this.GetVal(arr, 2);
        break;
      case DestinationTypes.FitR:
        this._left = this.GetVal(arr, 2);
        this._bottom = this.GetVal(arr, 3);
        this._right = this.GetVal(arr, 4);
        this._top = this.GetVal(arr, 5);
        break;
      case DestinationTypes.FitBH:
        this._top = this.GetVal(arr, 2);
        break;
      case DestinationTypes.FitBV:
        this._left = this.GetVal(arr, 2);
        break;
    }
  }

  private float? GetVal(PdfTypeArray arr, int idx)
  {
    return arr.Count > idx && arr[idx].Is<PdfTypeNumber>() ? new float?(arr[idx].As<PdfTypeNumber>().FloatValue) : new float?();
  }

  private void AddVal(PdfTypeArray arr, float? val)
  {
    if (!val.HasValue)
      arr.Add((PdfTypeBase) PdfTypeNull.Create());
    else
      arr.Add((PdfTypeBase) PdfTypeNumber.Create(val.Value));
  }

  internal PdfTypeBase GetForInsert(PdfDocument doc = null)
  {
    if (this.Name != null)
      return (PdfTypeBase) PdfTypeString.Create(this.Name, true);
    if (this.Array.ObjectNumber != 0)
    {
      if (doc == null)
        throw new ArgumentException(string.Format(Error.err0067, (object) "destination", (object) "object"));
      return (PdfTypeBase) PdfTypeIndirect.Create(PdfIndirectList.FromPdfDocument(doc), this.Array.ObjectNumber);
    }
    if (Pdfium.FPDFOBJ_GetParentObj(this.Array.Handle) != IntPtr.Zero)
      throw new ArgumentException(string.Format(Error.err0067, (object) "destination", (object) "object"));
    return (PdfTypeBase) this.Array;
  }
}
