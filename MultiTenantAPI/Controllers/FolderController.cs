using Microsoft.AspNetCore.Mvc;
using MultiTenantAPI.Models;
using System.IO;

namespace MultiTenantAPI.Controllers
{
    [ApiController]
    [Route("api/folder")]
    public class FolderController : ControllerBase
    {
        [HttpPost("copy")]
        public IActionResult Copy([FromBody] CopyRequest request)
        {
            try
            {

                // Angular template folder
                //  string source = @"C:\Inetpub\vhosts\demo1.jailscorp.com\site-template";

                string source = @"D:\Source1";

                // Tenant folder
                //string destination = $@"C:\Inetpub\vhosts\demo1.jailscorp.com\{request.TenantName}";


                //  string basePath = @"C:\Inetpub\vhosts\demo1.jailscorp.com";

                string basePath = @"D:\Destination";

                int siteNumber = 1;

                while (Directory.Exists(Path.Combine(basePath, $"site{siteNumber}")))
                {
                    siteNumber++;
                }

                // Last existing site
                siteNumber--;

                string destination = Path.Combine(basePath, $"site{siteNumber}");

                // Clean destination
                DirectoryInfo dir = new DirectoryInfo(destination);

                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    subDir.Delete(true);
                }




                // Source exists?
                if (!Directory.Exists(source))
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Source folder not found.",
                        Source = source
                    });
                }

                

                // Create tenant folder
                //if (!Directory.Exists(destination))
                //{
                //    Directory.CreateDirectory(destination);
                    
                //}

                // Copy files
                CopyFolder(source, destination);

                

                return Ok(new
                {
                    Success = true,
                    Source = source,
                    Destination = destination
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        private void CopyFolder(string source, string destination)
        {
            try
            {
                // Create all directories
                foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(source, destination));
                }

                // Copy all files
                foreach (string filePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
                {
                

                    string destFile = filePath.Replace(source, destination);

                    string? destDir = Path.GetDirectoryName(destFile);

                    if (!Directory.Exists(destDir))
                    {
                        Directory.CreateDirectory(destDir!);
                    }

                    System.IO.File.Copy(filePath, destFile, true);

                  
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"CopyFolder Error : {ex.Message}",
                    ex
                );
            }
        }
    }
}