using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using X.PagedList;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using System;
using GoodCrud.Domain.Contract.Interfaces;
using GoodCrud.Data.Contract.Interfaces;
using GoodCrud.Application.Contract.Dtos;

namespace GoodCrud.Application.Services
{
    public abstract class EntityService<E, ShowT, CreateT, UpdateT, FilterT>
    where E : class, IIdentifiable
    where ShowT : class
    where FilterT : FilterDto
    {
        public readonly IMapper Mapper;
        public readonly IBaseUnitOfWork Uow;
        public readonly IValidator<E> Validator;
        public IRepo<E> Repo { get; set; }

        protected int PageSize = 50;

        public EntityService(IBaseUnitOfWork uow, IMapper mapper, IValidator<E> validator)
        {
            Uow = uow;
            Mapper = mapper;
            Validator = validator;
            Repo = Uow.GetRepo<E>();
        }

        public async Task<PagedListDto<ShowT>> ListAsync(FilterT filter)
        {
            int pageNumber = (filter.Page ?? 1);
            var query = (await ListFilterAsync(filter));
            var pagedList = await query.ToPagedListAsync(pageNumber, PageSize);

            // IQuery use this:
            // var total = await query.CountAsync();

            // List<ShowT> showList = null;
            // if (total > 0)
            // {
            //     var list = await query.Skip(PageSize * (pageNumber - 1)).Take(PageSize).SelectAsync();
            //     showList = list.Select(e => EntityDto(e)).ToList();
            // }
            // else { showList = new List<ShowT>(); }

            // var pagedList = new StaticPagedList<ShowT>(showList, pageNumber, PageSize, total);

            var metaData = Mapper.Map<PagedListOpenMetaData>(pagedList.GetMetaData());
            var list = pagedList.Select(e => EntityDto(e)).ToList();
            return new PagedListDto<ShowT>()
            {
                List = list,
                MetaData = metaData,
            };
        }

        public virtual async Task<IQueryable<E>> ListFilterAsync(FilterT filter)
        {
            return await Task.FromResult<IQueryable<E>>(Repo.Queryable());
        }

        public async Task<ResultDto<ShowT>> GetAsync(int id)
        {
            var entity = await Repo.FindAsync(id);
            if (entity == null) { return ResultDto<ShowT>.NotFound(); }
            return ResultDto<ShowT>.Succeed(EntityDto(entity));
        }

        public async Task<ResultDto<ShowT>> CreateAsync(CreateT dto, Action<ValidationResult> func = null)
        {
            var entity = Mapper.Map<E>(dto);
            var result = await CreateCallbackAsync(dto, entity);
            if (result != null) { return result; }

            var validationResult = Validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                if (func != null) { func(validationResult); }
                return ResultDto<ShowT>.Failed(validationResult.ToString());
            }

            Repo.Insert(entity);
            await Uow.SaveChangesAsync();

            return ResultDto<ShowT>.Succeed(EntityDto(entity), $"{typeof(E).Name} #{entity.Id} created.");
        }

        public async Task<List<ResultDto<ShowT>>> BulkCreateAsync(List<CreateT> dtoList)
        {
            var outList = new List<ResultDto<ShowT>>();
            foreach (var dto in dtoList)
            {
                outList.Add(await CreateAsync(dto));
            }
            return outList;
        }
        public virtual async Task<ResultDto<ShowT>> CreateCallbackAsync(CreateT dto, E entity)
        {
            return await Task.FromResult<ResultDto<ShowT>>(null);
        }

        public async Task<ResultDto<ShowT>> UpdateAsync(int id, UpdateT dto, Action<ValidationResult> func = null)
        {
            var entity = await Repo.FindAsync(id);
            if (entity == null) { return ResultDto<ShowT>.NotFound(); }

            Mapper.Map(dto, entity);
            var result = await UpdateCallbackAsync(dto, entity);
            if (result != null) { return result; }

            var validationResult = Validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                if (func != null) { func(validationResult); }
                return ResultDto<ShowT>.Failed(validationResult.ToString());
            }

            await Repo.UpdateAndSaveAsync(entity);

            return ResultDto<ShowT>.Succeed(EntityDto(entity), $"{typeof(E).Name} #{entity.Id} updated.");
        }

        public virtual async Task<ResultDto<ShowT>> UpdateCallbackAsync(UpdateT dto, E entity)
        {
            return await Task.FromResult<ResultDto<ShowT>>(null);
        }

        public async Task<ResultDto<ShowT>> DeleteAsync(int id)
        {
            var entity = await Repo.FindAsync(id);
            if (entity == null) { return ResultDto<ShowT>.NotFound(); }
            var result = await DeleteCallbackAsync(entity);
            if (result != null) { return result; }

            await Repo.DeleteAndSaveAsync(entity);

            return ResultDto<ShowT>.Succeed(EntityDto(entity), $"{typeof(E).Name} #{entity.Id} deleted.");
        }

        public virtual async Task<ResultDto<ShowT>> DeleteCallbackAsync(E entity)
        {
            return await Task.FromResult<ResultDto<ShowT>>(null);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Repo.ExistsAsync(id);
        }

        public ShowT EntityDto(E e)
        {
            return Mapper.Map<ShowT>(e);
        }

        public UpdateT UpdateDto(E e)
        {
            return Mapper.Map<UpdateT>(e);
        }

    }
}