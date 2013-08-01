//-------------------------
// 作者：张鹏
// 邮箱：scbeta@qq.com
// 时间：2013-8-1
//-------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;

namespace DCIMSServiceSimulator.Utils
{
    /// <summary>
    /// WCF客户端辅助类
    /// 使用示例：
    /// //得到Service对象
    /// IClientService client = WcfClientUtils.GetClient<IClientService>(wcfAddress);
    /// //使用Service对象
    /// Console.WriteLine("最后修改时间:" + client.GetFileLastModified("waterleak/1F13-15.xaml"));
    /// //释放Service对象
    ///WcfClientUtils.ReleaseClient(client);
    /// </summary>
    public class WcfClientUtils
    {
        //用于保存WCF Service接口与动态生成的实现类的字典
        private static Dictionary<Type, Type> serviceInterfaceClientDict = new Dictionary<Type, Type>();

        /// <summary>
        /// 初始化根据Visual Studio生成的ServiceClient
        /// </summary>
        /// <typeparam name="TServiceInterface"></typeparam>
        /// <typeparam name="TServiceClient"></typeparam>
        /// <param name="remoteAddress"></param>
        /// <returns></returns>
        public static TServiceInterface GetClient<TServiceInterface>(String remoteAddress)
            where TServiceInterface : class
        {
            System.ServiceModel.Channels.Binding binding = null;
            if (remoteAddress.StartsWith("http://"))
            {
                binding = new BasicHttpBinding();
            }
            else if (remoteAddress.StartsWith("net.tcp://"))
            {
                binding = new NetTcpBinding();
            }
            else
            {
                throw new NotSupportedException("不支持的协议前缀。" + remoteAddress);
            }
            return (TServiceInterface)Activator.CreateInstance(GetServiceClientClass(typeof(TServiceInterface)), binding, new EndpointAddress(new Uri(remoteAddress)));
        }

        /// <summary>
        /// 释放客户端
        /// </summary>
        /// <param name="obj"></param>
        public static void ReleaseClient(Object obj)
        {
            ICommunicationObject communicationObject = obj as ICommunicationObject;
            if (communicationObject != null)
            {
                communicationObject.Close();
            }
            IDisposable disposableObject = obj as IDisposable;
            if (disposableObject != null)
            {
                disposableObject.Dispose();
            }
        }

        //得到类型的字符串
        private static String GetTypeString(Type type)
        {
            if (type.IsGenericType)
            {
                String genericTypeString = type.GetGenericTypeDefinition().FullName;
                genericTypeString = genericTypeString.Substring(0, genericTypeString.IndexOf("`"));

                String rtnStr = genericTypeString + "<";
                Type[] genericArguments = type.GetGenericArguments();
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    if (i > 0) rtnStr += ", ";
                    rtnStr += GetTypeString(genericArguments[i]);
                }
                rtnStr += ">";
                return rtnStr;
            }
            else
            {
                return type.FullName;
            }
        }

        /// <summary>
        /// 得到服务的Client类
        /// </summary>
        /// <param name="serviceInterface"></param>
        /// <returns></returns>
        private static Type GetServiceClientClass(Type serviceInterface)
        {
            if (serviceInterfaceClientDict.ContainsKey(serviceInterface))
            {
                return serviceInterfaceClientDict[serviceInterface];
            }
            //生成源代码
            StringBuilder sb = new StringBuilder();

            //接口的名称
            String serviceInterfaceName = serviceInterface.Name;
            //接口的完整名称
            String serviceInterfaceFullName = serviceInterface.FullName;
            //动态命名空间的名称
            String dynamicNamespaceName = "DynamicNamespace_" + Guid.NewGuid().ToString().Replace("-", "");
            //动态ServiceClient类名
            String serviceClientClassName = serviceInterfaceName + "Client";

            //命名空间
            sb.Append("namespace ").Append(dynamicNamespaceName).AppendLine(" {");
            //添加IXXXChannel接口
            sb.AppendLine(String.Format(@"public interface {0}Channel : {1}, System.ServiceModel.IClientChannel {2}", serviceInterfaceName, serviceInterfaceFullName, "{}"));
            //====
            //添加ServiceClient类
            //====
            sb.AppendLine(String.Format("public partial class {0} : System.ServiceModel.ClientBase<{1}>, {1} {2}", serviceClientClassName, serviceInterfaceFullName, "{"));
            //添加构造函数
            sb.AppendLine(String.Format(@"public {0}(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : base(binding, remoteAddress) {1}", serviceClientClassName, "{}"));
            //添加实现方法
            foreach (System.Reflection.MethodInfo methodInfo in serviceInterface.GetMethods())
            {
                //如果在方法上没有定义 OperationContractAttribute
                if (methodInfo.GetCustomAttributes(typeof(OperationContractAttribute), true).Length == 0) continue;
                String methodName = methodInfo.Name;
                String methodReturnTypeString = GetTypeString(methodInfo.ReturnType);
                if (methodInfo.ReturnType.Equals(typeof(void)))
                {
                    methodReturnTypeString = "void";
                }
                sb.Append(String.Format("public {0} {1}(", methodReturnTypeString, methodName));
                //添加方法参数
                ParameterInfo[] methodParameters = methodInfo.GetParameters();
                for (int i = 0; i < methodParameters.Length; i++)
                {
                    if (i > 0) sb.Append(", ");
                    ParameterInfo parameterInfo = methodParameters[i];
                    sb.Append(GetTypeString(parameterInfo.ParameterType)).Append(" ").Append(parameterInfo.Name);
                }
                //如果没有返回值
                if (methodReturnTypeString == "void")
                {
                    sb.Append(String.Format(") {0} base.Channel.{1}(", "{", methodName));
                }
                //如果有返回值
                else
                {
                    sb.Append(String.Format(") {0} return base.Channel.{1}(", "{", methodName));
                }
                for (int i = 0; i < methodParameters.Length; i++)
                {
                    if (i > 0) sb.Append(", ");
                    ParameterInfo parameterInfo = methodParameters[i];
                    sb.Append(parameterInfo.Name);
                }
                sb.AppendLine("); }");
            }
            //类结束
            sb.AppendLine("}");
            //命名空间结束
            sb.AppendLine("}");

            //动态编译
            System.CodeDom.Compiler.CodeDomProvider csharpCodeDomProvider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp");
            System.CodeDom.Compiler.CompilerParameters compilerParameters = new System.CodeDom.Compiler.CompilerParameters();
            compilerParameters.GenerateInMemory = true;
            compilerParameters.OutputAssembly = "DynamicAssembly_" + Guid.NewGuid().ToString().Replace("-", "");

            Assembly[] loadedAssemblys = AppDomain.CurrentDomain.GetAssemblies();
            //添加程序集的引用
            foreach (AssemblyName assemblyName in serviceInterface.Assembly.GetReferencedAssemblies())
            {
                foreach (Assembly assembly in loadedAssemblys)
                {
                    if (assembly.GetName().FullName == assemblyName.FullName)
                    {
                        //如果此程序集没有加载路径，则不添加此引用
                        if (String.IsNullOrEmpty(assembly.Location)) continue;
                        compilerParameters.ReferencedAssemblies.Add(assembly.Location);
                        break;
                    }
                }
            }
            compilerParameters.ReferencedAssemblies.Add(serviceInterface.Assembly.Location);
            //开始编译
            System.CodeDom.Compiler.CompilerResults results = csharpCodeDomProvider.CompileAssemblyFromSource(compilerParameters, sb.ToString());

            if (results.Errors.Count > 0)
            {
                StringBuilder sbError = new StringBuilder();
                sbError.AppendLine("编译失败！");
                foreach (var error in results.Errors)
                {
                    sbError.AppendLine(error.ToString());
                }
                throw new Exception(sbError.ToString());
            }
            Type clientType = results.CompiledAssembly.GetType(dynamicNamespaceName + "." + serviceClientClassName);
            serviceInterfaceClientDict.Add(serviceInterface, clientType);
            return clientType;
        }
    }
}
