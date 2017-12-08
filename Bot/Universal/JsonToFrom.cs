using Newtonsoft.Json;

namespace Bot.Universal
{
    public static class JsonToFrom
    {
        public static string ToJson<T>(this T objToConvert)
        {
            return JsonConvert.SerializeObject(objToConvert);
        }
        public static U FromJson<U>(this string objInString)
        {
            return JsonConvert.DeserializeObject<U>(objInString);
        }
    }
}
