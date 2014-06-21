using Protocol.Examples.Shared;
using Protocol.ProtoBuffers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Examples.Shared
{
    public class HttpRequestHelper
    {
        public static TResponse HttpPostRequestWithProtoContent<TParameter, TResponse>(String httpEndpointAddress, IWebProxy proxy, TParameter parameter)
        {
            System.Net.WebResponse response = null;
            System.Net.WebRequest request = null;
            TResponse responseObject;
            byte[] requestBody = null;

            if (parameter != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Serializers.Serialize(memoryStream, parameter);
                    requestBody = memoryStream.ToArray();
                }
            }

            request = System.Net.WebRequest.Create(httpEndpointAddress);
            request.ContentType = "application/proto";
            request.Method = "POST";
            request.Proxy = proxy;
            
            if (requestBody != null)
            {
                request.ContentLength = requestBody.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(requestBody, 0, requestBody.Length);
                }
            }
            else
            {
                request.ContentLength = 0;
            }

            try
            {
                response = request.GetResponse();

                if (!"application/proto".Equals(response.ContentType))
                {
                    throw new InvalidProgramException();
                }

                using (Stream stream = response.GetResponseStream())
                {
                    responseObject = Serializers.Deserialize<TResponse>(stream);
                }

                return responseObject;
            }
            catch(WebException exception)
            {
                if (exception.Response != null && "application/proto".Equals(exception.Response.ContentType))
                {
                    using (Stream stream = exception.Response.GetResponseStream())
                    {
                        ErrorReport errorReport = Serializers.Deserialize<ErrorReport>(stream);

                        if (errorReport == null)
                            throw;

                        throw new ServiceCallException(errorReport.Code, errorReport.Message, exception);
                    }                   
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
