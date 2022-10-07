using Microsoft.AspNetCore.Http;
using System.Text;
using ToDo.Models;

namespace ToDo.SessionHelper
{
    public static class SessionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value">This is the value we add to session</param>
        /// <param name="key">set key to session value</param>
        public static void SetSessionString(this ISession session, string value, string key)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
            // Using Encoding so value is not shown plain text
        }       
    }
}
