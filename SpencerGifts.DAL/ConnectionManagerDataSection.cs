using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpencerGifts.ExchangeRatesUpdater.DAL
{

        public class ConnectionManagerDataSection : ConfigurationSection
        {
            /// <summary>
            /// The name of this section in the app.config.
            /// </summary>
            public const string SectionName = "ConnectionManagerDataSection";

            private const string EndpointCollectionName = "ConnectionManagerEndpoints";

            [ConfigurationProperty(EndpointCollectionName)]
            [ConfigurationCollection(typeof(ConnectionManagerEndpointsCollection), AddItemName = "add")]
            public ConnectionManagerEndpointsCollection ConnectionManagerEndpoints { get { return (ConnectionManagerEndpointsCollection)base[EndpointCollectionName]; } }
        }

        public class ConnectionManagerEndpointsCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new ConnectionManagerEndpointElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((ConnectionManagerEndpointElement)element).Name;
            }
        }

        public class ConnectionManagerEndpointElement : ConfigurationElement
        {
            [ConfigurationProperty("name", IsRequired = true)]
            public string Name
            {
                get { return (string)this["name"]; }
                set { this["name"] = value; }
            }

            [ConfigurationProperty("rateParam", IsRequired = true)]
            public string RateParam
            {
                get { return (string)this["rateParam"]; }
                set { this["rateParam"] = value; }
            }

            [ConfigurationProperty("dateParam", IsRequired = false)]
            public string DateParam
            {
                get { return (string)this["dateParam"]; }
                set { this["dateParam"] = value; }
            }

            [ConfigurationProperty("dateFormat", IsRequired = false)]
            public string DateFormat
            {
                get { return (string)this["dateFormat"]; }
                set { this["dateFormat"] = value; }
            }

            [ConfigurationProperty("query", IsRequired = false)]
            public string Query
            {
                get { return (string)this["query"]; }
                set { this["query"] = value; }
            }

            [ConfigurationProperty("executeQuery", IsRequired = false, DefaultValue = false)]
            public bool ExecuteQuery
            {
                get { return (bool)this["executeQuery"]; }
                set { this["executeQuery"] = value; }
            }
        }
  
}
