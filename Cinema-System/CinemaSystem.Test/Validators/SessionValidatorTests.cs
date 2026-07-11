using Cinema_System.DTOs;
using Cinema_System.Validators;
using FluentValidation.Results;

namespace CinemaSystem.Test.Validators
{
    public class SessionValidatorTests
    {
        private readonly SessionValidator _validator;

        public SessionValidatorTests()
        {
            _validator = new SessionValidator();
        }

        private SessionDTO CreateValidSessionDTO()
        {
            return new SessionDTO
            {
                MovieTitle = "Inception",
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(16, 30, 0),
                Date = DateTime.Today.AddDays(1),
                HallId = 1,
                HallName = "Main Hall",
                TicketPrice = 12.50m
            };
        }

        [Fact]
        public void Validate_WithValidDTO_ShouldPass()
        {
            // Arrange
            var dto = CreateValidSessionDTO();

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Validate_EmptyMovieTitle_ShouldFail(string? invalidTitle)
        {
            // Arrange
            var dto = CreateValidSessionDTO();
            dto.MovieTitle = invalidTitle!;

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(SessionDTO.MovieTitle));
        }

        [Fact]
        public void Validate_StartTimeAfterEndTime_ShouldFail()
        {
            // Arrange
            var dto = CreateValidSessionDTO();
            dto.StartTime = new TimeSpan(17, 0, 0);
            dto.EndTime = new TimeSpan(16, 0, 0);

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(SessionDTO.StartTime) || e.PropertyName == nameof(SessionDTO.EndTime));
        }

        [Fact]
        public void Validate_StartTimeEqualToEndTime_ShouldFail()
        {
            // Arrange
            var dto = CreateValidSessionDTO();
            dto.StartTime = new TimeSpan(16, 0, 0);
            dto.EndTime = new TimeSpan(16, 0, 0);

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_PastDate_ShouldFail()
        {
            // Arrange
            var dto = CreateValidSessionDTO();
            dto.Date = DateTime.Today.AddDays(-1);

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(SessionDTO.Date));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Validate_InvalidHallId_ShouldFail(int invalidHallId)
        {
            // Arrange
            var dto = CreateValidSessionDTO();
            dto.HallId = invalidHallId;

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(SessionDTO.HallId));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Validate_EmptyHallName_ShouldFail(string? invalidHallName)
        {
            // Arrange
            var dto = CreateValidSessionDTO();
            dto.HallName = invalidHallName!;

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(SessionDTO.HallName));
        }

        [Theory]
        [InlineData("Hall 1")]
        [InlineData("Hall_Red")]
        [InlineData("Hall-A")]
        public void Validate_HallNameWithSpecialCharactersOrNumbers_ShouldFail(string invalidHallName)
        {
            // Arrange
            var dto = CreateValidSessionDTO();
            dto.HallName = invalidHallName;

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(SessionDTO.HallName));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Validate_InvalidTicketPrice_ShouldFail(decimal invalidPrice)
        {
            // Arrange
            var dto = CreateValidSessionDTO();
            dto.TicketPrice = invalidPrice;

            // Act
            var result = _validator.Validate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(SessionDTO.TicketPrice));
        }
    }
}
