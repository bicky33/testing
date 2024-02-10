namespace NorthwindWebMvc.Basic.Service
{
    public interface IProductService<TEntityDto>
    {

        Task<IEnumerable<TEntityDto>> FindAll(bool trackChanges);


        Task<TEntityDto> FindById(int id, bool trackChanges);

        void Create(TEntityDto entity);

        void Update(TEntityDto entity);

        Task Delete(TEntityDto entity);

    }
}
