using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TicketingSystem.Core.DTOs;

namespace TicketingSystem.UI.ModelBinders
{
    public class FromUser: ModelBinderAttribute
    {
        public FromUser(): base(typeof(CustomUserBinder)) { }
    }

    public class CustomUserBinder : IModelBinder
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public CustomUserBinder(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            return Task.CompletedTask;  
        }
    }

}
