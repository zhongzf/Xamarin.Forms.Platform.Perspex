using Perspex.Controls.Templates;
using System;
using System.Windows.Input;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public class TextCellRenderer : ICellRenderer
	{
		public virtual IDataTemplate GetTemplate(Cell cell)
		{
			if (cell.RealParent is ListView)
			{
				if (TemplatedItemsList<ItemsView<Cell>, Cell>.GetIsGroupHeader(cell))
					return (IDataTemplate)Application.Current.Resources["ListViewHeaderTextCell"];

				//return (IDataTemplate) Application.Current.Resources["ListViewTextCell"];
			}

			return (IDataTemplate)Application.Current.Resources["TextCell"];
		}
	}

	public class EntryCellRendererCompleted : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

#pragma warning disable 0067 // Revisit: Can't remove; required by interface
		public event EventHandler CanExecuteChanged;
#pragma warning restore

		public void Execute(object parameter)
		{
			var entryCell = (EntryCell)parameter;
			entryCell.SendCompleted();
		}
	}

	public class EntryCellRenderer : ICellRenderer
	{
		public virtual IDataTemplate GetTemplate(Cell cell)
		{
			return (IDataTemplate)Application.Current.Resources["EntryCell"];
		}
	}

	public class ViewCellRenderer : ICellRenderer
	{
		public virtual IDataTemplate GetTemplate(Cell cell)
		{
			return (IDataTemplate)Application.Current.Resources["ViewCell"];
		}
	}

	public class SwitchCellRenderer : ICellRenderer
	{
		public virtual IDataTemplate GetTemplate(Cell cell)
		{
			return (IDataTemplate)Application.Current.Resources["SwitchCell"];
		}
	}

	public class ImageCellRenderer : ICellRenderer
	{
		public virtual IDataTemplate GetTemplate(Cell cell)
		{
			//if (cell.Parent is ListView)
			//	return (IDataTemplate)Application.Current.Resources["ListImageCell"];
			return (IDataTemplate)Application.Current.Resources["ImageCell"];
		}
	}
}