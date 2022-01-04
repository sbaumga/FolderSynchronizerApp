using FolderSynchronizerApp.Business.Abstractions;
using Newtonsoft.Json;

namespace FolderSynchronizerApp.Business.Implementations
{
    public class JSONSerializationServiceImp : IJsonSerializationService
    {
        public string Serialize<T>(T data)
        {
            var result = JsonConvert.SerializeObject(data);
            return result;
        }

        public T Deserialize<T>(string data)
        {
            var result = JsonConvert.DeserializeObject<T>(data);
            return result;
        }
    }
}