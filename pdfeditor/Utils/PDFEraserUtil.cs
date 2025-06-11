// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PDFEraserUtil
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Models.Annotations;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils;

internal class PDFEraserUtil
{
  private List<InkAnnotation> record = new List<InkAnnotation>();
  private List<IndexedPdfInkAnnotation> pdfInks = new List<IndexedPdfInkAnnotation>();
  private List<InkAnnotation> record2 = new List<InkAnnotation>();
  private List<InkAnnotation> changeRecord = new List<InkAnnotation>();
  private PdfAnnotationCollection pdfAnnotations;
  private int deletePageIndex = -1;

  public bool DeleteInk(
    PdfDocument Document,
    int pageIdx,
    Point pos,
    ToolbarSettingInkEraserModel inkEraserModel)
  {
    for (int i = 0; i < Document.Pages[pageIdx].Annots.Count; i++)
    {
      PdfAnnotation annot = Document.Pages[pageIdx].Annots[i];
      if (annot is PdfInkAnnotation inkannot && AnnotationHitTestHelper.HitTest(annot, pos))
      {
        PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(annot.Page?.Document);
        InkAnnotation inkAnnotation = (InkAnnotation) AnnotationFactory.Create((PdfAnnotation) inkannot);
        int pageIndex = annot.Page.PageIndex;
        Point clientPoint = pos;
        Point point;
        ref Point local = ref point;
        pdfControl.TryGetPagePoint(pageIndex, clientPoint, out local);
        int radius = inkEraserModel.SelectSize;
        foreach (PdfLinePointCollection<PdfInkAnnotation> ink in inkannot.InkList)
        {
          if (inkEraserModel.IsPartial == ToolbarSettingInkEraserModel.EraserType.Partial)
          {
            List<FS_POINTF> list = ink.Where<FS_POINTF>((Func<FS_POINTF, bool>) (pt => this.IsInRangeOfPoint(pt, point, (float) radius))).ToList<FS_POINTF>();
            if (list.Count > 0)
            {
              IndexedPdfInkAnnotation pdfInkAnnotation = new IndexedPdfInkAnnotation()
              {
                PdfInkAnnotation = inkannot,
                Index = i
              };
              if (this.pdfInks.FindAll((Predicate<IndexedPdfInkAnnotation>) (X => X.Index == i)).Count <= 0)
                this.pdfInks.Add(pdfInkAnnotation);
              this.DeleteInk(list, inkannot, Document.Pages[pageIdx], ink, point, (float) radius);
            }
          }
          else if (inkEraserModel.IsPartial == ToolbarSettingInkEraserModel.EraserType.Whole && this.HitTest(ink, point))
            this.DeleteWhole(Document, inkannot);
        }
      }
    }
    return this.deletePageIndex != -1;
  }

  public void MouseStyle(
    PdfDocument Document,
    AnnotationCanvas annotationCanvas,
    ToolbarSettingInkEraserModel inkEraserModel,
    MainViewModel VM)
  {
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(Document);
    if (inkEraserModel.IsPartial == ToolbarSettingInkEraserModel.EraserType.Partial)
    {
      int cursorSize = (int) ((double) (inkEraserModel.SelectSize * 2) * (double) VM.ViewToolbar.DocZoom);
      annotationCanvas.Cursor = BitmapCursor.CreateCustomCursor(inkEraserModel, VM, cursorSize);
      pdfControl.Viewer.OverrideCursor = BitmapCursor.CreateCustomCursor(inkEraserModel, VM, cursorSize);
    }
    else
    {
      if (inkEraserModel.IsPartial != ToolbarSettingInkEraserModel.EraserType.Whole)
        return;
      Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/Style/Resources/Annonate/Eraser.cur")).Stream);
      annotationCanvas.Cursor = cursor;
      pdfControl.Viewer.OverrideCursor = cursor;
    }
  }

  public void MouseDownRecord(
    int pageindex,
    MainViewModel VM,
    PdfDocument Document,
    ToolbarSettingInkEraserModel inkEraserModel,
    Point pos,
    Point Point)
  {
    this.changeRecord.Clear();
    if (pageindex < 0)
      pageindex = VM.SelectedPageIndex;
    this.pdfAnnotations = Document.Pages[pageindex].Annots;
    if (this.pdfAnnotations == null)
      return;
    for (int i = 0; i < Document.Pages[pageindex].Annots.Count; i++)
    {
      PdfAnnotation annot = Document.Pages[pageindex].Annots[i];
      if (annot is PdfInkAnnotation inkannot1)
        this.AddToChangeRecord(inkannot1, i);
      if (annot is PdfInkAnnotation inkannot2 && AnnotationHitTestHelper.HitTest(annot, pos))
      {
        PDFKit.PdfControl.GetPdfControl(annot.Page?.Document);
        InkAnnotation inkAnnotation = (InkAnnotation) AnnotationFactory.Create((PdfAnnotation) inkannot2);
        int radius = inkEraserModel.SelectSize / 2;
        if (radius == 0)
          radius = 1;
        foreach (PdfLinePointCollection<PdfInkAnnotation> ink in inkannot2.InkList)
        {
          if (inkEraserModel.IsPartial == ToolbarSettingInkEraserModel.EraserType.Partial)
          {
            List<FS_POINTF> list = ink.Where<FS_POINTF>((Func<FS_POINTF, bool>) (pt => this.IsInRangeOfPoint(pt, Point, (float) radius))).ToList<FS_POINTF>();
            if (list.Count > 0)
            {
              IndexedPdfInkAnnotation pdfInkAnnotation = new IndexedPdfInkAnnotation()
              {
                PdfInkAnnotation = inkannot2,
                Index = i
              };
              if (this.pdfInks.FindAll((Predicate<IndexedPdfInkAnnotation>) (X => X.Index == i)).Count <= 0)
                this.pdfInks.Add(pdfInkAnnotation);
              this.DeleteInk(list, inkannot2, Document.Pages[pageindex], ink, Point, (float) radius);
            }
          }
          else if (inkEraserModel.IsPartial == ToolbarSettingInkEraserModel.EraserType.Whole && this.HitTest(ink, Point))
            this.DeleteWhole(Document, inkannot2);
        }
      }
    }
  }

  public async Task ReflashPage(PdfPage page) => await page.TryRedrawPageAsync();

  private bool IsInRangeOfPoint(FS_POINTF pt1, Point pt2, float distance)
  {
    return Math.Pow(Math.Abs((double) pt1.X - pt2.X), 2.0) + Math.Pow(Math.Abs((double) pt1.Y - pt2.Y), 2.0) < Math.Pow((double) distance, 2.0);
  }

  public async Task DeleteWhole(PdfDocument Document, PdfInkAnnotation inkannot)
  {
    if (Document == null || (PdfWrapper) inkannot == (PdfWrapper) null)
      return;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(Document);
    if (pdfControl == null)
      return;
    AnnotationHolderManager annotationHolderManager = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl);
    if (annotationHolderManager == null)
      return;
    await annotationHolderManager.DeleteAnnotationAsync((PdfAnnotation) inkannot);
  }

  public async void CommitRedoRecords(MainViewModel VM, PdfDocument Document)
  {
    int deletePageIndex = this.deletePageIndex;
    if (deletePageIndex == -1)
      return;
    this.record2.Clear();
    this.record.AddRange((IEnumerable<InkAnnotation>) this.changeRecord);
    VM?.PageEditors?.NotifyPageAnnotationChanged(deletePageIndex);
    await this.RedoUndoRecord(VM, Document.Pages[deletePageIndex]);
    this.deletePageIndex = -1;
  }

  private void DeleteInk(
    List<FS_POINTF> matchedPoints,
    PdfInkAnnotation inkannot,
    PdfPage page,
    PdfLinePointCollection<PdfInkAnnotation> list,
    Point Mousepoint,
    float radius)
  {
    PdfLinePointCollection<PdfInkAnnotation> linePointCollection1 = new PdfLinePointCollection<PdfInkAnnotation>();
    PdfLinePointCollection<PdfInkAnnotation> linePointCollection2 = new PdfLinePointCollection<PdfInkAnnotation>();
    try
    {
      FS_POINTF point1 = new FS_POINTF(Mousepoint.X, Mousepoint.Y);
      FS_POINTF[] points = this.MathGetPoints(list, Mousepoint, radius, inkannot, page);
      this.Distance(point1, matchedPoints[0]);
      if (points != null)
      {
        if (points.Length == 1)
        {
          int num = list.IndexOf(matchedPoints[matchedPoints.Count - 1]);
          if (matchedPoints[0] == points[0])
            list.Remove(matchedPoints[0]);
          else if (num - list.Count + 1 < 0)
          {
            foreach (FS_POINTF matchedPoint in matchedPoints)
              list.Remove(matchedPoint);
            list.Insert(0, points[0]);
          }
          else
          {
            foreach (FS_POINTF matchedPoint in matchedPoints)
              list.Remove(matchedPoint);
            list.Add(points[0]);
          }
          inkannot.RegenerateAppearances();
          this.deletePageIndex = page.PageIndex;
        }
        else if (points.Length == 2)
        {
          List<int> intList = new List<int>();
          for (int index = 0; index < matchedPoints.Count; ++index)
          {
            if (!intList.Contains(list.IndexOf(matchedPoints[index])))
              intList.Add(list.IndexOf(matchedPoints[index]));
          }
          if (intList[0] == 0)
          {
            for (int index = intList.Count - 1; index >= 0; --index)
              list.RemoveAt(intList[index]);
            if (list.Count != 0)
            {
              if (this.Distance(points[0], list[0]) < this.Distance(points[1], list[0]))
              {
                list.Insert(0, new FS_POINTF(points[0].X, points[0].Y));
                list.Add(points[1]);
              }
              else
              {
                list.Insert(0, new FS_POINTF(points[1].X, points[1].Y));
                list.Add(points[0]);
              }
            }
          }
          else if (intList[0] == list.Count - 1)
          {
            for (int index = intList.Count - 1; index >= 0; --index)
              list.RemoveAt(intList[index]);
          }
          else
          {
            double num1 = this.Distance(points[0], list[intList[0] - 1]);
            double num2 = this.Distance(points[1], list[intList[0] - 1]);
            double num3 = intList[intList.Count - 1] + 1 == list.Count ? 0.0 : this.Distance(points[0], list[intList[intList.Count - 1] + 1]);
            double num4 = intList[intList.Count - 1] + 1 == list.Count ? 0.0 : this.Distance(points[1], list[intList[intList.Count - 1] + 1]);
            if (num1 + num4 - num2 - num3 > 0.0)
            {
              for (int index = 0; index < intList[0]; ++index)
                linePointCollection1.Add(list[index]);
              linePointCollection1.Add(points[1]);
              linePointCollection2.Add(points[0]);
              for (int index = intList[intList.Count - 1] + 1; index < list.Count; ++index)
                linePointCollection2.Add(list[index]);
            }
            else
            {
              for (int index = 0; index < intList[0]; ++index)
                linePointCollection1.Add(list[index]);
              linePointCollection1.Add(points[0]);
              linePointCollection2.Add(points[1]);
              for (int index = intList[intList.Count - 1] + 1; index < list.Count; ++index)
                linePointCollection2.Add(list[index]);
            }
            inkannot.InkList.Remove(list);
            inkannot.InkList.Add(linePointCollection1);
            inkannot.InkList.Add(linePointCollection2);
          }
          inkannot.RegenerateAppearances();
          this.deletePageIndex = page.PageIndex;
        }
        else
        {
          if (points.Length < 3)
            return;
          int num = list.IndexOf(matchedPoints[0]);
          if (num >= 0)
          {
            for (int index = 0; index < num; ++index)
              linePointCollection1.Add(list[index]);
          }
          if (num < list.Count)
          {
            for (int index = num + 1; index < list.Count; ++index)
              linePointCollection2.Add(list[index]);
          }
          inkannot.InkList.Remove(list);
          if (linePointCollection1.Count > 0)
            inkannot.InkList.Add(linePointCollection1);
          if (linePointCollection2.Count > 0)
            inkannot.InkList.Add(linePointCollection2);
          inkannot.RegenerateAppearances();
          this.deletePageIndex = page.PageIndex;
        }
      }
      else
      {
        foreach (FS_POINTF matchedPoint in matchedPoints)
          list.Remove(matchedPoint);
        if (list.Count <= 0)
          inkannot.InkList.Remove(list);
        inkannot.RegenerateAppearances();
        this.deletePageIndex = page.PageIndex;
      }
    }
    catch
    {
      int num = list.IndexOf(matchedPoints[0]);
      if (num >= 0)
      {
        for (int index = 0; index < num; ++index)
          linePointCollection1.Add(list[index]);
      }
      if (num < list.Count)
      {
        for (int index = num + 1; index < list.Count; ++index)
          linePointCollection2.Add(list[index]);
      }
      inkannot.InkList.Remove(list);
      if (linePointCollection1.Count > 0)
        inkannot.InkList.Add(linePointCollection1);
      if (linePointCollection2.Count > 0)
        inkannot.InkList.Add(linePointCollection2);
      inkannot.RegenerateAppearances();
      this.deletePageIndex = page.PageIndex;
    }
    finally
    {
      foreach (PdfLinePointCollection<PdfInkAnnotation> ink in inkannot.InkList)
      {
        if (ink.Count == 1)
          inkannot.InkList.Remove(ink);
      }
    }
  }

  private void TestInk(PdfLinePointCollection<PdfInkAnnotation> list)
  {
    if (this.Distance(list[list.Count - 2], list[list.Count - 1]) < 3.0)
      return;
    list.RemoveAt(list.Count - 1);
  }

  public bool HitTest(PdfLinePointCollection<PdfInkAnnotation> ink, Point point)
  {
    EllipseGeometry ellipseGeometry = new EllipseGeometry()
    {
      Center = point,
      RadiusX = 2.0,
      RadiusY = 2.0
    };
    PolyLineSegment polyLineSegment = new PolyLineSegment();
    PathFigure pathFigure = new PathFigure()
    {
      StartPoint = ink[0].ToPoint(),
      Segments = {
        (PathSegment) polyLineSegment
      }
    };
    for (int index = 1; index < ink.Count; ++index)
      polyLineSegment.Points.Add(ink[index].ToPoint());
    PathGeometry pathGeometry = new PathGeometry()
    {
      Figures = {
        pathFigure
      }
    };
    PathGeometry widenedPathGeometry1 = pathGeometry.GetWidenedPathGeometry(new Pen((Brush) Brushes.Black, 0.01), 0.5, ToleranceType.Absolute);
    switch (ellipseGeometry.FillContainsWithDetail((Geometry) pathGeometry, 0.5, ToleranceType.Absolute))
    {
      case IntersectionDetail.FullyContains:
      case IntersectionDetail.Intersects:
        PathGeometry widenedPathGeometry2 = ellipseGeometry.GetWidenedPathGeometry(new Pen((Brush) Brushes.Black, 0.01), 0.5, ToleranceType.Absolute);
        if (Geometry.Combine((Geometry) widenedPathGeometry1, (Geometry) widenedPathGeometry2, GeometryCombineMode.Intersect, (Transform) null, 0.5, ToleranceType.Absolute).Figures.Select<PathFigure, Point>((Func<PathFigure, Point>) (c =>
        {
          Rect bounds = new PathGeometry((IEnumerable<PathFigure>) new PathFigure[1]
          {
            c.Clone()
          }).Bounds;
          return new Point(bounds.X + bounds.Width / 2.0, bounds.Y + bounds.Height / 2.0);
        })).ToArray<Point>().Length != 0)
          return true;
        break;
    }
    return false;
  }

  public FS_POINTF[] MathGetPoints(
    PdfLinePointCollection<PdfInkAnnotation> ink,
    Point point,
    float radius,
    PdfInkAnnotation inkannot,
    PdfPage page)
  {
    EllipseGeometry ellipseGeometry = new EllipseGeometry()
    {
      Center = point,
      RadiusX = (double) radius + 0.2,
      RadiusY = (double) radius + 0.2
    };
    PolyLineSegment polyLineSegment = new PolyLineSegment();
    PathFigure pathFigure = new PathFigure()
    {
      StartPoint = ink[0].ToPoint(),
      Segments = {
        (PathSegment) polyLineSegment
      }
    };
    for (int index = 1; index < ink.Count; ++index)
      polyLineSegment.Points.Add(ink[index].ToPoint());
    PathGeometry pathGeometry = new PathGeometry()
    {
      Figures = {
        pathFigure
      }
    };
    PathGeometry widenedPathGeometry1 = pathGeometry.GetWidenedPathGeometry(new Pen((Brush) Brushes.Black, 0.01), 0.5, ToleranceType.Absolute);
    if (ellipseGeometry.FillContainsWithDetail((Geometry) pathGeometry, 0.5, ToleranceType.Absolute) != IntersectionDetail.Intersects)
      return (FS_POINTF[]) null;
    PathGeometry widenedPathGeometry2 = ellipseGeometry.GetWidenedPathGeometry(new Pen((Brush) Brushes.Black, 0.01), 0.5, ToleranceType.Absolute);
    Point[] array = Geometry.Combine((Geometry) widenedPathGeometry1, (Geometry) widenedPathGeometry2, GeometryCombineMode.Intersect, (Transform) null, 0.5, ToleranceType.Absolute).Figures.Select<PathFigure, Point>((Func<PathFigure, Point>) (c =>
    {
      Rect bounds = new PathGeometry((IEnumerable<PathFigure>) new PathFigure[1]
      {
        c.Clone()
      }).Bounds;
      return new Point(bounds.X + bounds.Width / 2.0, bounds.Y + bounds.Height / 2.0);
    })).ToArray<Point>();
    FS_POINTF[] points = new FS_POINTF[array.Length];
    for (int index = 0; index < array.Length; ++index)
      points[index] = new FS_POINTF(array[index].X, array[index].Y);
    return points;
  }

  public void AddToChangeRecord(PdfInkAnnotation inkannot, int i)
  {
    this.changeRecord.Add((InkAnnotation) AnnotationFactory.Create((PdfAnnotation) inkannot));
    this.changeRecord[this.changeRecord.Count - 1].AnnotIndex = i;
  }

  public void AddToRecord2(PdfInkAnnotation inkannot, int i)
  {
    this.record2.Add((InkAnnotation) AnnotationFactory.Create((PdfAnnotation) inkannot));
    this.record2[this.record2.Count - 1].AnnotIndex = i;
  }

  public void AddToRecord3(PdfInkAnnotation inkannot, int i)
  {
    this.record.Add((InkAnnotation) AnnotationFactory.Create((PdfAnnotation) inkannot));
    this.record[this.record.Count - 1].AnnotIndex = i;
  }

  public async Task RedoUndoRecord(MainViewModel vm, PdfPage page)
  {
    int pageIndex = page.PageIndex;
    await vm.OperationManager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      MainViewModel dataContext = PDFKit.PdfControl.GetPdfControl(doc).DataContext as MainViewModel;
      PdfPage page1 = doc.Pages[pageIndex];
      this.pdfAnnotations = page1.Annots;
      List<IndexedPdfInkAnnotation> pdfinks = this.pdfInks.FindAll((Predicate<IndexedPdfInkAnnotation>) (x => x.Index < this.pdfAnnotations.Count));
      for (int i = 0; i < pdfinks.Count; i++)
      {
        List<InkAnnotation> all = this.record.FindAll((Predicate<InkAnnotation>) (X => X.AnnotIndex == pdfinks[i].Index));
        if (all.Count > 0)
        {
          if (this.pdfAnnotations != null)
          {
            try
            {
              if (this.pdfAnnotations[pdfinks[i].Index] is PdfInkAnnotation pdfAnnotation2)
              {
                this.AddToRecord2(pdfAnnotation2, pdfinks[i].Index);
                pdfAnnotation2.InkList = new PdfInkPointCollection();
                foreach (System.Collections.Generic.IReadOnlyList<FS_POINTF> ink in (IEnumerable<System.Collections.Generic.IReadOnlyList<FS_POINTF>>) all[all.Count - 1].InkList)
                {
                  PdfLinePointCollection<PdfInkAnnotation> linePointCollection = new PdfLinePointCollection<PdfInkAnnotation>();
                  foreach (FS_POINTF fsPointf in (IEnumerable<FS_POINTF>) ink)
                    linePointCollection.Add(fsPointf);
                  pdfAnnotation2.InkList.Add(linePointCollection);
                }
                pdfAnnotation2.TryRedrawAnnotation();
                this.record.Remove(all[all.Count - 1]);
              }
            }
            catch
            {
            }
          }
        }
      }
      dataContext?.PageEditors?.NotifyPageAnnotationChanged(page.PageIndex);
      await page1.TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      MainViewModel dataContext = PDFKit.PdfControl.GetPdfControl(doc).DataContext as MainViewModel;
      PdfPage page2 = doc.Pages[pageIndex];
      this.pdfAnnotations = page2.Annots;
      List<IndexedPdfInkAnnotation> pdfinks = this.pdfInks.FindAll((Predicate<IndexedPdfInkAnnotation>) (x => x.Index < this.pdfAnnotations.Count));
      for (int i = 0; i < pdfinks.Count; i++)
      {
        List<InkAnnotation> all = this.record2.FindAll((Predicate<InkAnnotation>) (X => X.AnnotIndex == pdfinks[i].Index));
        if (all.Count > 0 && this.pdfAnnotations != null && this.pdfAnnotations[pdfinks[i].Index] is PdfInkAnnotation pdfAnnotation4)
        {
          this.AddToRecord3(pdfAnnotation4, pdfinks[i].Index);
          pdfAnnotation4.InkList = new PdfInkPointCollection();
          foreach (System.Collections.Generic.IReadOnlyList<FS_POINTF> ink in (IEnumerable<System.Collections.Generic.IReadOnlyList<FS_POINTF>>) all[all.Count - 1].InkList)
          {
            PdfLinePointCollection<PdfInkAnnotation> linePointCollection = new PdfLinePointCollection<PdfInkAnnotation>();
            foreach (FS_POINTF fsPointf in (IEnumerable<FS_POINTF>) ink)
              linePointCollection.Add(fsPointf);
            pdfAnnotation4.InkList.Add(linePointCollection);
          }
          pdfAnnotation4.TryRedrawAnnotation();
          this.record2.Remove(all[all.Count - 1]);
        }
      }
      dataContext?.PageEditors?.NotifyPageAnnotationChanged(page2.PageIndex);
      await page2.TryRedrawPageAsync();
    }));
  }

  public FS_POINTF GetIntersectionPoint(
    FS_POINTF point1,
    FS_POINTF point2,
    FS_POINTF center,
    double radius)
  {
    if (this.Distance(center, point1, point2) > radius)
      return new FS_POINTF(float.NaN, float.NaN);
    if ((double) Math.Abs(point2.X - point1.X) < 1E-06)
    {
      double x = (double) point1.X;
      double num = Math.Sqrt(Math.Pow(radius, 2.0) - Math.Pow((double) point1.X - (double) center.X, 2.0));
      double y = (double) point1.Y - (double) point2.Y <= 0.0 ? (double) center.Y - num : num + (double) center.Y;
      return new FS_POINTF((float) x, (float) y);
    }
    if ((double) Math.Abs(point2.Y - point1.Y) < 1E-06)
    {
      double y = (double) point2.Y;
      double num = Math.Sqrt(Math.Pow(radius, 2.0) - Math.Pow((double) point1.Y - (double) center.Y, 2.0));
      return new FS_POINTF((double) point1.X - (double) point2.X <= 0.0 ? center.X - (float) num : center.X + (float) num, (float) y);
    }
    double k = ((double) point2.Y - (double) point1.Y) / ((double) point2.X - (double) point1.X);
    double b = (double) point1.Y - k * (double) point1.X;
    double y1 = (double) center.Y;
    FS_POINTF[] intersectionPoints = PDFEraserUtil.GetIntersectionPoints(k, b, (double) center.X, (double) center.Y, radius);
    if (intersectionPoints.Length == 2)
    {
      if ((double) intersectionPoints[0].X >= (double) Math.Min(point1.X, point2.X) && (double) intersectionPoints[0].X <= (double) Math.Max(point1.X, point2.X) && (double) intersectionPoints[0].Y >= (double) Math.Min(point1.Y, point2.Y) && (double) intersectionPoints[0].Y <= (double) Math.Max(point1.Y, point2.Y))
        return intersectionPoints[0];
      return (double) intersectionPoints[1].X >= (double) Math.Min(point1.X, point2.X) && (double) intersectionPoints[1].X <= (double) Math.Max(point1.X, point2.X) && (double) intersectionPoints[1].Y >= (double) Math.Min(point1.Y, point2.Y) && (double) intersectionPoints[1].Y <= (double) Math.Max(point1.Y, point2.Y) ? intersectionPoints[1] : new FS_POINTF(float.NaN, float.NaN);
    }
    if (intersectionPoints.Length != 1)
      return new FS_POINTF(float.NaN, float.NaN);
    return (double) intersectionPoints[0].X >= (double) Math.Min(point1.X, point2.X) && (double) intersectionPoints[0].X <= (double) Math.Max(point1.X, point2.X) && (double) intersectionPoints[0].Y >= (double) Math.Min(point1.Y, point2.Y) && (double) intersectionPoints[0].Y <= (double) Math.Max(point1.Y, point2.Y) ? intersectionPoints[0] : new FS_POINTF(float.NaN, float.NaN);
  }

  public static FS_POINTF[] GetIntersectionPoints(
    double k,
    double b,
    double xc,
    double yc,
    double r)
  {
    double num1 = k * k + 1.0;
    double num2 = 2.0 * (k * b - k * yc - xc);
    double num3 = yc * yc + b * b + xc * xc - 2.0 * b * yc - r * r;
    double d = num2 * num2 - 4.0 * num1 * num3;
    if (d < 0.0)
      return new FS_POINTF[0];
    if (d == 0.0)
    {
      double x = -num2 / (2.0 * num1);
      double y = k * x + b;
      return new FS_POINTF[1]
      {
        new FS_POINTF((float) x, (float) y)
      };
    }
    double x1 = (-num2 - Math.Sqrt(d)) / (2.0 * num1);
    double y1 = k * x1 + b;
    double x2 = (-num2 + Math.Sqrt(d)) / (2.0 * num1);
    double y2 = k * x2 + b;
    return new FS_POINTF[2]
    {
      new FS_POINTF((float) x1, (float) y1),
      new FS_POINTF((float) x2, (float) y2)
    };
  }

  private double Distance(FS_POINTF point1, FS_POINTF point2)
  {
    return Math.Sqrt(Math.Pow((double) point1.X - (double) point2.X, 2.0) + Math.Pow((double) point1.Y - (double) point2.Y, 2.0));
  }

  private double Distance(FS_POINTF point, FS_POINTF point1, FS_POINTF point2)
  {
    double x1 = (double) point.X;
    double y1 = (double) point.Y;
    double x2 = (double) point1.X;
    double y2 = (double) point1.Y;
    double x3 = (double) point2.X;
    double y3 = (double) point2.Y;
    double num1 = y3 - y2;
    double num2 = x2 - x3;
    double num3 = x3 * y2 - x2 * y3;
    return Math.Abs(num1 * x1 + num2 * y1 + num3) / Math.Sqrt(num1 * num1 + num2 * num2);
  }
}
