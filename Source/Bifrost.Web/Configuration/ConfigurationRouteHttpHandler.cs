﻿#region License
//
// Copyright (c) 2008-2013, Dolittle (http://www.dolittle.com)
//
// Licensed under the MIT License (http://opensource.org/licenses/MIT)
//
// You may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://github.com/dolittle/Bifrost/blob/master/MIT-LICENSE.txt
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
using System.Text;
using System.Web;
using Bifrost.Configuration;
using Bifrost.Web.Proxies;

namespace Bifrost.Web.Configuration
{
    public class ConfigurationRouteHttpHandler : IHttpHandler
    {
        string _configurationAsString;

        public ConfigurationRouteHttpHandler() : 
            this(Configure.Instance.Container.Get<GeneratedProxies>())
        {
        }

        public ConfigurationRouteHttpHandler(GeneratedProxies proxies)
        {
            var builder = new StringBuilder();

            builder.Append(GetResource("Bifrost.Web.Scripts.jquery-1.7.1.min.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.knockout-2.0.0.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.knockout.mapping-2.0.0.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.jquery.history.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.require.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.order.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.text.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.domReady.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.Bifrost.debug.js"));
            builder.Append(GetResource("Bifrost.Web.Scripts.defaultConfiguration.js"));
            builder.Append(proxies.All);

            _configurationAsString = builder.ToString();
        }

        string GetResource(string name)
        {
            var stream = typeof(ConfigurationRouteHttpHandler).Assembly.GetManifestResourceStream(name);
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            var content = UTF8Encoding.UTF8.GetString(bytes);
            return content;
        }

        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            context.Response.Write(_configurationAsString);
        }
    }
}
