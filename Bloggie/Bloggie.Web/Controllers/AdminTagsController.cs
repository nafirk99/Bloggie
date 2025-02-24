using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext _dbContext;

        public AdminTagsController(BloggieDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            return RedirectToAction("List", "AdminTags");
        }

        [HttpGet]
        public IActionResult List()
        {
            var tags = _dbContext.Tags.AsQueryable();
            return View(tags);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            //var tag = _dbContext.Tags.Where(t => t.Id == id).FirstOrDefault();
            var tag = _dbContext.Tags.Find(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = _dbContext.Tags.Find(editTagRequest.Id);

            if (tag != null) 
            { 
                tag.Name = editTagRequest.Name;
                tag.DisplayName = editTagRequest.DisplayName;
                _dbContext.SaveChanges();
                return RedirectToAction("List", "AdminTags");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = _dbContext.Tags.Find(editTagRequest.Id);
            if(tag != null)
            {
                _dbContext.Tags.Remove(tag);
                _dbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id});
        }

        [HttpGet]
        public IActionResult DeleteFront(Guid id)
        {
            var tag = _dbContext.Tags.Find(id);
            if (tag != null)
            {
                _dbContext.Tags.Remove(tag);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("List", "AdminTags");
        }
    }
}

