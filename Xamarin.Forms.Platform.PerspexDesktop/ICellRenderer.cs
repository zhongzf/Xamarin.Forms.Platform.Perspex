
using Perspex.Controls.Templates;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    public interface ICellRenderer : IRegisterable
	{
		IDataTemplate GetTemplate(Cell cell);
	}
}