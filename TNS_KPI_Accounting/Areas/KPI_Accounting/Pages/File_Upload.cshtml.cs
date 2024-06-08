using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace TNS_KPI.Areas.KPI_Accounting.Pages
{
    [IgnoreAntiforgeryToken]
    public class File_UploadModel : PageModel
    {
     
        public void OnGet()
        {
        }
        [BindProperty]
        public IFormFile FileUpload { get; set; }

        [BindProperty]
        public string FileMonth { get; set; }
        public async Task OnPostAsync()
        {
            // string zFileName = Upload.FileName;
            var file = Path.Combine("D:\\Music\\", FileUpload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await FileUpload.CopyToAsync(fileStream);
            }
        }
        
        public IActionResult OnPostUpData(FileUpInfo FileData)
        {
            
            ItemView zItem = new ItemView();

           
            //zItem.Name = request.Name;
            return new JsonResult(zItem);
        }
        public class FileUpInfo
        {
            public string Name { get; set; }
            public string Size { get; set; }
        }
        public class ItemView
        {
            public string Key { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }

            public ItemView() { }

        }
    }
}
