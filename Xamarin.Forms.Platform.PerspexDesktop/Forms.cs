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
		const string LogFormat = "[{0}] {1}";

		static bool s_isInitialized;

        public static void Init()
		{
			if (s_isInitialized)
				return;

			Device.OS = TargetPlatform.Other;
			Device.Idiom = TargetIdiom.Desktop;
			Device.PlatformServices = new DesktopPlatformServices();
			Device.Info = new DesktopDeviceInfo();

			ExpressionSearch.Default = new WindowsExpressionSearch();

			Registrar.RegisterAll(new[] { typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute) });

			s_isInitialized = true;
		}
	}
}