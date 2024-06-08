using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TNS_WebApp.Pages.Guide
{
    public class WatchingModel : PageModel
    {
        public string GuideName { get; set; }
        public string GuiderFileVideo{ get; set; }
        public void OnGet(int id)
        {
            switch (id)
            {
                case 1:
                    GuideName = "Thay đổi thông tin cá nhân của user đăng nhập";
                    GuiderFileVideo = "ThayDoiThongTinCaNhan.mp4";
                    break;
                case 2:
                    GuideName = "Quản lý hồ sơ nhân sự";
                    GuiderFileVideo = "Quanlynhansu.mp4";
                    break;
                case 3:
                    GuideName = "Quản lý User đăng nhập";
                    GuiderFileVideo = "Quanlyusers.mp4";
                    break;
                case 4:
                    GuideName = "Truy cập && Quản lý Công văn";
                    GuiderFileVideo = "Congvan.mp4";
                    break;
            }
        }
    }
}
