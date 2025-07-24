using AutoMapper;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserDTO, User>();        
        CreateMap<OrderDTO, Order>();
        CreateMap<SwordDTO, Sword>();
        //CreateMap<List<BookDTO>, List<Book>>();
    }
}