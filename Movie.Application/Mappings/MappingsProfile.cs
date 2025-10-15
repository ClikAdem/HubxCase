using AutoMapper;
using Movie.Application.Dtos.Director;
using Movie.Application.Dtos.Movie;

namespace Movie.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Movie mappings
            CreateMap<Domain.Entities.Movie, MovieDto>()
                .ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src =>
                    src.Director != null ? $"{src.Director.FirstName} {src.Director.LastName}" : null));

            CreateMap<CreateMovieRequest, Domain.Entities.Movie>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Director, opt => opt.Ignore());

            CreateMap<UpdateMovieRequest, Domain.Entities.Movie>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Director, opt => opt.Ignore());

            // Director mappings
            CreateMap<Domain.Entities.Director, DirectorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<CreateDirectorRequest, Domain.Entities.Director>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateDirectorRequest, Domain.Entities.Director>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}