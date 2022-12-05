using SAM.NUGET.Domain.Dtos;
using SAM.NUGET.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SAM.NUGET.Services
{
    public static class ControllerHelper
    {
        public static string GetAppUserFromHttpContext(HttpContext context)
        {
            try
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    var userclaims = context.User.Claims.ToList();
                    var useremail = userclaims.Find(x => x.Type == ClaimTypes.Email).Value.ToString();

                    return useremail;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task<IFormFile> CreateFormFileAsync(SupportingDocFile sFile, string uploads)
        {
            var fileNameWithPath = uploads + sFile.FileName;
            IFormFile file = null;

            using (var fs = new FileStream(fileNameWithPath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    await fs.CopyToAsync(ms);

                    file = new FormFile(ms, 0, ms.ToArray().Length, sFile.Name, sFile.FileName);
                }
            }

            return file;
        }

        public static void DeleteFile(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
        }

        public static void SaveFileToDirectory(IFormFile file, string filename)
        {
            using (FileStream fs = System.IO.File.Create(filename))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

    }
}