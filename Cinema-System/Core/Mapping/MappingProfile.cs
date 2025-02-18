using AutoMapper;
using Cinema_System.DTOs;
using Infrastructure.Entities;

namespace Cinema_System.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();
            
            CreateMap<Movie, MovieDTO>();
            CreateMap<MovieDTO, Movie>();
            
            CreateMap<Session, SessionDTO>();
            CreateMap<SessionDTO, Session>();
            
            CreateMap<Hall, HallDTO>();
            CreateMap<HallDTO, Hall>();
            
            CreateMap<Ticket, TicketDTO>();
            CreateMap<TicketDTO, Ticket>();
            
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            
            CreateMap<Payment, PaymentDTO>();
            CreateMap<PaymentDTO, Payment>();
            
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
        }
    }
}

