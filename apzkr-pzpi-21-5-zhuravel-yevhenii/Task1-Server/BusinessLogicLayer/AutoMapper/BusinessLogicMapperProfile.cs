using AutoMapper;
using BusinessLogicLayer.Entities.AnimalCenters;
using BusinessLogicLayer.Entities.AnimalFeeders;
using BusinessLogicLayer.Entities.Animals;
using BusinessLogicLayer.Entities.AnimalTypes;
using BusinessLogicLayer.Entities.Feeders;
using BusinessLogicLayer.Entities.GeoPoints;
using BusinessLogicLayer.Entities.Sensors;
using BusinessLogicLayer.Entities.SensorTypes;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.AutoMapper
{
    public class BusinessLogicMapperProfile : Profile
    {
        public BusinessLogicMapperProfile()
        {
            CreateMap<AnimalCenter, AnimalCenterDto>().ReverseMap();
            CreateMap<AnimalCenter, AnimalCenterCreationDto>()
                .ForMember(ac => ac.Password, config => config.MapFrom(ac => ac.PasswordHash))
                .ReverseMap();
            CreateMap<AnimalCenter, AnimalCenterIdDto>().ReverseMap();
            CreateMap<AnimalCenter, AnimalCenterUpdateDto>().ReverseMap();

            CreateMap<AnimalFeeder, AnimalFeederDto>().ReverseMap();
            CreateMap<AnimalFeeder, AnimalFeederCreationDto>().ReverseMap();
            CreateMap<AnimalFeeder, AnimalFeederIdDto>().ReverseMap();
            CreateMap<AnimalFeeder, AnimalFeederUpdateDto>().ReverseMap();

            CreateMap<Animal, AnimalDto>().ReverseMap();
            CreateMap<Animal, AnimalCreationDto>().ReverseMap();
            CreateMap<Animal, AnimalIdDto>().ReverseMap();
            CreateMap<Animal, AnimalUpdateDto>().ReverseMap();

            CreateMap<AnimalType, AnimalTypeDto>().ReverseMap();
            CreateMap<AnimalType, AnimalTypeCreationDto>().ReverseMap();
            CreateMap<AnimalType, AnimalTypeIdDto>().ReverseMap();
            CreateMap<AnimalType, AnimalTypeUpdateDto>().ReverseMap();

            CreateMap<Feeder, FeederDto>().ReverseMap();
            CreateMap<Feeder, FeederCreationDto>().ReverseMap();
            CreateMap<Feeder, FeederIdDto>().ReverseMap();
            CreateMap<Feeder, FeederUpdateDto>().ReverseMap();

            CreateMap<Sensor, SensorDto>().ReverseMap();
            CreateMap<Sensor, SensorCreationDto>().ReverseMap();
            CreateMap<Sensor, SensorIdDto>().ReverseMap();
            CreateMap<Sensor, SensorUpdateDto>().ReverseMap();

            CreateMap<SensorType, SensorTypeDto>().ReverseMap();
            CreateMap<SensorType, SensorTypeCreationDto>().ReverseMap();
            CreateMap<SensorType, SensorTypeIdDto>().ReverseMap();
            CreateMap<SensorType, SensorTypeUpdateDto>().ReverseMap();

            CreateMap<GeoPoint, GeoPointDto>().ReverseMap();
            CreateMap<GeoPoint, GeoPointCreationDto>().ReverseMap();
            CreateMap<GeoPoint, GeoPointIdDto>().ReverseMap();
            CreateMap<GeoPoint, GeoPointUpdateDto>().ReverseMap();
        }
    }
}
