namespace BusinessLogicLayer.Abstractions.BaseInterfaces
{
    public interface ICrudService<T, TIdDto, TDetailedDto, TCreateDto, TUpdateDto> :
        ICreateService<TDetailedDto, TCreateDto>,
        IReadByIdService<TIdDto, TDetailedDto>,
        IReadByPredicateService<T, TDetailedDto>,
        IReadAllService<TDetailedDto>,
        IUpdateService<TDetailedDto, TUpdateDto>,
        IDeleteService<TIdDto>
    {
    }
}
