// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.SearchBar
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace PDFKit.ToolBars;

internal partial class SearchBar : UserControl, IComponentConnector
{
  private int _totalRecords = 0;
  private int _currentRecord = 0;
  private DispatcherTimer _onsearchTimer;
  private Color _borderColor;
  internal Border pnlBorder;
  internal Button picMenu;
  internal TextBox tbSearch;
  internal TextBlock lblInfo;
  internal Button picUp;
  internal Button picDown;
  private bool _contentLoaded;

  public event EventHandler CurrentRecordChanged = null;

  public event EventHandler NeedSearch = null;

  public string MenuItemMathCaseText => PDFKit.Properties.Resources.menuItemMatchCase;

  public string MenuItemMatchWholeWordText => PDFKit.Properties.Resources.menuItemMatchWholeWord;

  public Color BorderColor
  {
    get => this._borderColor;
    set
    {
      this._borderColor = value;
      this.pnlBorder.BorderBrush = (Brush) new SolidColorBrush(value);
    }
  }

  public FindFlags FindFlags { get; set; }

  public int TotalRecords
  {
    get => this._totalRecords;
    set
    {
      this._totalRecords = value;
      if (this._totalRecords < 0)
        this._totalRecords = 0;
      if (this._currentRecord > this._totalRecords)
        this._currentRecord = this._totalRecords;
      this.lblInfo.Background = this._totalRecords != 0 ? (Brush) new SolidColorBrush(Colors.Transparent) : (Brush) new SolidColorBrush(Colors.PaleVioletRed);
      this.SetInfoText();
      this.EnableButton(this.picUp, this._totalRecords > 0);
      this.EnableButton(this.picDown, this._totalRecords > 0);
      if (this._totalRecords <= 0 || this._currentRecord != 0)
        return;
      this.CurrentRecord = 1;
    }
  }

  public int CurrentRecord
  {
    get => this._currentRecord;
    set
    {
      if (this._currentRecord == value)
        return;
      this._currentRecord = value;
      this.SetInfoText();
      if (this.CurrentRecordChanged != null)
        this.CurrentRecordChanged((object) this, EventArgs.Empty);
    }
  }

  private void SetInfoText()
  {
    this.lblInfo.Text = string.Format(PDFKit.Properties.Resources.searchLblnfo, (object) this.CurrentRecord, (object) this.TotalRecords);
  }

  public string SearchText
  {
    get => this.tbSearch.Text;
    set
    {
      if (!(this.SearchText != value))
        return;
      this.tbSearch.Text = value;
    }
  }

  public bool IsCheckedMatchCase => (this.FindFlags & FindFlags.MatchCase) == FindFlags.MatchCase;

  public bool IsCheckedMatchWholeWord
  {
    get => (this.FindFlags & FindFlags.MatchWholeWord) == FindFlags.MatchWholeWord;
  }

  public SearchBar()
  {
    this.InitializeComponent();
    this.DataContext = (object) this;
    this.pnlBorder.Background = (Brush) new SolidColorBrush(this.BorderColor);
    this.EnableButton(this.picUp, false);
    this.EnableButton(this.picDown, false);
    this.lblInfo.Visibility = Visibility.Hidden;
    this._onsearchTimer = new DispatcherTimer();
    this._onsearchTimer.Interval = TimeSpan.FromMilliseconds(50.0);
    this._onsearchTimer.Tick += new EventHandler(this._onsearchTimer_Tick);
  }

  private void picDown_Click(object sender, RoutedEventArgs e)
  {
    if (this.CurrentRecord < this.TotalRecords)
      ++this.CurrentRecord;
    else
      this.CurrentRecord = 1;
  }

  private void picUp_Click(object sender, RoutedEventArgs e)
  {
    if (this.CurrentRecord > 1)
      --this.CurrentRecord;
    else
      this.CurrentRecord = this.TotalRecords;
  }

  private void picMenu_Click(object sender, RoutedEventArgs e)
  {
    (sender as Button).ContextMenu.IsEnabled = true;
    (sender as Button).ContextMenu.PlacementTarget = (UIElement) (sender as Button);
    (sender as Button).ContextMenu.Placement = PlacementMode.Bottom;
    (sender as Button).ContextMenu.IsOpen = true;
  }

  private void searchMenuItem_Click(object sender, RoutedEventArgs e)
  {
    this.FindFlags ^= (FindFlags) Enum.Parse(this.FindFlags.GetType(), (sender as MenuItem).Tag.ToString());
    this.OnSearch();
  }

  private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
  {
    this.lblInfo.Visibility = this.tbSearch.Text != "" ? Visibility.Visible : Visibility.Hidden;
    this.EnableButton(this.picUp, this.tbSearch.Text != "");
    this.EnableButton(this.picDown, this.tbSearch.Text != "");
    this._onsearchTimer.Stop();
    this._onsearchTimer.Start();
  }

  private void TbSearch_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (Keyboard.IsKeyDown(Key.Return) && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
    {
      this.picUp_Click((object) null, (RoutedEventArgs) null);
      e.Handled = true;
    }
    else
    {
      if (!Keyboard.IsKeyDown(Key.Return))
        return;
      this.picDown_Click((object) null, (RoutedEventArgs) null);
      e.Handled = true;
    }
  }

  private void _onsearchTimer_Tick(object sender, EventArgs e)
  {
    this._onsearchTimer.Stop();
    this.OnSearch();
  }

  private void pnlHostTextBox_Click(object sender, EventArgs e) => this.tbSearch.Focus();

  private void EnableButton(Button button, bool enabled)
  {
    switch (button.Name)
    {
      case "picUp":
        this.picUp.IsEnabled = enabled;
        break;
      case "picDown":
        this.picDown.IsEnabled = enabled;
        break;
    }
  }

  private void OnSearch()
  {
    if (this.NeedSearch == null)
      return;
    this.NeedSearch((object) this, EventArgs.Empty);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFKit;component/toolbars/searchbar.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.pnlBorder = (Border) target;
        break;
      case 2:
        this.picMenu = (Button) target;
        this.picMenu.Click += new RoutedEventHandler(this.picMenu_Click);
        break;
      case 3:
        ((MenuItem) target).Click += new RoutedEventHandler(this.searchMenuItem_Click);
        break;
      case 4:
        ((MenuItem) target).Click += new RoutedEventHandler(this.searchMenuItem_Click);
        break;
      case 5:
        this.tbSearch = (TextBox) target;
        this.tbSearch.TextChanged += new TextChangedEventHandler(this.tbSearch_TextChanged);
        this.tbSearch.PreviewKeyDown += new KeyEventHandler(this.TbSearch_PreviewKeyDown);
        break;
      case 6:
        this.lblInfo = (TextBlock) target;
        break;
      case 7:
        this.picUp = (Button) target;
        this.picUp.Click += new RoutedEventHandler(this.picUp_Click);
        break;
      case 8:
        this.picDown = (Button) target;
        this.picDown.Click += new RoutedEventHandler(this.picDown_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
