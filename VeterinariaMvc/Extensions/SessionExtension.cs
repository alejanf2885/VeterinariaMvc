using MvcCoreSession.Helpers;

namespace MvcCoreSession.Extensions
{
    public static class SessionExtension
    {
        public static T? GetObject<T>(this ISession session, string key)
        {
            string? json = session.GetString(key);

            if (json == null)
            {
                return default(T);
            }
            else
            {
                T data = HelperJsonSession.DeserializeObject<T>(json);
                return data;
            }
        }

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            string data = HelperJsonSession.SerializeObject(value);

            session.SetString(key, data);
        }
    }
}