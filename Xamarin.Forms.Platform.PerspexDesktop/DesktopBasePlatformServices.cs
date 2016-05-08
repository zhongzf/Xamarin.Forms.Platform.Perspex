using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using System.Linq;
using Perspex.Threading;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
	internal abstract class DesktopBasePlatformServices : IPlatformServices
	{
		public DesktopBasePlatformServices()
		{
		}

        public bool IsInvokeRequired
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void BeginInvokeOnMainThread(Action action)
        {
            Dispatcher.UIThread.InvokeAsync(() => action(), DispatcherPriority.Normal);
        }

        public Ticker CreateTicker()
        {
            throw new NotImplementedException();
        }

        public Assembly[] GetAssemblies()
        {
            var fileTypeFilter = new[] { "*.exe", "*.dll" };

            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, fileTypeFilter[0], SearchOption.AllDirectories)
                .Union(Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, fileTypeFilter[1], SearchOption.AllDirectories))
                .Distinct()
                .ToList();

            var assemblies = new List<Assembly>(files.Count);
            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];
                try
                {
                    Assembly assembly = Assembly.Load(new AssemblyName { Name = Path.GetFileNameWithoutExtension(file) });

                    assemblies.Add(assembly);
                }
                catch (IOException)
                {
                }
                catch (BadImageFormatException)
                {
                }
            }

            Assembly thisAssembly = GetType().GetTypeInfo().Assembly;
            // this happens with .NET Native
            if (!assemblies.Contains(thisAssembly))
                assemblies.Add(thisAssembly);

            return assemblies.ToArray();
        }

        public string GetMD5Hash(string input)
        {
            throw new NotImplementedException();
        }

        public double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes)
        {
            return size.GetFontSize();
        }

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IIsolatedStorageFile GetUserStoreForApplication()
        {
            throw new NotImplementedException();
        }

        public void OpenUriAction(Uri uri)
        {
            throw new NotImplementedException();
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            throw new NotImplementedException();
        }
    }
}