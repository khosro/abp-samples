using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web.Pages.Account;

namespace DevExtremeAngular
{
    public class CustomeRegisterModel : RegisterModel
    {
        public CustomeRegisterModel(IAccountAppService accountAppService) : base(accountAppService)
        {
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await CheckSelfRegistrationAsync();

                if (IsExternalLogin)
                {
                    var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        Logger.LogWarning("External login info is not available");
                        return RedirectToPage("./Login");
                    }

                    await RegisterExternalUserAsync(externalLoginInfo, Input.EmailAddress);


                }
                else
                {
                    var result = await RegisterLocalUserAsync1();
                    if (!result)
                    {
                        Alerts.Warning(L["LoginIsNotAllowed"]);
                        return Page();
                    }
                }

                return Redirect(ReturnUrl ?? "~/"); //TODO: How to ensure safety? IdentityServer requires it however it should be checked somehow!
            }
            catch (BusinessException e)
            {
                Alerts.Danger(e.Message);
                return Page();
            }
        }

        async Task<bool> RegisterLocalUserAsync1()
        {
            ValidateModel();

            var userDto = await AccountAppService.RegisterAsync(
                new RegisterDto
                {
                    AppName = "MVC",
                    EmailAddress = Input.EmailAddress,
                    Password = Input.Password,
                    UserName = Input.UserName
                }
            );

            var result = await SignInManager.PasswordSignInAsync(
                          Input.UserName,
                          Input.Password,
                          true,
                          true);

            if (result.IsNotAllowed)
            {
                return false;
            }

            var user = await UserManager.GetByIdAsync(userDto.Id);
            await SignInManager.SignInAsync(user, isPersistent: true);
            return true;
        }
    }
}
