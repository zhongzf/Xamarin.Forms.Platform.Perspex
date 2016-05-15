using Perspex;
using Perspex.Controls;
using Perspex.Controls.Templates;
using Perspex.Markup.Xaml;
using System.Collections.Generic;
using Xamarin.Forms;
using System;

namespace XamarinFormsPerspexDemo
{
    public class TextCellControl : /*ContentControl*/ StackPanel
    {
        /// <summary>
        /// Defines the <see cref="Cell"/> property.
        /// </summary>
        public static readonly StyledProperty<object> CellProperty =
            PerspexProperty.Register<TextCellControl, object>(nameof(Cell));

        public Cell Cell
        {
            get { return (Cell)GetValue(CellProperty); }
            set
            {
                var oldCell = Cell;
                var newCell = value;
                SetSource(oldCell, newCell);
                SetValue(CellProperty, value);
            }
        }

        private void SetSource(Cell oldCell, Cell newCell)
        {
            IDataTemplate dt = GetTemplate(newCell);
            //if (dt != null || Content == null)
            //{
            //    //_currentTemplate = dt;
            //    // Content
            //    Content = dt.Build(newCell);
            //}

            //((Control)Content).DataContext = newCell;
            Children.Add(dt.Build(newCell));
        }

        private IDataTemplate GetTemplate(Cell newCell)
        {
            return new FuncDataTemplate<TextCell>(x =>
                new StackPanel
                {
                    Children =
                    {
                        new TextBlock {Text=x.Text }
                    }
                }
            );
        }

        public static TextCell Convert(Xamarin.Forms.Color color)
        {
            return new TextCell { Text = color.A.ToString() };
        }

        //protected override Perspex.Size MeasureCore(Perspex.Size availableSize)
        //{
        //    //return base.MeasureCore(availableSize);
        //    return new Perspex.Size(100, 100);
        //}
    }



    public class Window1 : Window
    {
        public Window1()
        {
            this.InitializeComponent();
            //App.AttachDevTools(this);
            var mainPanel = this.FindControl<Panel>("MainPanel");
            var listBox = new ListBox
            {
                DataTemplates = new DataTemplates {
                    new FuncDataTemplate<Xamarin.Forms.Color>(x =>
                    new TextCellControl
                    {
                        Cell = (TextCell)TextCellControl.Convert(x)
                    })
                }
            };
            listBox.Items = new List<Xamarin.Forms.Color>
            {
                Xamarin.Forms.Color.Red,
                Xamarin.Forms.Color.Blue,
                Xamarin.Forms.Color.Yellow,
                Xamarin.Forms.Color.Green
            };
            mainPanel.Children.Add(listBox);
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
