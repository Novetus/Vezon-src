using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;
using VezonCore;

namespace VezonWebProxy
{
    public class IWebProxyExtension : IVezonExtensionAddon
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

    public class WebProxy
    {
        public IVezonExtension? Parent = null;
        private ProxyServer Server = new ProxyServer();
        private ExplicitProxyEndPoint? end;
        private static readonly SemaphoreLocker _locker = new SemaphoreLocker();
        public bool Started { get { return Server.ProxyRunning; } }
        private int WebProxyPort = 6171;

        public void Start()
        {
            if (Server.ProxyRunning)
            {
                Global.WriteLine("The web proxy is already on and running.");
                return;
            }

            try
            {
                Global.WriteLine("Booting up Web Proxy...");
                Server.CertificateManager.RootCertificateIssuerName = Global.ProjectName;
                Server.CertificateManager.RootCertificateName = $"{Global.ProjectName} Web Proxy";
                Server.BeforeRequest += new AsyncEventHandler<SessionEventArgs>(OnRequest);

                end = new ExplicitProxyEndPoint(IPAddress.Any, WebProxyPort, true);
                end.BeforeTunnelConnectRequest += new AsyncEventHandler<TunnelConnectSessionEventArgs>(OnBeforeTunnelConnectRequest);
                Server.AddEndPoint(end);

                Server.Start();

                foreach (ProxyEndPoint endPoint in Server.ProxyEndPoints)
                {
                    Server.SetAsSystemProxy(end, ProxyProtocolType.AllHttp);
                }

                Global.WriteLine("Web Proxy started on port " + WebProxyPort);

                try
                {
                    foreach (IVezonExtensionAddon extension in Parent.Manager.GetExtensionList().ToArray())
                    {
                        IWebProxyExtension webProxyExtension = extension as IWebProxyExtension;
                        if (webProxyExtension != null)
                        {
                            webProxyExtension.OnProxyStart();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception e)
            {
                Global.WriteLine(e.ToString());
            }
        }

        private bool IsValidURL(HttpWebClient client)
        {
            string uri = client.Request.RequestUri.Host;

            if ((!uri.StartsWith("www.") &&
                !uri.StartsWith("web.") &&
                !uri.StartsWith("assetgame.") &&
                !uri.StartsWith("wiki.") &&
                !uri.EndsWith("api.roblox.com") &&
                !uri.StartsWith("roblox.com") || !uri.EndsWith("roblox.com")) &&
                !uri.EndsWith("robloxlabs.com"))
            {
                return false;
            }

            //we check the header
            HeaderCollection headers = client.Request.Headers;
            List<HttpHeader> userAgents = headers.GetHeaders("User-Agent");

            if (userAgents == null)
                return false;

            if (string.IsNullOrWhiteSpace(userAgents.FirstOrDefault().Value))
                return false;

            string ua = userAgents.FirstOrDefault().Value.ToLowerInvariant();

            //for some reason, this doesn't go through for the browser unless we look for mozilla/4.0.
            //this shouldn't break modern mozilla browsers though.
            return (ua.Contains("mozilla/4.0") || ua.Contains("roblox"));
        }

        private async Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e)
        {
            if (!IsValidURL(e.HttpClient))
            {
                e.DecryptSsl = false;
            }

            Uri uri = e.HttpClient.Request.RequestUri;

            foreach (IVezonExtensionAddon extension in Parent.Manager.GetExtensionList().ToArray())
            {
                IWebProxyExtension webProxyExtension = extension as IWebProxyExtension;
                if (webProxyExtension != null)
                {
                    if (webProxyExtension.IsValidURL(uri.AbsolutePath.ToLowerInvariant(), uri.Host))
                    {
                        try
                        {
                            await webProxyExtension.OnBeforeTunnelConnectRequest(sender, e);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private async Task OnRequest(object sender, SessionEventArgs e)
        {
            await _locker.LockAsync(async () =>
            {
                if (!IsValidURL(e.HttpClient))
                {
                    return;
                }

                Uri uri = e.HttpClient.Request.RequestUri;

                foreach (IVezonExtensionAddon extension in Parent.Manager.GetExtensionList().ToArray())
                {
                    IWebProxyExtension webProxyExtension = extension as IWebProxyExtension;
                    if (webProxyExtension != null)
                    {
                        if (webProxyExtension.IsValidURL(uri.AbsolutePath.ToLowerInvariant(), uri.Host))
                        {
                            try
                            {
                                await webProxyExtension.OnRequest(sender, e);
                                return;
                            }
                            catch (Exception)
                            {
                                e.GenericResponse("", HttpStatusCode.InternalServerError);
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                e.GenericResponse("", HttpStatusCode.NotFound);
            });
        }

        public void Stop()
        {
            try
            {
                if (!Server.ProxyRunning)
                {
                    Global.WriteLine("The web proxy is already turned off.");
                    return;
                }

                Global.WriteLine($"Web Proxy stopping on port {WebProxyPort}");

                foreach (IVezonExtensionAddon extension in Parent.Manager.GetExtensionList().ToArray())
                {
                    try
                    {
                        IWebProxyExtension webProxyExtension = extension as IWebProxyExtension;
                        if (webProxyExtension != null)
                        {
                            webProxyExtension.OnProxyStopped();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                Server.BeforeRequest -= new AsyncEventHandler<SessionEventArgs>(OnRequest);
                Server.Stop();
            }
            catch
            {
            }
        }
    }
}