using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;

namespace DCIMSServiceSimulator.Utils
{
    /// <summary>
    /// WCF服务启动辅助类
    /// 使用示例：
    /// WcfServer wcfServer = new WcfServer("localhost", 8733, typeof(ClientService), typeof(CommonService));
    /// wcfServer.BindingType = WcfServer.WcfBindingType.NetTcp;
    /// wcfServer.Start();
    /// </summary>
    public class WcfServer
    {
        /// <summary>
        /// WCF绑定类型
        /// </summary>
        public enum WcfBindingType
        {
            HTTP = 1,
            NetTcp = 2
        }

        private String host;
        private Int32 listenPort;
        private Type[] wcfServiceTypes;
        private Dictionary<Type, ServiceHost> serviceHostDict;

        private WcfBindingType _BindingType = WcfBindingType.HTTP;
        /// <summary>
        /// WCF绑定类型
        /// </summary>
        public WcfBindingType BindingType
        {
            get { return _BindingType; }
            set { _BindingType = value; }
        }

        /// <summary>
        /// 服务绑定事件参数
        /// </summary>
        public class ServiceBindingEventArgs : EventArgs
        {
            /// <summary>
            /// 契约接口
            /// </summary>
            public Type ContractInterface { get; set; }
            /// <summary>
            /// 服务实现类
            /// </summary>
            public Type ServiceClass { get; set; }
            /// <summary>
            /// 地址(除了协议，主机，端口部分)
            /// </summary>
            public String Address { get; set; }
        }
        /// <summary>
        /// 服务绑定时
        /// </summary>
        public event EventHandler<ServiceBindingEventArgs> ServiceBinding;

        /// <summary>
        /// WcfServer构造函数，将扫描传入的程序集中所有的WCF服务类
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="listenPort">监听端口</param>
        /// <param name="assembly">要扫描的程序集</param>
        public WcfServer(String host, Int32 listenPort, Assembly assembly)
        {
            init(host, listenPort, getWcfTypes(assembly));
        }

        /// <summary>
        /// WcfServer构造函数
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="listenPort">监听端口</param>
        /// <param name="wcfServiceTypes">WCF服务类型数组</param>
        public WcfServer(String host, Int32 listenPort, params Type[] wcfServiceTypes)
        {
            init(host, listenPort, wcfServiceTypes);
        }
        private void init(String host, Int32 listenPort, Type[] wcfServiceTypes)
        {
            this.host = host;
            this.listenPort = listenPort;
            this.wcfServiceTypes = wcfServiceTypes;
            serviceHostDict = new Dictionary<Type, ServiceHost>();
        }

        //得到包括了 ServiceContractAttribute 自定义属性的类型
        private Type getContractType(Type type)
        {
            if (type == null) return null;
            //查看自身
            if (type.GetCustomAttributes(typeof(ServiceContractAttribute), false).Length != 0)
            {
                return type;
            }
            //搜索接口
            foreach (Type typeInterface in type.GetInterfaces())
            {
                Type tmpSub = getContractType(typeInterface);
                if (tmpSub != null) return tmpSub;
            }
            //搜索基类
            Type tmp = getContractType(type.BaseType);
            if (tmp != null) return tmp;

            return null;
        }

        private Type[] getWcfTypes(Assembly assembly)
        {
            List<Type> wcfTypeList = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract) continue;
                if (getContractType(type) != null)
                {
                    wcfTypeList.Add(type);
                }
            }
            return wcfTypeList.ToArray();
        }

        private ServiceHost start(Type wcfType)
        {
            Type interfaceType = getContractType(wcfType);

            String protocolPrefix = null;
            ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
            Binding mexBinding = null;

            switch (BindingType)
            {
                case WcfBindingType.HTTP:
                    protocolPrefix = "http";
                    serviceBehavior.HttpGetEnabled = true;
                    mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
                    break;
                case WcfBindingType.NetTcp:
                    protocolPrefix = "net.tcp";
                    mexBinding = MetadataExchangeBindings.CreateMexTcpBinding();
                    break;
            }

            //事件参数
            ServiceBindingEventArgs args = new ServiceBindingEventArgs();
            args.Address = interfaceType.Name;
            args.ContractInterface = interfaceType;
            args.ServiceClass = wcfType;
            //触发“服务绑定时”事件
            if (ServiceBinding != null) ServiceBinding(this, args);

            //得到URL
            String url = String.Format("{0}://{1}:{2}/{3}/", protocolPrefix, host, listenPort, args.Address);
            System.ServiceModel.ServiceHost serviceHost = new System.ServiceModel.ServiceHost(wcfType, new Uri(url));
            serviceHost.Description.Behaviors.Add(serviceBehavior);
            ServiceEndpoint mexEndpoint = new ServiceMetadataEndpoint(mexBinding, new EndpointAddress(url + "mex"));
            serviceHost.AddServiceEndpoint(mexEndpoint);

            serviceHost.AddDefaultEndpoints();
            //打开
            serviceHost.Open();
            return serviceHost;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            foreach (Type wcfType in wcfServiceTypes)
            {
                ServiceHost serviceHost = start(wcfType);
                serviceHostDict.Add(wcfType, serviceHost);
                Debug.Print(String.Format("WCF服务[{0}]启动完成，地址：", wcfType.FullName));
                foreach (Uri uri in serviceHost.BaseAddresses)
                {
                    Debug.Print(uri.ToString());
                }
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            foreach (Type wcfType in serviceHostDict.Keys)
            {
                ServiceHost serviceHost = serviceHostDict[wcfType];
                serviceHost.Close();
                Debug.Print(String.Format("WCF服务[{0}]已停止，地址：", wcfType.FullName));
                foreach (Uri uri in serviceHost.BaseAddresses)
                {
                    Debug.Print(uri.ToString());
                }
            }
        }
    }
}
