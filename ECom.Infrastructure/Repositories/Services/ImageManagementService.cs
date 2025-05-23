using ECom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure.Repositories.Services
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider fileProvider;

        public ImageManagementService(IFileProvider fileProvider )
        {
            this.fileProvider = fileProvider;
        }

        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var  SaveImageSrc = new List<string>();

            var ImageDirectory = Path.Combine("wwwroot","Images",src);

            if(Directory.Exists(ImageDirectory) is not true )
            {
                Directory.CreateDirectory(ImageDirectory);
            }
            foreach( var item in files)
            {
                if (item.Length>0)
                {
                    var imageName = item.FileName;
                    var imagesrc =$"/Images/{src}/{imageName}";

                    var root = Path.Combine (ImageDirectory, imageName);
                    using (FileStream stream = new FileStream(root,FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(imagesrc);
                }
            }
            return SaveImageSrc;

        }

        public void DeleteImageAsync(string src)
        {

            var info = fileProvider.GetFileInfo(src);
            if (info.Exists)
            {
                 File.Delete(info.PhysicalPath);
            }





        }
    }
}
