using System.Text;
using Newtonsoft.Json;
using Simulation_CSharp.Serialization.Data;
using Simulation_CSharp.Serialization.Data.Types;

namespace Simulation_CSharp.Serialization
{
    public static class SaveLoad
    {
        private const string Path = "saves/";
        private const string Extension = ".json";

        private static string GetLoc(string saveName)
        {
            var stringBuilder = new StringBuilder(Path);
            stringBuilder.Append(saveName);
            stringBuilder.Append(Extension);
            return stringBuilder.ToString();
        }

        private static bool DataExist(string saveName)
        {
            return File.Exists(GetLoc(saveName));
        }

        public static T LoadOrCreate<T>(string saveName, Func<T> factory) where T : ISerializable
        {
            if (!DataExist(saveName))
            {
                return factory();
            }

            var type = StoredData.GetNameOfType(typeof(T));
            var created = StoredDataTypes.Types[type!].Value();
            if (created is T storable)
            {
                storable.Load(LoadData(saveName)!);
                return storable;
            }

            return factory();
        }

        public static PairStoredData LoadOrEmpty(string saveName)
        {
            var data = LoadData(saveName);
            return data ?? new PairStoredData();
        }

        public static void SaveData(string saveName, PairStoredData data)
        {
            if (!File.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            var loc = GetLoc(saveName);

            if (File.Exists(loc))
            {
                File.Delete(loc);
            }

            using var stream = File.CreateText(loc);
            stream.Write(StoredData.SerializeObject(data));
        }

        public static PairStoredData? LoadData(string saveName)
        {
            var loc = GetLoc(saveName);

            if (!File.Exists(loc))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<PairStoredData>(
                File.ReadAllText(loc),
                new StoredDataConverter()
            );
        }
    }
}