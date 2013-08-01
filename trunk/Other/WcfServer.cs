//-------------------------
// 作者：张鹏
// 邮箱：scbeta@qq.com
// 时间：2013-8-1
//-------------------------
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
        private String baseAddress;
        private Type[] wcfServiceTypes;
        private Dictionary<Type, ServiceHost> serviceHostDict;

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
            /// <summary>
            /// 服务元数据行为
            /// </summary>
            public ServiceMetadataBehavior ServiceMetadataBehavior { get; set; }
            /// <summary>
            /// MEX终结点地址(相对于服务地址)
            /// </summary>
            public string MexEndpointAddress { get; set; }
        }
        /// <summary>
        /// 服务绑定时
        /// </summary>
        public event EventHandler<ServiceBindingEventArgs> ServiceBinding;

        /// <summary>
        /// WcfServer构造函数，将扫描传入的程序集中所有的WCF服务类
        /// </summary>
        /// <param name="baseAddress">基础地址</param>
        /// <param name="assembly">要扫描的程序集</param>
        public WcfServer(String baseAddress, Assembly assembly)
        {
            init(baseAddress, getWcfTypes(assembly));
        }

        /// <summary>
        /// WcfServer构造函数
        /// </summary>
        /// <param name="baseAddress">基础地址</param>
        /// <param name="wcfServiceTypes">WCF服务类型数组</param>
        public WcfServer(String baseAddress, params Type[] wcfServiceTypes)
        {
            init(baseAddress, wcfServiceTypes);
        }
        private void init(String baseAddress, Type[] wcfServiceTypes)
        {
            if (!baseAddress.EndsWith("/")) baseAddress += "/";
            this.baseAddress = baseAddress;
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

        //得到程序集中的所有WCF服务类
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

            if (baseAddress.StartsWith("http://"))
            {
                serviceBehavior.HttpGetEnabled = true;
                mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
            }
            else if (baseAddress.StartsWith("net.tcp://"))
            {
                mexBinding = MetadataExchangeBindings.CreateMexTcpBinding();
            }
            else
            {
                throw new NotSupportedException("不支持的协议前缀。" + baseAddress);
            }

            //事件参数
            ServiceBindingEventArgs args = new ServiceBindingEventArgs();
            args.Address = interfaceType.Name;
            args.ContractInterface = interfaceType;
            args.ServiceClass = wcfType;
            args.ServiceMetadataBehavior = serviceBehavior;
            args.MexEndpointAddress = "mex";
            //触发“服务绑定时”事件
            if (ServiceBinding != null) ServiceBinding(this, args);

            //得到URL
            String url = baseAddress + args.Address + "/";
            System.ServiceModel.ServiceHost serviceHost = new System.ServiceModel.ServiceHost(wcfType, new Uri(url));
            serviceHost.Description.Behaviors.Add(args.ServiceMetadataBehavior);
            ServiceEndpoint mexEndpoint = new ServiceMetadataEndpoint(mexBinding, new EndpointAddress(url + args.MexEndpointAddress));
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
