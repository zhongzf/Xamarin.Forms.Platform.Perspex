using Perspex.Threading;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    internal class WindowsIsolatedStorage : IIsolatedStorageFile
    {
        string _folder;

        public WindowsIsolatedStorage(string folder)
        {
            if (folder == null)
                throw new ArgumentNullException("folder");

            _folder = folder;
        }

        public Task CreateDirectoryAsync(string path)
        {
            return Task.Run(() =>
            {
                Directory.CreateDirectory(path);
            });
        }

        public Task<bool> GetDirectoryExistsAsync(string path)
        {
            return Task.Run(() => Directory.Exists(path));
        }

        public Task<bool> GetFileExistsAsync(string path)
        {
            return Task.Run(() => File.Exists(path));
        }

        public Task<DateTimeOffset> GetLastWriteTimeAsync(string path)
        {
            return Task.Run(() =>
            {
                var fileInfo = new FileInfo(path);
                return (DateTimeOffset)fileInfo.LastWriteTime;
            });
        }

        private System.IO.FileMode ConvertFrom(FileMode mode)
        {
            return (System.IO.FileMode)mode;
        }


        private System.IO.FileAccess ConvertFrom(FileAccess access)
        {
            return (System.IO.FileAccess)access;
        }

        public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access)
        {
            return Task.Run(() =>
            {
                var fileMode = ConvertFrom(mode);
                var fileAccess = ConvertFrom(access);
                var stream = File.Open(path, fileMode, fileAccess);
                return (Stream)stream;
            });
        }

        public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return OpenFileAsync(path, mode, access);
        }
    }
}