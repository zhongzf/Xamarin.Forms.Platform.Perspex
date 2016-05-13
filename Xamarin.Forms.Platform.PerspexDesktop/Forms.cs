using Perspex.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xamarin.Forms.Platform.PerspexDesktop;

namespace Xamarin.Forms
{
	public static class Forms
	{
        static DesktopWindow _window;

        const string LogFormat = "[{0}] {1}";

		static bool s_isInitialized;

        public static void Init(DesktopWindow window)
		{
			if (s_isInitialized)
				return;

            _window = window;
			Device.OS = TargetPlatform.Other;
			Device.Idiom = TargetIdiom.Desktop;
			Device.PlatformServices = new WindowsPlatformServices();
			Device.Info = new WindowsDeviceInfo(window);

			ExpressionSearch.Default = new WindowsExpressionSearch();

			Registrar.RegisterAll(new[] { typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute) });

			s_isInitialized = true;
		}
	}
}