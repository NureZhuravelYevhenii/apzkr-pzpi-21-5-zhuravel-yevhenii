namespace BusinessLogicLayer.Abstractions.BaseInterfaces
{
    public interface ICrudServiceWithoutLogging<T, TIdDto, TDetailedDto, TCreateDto, TUpdateDto> 
        : ICrudService<T, TIdDto, TDetailedDto, TCreateDto, TUpdateDto>
    {
    }
}
