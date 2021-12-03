using AutoMapper;
using InstantQuery.Examples.DAL;

namespace InstantQuery.Examples.Orders
{
    public class OrderDetailsMapProfile : Profile
    {
        public OrderDetailsMapProfile()
        {
            this.CreateMap<Order, OrderDetailsDto>()
                .ForMember(d => d.UserFullName, opt => opt.MapFrom(c => c.User.FirstName + " " + c.User.LastName))
                .ForMember(d => d.StatusName, opt => opt.MapFrom(c => c.OrderStatus.Name));

            this.CreateMap<Order, OrderDetailsRes>();

            this.CreateMap<OrderDetailsDto, OrderDetailsRes>();
        }
    }
}
