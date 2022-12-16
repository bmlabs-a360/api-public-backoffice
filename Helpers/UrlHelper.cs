using System;
using System.Collections.Generic;

namespace api_public_backOffice.Helpers
{
    public interface IUrlHelper
    {
        Dictionary<string, string> GetValuesFromUrlEncode(string uri);
    }

    public class UrlHelper : IUrlHelper, IDisposable
    {

        public Dictionary<string, string> GetValuesFromUrlEncode(string uri)
        {
            if (string.IsNullOrEmpty(uri)) throw new ArgumentNullException("uri");
            Dictionary<string, string> ListValues = new();

            string[] valuesFrom = uri.Split('&');

            foreach (var item in valuesFrom)
            {
                string key = item.Split('=')[0];
                string valor = item.Split('=')[1];
                ListValues.Add(key, valor);
            }

            return ListValues;
        }

        public void Dispose()
        {
            
        }
    }
}
