using System;
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

        public async Task<bool> GetDirectoryExistsAsync(string path)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return Directory.Exists(path);
                }
                catch (FileNotFoundException)
                {
                    return false;
                }
            });
        }

		public async Task<bool> GetFileExistsAsync(string path)
		{
            return await Task.Run(() =>
            {
                try
                {
                    return File.Exists(path);
                }
                catch (FileNotFoundException)
                {
                    return false;
                }
            });
		}

		public async Task<DateTimeOffset> GetLastWriteTimeAsync(string path)
		{
            return await Task.Run(() =>
            {
                var fileInfo = new FileInfo(path);
                return fileInfo.LastWriteTime;
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

        public async Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access)
        {
            return await Task.Run(() =>
            {
                var fileMode = ConvertFrom(mode);
                var fileAccess = ConvertFrom(access);
                var stream = File.Open(path, fileMode, fileAccess);
                return stream;
            });
        }

        public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access, FileShare share)
		{
			return OpenFileAsync(path, mode, access);
		}
	}
}