using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Newtonsoft.Json.Linq;
/// <summary>
/// Nuget 에서 Rhino3dmIO, Newtonsoft.Json 패키지 설치
/// </summary>


namespace HttpListnerTest
{
    class Program
    {
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx
        /// 예제 코드 약간 수정
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SimpleListenerExample(new string[] { "http://localhost:8080/" });
        }

        /// <summary>
        /// Rhino Geometry Test Function
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Point3d CenterPoint(Line line)
        {
            return line.From + line.Direction * 0.5;
        }


        public static void SimpleListenerExample(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.

            //-----------------------------추가한부분------------------------------//

            //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            //

            Line line = new Line(new Point3d(0, 0, 0), new Point3d(2, 5, 3));
            var json = JObject.FromObject(CenterPoint(line));
            
            var linefrom = JObject.FromObject(line.From);
            var lineto = JObject.FromObject(line.To);

            var resultjson = new JObject();
            resultjson.Add("from", linefrom);
            resultjson.Add("to", lineto);
            resultjson.Add("center", json);

            string responseString = resultjson.ToString();

            //-----------------------------추가한부분------------------------------//

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            listener.Stop();
        }
    }
}
