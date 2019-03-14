using AutoMapper;
using SIL.Transcriber.Models;


namespace SIL.Transcriber.Services
{
    public class TranscriberMapperProfile : Profile
    {
        public TranscriberMapperProfile()
        {
            CreateMap<TranscriberTaskEntity, TranscriberTaskResource>().ReverseMap();
        }
    }
}