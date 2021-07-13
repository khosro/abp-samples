using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using DevExtremeAngular.Localization;

namespace DevExtremeAngular
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits Acme.BookStore.Web.Pages.BookStorePage
     */
    public abstract class BookStorePage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<DevExtremeAngularResource> L { get; set; }
    }
}
