using System;
using System.Collections.Generic;
using System.Text;
using DatabaseKeeperCore;

namespace DatabaseKeeper
{
    public class KeeperProviderController
    {
        public IDictionary<String, IDatabaseKeeperProvider> DatabaseKeeperProviderDict { get; set; }

        /// <summary>
        /// 得到Provider的名称列表
        /// </summary>
        /// <returns></returns>
        public String[] GetProvidersNameArray()
        {
            List<String> nameList = new List<string>();

            foreach (var key in DatabaseKeeperProviderDict.Keys)
            {
                nameList.Add(key);
            }
            return nameList.ToArray();
        }

        public IDatabaseKeeperProvider GetProvider(String name)
        {
            return DatabaseKeeperProviderDict[name];
        }
    }
}
