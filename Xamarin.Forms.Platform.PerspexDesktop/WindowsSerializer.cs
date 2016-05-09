using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.PerspexDesktop
{
    internal sealed class WindowsSerializer : IDeserializer
	{
		const string PropertyStoreFile = "PropertyStore.forms";

        public async Task<IDictionary<string, object>> DeserializePropertiesAsync()
        {
            return await Task.Run<IDictionary<string, object>>(() =>
            {
                try
                {
                    var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PropertyStoreFile));
                    using (Stream stream = file.OpenRead())
                    {
                        if (stream.Length == 0)
                            return new Dictionary<string, object>(4);

                        var serializer = new DataContractSerializer(typeof(IDictionary<string, object>));
                        return (IDictionary<string, object>)serializer.ReadObject(stream);
                    }
                }
                catch (FileNotFoundException)
                {
                    return new Dictionary<string, object>(4);
                }
            });
        }

		public async Task SerializePropertiesAsync(IDictionary<string, object> properties)
		{
            await Task.Run(() =>
            {
                var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PropertyStoreFile));
                using (var stream = file.OpenWrite())
                {
                    try
                    {
                        var serializer = new DataContractSerializer(typeof(IDictionary<string, object>));
                        serializer.WriteObject(stream, properties);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Could not move new serialized property file over old: " + e.Message);
                    }
                }
            });
		}
	}
}