// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ImageControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Win32;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using pdfeditor.Controls.Screenshots;
using pdfeditor.Models.PageContents;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using pdfeditor.Views;
using PDFKit;
using PDFKit.Utils;
using PDFKit.Utils.PageContents;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls;

public partial class ImageControl : UserControl, IComponentConnector
{
  private AnnotationCanvas annotationCanvas;
  private PdfViewerContextMenu contextMenu;
  private PdfDocument Document;
  private int Pageindex = -1;
  public int imageindex = -1;
  private PdfViewer PdfViewer;
  public FS_RECTF ImageRect;
  public bool Isseleted;
  private PageImageModel PageImageModel;
  public bool IsMoved;
  private double Left;
  private double Top;
  public ImageControl.MousePosition mousePosition = ImageControl.MousePosition.None;
  public System.Windows.Point clickStartPosition;
  public bool ImageControlState;
  internal Grid selectedBoder;
  internal System.Windows.Shapes.Rectangle Imageborder2;
  internal Grid editorBoder;
  internal System.Windows.Shapes.Rectangle Imageborder;
  internal System.Windows.Shapes.Rectangle Topleft;
  internal System.Windows.Shapes.Rectangle Topright;
  internal System.Windows.Shapes.Rectangle Bottomleft;
  internal System.Windows.Shapes.Rectangle Bottomright;
  internal System.Windows.Shapes.Rectangle Topcenter;
  internal System.Windows.Shapes.Rectangle Leftcenter;
  internal System.Windows.Shapes.Rectangle Bottomcenter;
  internal System.Windows.Shapes.Rectangle Rightcenter;
  internal Grid Siderbar;
  internal Grid SiderEditorActionBtn;
  internal Button editorImagebtn;
  internal Grid SiderEditorbar;
  internal Button exprotbtn;
  internal Button RotateBtn;
  internal Button OcrBtn;
  internal Button DeleteBtn;
  internal Button ReplaceBtn;
  internal Button quitBtn;
  private bool _contentLoaded;

  private MainViewModel VM => this.annotationCanvas.DataContext as MainViewModel;

  public ImageControl() => this.InitializeComponent();

  public void CreateImageborder(
    AnnotationCanvas annotationCanvas,
    PdfDocument document,
    int PageIndex,
    int Imageindex,
    PdfViewer pdfViewer)
  {
    this.clickStartPosition = new System.Windows.Point(0.0, 0.0);
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
    this.Document = document;
    this.Pageindex = PageIndex;
    this.imageindex = Imageindex;
    this.PdfViewer = pdfViewer;
    PdfImageObject pageObject = this.Document.Pages[this.Pageindex].PageObjects[this.imageindex] as PdfImageObject;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
    this.ImageRect = new FS_RECTF()
    {
      right = pageObject.BoundingBox.right,
      left = pageObject.BoundingBox.left,
      top = pageObject.BoundingBox.top,
      bottom = pageObject.BoundingBox.bottom
    };
    Rect clientRect;
    pdfControl.TryGetClientRect(this.Pageindex, this.ImageRect, out clientRect);
    this.Imageborder.Width = clientRect.Width;
    this.Imageborder.Height = clientRect.Height;
    this.Left = clientRect.Left;
    this.Top = clientRect.Top;
    if (pdfControl != null)
    {
      this.Siderbar.VerticalAlignment = VerticalAlignment.Top;
      if (pdfControl.ActualWidth - this.Left - this.Imageborder.Width <= 38.0 && this.Top < 0.0 && Math.Abs(this.Top) + 200.0 < this.Imageborder.Height)
        this.Siderbar.Margin = new Thickness(-170.0, Math.Abs(this.Top), 0.0, 0.0);
      else if (pdfControl.ActualWidth - this.Left - this.Imageborder.Width <= 38.0 && this.Top < 0.0 && Math.Abs(this.Top) + 200.0 > this.Imageborder.Height)
      {
        this.Siderbar.Margin = new Thickness(-170.0, 0.0, 0.0, 0.0);
        this.Siderbar.VerticalAlignment = VerticalAlignment.Bottom;
      }
      else if (pdfControl.ActualWidth - this.Left - this.Imageborder.Width <= 38.0)
        this.Siderbar.Margin = new Thickness(-170.0, 0.0, 0.0, 0.0);
      else if (this.Top < 0.0 && Math.Abs(this.Top) + 200.0 < this.Imageborder.Height)
        this.Siderbar.Margin = new Thickness(0.0, Math.Abs(this.Top), 0.0, 0.0);
      else if (this.Top < 0.0 && Math.Abs(this.Top) + 200.0 > this.Imageborder.Height)
      {
        this.Siderbar.Margin = new Thickness(0.0);
        this.Siderbar.VerticalAlignment = VerticalAlignment.Bottom;
      }
      else
        this.Siderbar.Margin = new Thickness(0.0);
    }
    this.layoutImageBorder();
    Canvas.SetLeft((UIElement) this, clientRect.Left);
    Canvas.SetTop((UIElement) this, clientRect.Top);
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ImageControl_IsVisibleChanged);
  }

  private void Topleft_MouseMove(object sender, MouseEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    System.Windows.Point position = e.GetPosition((IInputElement) this);
    this.IsMoved = true;
    this.Imageborder.StrokeDashArray = new DoubleCollection()
    {
      2.0,
      2.0
    };
    float num1 = (float) (position.X - this.clickStartPosition.X);
    float num2 = (float) (position.Y - this.clickStartPosition.Y);
    this.Imageborder.Width += (double) num1;
    this.Imageborder.Height += (double) num2;
  }

  private void layoutImageBorder()
  {
    if (this.Imageborder.Height < 210.0)
    {
      this.Leftcenter.VerticalAlignment = VerticalAlignment.Top;
      this.Rightcenter.VerticalAlignment = VerticalAlignment.Top;
      this.Leftcenter.Margin = new Thickness(-4.0, this.Imageborder.Height / 2.0, 4.0, 0.0);
      this.Rightcenter.Margin = new Thickness(4.0, this.Imageborder.Height / 2.0, -4.0, 0.0);
      this.Bottomleft.VerticalAlignment = VerticalAlignment.Top;
      this.Bottomright.VerticalAlignment = VerticalAlignment.Top;
      this.Bottomleft.Margin = new Thickness(-4.0, this.Imageborder.Height - 2.0, 0.0, -4.0);
      this.Bottomright.Margin = new Thickness(0.0, this.Imageborder.Height - 2.0, -4.0, -4.0);
      this.Bottomcenter.VerticalAlignment = VerticalAlignment.Top;
      this.Bottomcenter.Margin = new Thickness(0.0, this.Imageborder.Height - 2.0, 0.0, -4.0);
    }
    else
    {
      this.Bottomcenter.VerticalAlignment = VerticalAlignment.Bottom;
      this.Bottomleft.VerticalAlignment = VerticalAlignment.Bottom;
      this.Bottomright.VerticalAlignment = VerticalAlignment.Bottom;
      this.Leftcenter.VerticalAlignment = VerticalAlignment.Center;
      this.Rightcenter.VerticalAlignment = VerticalAlignment.Center;
      this.Leftcenter.Margin = new Thickness(-4.0, 0.0, 4.0, 0.0);
      this.Rightcenter.Margin = new Thickness(4.0, 0.0, -4.0, 0.0);
    }
    this.Imageborder2.Width = this.Imageborder.Width;
    this.Imageborder2.Height = this.Imageborder.Height;
  }

  private void MouseButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.Siderbar.Visibility = Visibility.Collapsed;
    if (sender == this.Topleft)
      this.mousePosition = ImageControl.MousePosition.topLeft;
    else if (sender == this.Topright)
      this.mousePosition = ImageControl.MousePosition.topRight;
    else if (sender == this.Bottomright)
      this.mousePosition = ImageControl.MousePosition.bottomRight;
    else if (sender == this.Bottomleft)
      this.mousePosition = ImageControl.MousePosition.bottomLeft;
    else if (sender == this.Topcenter)
      this.mousePosition = ImageControl.MousePosition.topCenter;
    else if (sender == this.Leftcenter)
      this.mousePosition = ImageControl.MousePosition.leftCenter;
    else if (sender == this.Rightcenter)
    {
      this.mousePosition = ImageControl.MousePosition.rightCenter;
    }
    else
    {
      if (sender != this.Bottomcenter)
        return;
      this.mousePosition = ImageControl.MousePosition.bottomCenter;
    }
  }

  private bool IsShiftPressedInternal
  {
    get
    {
      return (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) > KeyStates.None || (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) > KeyStates.None;
    }
  }

  public void ImageControlReSizeImage(System.Windows.Point point)
  {
    this.IsMoved = true;
    if (this.clickStartPosition == new System.Windows.Point(0.0, 0.0))
      this.clickStartPosition = point;
    this.Imageborder.StrokeDashArray = new DoubleCollection()
    {
      2.0,
      2.0
    };
    float num1 = (float) (point.X - this.clickStartPosition.X);
    float num2 = (float) (point.Y - this.clickStartPosition.Y);
    double width = this.Imageborder.Width;
    double height = this.Imageborder.Height;
    this.clickStartPosition = point;
    this.layoutImageBorder();
    if (width != 0.0 && height != 0.0 && this.IsShiftPressedInternal)
    {
      double num3 = width / height;
      num2 = num1 / (float) num3;
      if (this.mousePosition == ImageControl.MousePosition.topRight || this.mousePosition == ImageControl.MousePosition.bottomLeft)
        num2 = -num1 / (float) num3;
    }
    double num4;
    double num5;
    if (this.mousePosition == ImageControl.MousePosition.topLeft)
    {
      if ((num4 = width - (double) num1) <= 0.0 || (num5 = height - (double) num2) <= 0.0)
        return;
      this.Imageborder.Width -= (double) num1;
      this.Imageborder.Height -= (double) num2;
      this.Top += (double) num2;
      this.Left += (double) num1;
      Canvas.SetTop((UIElement) this, this.Top);
      Canvas.SetLeft((UIElement) this, this.Left);
    }
    else if (this.mousePosition == ImageControl.MousePosition.topRight)
    {
      if ((num4 = width + (double) num1) <= 0.0 || (num5 = height - (double) num2) <= 0.0)
        return;
      this.Imageborder.Width += (double) num1;
      this.Imageborder.Height -= (double) num2;
      this.Top += (double) num2;
      Canvas.SetTop((UIElement) this, this.Top);
    }
    else if (this.mousePosition == ImageControl.MousePosition.bottomLeft)
    {
      if ((num4 = width - (double) num1) <= 0.0 || (num5 = height + (double) num2) <= 0.0)
        return;
      this.Imageborder.Width -= (double) num1;
      this.Imageborder.Height += (double) num2;
      this.Left += (double) num1;
      Canvas.SetLeft((UIElement) this, this.Left);
    }
    else if (this.mousePosition == ImageControl.MousePosition.bottomRight)
    {
      if ((num4 = width + (double) num1) <= 0.0 || (num5 = height + (double) num2) <= 0.0)
        return;
      this.Imageborder.Width += (double) num1;
      this.Imageborder.Height += (double) num2;
    }
    else if (this.mousePosition == ImageControl.MousePosition.topCenter)
    {
      if ((num5 = height - (double) num2) <= 0.5)
        return;
      this.Imageborder.Height -= (double) num2;
      this.Top += (double) num2;
      Canvas.SetTop((UIElement) this, this.Top);
    }
    else if (this.mousePosition == ImageControl.MousePosition.bottomCenter)
    {
      if ((num5 = height + (double) num2) <= 0.0)
        return;
      this.Imageborder.Height += (double) num2;
    }
    else if (this.mousePosition == ImageControl.MousePosition.leftCenter)
    {
      if ((num4 = width - (double) num1) <= 0.0)
        return;
      this.Imageborder.Width -= (double) num1;
      this.Left += (double) num1;
      Canvas.SetLeft((UIElement) this, this.Left);
    }
    else
    {
      if (this.mousePosition != ImageControl.MousePosition.rightCenter || (num4 = width + (double) num1) <= 0.0)
        return;
      this.Imageborder.Width += (double) num1;
    }
  }

  public async void ImageControlMoveImage(System.Windows.Point point)
  {
    ImageControl element = this;
    element.IsMoved = false;
    PdfImageObject imageObject = (PdfImageObject) null;
    PDFKit.PdfControl pdfControl;
    if (element.mousePosition != ImageControl.MousePosition.None)
    {
      element.clickStartPosition = new System.Windows.Point(0.0, 0.0);
      Rect clientRect1 = new Rect()
      {
        Height = element.Imageborder.Height,
        Width = element.Imageborder.Width,
        X = element.Left,
        Y = element.Top
      };
      pdfControl = PDFKit.PdfControl.GetPdfControl(element.Document);
      FS_RECTF pageRect;
      pdfControl.TryGetPageRect(element.Pageindex, clientRect1, out pageRect);
      if (element.Isseleted)
        imageObject = element.Document.Pages[element.Pageindex].PageObjects[element.Document.Pages[element.Pageindex].PageObjects.Count - 1] as PdfImageObject;
      else if (element.Pageindex >= 0 && element.imageindex >= 0)
        imageObject = element.Document.Pages[element.Pageindex].PageObjects[element.imageindex] as PdfImageObject;
      float distanceX = element.ImageRect.Width - pageRect.Width;
      float distanceY = element.ImageRect.Height - pageRect.Height;
      if (imageObject != null)
      {
        FS_POINTF fsMatrix = element.GetFS_MATRIX(imageObject, distanceX, distanceY);
        float sx = pageRect.Width / element.ImageRect.Width;
        float sy = pageRect.Height / element.ImageRect.Height;
        FS_MATRIX matrix = imageObject.Matrix;
        double right = (double) imageObject.BoundingBox.right;
        FS_RECTF fsRectf1 = imageObject.BoundingBox;
        double num1 = (double) fsRectf1.Width / 2.0;
        float num2 = (float) (right - num1);
        double top = (double) imageObject.BoundingBox.top;
        fsRectf1 = imageObject.BoundingBox;
        double num3 = (double) fsRectf1.Height / 2.0;
        float num4 = (float) (top - num3);
        matrix.Translate(-num2, -num4);
        matrix.Scale(sx, sy);
        matrix.Translate(fsMatrix.X, fsMatrix.Y);
        await element.ImageOperationCmd(imageObject, element.Pageindex, element.imageindex, matrix);
        PdfImageObject pageObject = element.Document.Pages[element.Pageindex].PageObjects[element.imageindex] as PdfImageObject;
        ImageControl imageControl = element;
        fsRectf1 = new FS_RECTF();
        fsRectf1.right = pageObject.BoundingBox.right;
        fsRectf1.left = pageObject.BoundingBox.left;
        fsRectf1.top = pageObject.BoundingBox.top;
        fsRectf1.bottom = pageObject.BoundingBox.bottom;
        FS_RECTF fsRectf2 = fsRectf1;
        imageControl.ImageRect = fsRectf2;
        Rect clientRect2;
        pdfControl.TryGetClientRect(element.Pageindex, element.ImageRect, out clientRect2);
        element.Top = clientRect2.Top;
        element.Left = clientRect2.Left;
        element.UpdateImageborder();
      }
      pdfControl = (PDFKit.PdfControl) null;
    }
    else
    {
      pdfControl = PDFKit.PdfControl.GetPdfControl(element.Document);
      System.Windows.Point pagePoint1;
      pdfControl.TryGetPagePoint(element.Pageindex, point, out pagePoint1);
      System.Windows.Point pagePoint2;
      pdfControl.TryGetPagePoint(element.Pageindex, element.clickStartPosition, out pagePoint2);
      float num5 = (float) (pagePoint2.X - pagePoint1.X);
      float num6 = (float) (pagePoint2.Y - pagePoint1.Y);
      if (element.Isseleted)
        imageObject = element.Document.Pages[element.Pageindex].PageObjects[element.Document.Pages[element.Pageindex].PageObjects.Count - 1] as PdfImageObject;
      else if (element.Pageindex >= 0 && element.imageindex >= 0)
        imageObject = element.Document.Pages[element.Pageindex].PageObjects[element.imageindex] as PdfImageObject;
      if (imageObject != null)
      {
        FS_MATRIX matrix = imageObject.Matrix;
        matrix.Translate(-num5, -num6);
        await element.ImageOperationCmd(imageObject, element.Pageindex, element.imageindex, matrix);
        PdfImageObject pageObject = element.Document.Pages[element.Pageindex].PageObjects[element.imageindex] as PdfImageObject;
        element.ImageRect = new FS_RECTF()
        {
          right = pageObject.BoundingBox.right,
          left = pageObject.BoundingBox.left,
          top = pageObject.BoundingBox.top,
          bottom = pageObject.BoundingBox.bottom
        };
        Rect clientRect;
        pdfControl.TryGetClientRect(element.Pageindex, element.ImageRect, out clientRect);
        element.Top = clientRect.Top;
        element.Left = clientRect.Left;
        element.UpdateImageborder();
      }
      pdfControl = (PDFKit.PdfControl) null;
    }
    if (!element.Isseleted)
      return;
    element.Isseleted = false;
    element.imageindex = element.Document.Pages[element.Pageindex].PageObjects.Count - 1;
    Rect clientRect3;
    PDFKit.PdfControl.GetPdfControl(element.Document).TryGetClientRect(element.Pageindex, element.ImageRect, out clientRect3);
    element.Imageborder.Width = clientRect3.Width;
    element.Imageborder.Height = clientRect3.Height;
    element.layoutImageBorder();
    element.Left = clientRect3.Left;
    element.Top = clientRect3.Top;
    Canvas.SetLeft((UIElement) element, clientRect3.Left);
    Canvas.SetTop((UIElement) element, clientRect3.Top);
  }

  private FS_POINTF GetFS_MATRIX(PdfImageObject imageObject, float distanceX, float distanceY)
  {
    ImageControl.MousePosition mousePosition = this.mousePosition;
    FS_POINTF fsMatrix = new FS_POINTF(0.0f, 0.0f);
    if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate90)
    {
      if (this.mousePosition == ImageControl.MousePosition.topLeft)
        mousePosition = ImageControl.MousePosition.bottomLeft;
      else if (this.mousePosition == ImageControl.MousePosition.topRight)
        mousePosition = ImageControl.MousePosition.topLeft;
      else if (this.mousePosition == ImageControl.MousePosition.bottomRight)
        mousePosition = ImageControl.MousePosition.topRight;
      else if (this.mousePosition == ImageControl.MousePosition.bottomLeft)
        mousePosition = ImageControl.MousePosition.bottomRight;
      else if (this.mousePosition == ImageControl.MousePosition.topCenter)
        mousePosition = ImageControl.MousePosition.leftCenter;
      else if (this.mousePosition == ImageControl.MousePosition.leftCenter)
        mousePosition = ImageControl.MousePosition.bottomCenter;
      else if (this.mousePosition == ImageControl.MousePosition.rightCenter)
        mousePosition = ImageControl.MousePosition.topCenter;
      else if (this.mousePosition == ImageControl.MousePosition.bottomCenter)
        mousePosition = ImageControl.MousePosition.rightCenter;
    }
    else if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate180)
    {
      if (this.mousePosition == ImageControl.MousePosition.topLeft)
        mousePosition = ImageControl.MousePosition.bottomRight;
      else if (this.mousePosition == ImageControl.MousePosition.topRight)
        mousePosition = ImageControl.MousePosition.bottomLeft;
      else if (this.mousePosition == ImageControl.MousePosition.bottomRight)
        mousePosition = ImageControl.MousePosition.topLeft;
      else if (this.mousePosition == ImageControl.MousePosition.bottomLeft)
        mousePosition = ImageControl.MousePosition.topRight;
      else if (this.mousePosition == ImageControl.MousePosition.topCenter)
        mousePosition = ImageControl.MousePosition.bottomCenter;
      else if (this.mousePosition == ImageControl.MousePosition.leftCenter)
        mousePosition = ImageControl.MousePosition.rightCenter;
      else if (this.mousePosition == ImageControl.MousePosition.rightCenter)
        mousePosition = ImageControl.MousePosition.leftCenter;
      else if (this.mousePosition == ImageControl.MousePosition.bottomCenter)
        mousePosition = ImageControl.MousePosition.topCenter;
    }
    else if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate270)
    {
      if (this.mousePosition == ImageControl.MousePosition.topLeft)
        mousePosition = ImageControl.MousePosition.topRight;
      else if (this.mousePosition == ImageControl.MousePosition.topRight)
        mousePosition = ImageControl.MousePosition.bottomRight;
      else if (this.mousePosition == ImageControl.MousePosition.bottomRight)
        mousePosition = ImageControl.MousePosition.bottomLeft;
      else if (this.mousePosition == ImageControl.MousePosition.bottomLeft)
        mousePosition = ImageControl.MousePosition.topLeft;
      else if (this.mousePosition == ImageControl.MousePosition.topCenter)
        mousePosition = ImageControl.MousePosition.rightCenter;
      else if (this.mousePosition == ImageControl.MousePosition.leftCenter)
        mousePosition = ImageControl.MousePosition.topCenter;
      else if (this.mousePosition == ImageControl.MousePosition.rightCenter)
        mousePosition = ImageControl.MousePosition.bottomCenter;
      else if (this.mousePosition == ImageControl.MousePosition.bottomCenter)
        mousePosition = ImageControl.MousePosition.leftCenter;
    }
    switch (mousePosition)
    {
      case ImageControl.MousePosition.topLeft:
        fsMatrix.X = imageObject.BoundingBox.right - (float) (((double) imageObject.BoundingBox.Width - (double) distanceX) / 2.0);
        fsMatrix.Y = (float) ((double) imageObject.BoundingBox.top - (double) distanceY - ((double) imageObject.BoundingBox.Height - (double) distanceY) / 2.0);
        return fsMatrix;
      case ImageControl.MousePosition.topRight:
        fsMatrix.X = (float) ((double) imageObject.BoundingBox.right - (double) distanceX - ((double) imageObject.BoundingBox.Width - (double) distanceX) / 2.0);
        fsMatrix.Y = (float) ((double) imageObject.BoundingBox.top - (double) distanceY - ((double) imageObject.BoundingBox.Height - (double) distanceY) / 2.0);
        return fsMatrix;
      case ImageControl.MousePosition.bottomLeft:
        fsMatrix.X = imageObject.BoundingBox.right - (float) (((double) imageObject.BoundingBox.Width - (double) distanceX) / 2.0);
        fsMatrix.Y = imageObject.BoundingBox.top - (float) (((double) imageObject.BoundingBox.Height - (double) distanceY) / 2.0);
        return fsMatrix;
      case ImageControl.MousePosition.bottomRight:
        fsMatrix.X = (float) ((double) imageObject.BoundingBox.right - (double) distanceX - ((double) imageObject.BoundingBox.Width - (double) distanceX) / 2.0);
        fsMatrix.Y = imageObject.BoundingBox.top - (float) (((double) imageObject.BoundingBox.Height - (double) distanceY) / 2.0);
        return fsMatrix;
      case ImageControl.MousePosition.topCenter:
        fsMatrix.X = imageObject.BoundingBox.right - imageObject.BoundingBox.Width / 2f;
        fsMatrix.Y = (float) ((double) imageObject.BoundingBox.top - (double) distanceY - ((double) imageObject.BoundingBox.Height - (double) distanceY) / 2.0);
        return fsMatrix;
      case ImageControl.MousePosition.leftCenter:
        fsMatrix.X = imageObject.BoundingBox.right - (float) (((double) imageObject.BoundingBox.Width - (double) distanceX) / 2.0);
        fsMatrix.Y = imageObject.BoundingBox.top - imageObject.BoundingBox.Height / 2f;
        return fsMatrix;
      case ImageControl.MousePosition.rightCenter:
        fsMatrix.X = (float) ((double) imageObject.BoundingBox.right - (double) distanceX - ((double) imageObject.BoundingBox.Width - (double) distanceX) / 2.0);
        fsMatrix.Y = imageObject.BoundingBox.top - imageObject.BoundingBox.Height / 2f;
        return fsMatrix;
      case ImageControl.MousePosition.bottomCenter:
        fsMatrix.X = imageObject.BoundingBox.right - imageObject.BoundingBox.Width / 2f;
        fsMatrix.Y = imageObject.BoundingBox.top - (float) (((double) imageObject.BoundingBox.Height - (double) distanceY) / 2.0);
        return fsMatrix;
      default:
        return fsMatrix;
    }
  }

  private void ImageControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    this.Isseleted = false;
    this.IsMoved = false;
  }

  public void MoveImageBorder(System.Windows.Point point)
  {
    this.IsMoved = true;
    this.Imageborder.StrokeDashArray = new DoubleCollection()
    {
      2.0,
      2.0
    };
    if (this.clickStartPosition == new System.Windows.Point(0.0, 0.0))
      this.clickStartPosition = point;
    float num1 = (float) (point.X - this.clickStartPosition.X);
    float num2 = (float) (point.Y - this.clickStartPosition.Y);
    Rect clientRect;
    PDFKit.PdfControl.GetPdfControl(this.Document).TryGetClientRect(this.Pageindex, this.ImageRect, out clientRect);
    double length1 = clientRect.Left + (double) num1;
    double length2 = clientRect.Top + (double) num2;
    Canvas.SetLeft((UIElement) this, length1);
    Canvas.SetTop((UIElement) this, length2);
  }

  public void UpdateImageborder()
  {
    if (this.Visibility == Visibility.Visible && this.VM.AnnotationMode != AnnotationMode.None)
    {
      this.Visibility = Visibility.Collapsed;
    }
    else
    {
      if (this.Visibility != Visibility.Visible)
        return;
      FS_RECTF imageRect = this.ImageRect;
      this.clickStartPosition = new System.Windows.Point(0.0, 0.0);
      this.mousePosition = ImageControl.MousePosition.None;
      this.IsMoved = false;
      this.Siderbar.Visibility = Visibility.Visible;
      this.Imageborder.StrokeDashArray = new DoubleCollection();
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
      Rect clientRect;
      pdfControl.TryGetClientRect(this.Pageindex, this.ImageRect, out clientRect);
      this.Imageborder.Width = clientRect.Width;
      this.Imageborder.Height = clientRect.Height;
      this.Top = clientRect.Top;
      this.Left = clientRect.Left;
      if (pdfControl != null)
      {
        this.Siderbar.VerticalAlignment = VerticalAlignment.Top;
        if (pdfControl.ActualWidth - this.Left - this.Imageborder.Width <= 38.0 && this.Top < 0.0 && Math.Abs(this.Top) + 200.0 < this.Imageborder.Height)
          this.Siderbar.Margin = new Thickness(-170.0, Math.Abs(this.Top), 0.0, 0.0);
        else if (pdfControl.ActualWidth - this.Left - this.Imageborder.Width <= 38.0 && this.Top < 0.0 && Math.Abs(this.Top) + 200.0 > this.Imageborder.Height)
        {
          this.Siderbar.Margin = new Thickness(-170.0, 0.0, 0.0, 0.0);
          this.Siderbar.VerticalAlignment = VerticalAlignment.Bottom;
        }
        else if (pdfControl.ActualWidth - this.Left - this.Imageborder.Width <= 38.0)
          this.Siderbar.Margin = new Thickness(-170.0, 0.0, 0.0, 0.0);
        else if (this.Top < 0.0 && Math.Abs(this.Top) + 200.0 < this.Imageborder.Height)
          this.Siderbar.Margin = new Thickness(0.0, Math.Abs(this.Top), 0.0, 0.0);
        else if (this.Top < 0.0 && Math.Abs(this.Top) + 200.0 > this.Imageborder.Height)
        {
          this.Siderbar.Margin = new Thickness(0.0);
          this.Siderbar.VerticalAlignment = VerticalAlignment.Bottom;
        }
        else
          this.Siderbar.Margin = new Thickness(0.0);
      }
      this.layoutImageBorder();
      Canvas.SetLeft((UIElement) this, clientRect.Left);
      Canvas.SetTop((UIElement) this, clientRect.Top);
    }
  }

  private void exprotbtn_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("ImageAction", "ExportImage", "Count", 1L);
    PdfImageObject imageObject = (PdfImageObject) null;
    if (this.Isseleted)
      imageObject = this.Document.Pages[this.Pageindex].PageObjects[this.Document.Pages[this.Pageindex].PageObjects.Count - 1] as PdfImageObject;
    else if (this.Pageindex >= 0 && this.imageindex >= 0)
      imageObject = this.Document.Pages[this.Pageindex].PageObjects[this.imageindex] as PdfImageObject;
    if (imageObject == null)
      return;
    this.SaveImageAsync(imageObject);
  }

  private async void rotate_Click(object sender, RoutedEventArgs e)
  {
    ImageControl imageControl = this;
    CommomLib.Commom.GAManager.SendEvent("ImageAction", "RotateImage", "Count", 1L);
    if (!(imageControl.Document.Pages[imageControl.Pageindex].PageObjects[imageControl.imageindex] is PdfImageObject pageObject1))
      return;
    FS_MATRIX matrix = pageObject1.Matrix;
    float x = pageObject1.BoundingBox.right - pageObject1.BoundingBox.Width / 2f;
    float y = pageObject1.BoundingBox.top - pageObject1.BoundingBox.Height / 2f;
    matrix.Translate(-x, -y);
    matrix.Rotate(-1.57079637f);
    matrix.Translate(x, y);
    await imageControl.ImageOperationCmd(pageObject1, imageControl.Pageindex, imageControl.imageindex, matrix);
    PdfImageObject pageObject2 = imageControl.Document.Pages[imageControl.Pageindex].PageObjects[imageControl.imageindex] as PdfImageObject;
    imageControl.ImageRect = new FS_RECTF()
    {
      right = pageObject2.BoundingBox.right,
      left = pageObject2.BoundingBox.left,
      top = pageObject2.BoundingBox.top,
      bottom = pageObject2.BoundingBox.bottom
    };
    imageControl.UpdateImageborder();
  }

  private async void ocrBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      CommomLib.Commom.GAManager.SendEvent("ImageAction", "OCRImage", "Count", 1L);
      PdfViewer viewer = this.annotationCanvas.PdfViewer;
      PdfImageObject imageObject = (PdfImageObject) null;
      if (this.Isseleted)
        imageObject = this.Document.Pages[this.Pageindex].PageObjects[this.Document.Pages[this.Pageindex].PageObjects.Count - 1] as PdfImageObject;
      else if (this.Pageindex >= 0 && this.imageindex >= 0)
        imageObject = this.Document.Pages[this.Pageindex].PageObjects[this.imageindex] as PdfImageObject;
      using (MemoryStream ms = new MemoryStream())
      {
        imageObject.Bitmap.Image.Save((Stream) ms, ImageFormat.Png);
        ms.Position = 0L;
        System.Drawing.Image Image = System.Drawing.Image.FromStream((Stream) ms);
        if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate90)
          Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        else if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate180)
          Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
        else if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate270)
          Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        WriteableBitmap imageAsync = await this.GetImageAsync(PdfBitmap.FromBitmap((Bitmap) Image));
        FS_RECTF fsRectf = new FS_RECTF()
        {
          bottom = imageObject.BoundingBox.bottom,
          top = imageObject.BoundingBox.top,
          left = imageObject.BoundingBox.left,
          right = imageObject.BoundingBox.right
        };
        ScreenshotDialog screenshotDialog = new ScreenshotDialog();
        System.Drawing.Image image = imageObject.Bitmap.Image;
        Rect clientRect;
        viewer.TryGetClientRect(this.Pageindex, fsRectf, out clientRect);
        ScreenshotDialogResult extractImageText = ScreenshotDialogResult.CreateExtractImageText(this.Pageindex, "", imageAsync, Image, fsRectf, clientRect, true);
        if (extractImageText != null && extractImageText.Completed)
        {
          ExtractTextResultDialog textResultDialog = new ExtractTextResultDialog(extractImageText);
          textResultDialog.Owner = (Window) Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
          textResultDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
          textResultDialog.ShowDialog();
        }
        Image = (System.Drawing.Image) null;
      }
      viewer = (PdfViewer) null;
      imageObject = (PdfImageObject) null;
    }
    catch
    {
      int num = (int) ModernMessageBox.Show("Failed to OCR from the image!", "PDFgear");
    }
  }

  private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
  {
    ImageControl imageControl = this;
    CommomLib.Commom.GAManager.SendEvent("ImageAction", "DeleteImage", "Count", 1L);
    if (ModernMessageBox.Show(pdfeditor.Properties.Resources.ImageControl_DeleteConfirm, "PDFgear", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
      return;
    PdfImageObject pageObject = imageControl.Document.Pages[imageControl.Pageindex].PageObjects[imageControl.imageindex] as PdfImageObject;
    await imageControl.DeleteImageCmd(pageObject, imageControl.Pageindex);
    imageControl.Visibility = Visibility.Collapsed;
  }

  private void ReplaceBtn_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("ImageAction", "ReplaceImage", "Count", 1L);
    try
    {
      if (this.Pageindex < 0 || this.imageindex < 0)
        return;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = "Image Files (*.jpg; *.png; *.bmp)|*.jpg; *.png; *.bmp";
      openFileDialog1.ShowReadOnly = false;
      openFileDialog1.ReadOnlyChecked = true;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (!openFileDialog2.ShowDialog(App.Current.MainWindow).GetValueOrDefault())
        return;
      PdfImageObject imageObject = !this.Isseleted ? this.Document.Pages[this.Pageindex].PageObjects[this.imageindex] as PdfImageObject : this.Document.Pages[this.Pageindex].PageObjects[this.Document.Pages[this.Pageindex].PageObjects.Count - 1] as PdfImageObject;
      if (imageObject == null)
        return;
      if (new FileInfo(openFileDialog2.FileName).Length == 0L)
      {
        int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.UnsupportedImageMsg, "PDFgear");
      }
      else
      {
        System.Drawing.Image image = System.Drawing.Image.FromFile(openFileDialog2.FileName);
        if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate90)
          image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        else if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate180)
          image.RotateFlip(RotateFlipType.Rotate180FlipNone);
        else if (this.Document.Pages[this.Pageindex].Rotation == PageRotate.Rotate270)
          image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        PdfBitmap pdfBitmap = PdfBitmap.FromBitmap((Bitmap) image);
        this.ReplaceImageCmd(imageObject, this.Pageindex, pdfBitmap, this);
        PdfImageObject pageObject = this.Document.Pages[this.Pageindex].PageObjects[this.imageindex] as PdfImageObject;
        this.ImageRect = new FS_RECTF()
        {
          right = pageObject.BoundingBox.right,
          left = pageObject.BoundingBox.left,
          top = pageObject.BoundingBox.top,
          bottom = pageObject.BoundingBox.bottom
        };
        this.UpdateImageborder();
      }
    }
    catch
    {
      int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.UnsupportedImageMsg, "PDFgear");
    }
  }

  private async Task DeleteImageCmd(PdfImageObject imageObject, int pageIndex)
  {
    PageImageModel imageModel = new PageImageModel(imageObject, pageIndex);
    this.Document.Pages[pageIndex].PageObjects.RemoveAt(imageModel.ImageIndex);
    this.Document.Pages[pageIndex].GenerateContentAdvance();
    await this.Document.Pages[pageIndex].TryRedrawPageAsync();
    await this.VM.OperationManager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      PdfImageObject pdfImageObject = PdfImageObject.Create(doc, PdfBitmap.FromBitmap((Bitmap) imageModel.Image), imageModel.Matrix.e, imageModel.Matrix.f);
      pdfImageObject.Matrix = new FS_MATRIX(imageModel.Matrix.a, imageModel.Matrix.b, imageModel.Matrix.c, imageModel.Matrix.d, imageModel.Matrix.e, imageModel.Matrix.f);
      if (imageModel.sMaskRef != null)
      {
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(doc);
        int objectNumber = list.Add(imageModel.sMaskRef);
        pdfImageObject.Stream.Dictionary.SetIndirectAt("SMask", list, objectNumber);
      }
      doc.Pages[imageModel.ImagePageIndex].PageObjects.Insert(imageModel.ImageIndex, (PdfPageObject) pdfImageObject);
      PageImageModel pageImageModel = new PageImageModel(pdfImageObject, imageModel.ImagePageIndex);
      doc.Pages[imageModel.ImagePageIndex].GenerateContentAdvance();
      await doc.Pages[imageModel.ImagePageIndex].TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      doc.Pages[pageIndex].PageObjects.RemoveAt(imageModel.ImageIndex);
      doc.Pages[imageModel.ImagePageIndex].GenerateContentAdvance();
      await doc.Pages[imageModel.ImagePageIndex].TryRedrawPageAsync();
    }));
  }

  private async Task ImageOperationCmd(
    PdfImageObject imageObject,
    int pageIndex,
    int imageIndex,
    FS_MATRIX matrix)
  {
    ImageControl target1 = this;
    WeakReference weakThis = new WeakReference((object) target1);
    FS_MATRIX oldMatrix = imageObject.Matrix;
    imageObject.Matrix = matrix;
    target1.Document.Pages[pageIndex].GenerateContentAdvance();
    await target1.Document.Pages[pageIndex].TryRedrawPageAsync();
    await target1.VM.OperationManager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      if (weakThis.Target is ImageControl target3 && target3.Visibility == Visibility.Visible)
        target3.Visibility = Visibility.Collapsed;
      if (!(doc.Pages[pageIndex].PageObjects[imageIndex] is PdfImageObject pageObject2))
        return;
      pageObject2.Matrix = oldMatrix;
      doc.Pages[pageIndex].GenerateContentAdvance();
      await doc.Pages[pageIndex].TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      if (weakThis.Target is ImageControl target5 && target5.Visibility == Visibility.Visible)
        target5.Visibility = Visibility.Collapsed;
      if (!(doc.Pages[pageIndex].PageObjects[imageIndex] is PdfImageObject pageObject4))
        return;
      pageObject4.Matrix = matrix;
      doc.Pages[pageIndex].GenerateContentAdvance();
      await doc.Pages[pageIndex].TryRedrawPageAsync();
    }));
  }

  public async void ReplaceImageCmd(
    PdfImageObject imageObject,
    int pageIndex,
    PdfBitmap pdfBitmap,
    ImageControl ImageControl)
  {
    ImageControl target1 = this;
    WeakReference weakThis = new WeakReference((object) target1);
    PageImageModel imageModel = new PageImageModel(imageObject, pageIndex);
    ImageControl imageControl = target1;
    ImageMatrix matrix = imageModel.Matrix;
    PdfBitmap bitmap = imageObject.Bitmap;
    PdfBitmap pdfbitmap2 = pdfBitmap;
    PdfImageObject imageObject1 = imageObject;
    FS_RECTF boundingBox = imageObject.BoundingBox;
    double height = (double) boundingBox.Height;
    boundingBox = imageObject.BoundingBox;
    double width = (double) boundingBox.Width;
    ImageMatrix replaceImageMatrix = imageControl.GetReplaceImageMatrix(matrix, bitmap, pdfbitmap2, imageObject1, (float) height, (float) width);
    PdfImageObject pdfImageObject1 = PdfImageObject.Create(target1.Document, pdfBitmap, replaceImageMatrix.e, replaceImageMatrix.f);
    if ((double) pdfBitmap.Width > (double) imageObject.BoundingBox.Width || (double) pdfBitmap.Height > (double) imageObject.BoundingBox.Height)
    {
      if (pdfBitmap.Height >= pdfBitmap.Width)
      {
        float num = imageObject.BoundingBox.Height / (float) pdfBitmap.Height;
        pdfImageObject1.Matrix = new FS_MATRIX((float) pdfBitmap.Width * num, 0.0f, 0.0f, imageObject.BoundingBox.Height, imageObject.BoundingBox.left + (float) (((double) imageObject.BoundingBox.Width - (double) pdfBitmap.Width * (double) num) / 2.0), imageObject.BoundingBox.bottom);
      }
      else if (pdfBitmap.Height < pdfBitmap.Width)
      {
        float num = imageObject.BoundingBox.Width / (float) pdfBitmap.Width;
        pdfImageObject1.Matrix = new FS_MATRIX(imageObject.BoundingBox.Width, 0.0f, 0.0f, imageObject.BoundingBox.Height * num, imageObject.BoundingBox.left, imageObject.BoundingBox.bottom + (float) (((double) imageObject.BoundingBox.Height - (double) pdfBitmap.Height * (double) num) / 2.0));
      }
    }
    target1.Document.Pages[pageIndex].PageObjects.RemoveAt(imageModel.ImageIndex);
    target1.Document.Pages[imageModel.ImagePageIndex].PageObjects.Insert(imageModel.ImageIndex, (PdfPageObject) pdfImageObject1);
    PageImageModel replaceitem = new PageImageModel(pdfImageObject1, imageModel.ImagePageIndex);
    target1.Document.Pages[pageIndex].GenerateContentAdvance();
    await target1.Document.Pages[pageIndex].TryRedrawPageAsync();
    await target1.VM.OperationManager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
    {
      if (weakThis.Target is ImageControl target3 && target3.Visibility == Visibility.Visible)
        target3.Visibility = Visibility.Collapsed;
      PdfImageObject pdfImageObject2 = PdfImageObject.Create(doc, PdfBitmap.FromBitmap((Bitmap) imageModel.Image), imageModel.Matrix.e, imageModel.Matrix.f);
      pdfImageObject2.Matrix = new FS_MATRIX(imageModel.Matrix.a, imageModel.Matrix.b, imageModel.Matrix.c, imageModel.Matrix.d, imageModel.Matrix.e, imageModel.Matrix.f);
      if (imageModel.sMaskRef != null)
      {
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(doc);
        int objectNumber = list.Add(imageModel.sMaskRef);
        pdfImageObject2.Stream.Dictionary.SetIndirectAt("SMask", list, objectNumber);
      }
      doc.Pages[replaceitem.ImagePageIndex].PageObjects.RemoveAt(replaceitem.ImageIndex);
      doc.Pages[imageModel.ImagePageIndex].PageObjects.Insert(imageModel.ImageIndex, (PdfPageObject) pdfImageObject2);
      doc.Pages[imageModel.ImagePageIndex].GenerateContentAdvance();
      await doc.Pages[imageModel.ImagePageIndex].TryRedrawPageAsync();
    }), (Func<PdfDocument, Task>) (async doc =>
    {
      if (weakThis.Target is ImageControl target5 && target5.Visibility == Visibility.Visible)
        target5.Visibility = Visibility.Collapsed;
      PdfImageObject pdfImageObject3 = PdfImageObject.Create(doc, PdfBitmap.FromBitmap((Bitmap) replaceitem.Image), replaceitem.Matrix.e, replaceitem.Matrix.f);
      pdfImageObject3.Matrix = new FS_MATRIX(replaceitem.Matrix.a, replaceitem.Matrix.b, replaceitem.Matrix.c, replaceitem.Matrix.d, replaceitem.Matrix.e, replaceitem.Matrix.f);
      if (replaceitem.sMaskRef != null)
      {
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(doc);
        int objectNumber = list.Add(replaceitem.sMaskRef);
        pdfImageObject3.Stream.Dictionary.SetIndirectAt("SMask", list, objectNumber);
      }
      doc.Pages[imageModel.ImagePageIndex].PageObjects.RemoveAt(imageModel.ImageIndex);
      doc.Pages[replaceitem.ImagePageIndex].PageObjects.Insert(replaceitem.ImageIndex, (PdfPageObject) pdfImageObject3);
      doc.Pages[replaceitem.ImagePageIndex].GenerateContentAdvance();
      await doc.Pages[replaceitem.ImagePageIndex].TryRedrawPageAsync();
    }));
  }

  private ImageMatrix GetReplaceImageMatrix(
    ImageMatrix imageMatrix,
    PdfBitmap pdfbitmap1,
    PdfBitmap pdfbitmap2,
    PdfImageObject imageObject,
    float heigth,
    float width)
  {
    float num1;
    float num2;
    if (pdfbitmap2.Height < 200 && pdfbitmap2.Width < 200 && pdfbitmap1.Height > 200 && pdfbitmap1.Width > 200)
    {
      if (pdfbitmap2.Height < pdfbitmap2.Width)
      {
        float num3 = 200f / (float) pdfbitmap2.Height;
        num1 = (float) pdfbitmap2.Width / (float) pdfbitmap1.Width * num3;
        num2 = (float) pdfbitmap2.Height / (float) pdfbitmap1.Height * num3;
      }
      else
      {
        float num4 = 200f / (float) pdfbitmap2.Width;
        num1 = (float) pdfbitmap2.Width / (float) pdfbitmap1.Width * num4;
        num2 = (float) pdfbitmap2.Height / (float) pdfbitmap1.Height * num4;
      }
    }
    else
    {
      num1 = (float) pdfbitmap2.Width / (float) pdfbitmap1.Width;
      num2 = (float) pdfbitmap2.Height / (float) pdfbitmap1.Height;
    }
    float num5 = 0.0f;
    float num6 = 0.0f;
    if ((double) num1 > 1.0)
    {
      num2 /= num1;
      num1 /= num1;
    }
    if ((double) num2 > 1.0)
    {
      num1 /= num2;
      num2 /= num2;
    }
    if ((double) num1 < 1.0 && (double) num2 < 1.0)
    {
      num5 = (float) ((double) (pdfbitmap1.Width - pdfbitmap2.Width) * ((double) width / (double) pdfbitmap1.Width) / 2.0);
      num6 = (float) ((double) (pdfbitmap1.Height - pdfbitmap2.Height) * ((double) heigth / (double) pdfbitmap1.Height) / 2.0);
    }
    else if ((double) num1 < 1.0)
      num5 = (float) (((double) pdfbitmap1.Width - (double) pdfbitmap2.Width * ((double) pdfbitmap1.Height / (double) pdfbitmap2.Height)) * ((double) width / (double) pdfbitmap1.Width) / 2.0);
    else if ((double) num2 < 1.0)
      num6 = (float) (((double) pdfbitmap1.Height - (double) pdfbitmap2.Height * ((double) pdfbitmap1.Width / (double) pdfbitmap2.Width)) * ((double) heigth / (double) pdfbitmap1.Height) / 2.0);
    float num7 = imageObject.BoundingBox.right - imageObject.BoundingBox.Width / 2f;
    float num8 = imageObject.BoundingBox.top - imageObject.BoundingBox.Height / 2f;
    if ((double) imageMatrix.e - (double) num7 >= 0.0)
      num5 = -num5;
    if ((double) imageMatrix.f - (double) num8 >= 0.0)
      num6 = -num6;
    return new ImageMatrix(imageMatrix.a * num1, imageMatrix.b, imageMatrix.c, imageMatrix.d * num2, imageMatrix.e + num5, imageMatrix.f + num6);
  }

  private async Task SaveImageAsync(PdfImageObject imageObject)
  {
    FileInfo fileInfo = new FileInfo(this.VM.DocumentWrapper?.DocumentPath);
    string directoryName = fileInfo.DirectoryName;
    string str1 = fileInfo.Name;
    if (!string.IsNullOrEmpty(fileInfo.Extension))
      str1 = str1.Substring(0, str1.Length - fileInfo.Extension.Length);
    string str2 = str1.Length <= 48 /*0x30*/ ? str1 + $" Image[{this.Pageindex + 1}].jpg" : str1 + $" [{this.Pageindex + 1}].jpg";
    if (str2.Length > 128 /*0x80*/)
    {
      string str3 = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
      string path2 = str3 + " Image.jpg";
      int num = 0;
      try
      {
        for (; File.Exists(System.IO.Path.Combine(directoryName, path2)); path2 = str3 + $" Image ({num}).jpg")
          ++num;
        str2 = path2;
      }
      catch
      {
        str2 = str3 + " Image.pdf";
      }
    }
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Png Image|*.png";
    saveFileDialog1.CreatePrompt = false;
    saveFileDialog1.OverwritePrompt = true;
    saveFileDialog1.InitialDirectory = directoryName;
    saveFileDialog1.FileName = str2;
    SaveFileDialog saveFileDialog2 = saveFileDialog1;
    if (!saveFileDialog2.ShowDialog().Value)
      return;
    System.Drawing.Image image = imageObject.Bitmap.Image;
    if (System.IO.Path.GetExtension(saveFileDialog2.FileName) == ".png")
      image.Save(saveFileDialog2.FileName, ImageFormat.Png);
    else if (System.IO.Path.GetExtension(saveFileDialog2.FileName) == ".jpg")
      image.Save(saveFileDialog2.FileName, ImageFormat.Jpeg);
    else
      image.Save(saveFileDialog2.FileName, ImageFormat.Bmp);
    int num1 = await new FileInfo(saveFileDialog2.FileName).ShowInExplorerAsync() ? 1 : 0;
  }

  private async Task<WriteableBitmap> GetImageAsync(PdfBitmap pdfBitmap)
  {
    return await pdfBitmap.ToWriteableBitmapAsync(new CancellationToken());
  }

  private void Topleft_MouseUp(object sender, MouseButtonEventArgs e)
  {
    this.ImageControlMoveImage(this.clickStartPosition);
  }

  private void editorImagebtn_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("ImageAction", "EditBtn", "Count", 1L);
    this.ImageControlState = true;
    this.SiderEditorbar.Visibility = Visibility.Visible;
    this.editorImagebtn.Visibility = Visibility.Collapsed;
    this.selectedBoder.Visibility = Visibility.Collapsed;
    this.editorBoder.Visibility = Visibility.Visible;
  }

  private void quitBtn_Click(object sender, RoutedEventArgs e) => this.quitImageControl();

  public void quitImageControl()
  {
    CommomLib.Commom.GAManager.SendEvent("ImageAction", "QuitBtn", "Count", 1L);
    this.ImageControlState = false;
    this.editorImagebtn.Visibility = Visibility.Visible;
    this.SiderEditorbar.Visibility = Visibility.Collapsed;
    this.selectedBoder.Visibility = Visibility.Visible;
    this.editorBoder.Visibility = Visibility.Collapsed;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/imagecontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.selectedBoder = (Grid) target;
        break;
      case 2:
        this.Imageborder2 = (System.Windows.Shapes.Rectangle) target;
        break;
      case 3:
        this.editorBoder = (Grid) target;
        break;
      case 4:
        this.Imageborder = (System.Windows.Shapes.Rectangle) target;
        break;
      case 5:
        this.Topleft = (System.Windows.Shapes.Rectangle) target;
        this.Topleft.MouseDown += new MouseButtonEventHandler(this.MouseButtonDown);
        this.Topleft.MouseUp += new MouseButtonEventHandler(this.Topleft_MouseUp);
        break;
      case 6:
        this.Topright = (System.Windows.Shapes.Rectangle) target;
        this.Topright.MouseDown += new MouseButtonEventHandler(this.MouseButtonDown);
        this.Topright.MouseUp += new MouseButtonEventHandler(this.Topleft_MouseUp);
        break;
      case 7:
        this.Bottomleft = (System.Windows.Shapes.Rectangle) target;
        this.Bottomleft.MouseDown += new MouseButtonEventHandler(this.MouseButtonDown);
        this.Bottomleft.MouseUp += new MouseButtonEventHandler(this.Topleft_MouseUp);
        break;
      case 8:
        this.Bottomright = (System.Windows.Shapes.Rectangle) target;
        this.Bottomright.MouseDown += new MouseButtonEventHandler(this.MouseButtonDown);
        this.Bottomright.MouseUp += new MouseButtonEventHandler(this.Topleft_MouseUp);
        break;
      case 9:
        this.Topcenter = (System.Windows.Shapes.Rectangle) target;
        this.Topcenter.MouseDown += new MouseButtonEventHandler(this.MouseButtonDown);
        this.Topcenter.MouseUp += new MouseButtonEventHandler(this.Topleft_MouseUp);
        break;
      case 10:
        this.Leftcenter = (System.Windows.Shapes.Rectangle) target;
        this.Leftcenter.MouseDown += new MouseButtonEventHandler(this.MouseButtonDown);
        this.Leftcenter.MouseUp += new MouseButtonEventHandler(this.Topleft_MouseUp);
        break;
      case 11:
        this.Bottomcenter = (System.Windows.Shapes.Rectangle) target;
        this.Bottomcenter.MouseDown += new MouseButtonEventHandler(this.MouseButtonDown);
        this.Bottomcenter.MouseUp += new MouseButtonEventHandler(this.Topleft_MouseUp);
        break;
      case 12:
        this.Rightcenter = (System.Windows.Shapes.Rectangle) target;
        this.Rightcenter.MouseDown += new MouseButtonEventHandler(this.MouseButtonDown);
        this.Rightcenter.MouseUp += new MouseButtonEventHandler(this.Topleft_MouseUp);
        break;
      case 13:
        this.Siderbar = (Grid) target;
        break;
      case 14:
        this.SiderEditorActionBtn = (Grid) target;
        break;
      case 15:
        this.editorImagebtn = (Button) target;
        this.editorImagebtn.Click += new RoutedEventHandler(this.editorImagebtn_Click);
        break;
      case 16 /*0x10*/:
        this.SiderEditorbar = (Grid) target;
        break;
      case 17:
        this.exprotbtn = (Button) target;
        this.exprotbtn.Click += new RoutedEventHandler(this.exprotbtn_Click);
        break;
      case 18:
        this.RotateBtn = (Button) target;
        this.RotateBtn.Click += new RoutedEventHandler(this.rotate_Click);
        break;
      case 19:
        this.OcrBtn = (Button) target;
        this.OcrBtn.Click += new RoutedEventHandler(this.ocrBtn_Click);
        break;
      case 20:
        this.DeleteBtn = (Button) target;
        this.DeleteBtn.Click += new RoutedEventHandler(this.DeleteBtn_Click);
        break;
      case 21:
        this.ReplaceBtn = (Button) target;
        this.ReplaceBtn.Click += new RoutedEventHandler(this.ReplaceBtn_Click);
        break;
      case 22:
        this.quitBtn = (Button) target;
        this.quitBtn.Click += new RoutedEventHandler(this.quitBtn_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  public enum MousePosition
  {
    topLeft,
    topRight,
    bottomLeft,
    bottomRight,
    topCenter,
    leftCenter,
    rightCenter,
    bottomCenter,
    None,
  }
}
