using AutoMapper;
using TACRM.Services.Dtos;
using TACRM.Services.Entities;

namespace TACRM.Services
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Map from Contact to ContactDto
			CreateMap<Contact, ContactDto>()
				.ForMember(dest => dest.KidsAges, opt => opt.MapFrom(src => src.KidsAges.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()));

			// Map from ContactDto to Contact
			CreateMap<ContactDto, Contact>()
				.ForMember(dest => dest.KidsAges, opt => opt.MapFrom(src => string.Join(",", src.KidsAges)));

		}
	}
}
