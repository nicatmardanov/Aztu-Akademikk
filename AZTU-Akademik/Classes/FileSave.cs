using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;

namespace AZTU_Akademik.Classes
{
    public class FileSave
    {
        public async static Task<string> Save(Microsoft.AspNetCore.Http.IFormFile attached_file, byte type)
        {


            string date = $"_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Month}_{DateTime.UtcNow.Year}_{DateTime.UtcNow.Hour}_{DateTime.UtcNow.Minute}_{DateTime.UtcNow.Second}";

            
            string file_path = Guid.NewGuid().ToString() + date + Path.GetExtension(attached_file.FileName);

            string folder_path = "Assets/";

            switch (type)
            {
                case 0: folder_path += "Dissertation"; break;
                case 1: folder_path += "Language"; break;
                case 2: folder_path += "Certificate"; break;
                case 3: folder_path += "ContactType"; break;
                case 4: folder_path += "ProfilePicture"; break;
                case 5: folder_path += "Article"; break;
            }


            var path = Path.Combine(
              Directory.GetCurrentDirectory(), folder_path,
              file_path);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await attached_file.CopyToAsync(stream);
            }

            return $"/{folder_path}/{file_path}";


        }
    }
}
