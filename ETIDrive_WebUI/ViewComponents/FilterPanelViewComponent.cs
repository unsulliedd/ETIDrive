using Microsoft.AspNetCore.Mvc;
using ETIDrive_Data.Abstract;
using ETIDrive_WebUI.Models.FolderModels;

namespace ETIDrive_WebUI.ViewComponents
{
    public class FilterPanelViewComponent : ViewComponent
    {
        private readonly IDataRepository _dataRepository;
        private readonly ICategoryRepository _categoryRepository;
        public FilterPanelViewComponent(IDataRepository dataRepository, ICategoryRepository categoryRepository)
        {
            _dataRepository = dataRepository;
            _categoryRepository = categoryRepository;
        }

        public IViewComponentResult Invoke()
        {
            var fileTypes = _categoryRepository.GetAll();
            var tags = _dataRepository.GetTags();

            var model = new FilterViewModel
            {
                FileTypes = fileTypes,
                Tags = tags
            };

            return View("FilterPanel",model);
        }
    }
}
