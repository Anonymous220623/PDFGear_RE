// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PathPointsCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represent the collection of the path's points.</summary>
public class PathPointsCollection : 
  IList<FS_PATHPOINTF>,
  ICollection<FS_PATHPOINTF>,
  IEnumerable<FS_PATHPOINTF>,
  IEnumerable,
  IDisposable
{
  private bool _isNeedDispose;

  /// <summary>Gets the parent object that hosts this collection.</summary>
  public PdfPathObject Host { get; set; }

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the object has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>Gets handle to PDF path object</summary>
  public IntPtr Handle { get; private set; }

  /// <summary>
  /// Gets a value indicating whether the path is rectangle.
  /// </summary>
  public bool IsRectangle => Pdfium.FPDFPath_IsRect(this.Handle);

  /// <summary>Gets a rectangle that bounds this Path.</summary>
  public FS_RECTF BoundingBox
  {
    get
    {
      FS_RECTF boundingBox = Pdfium.FPDFPath_GetBoundingBox(this.Handle);
      this.CorrectRect(ref boundingBox);
      return boundingBox;
    }
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.PathPointsCollection" /> class.
  /// </summary>
  /// <param name="handle">Handle to PDF path object</param>
  /// <param name="host">The parent object that hosts this collection.</param>
  public PathPointsCollection(IntPtr handle, PdfPathObject host)
  {
    this.Handle = handle;
    this._isNeedDispose = false;
    this.Host = host;
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.PathPointsCollection" /> class.
  /// </summary>
  public PathPointsCollection()
  {
    this.Handle = Pdfium.FPDFPath_Create();
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    this._isNeedDispose = true;
  }

  /// <summary>Releases all resources used by the Path.</summary>
  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Releases all resources used by the Path.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this._isNeedDispose)
      Pdfium.FPDFPath_Delete(this.Handle);
    this.Handle = IntPtr.Zero;
    this.IsDisposed = true;
  }

  /// <summary>Copies the points of the path to a new array.</summary>
  /// <returns>An array containing copies of the points of the path.</returns>
  public FS_PATHPOINTF[] ToArray()
  {
    FS_PATHPOINTF[] points = Pdfium.FPDFPath_GetPoints(this.Handle);
    for (int index = 0; index < points.Length; ++index)
    {
      float x = points[index].X;
      float y = points[index].Y;
      this.CorrectPoint(ref x, ref y);
      points[index].X = x;
      points[index].Y = y;
    }
    return points;
  }

  /// <summary>Get a rectangle that bounds this Path.</summary>
  /// <param name="lineWidth">Line width.</param>
  /// <param name="miterLimit">The maximum length of mitered line joins for stroked paths.</param>
  /// <returns>A rectangle that bounds this Path.</returns>
  public FS_RECTF GetBoundingBox(float lineWidth, float miterLimit)
  {
    FS_RECTF boundingBoxEx = Pdfium.FPDFPath_GetBoundingBoxEx(this.Handle, lineWidth, miterLimit);
    this.CorrectRect(ref boundingBoxEx);
    return boundingBoxEx;
  }

  /// <summary>Appends the specified Path to this path.</summary>
  /// <param name="addingPath">The Path to add.</param>
  public void AppendPath(PathPointsCollection addingPath)
  {
    Pdfium.FPDFPath_Append(this.Handle, addingPath.Handle);
  }

  /// <summary>Appends the specified Path to this path.</summary>
  /// <param name="addingPath">The Path to add.</param>
  /// <param name="matrix">The transformation matrix. The matrix will be applied to addingPath.</param>
  public void AppendPath(PathPointsCollection addingPath, FS_MATRIX matrix)
  {
    Pdfium.FPDFPath_AppendEx(this.Handle, addingPath.Handle, matrix);
  }

  /// <summary>Adds a rectangle to this path.</summary>
  /// <param name="rect">A <see cref="T:Patagames.Pdf.FS_RECTF" /> that represents the rectangle to add.</param>
  public void AppendRect(FS_RECTF rect) => Pdfium.FPDFPath_AppendRect(this.Handle, rect);

  /// <summary>Copy points from source Path to this Path.</summary>
  /// <param name="sourcePath">The source Path.</param>
  public void CopyPath(PathPointsCollection sourcePath)
  {
    Pdfium.FPDFPath_Copy(this.Handle, sourcePath.Handle);
  }

  /// <summary>Transform (scale, rotate, shear, move) the path.</summary>
  /// <param name="matrix">Transformation matrix</param>
  public void Transform(FS_MATRIX matrix)
  {
    Pdfium.FPDFPath_Transform(this.Handle, matrix.a, matrix.b, matrix.c, matrix.d, matrix.e, matrix.f);
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> at the specified index.</returns>
  public FS_PATHPOINTF this[int index]
  {
    get
    {
      float x = index >= 0 && index < this.Count ? Pdfium.FPDFPath_GetPointX(this.Handle, index) : throw new IndexOutOfRangeException();
      float pointY = Pdfium.FPDFPath_GetPointY(this.Handle, index);
      PathPointFlags flag = Pdfium.FPDFPath_GetFlag(this.Handle, index);
      this.CorrectPoint(ref x, ref pointY);
      return new FS_PATHPOINTF(x, pointY, flag);
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      float x = value.X;
      float y = value.Y;
      this.ReverseTransformPoint(ref x, ref y);
      Pdfium.FPDFPath_SetPoint(this.Handle, index, new FS_PATHPOINTF(x, y, value.Flags));
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> contained in the collection.
  /// </summary>
  public int Count => Pdfium.FPDFPath_GetPointCount(this.Handle);

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(FS_PATHPOINTF item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(FS_PATHPOINTF item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> from the collection
  /// </summary>
  public void Clear() => Pdfium.FPDFPath_TrimPoints(this.Handle, 0);

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />  to remove.</param>
  public void RemoveAt(int index)
  {
    int count = this.Count;
    if (index < 0 || index >= count)
      throw new ArgumentOutOfRangeException();
    if (count <= 1)
      this.Clear();
    else if (index == count - 1)
    {
      Pdfium.FPDFPath_TrimPoints(this.Handle, count - 1);
    }
    else
    {
      for (int index1 = index; index1 < count - 1; ++index1)
        this[index1] = this[index1 + 1];
      Pdfium.FPDFPath_TrimPoints(this.Handle, count - 1);
    }
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(FS_PATHPOINTF[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (FS_PATHPOINTF fsPathpointf in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = fsPathpointf;
    }
  }

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(FS_PATHPOINTF item)
  {
    int index = this.IndexOf(item);
    if (index < 0)
      return false;
    this.RemoveAt(index);
    return true;
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<FS_PATHPOINTF> GetEnumerator()
  {
    return (IEnumerator<FS_PATHPOINTF>) new CollectionEnumerator<FS_PATHPOINTF>((IList<FS_PATHPOINTF>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_PATHPOINTF" />  to add to the collection</param>
  /// <remarks>
  /// To append a cubic Bézier curve to the current path you should add three point with <see cref="F:Patagames.Pdf.Enums.PathPointFlags.BezierTo" /> flag.
  /// The curve extends from the current point to the point (x3, y3), using (x1, y1) and (x2, y2) as the Bézier control points.
  /// The new current point will be (x3, y3).
  /// </remarks>
  public void Add(FS_PATHPOINTF item)
  {
    Pdfium.FPDFPath_AddPointCount(this.Handle, 1);
    this[this.Count - 1] = item;
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_PATHPOINTF" /> to insert into the collection.</param>
  /// <remarks>
  /// To append a cubic Bézier curve to the current path you should add three point with <see cref="F:Patagames.Pdf.Enums.PathPointFlags.BezierTo" /> flag.
  /// The curve extends from the current point to the point (x3, y3), using (x1, y1) and (x2, y2) as the Bézier control points.
  /// The new current point will be (x3, y3).
  /// </remarks>
  public void Insert(int index, FS_PATHPOINTF item)
  {
    int count = this.Count;
    if (index < 0 || index > count)
      throw new ArgumentOutOfRangeException();
    if (index == count)
    {
      this.Add(item);
    }
    else
    {
      Pdfium.FPDFPath_AddPointCount(this.Handle, 1);
      for (int index1 = this.Count - 1; index1 > index; --index1)
        this[index1] = this[index1 - 1];
      this[index] = item;
    }
  }

  private void CorrectRect(ref FS_RECTF rect)
  {
    if (this.Host == null)
      return;
    FS_MATRIX matrix = this.Host.Matrix;
    if (this.Host.Container != null && this.Host.Container.Form != null)
      matrix.Concat(this.Host.Container.Form.Matrix);
    matrix.TransformRect(ref rect);
  }

  private void CorrectPoint(ref float x, ref float y)
  {
    if (this.Host == null)
      return;
    FS_MATRIX matrix = this.Host.Matrix;
    if (this.Host.Container != null && this.Host.Container.Form != null)
      matrix.Concat(this.Host.Container.Form.Matrix);
    matrix.TransformPoint(ref x, ref y);
  }

  private void ReverseTransformPoint(ref float x, ref float y)
  {
    if (this.Host == null)
      return;
    FS_MATRIX matrix = this.Host.Matrix;
    if (this.Host.Container != null && this.Host.Container.Form != null)
      matrix.Concat(this.Host.Container.Form.Matrix);
    FS_MATRIX fsMatrix = new FS_MATRIX();
    fsMatrix.SetReverse(matrix);
    fsMatrix.TransformPoint(ref x, ref y);
  }
}
