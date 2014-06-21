using Protocol.Examples.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Protocol.Examples.Web
{
    public class BaseApplicationInterface : IHttpHandler
    {
        Dictionary<String, MethodInfo> serviceMethods = new Dictionary<string, MethodInfo>();
        object serviceImplementation;

        protected BaseApplicationInterface(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("implementation");
            }

            var methodInfos = serviceType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var m in methodInfos)
            {
                if (serviceMethods.ContainsKey(m.Name))
                {
                    throw new NotSupportedException("Method Overriding");
                }

                var parameters = m.GetParameters();

                if (parameters.Length > 1)
                {
                    throw new NotSupportedException("Only one or zero parameter supported");
                }

                foreach (var p in parameters)
                {
                    if (p.IsOut)
                    {
                        throw new NotSupportedException("Output parameter not supported");
                    }
                }

                serviceMethods.Add(m.Name, m);
            }

            serviceImplementation = Activator.CreateInstance(serviceType);
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/proto";

            if (! "application/proto".Equals(context.Request.ContentType))
            {
                MakeBadRequest(context);
                return;
            }

            String methodName = context.Request.PathInfo;

            if (String.IsNullOrWhiteSpace(methodName))
            {
                MakeBadRequest(context);
                return;
            }

            if (methodName.Length > 0)
            {
                methodName = methodName.Substring(1);

                MethodInfo methodInfo;
                if (serviceMethods.TryGetValue(methodName, out methodInfo))
                {
                    MethodInvokeWrapper(context, methodInfo);
                    return;
                }
                else
                {
                    MakeNotFoundRequest(context);
                    return;
                }
            }

            MakeBadRequest(context);
        }

        protected virtual void MakeBadRequest(HttpContext context)
        {
            context.Response.StatusCode = 400; // Bad Request
            context.Response.End();
        }

        protected virtual void MakeNotFoundRequest(HttpContext context)
        {
            context.Response.StatusCode = 404; // Bad Request
            context.Response.End();
        }

        private void MethodInvokeWrapper(HttpContext context, MethodInfo handler)
        {
            try
            {
                object parameter = null;

                var parameters = handler.GetParameters();
                if (parameters.Length > 0)
                {
                    parameter = ProtoBuffers.Serializers.Deserialize(context.Request.InputStream, parameters[0].ParameterType);
                }

                object response = handler.Invoke(this.serviceImplementation, new object[] { parameter });
                ProtoBuffers.Serializers.Serialize(context.Response.OutputStream, response);
            }
            catch(ServiceCallException exception)
            {
                context.Response.Clear();
                context.Response.StatusCode = 500;

                ProtoBuffers.Serializers.Serialize(context.Response.OutputStream,
                    new ErrorReport
                    {
                        Code = exception.ErrorCode,
                        Message = exception.Message
                    });
            }
            catch (Exception)
            {
                context.Response.Clear();
                context.Response.StatusCode = 500;

                ProtoBuffers.Serializers.Serialize(context.Response.OutputStream,
                    new ErrorReport
                    {
                        Code = -1,
                        Message = "Internal service error"
                    });
            }
        }
    }
}
