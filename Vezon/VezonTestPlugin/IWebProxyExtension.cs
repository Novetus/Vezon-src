using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy.EventArguments;

namespace VezonWebProxy
{
    public class IWebProxyExtension
    {
        public virtual void OnProxyStart() { }
        public virtual void OnProxyStopped() { }

        public virtual bool IsValidURL(string absolutePath, string host) { return false; }

        public virtual Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e) { return Task.FromResult(0); }
        public virtual async Task OnRequest(object sender, SessionEventArgs e)
        {
            string query = e.HttpClient.Request.RequestUri.Query;
            e.Ok($"Response to '{query}':\nTest successful.");
        }
    }
}
