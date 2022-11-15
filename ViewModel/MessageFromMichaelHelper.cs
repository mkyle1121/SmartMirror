using SmartMirror.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.ViewModel
{
    public class MessageFromMichaelHelper
    {
        private static string messageFromMichaelUri = "https://mikesmartmirror.blob.core.windows.net/messagefrommichael/MessageFromMichael.txt?sp=r&st=2022-10-09T22:16:08Z&se=2032-10-10T06:16:08Z&spr=https&sv=2021-06-08&sr=b&sig=rtOsfhHl60G8uac5hRtx4VAsJPVywD%2F9NIXZvWahhZg%3D";

        public static async Task<string> GetMessageFromMichaelAsync()
        {
            try
            {
                using var client = new HttpClient();            
                var messageFromMichael = await client.GetStringAsync(messageFromMichaelUri);
                return messageFromMichael;
            }
            catch (Exception e)
            {
                var file = new StreamWriter("smartmirror.log", true);
                file.WriteLine(e.Message);
                file.Close();
                return string.Empty;
            }
            
        }
    }
}
