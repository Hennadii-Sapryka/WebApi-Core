using AutoMapper;
using WebApi.Models;

namespace WebApi.Mapping
{
    public class UserMapper:Profile
    {
        public UserMapper()
        {
            CreateMap<LoginUser, User>().ReverseMap();
        }
    }
}
