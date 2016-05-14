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
using System.Diagnostics;
using System.Security.Cryptography;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    internal abstract class WindowsBasePlatformServices : IPlatformServices
    {
        public WindowsBasePlatformServices()
        {
        }

        public bool IsInvokeRequired
        {
            get
            {
                return !Dispatcher.UIThread.CheckAccess();
            }
        }

        public void BeginInvokeOnMainThread(Action action)
        {
            Dispatcher.UIThread.InvokeAsync(action);
        }

        public Ticker CreateTicker()
        {
            return new WindowsTicker();
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
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes_md5_in = UTF8Encoding.Default.GetBytes(input);
            byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);
            string output = BitConverter.ToString(bytes_md5_out);
            return output;
        }

        public double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes)
        {
            return size.GetFontSize();
        }

        public async Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage streamResponse = await client.GetAsync(uri.AbsoluteUri).ConfigureAwait(false);
                return streamResponse.IsSuccessStatusCode ? await streamResponse.Content.ReadAsStreamAsync().ConfigureAwait(false) : null;
            }
        }

        public IIsolatedStorageFile GetUserStoreForApplication()
        {
            return new WindowsIsolatedStorage(AppDomain.CurrentDomain.BaseDirectory);
        }

        public void OpenUriAction(Uri uri)
        {
            Process.Start(uri.AbsoluteUri);
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            var timer = new DispatcherTimer { Interval = interval };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                bool result = callback();
                if (!result)
                    timer.Stop();
            };
        }

        //internal class WindowsTimer : ITimer
        //{
        //    readonly Timer _timer;

        //    public WindowsTimer(Timer timer)
        //    {
        //        _timer = timer;
        //    }

        //    public void Change(int dueTime, int period)
        //    {
        //        _timer.Change(dueTime, period);
        //    }

        //    public void Change(long dueTime, long period)
        //    {
        //        Change(TimeSpan.FromMilliseconds(dueTime), TimeSpan.FromMilliseconds(period));
        //    }

        //    public void Change(TimeSpan dueTime, TimeSpan period)
        //    {
        //        _timer.Change(dueTime, period);
        //    }

        //    public void Change(uint dueTime, uint period)
        //    {
        //        Change(TimeSpan.FromMilliseconds(dueTime), TimeSpan.FromMilliseconds(period));
        //    }
        //}
    }
}