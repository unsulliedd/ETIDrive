using ETIDrive_Entity.Identity;
using ETIDrive_Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ETIDrive_Data.Abstract;
using ETIDrive_WebUI.Models.FolderModels;

namespace ETIDrive_WebUI.ViewComponents
{
    public class FolderNavigationBarViewComponent : ViewComponent
    {
        private readonly IFolderRepository _folderRepository;
        private readonly UserManager<User> _userManager;

        public FolderNavigationBarViewComponent(IFolderRepository folderRepository, UserManager<User> userManager)
        {
            _folderRepository = folderRepository;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var folder = ViewBag.folder;

            if (folder != null)
            {
                var parentFolders = new List<Folder>();
                var currentFolder = folder;

                while (currentFolder.ParentFolderId != null)
                {
                    currentFolder = _folderRepository.GetbyId(currentFolder.ParentFolderId);
                    parentFolders.Insert(0, currentFolder);
                }

                var model = new FolderNavigationBarViewModel
                {
                    ParentFolders = parentFolders,
                    CurrentFolder = folder
                };

                return View("FolderNavigationBar", model);
            }
            return View("FolderNavigationBar");
        }

    }
}
