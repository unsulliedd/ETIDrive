using ETIDrive_Entity;

namespace ETIDrive_WebUI.Models.FolderModels
{
    public class FolderContentViewModel
    {
        public Folder Folder { get; set; }
    }

    public class FolderSubFoldersViewModel
    {
        public List<Folder> Folders { get; set; }
        public int ParentFolderId { get; set; } 
        public List<Folder> Subfolders { get; set; } 
    }

    public class FolderNavigationBarViewModel
    {
        public List<Folder> ParentFolders { get; set; }
        public Folder CurrentFolder { get; set; }
    }

}