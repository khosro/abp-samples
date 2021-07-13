using DevExtremeAngular.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace DevExtremeAngular
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class BookStorePageModel : AbpPageModel
    {
        protected BookStorePageModel()
        {
            LocalizationResourceType = typeof(DevExtremeAngularResource);
        }
    }
}