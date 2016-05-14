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
					return (IDataTemplate)DesktopResources.GetDefault("ListViewHeaderTextCell");

				//return (IDataTemplate) DesktopResourcesManager.GetDefaultResource("ListViewTextCell"];
			}

			return (IDataTemplate)DesktopResources.GetDefault("TextCell");
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
			return (IDataTemplate)DesktopResources.GetDefault("EntryCell");
		}
	}

	public class ViewCellRenderer : ICellRenderer
	{
		public virtual IDataTemplate GetTemplate(Cell cell)
		{
			return (IDataTemplate)DesktopResources.GetDefault("ViewCell");
		}
	}

	public class SwitchCellRenderer : ICellRenderer
	{
		public virtual IDataTemplate GetTemplate(Cell cell)
		{
			return (IDataTemplate)DesktopResources.GetDefault("SwitchCell");
		}
	}

	public class ImageCellRenderer : ICellRenderer
	{
		public virtual IDataTemplate GetTemplate(Cell cell)
		{
			//if (cell.Parent is ListView)
			//	return (IDataTemplate)DesktopResourcesManager.GetDefaultResource("ListImageCell");
			return (IDataTemplate)DesktopResources.GetDefault("ImageCell");
		}
	}
}