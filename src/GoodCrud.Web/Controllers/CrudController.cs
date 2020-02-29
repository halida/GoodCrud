using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoodCrud.Domain;
using GoodCrud.Application.Services;
using FluentValidation.AspNetCore;
using GoodCrud.Data.Contract.Interfaces;
using GoodCrud.Application.Contract.Dtos;

namespace GoodCrud.Web.Controllers
{
    [Route("[controller]")]
    public abstract class CrudController<Service, E, T, CreateT, UpdateT, FilterT> : Controller
    where Service : EntityService<E, T, CreateT, UpdateT, FilterT>
    where E : BaseEntity
    where T : EntityDto
    where CreateT : class
    where UpdateT : class
    where FilterT : FilterDto
    {
        protected Service _service;
        protected int PageSize = 50;

        public CrudController(Service service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index([FromQuery] FilterT filter)
        {
            var result = await _service.ListAsync(filter);
            return View((filter, result));
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Show(int id)
        {
            var result = await _service.GetAsync(id);
            if (result.Status == ResultStatus.NotFound) { return NotFound(); }
            return View(result.Data);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("Edit");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateT dto)
        {
            var result = await _service.CreateAsync(dto, (vr) => vr.AddToModelState(ModelState, null));
            FlashMessage(result);
            if (result.Status == ResultStatus.Failed) { return View("Edit", dto); }
            return RedirectToAction(nameof(Show), new { id = result.Data.Id });
        }

        [HttpGet("{id}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _service.Repo.FindAsync(id);
            if (entity == null) { return NotFound(); }
            return View(_service.UpdateDto(entity));
        }

        [HttpPost("{id}/Edit")]
        public async Task<IActionResult> Update(int id, UpdateT dto)
        {

            var result = await _service.UpdateAsync(id, dto, (vr) => vr.AddToModelState(ModelState, null));
            if (result.Status == ResultStatus.NotFound) { return NotFound(); }
            FlashMessage(result);
            if (result.Status == ResultStatus.Failed) { return View("Edit", dto); }
            return RedirectToAction(nameof(Show), new { id });
        }

        [HttpPost("{id}/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            FlashMessage(result);
            return RedirectToAction("Index");
        }

        protected void FlashMessage<V>(ResultDto<V> result) where V : class
        {
            if (result.Status == ResultStatus.Succeed)
            {
                TempData["alert-success"] = result.Description;
            }
            else
            {
                TempData["alert-error"] = result.Description;
            }
        }

    }
}
