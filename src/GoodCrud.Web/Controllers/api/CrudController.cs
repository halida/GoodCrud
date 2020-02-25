using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoodCrud.Domain;
using GoodCrud.Contract.Dtos;
using GoodCrud.Application.WebServices;
using GoodCrud.Contract.Interfaces;

namespace GoodCrud.Web.Api.Controllers
{
    [ApiController]
    public abstract class CrudController<Service, E, U, T, CreateT, UpdateT, FilterT> : ControllerBase
    where Service : CrudWebService<E, U, T, CreateT, UpdateT, FilterT>
    where E : BaseEntity
    where U : IBaseUnitOfWork
    where T : EntityDto
    where CreateT : class
    where UpdateT : class
    where FilterT : FilterDto

    {
        protected Service _service;

        public CrudController(Service service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<PagedListDto<T>> Gets([FromQuery] FilterT filter)
        {
            return await _service.ListAsync(filter);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ResultDto<T>> Get(int id)
        {
            return await _service.GetAsync(id);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<ResultDto<T>> Update(int id, UpdateT dto)
        {
            return await _service.UpdateAsync(id, dto);
        }

        // POST: api/Categories
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ResultDto<T>> Create(CreateT dto)
        {
            return await _service.CreateAsync(dto);
        }

        [HttpPost("Bulk")]
        public async Task<List<ResultDto<T>>> BulkCreate(List<CreateT> dtoList)
        {
            return await _service.BulkCreateAsync(dtoList);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ResultDto<T>> Delete(int id)
        {
            return await _service.DeleteAsync(id);
        }

    }
}
