using AutoMapper;
using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Cinema_System.Services
{
    public class MovieService: IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MovieDTO> GetAllMovies()
        {
            var movies = _unitOfWork.Movies.GetAll();
            return _mapper.Map<IEnumerable<MovieDTO>>(movies);
        }

        public MovieDTO? GetMovieById(int id)
        {
            var movie = _unitOfWork.Movies.GetById(id);
            return movie == null ? null : _mapper.Map<MovieDTO>(movie);
        }

        public void CreateMovie(MovieDTO movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            _unitOfWork.Movies.Insert(movie);
            _unitOfWork.Save();
        }

        public void UpdateMovie(MovieDTO movieDto)
        {
            var movie = _unitOfWork.Movies.GetById(movieDto.MovieId);
            if (movie == null) return;

            _mapper.Map(movieDto, movie);
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Save();
        }

        public void DeleteMovie(int id)
        {
            _unitOfWork.Movies.Delete(id);
            _unitOfWork.Save();
        }
    }
}