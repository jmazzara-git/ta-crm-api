using AutoMapper;
using TACRM.Services.Dtos;
using TACRM.Services.Entities;

namespace TACRM.Services
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Contact, ContactDto>().ReverseMap();
		}
	}
}
