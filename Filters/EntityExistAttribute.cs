using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Recommend_Movie_System.Models.Entity;
using Recommend_Movie_System.Repository;

using System.Linq;

namespace Recommend_Movie_System.Filters
{
    public class EntityExistAttribute<T> : IActionFilter where T : class, IEntity
    {
        private readonly ApplicationContext context;

        public EntityExistAttribute(ApplicationContext context)
        {
            this.context = context;
        }

        public void OnActionExecuting(ActionExecutingContext executingContext)
        {
            int id;
            if (executingContext.ActionArguments.ContainsKey("id"))
            {
                id = (int)executingContext.ActionArguments["id"];
            }
            else
            {
                executingContext.Result = new BadRequestObjectResult("No id parameter");
                return;
            }

            var entity = this.context.Set<T>().SingleOrDefault(item => item.id.Equals(id));
            if (entity == null)
            {
                executingContext.Result = new NotFoundObjectResult("Entity not found");
            }
            else
            {
                executingContext.HttpContext.Items.Add("entity", entity);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}