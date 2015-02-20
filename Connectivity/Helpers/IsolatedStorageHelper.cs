using System.IO;
using System.IO.IsolatedStorage;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace OfflineTools.Connectivity.Helpers
{
    public static class IsolatedStorageHelper
    {
        internal static void Save<T>(string file, T thing)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Delete(file);
                using (var isoStream = new IsolatedStorageFileStream(file, FileMode.CreateNew, store))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        var stringWriter = new StringWriter();
                        new Serializer().Serialize(stringWriter, thing);
                        writer.Write(stringWriter);
                    }
                }
            }
        }

        internal static T Load<T>(string file)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var isoStream = new IsolatedStorageFileStream(file, FileMode.Open, store))
                {
                    using (var reader = new StreamReader(isoStream))
                    {
                        var s = reader.ReadToEnd();
                        return new Deserializer().Deserialize<T>(new StringReader(s));
                    }
                }
            }
        }

        internal static void Delete(string file)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                if (Exists(file))
                    store.DeleteFile(file);
        }

        internal static bool Exists(string file)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                return store.FileExists(file);
        }

        public static void EnsureValid<T>(string file, T defaultThing)
        {
            if (!Exists(file) || IsCorrupt(file, defaultThing))
                Save(file, defaultThing);
        }

        public static bool IsCorrupt<T>(string file, T defaultThing)
        {
            try
            {
                Load<T>(file);
            }
            catch (YamlException)
            {
                return true;
            }
            return false;
        }
    }
}