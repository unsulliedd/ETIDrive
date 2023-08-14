using ETIDrive_Entity;

namespace ETIDrive_WebUI.Models.FolderModels
{
    internal class FilterViewModel
    {
        public List<Category>? FileTypes { get; set; }
        public List<string> Tags { get; set; }
    }
}