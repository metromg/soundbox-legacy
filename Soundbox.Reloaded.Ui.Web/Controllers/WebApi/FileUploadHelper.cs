namespace Soundbox.Reloaded.Ui.Web.Controllers.WebApi
{
    using System.IO;
    using System.Net.Http;
    using System.Web;

    using Newtonsoft.Json;
    using System.Linq;
    using System;

    public class FileUploadHelper
    {
        public static T GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData
                    .GetValues(0).FirstOrDefault() ?? string.Empty);

                if (!string.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return default(T);
        }

        public static MultipartFormDataStreamProvider GetMultipartProvider()
        {
            var uploadFolder = "~/App_Data/Tmp/FileUploads";

            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);

            return new MultipartFormDataStreamProvider(root);
        }
        
        public static string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }
        
        public static string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }
}