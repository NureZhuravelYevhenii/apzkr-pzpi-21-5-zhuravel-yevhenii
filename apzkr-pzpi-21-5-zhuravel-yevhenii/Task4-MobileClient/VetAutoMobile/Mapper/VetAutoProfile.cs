using AutoMapper;
using VetAutoMobile.Entities.AnimalCenters;
using VetAutoMobile.Entities.Animals;
using VetAutoMobile.Entities.AnimalTypes;
using VetAutoMobile.Entities.Feeders;
using VetAutoMobile.Entities.Sensors;
using VetAutoMobile.Entities.SensorTypes;

namespace VetAutoMobile.Mapper
{
    public class VetAutoProfile : Profile
    {
        public VetAutoProfile()
        {
            CreateMap<AnimalCenter, AnimalCenterId>();
            CreateMap<AnimalCenter, AnimalCenterUpdate>();

            CreateMap<Animal, AnimalId>();
            CreateMap<Animal, AnimalUpdate>();

            CreateMap<AnimalType, AnimalTypeId>();
            CreateMap<AnimalType, AnimalTypeUpdate>();

            CreateMap<Feeder, FeederId>();
            CreateMap<Feeder, FeederUpdate>();

            CreateMap<Sensor, SensorId>();
            CreateMap<Sensor, SensorUpdate>();

            CreateMap<SensorType, SensorTypeId>();
            CreateMap<SensorType, SensorTypeUpdate>();
        }
    }
}
