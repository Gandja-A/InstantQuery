using AutoMapper;
using InstantQuery.Models;

namespace InstantQuery.Examples.Common
{
    public class CommonMapConf : Profile
    {
        public CommonMapConf()
        {
            this.CreateMap(typeof(ListResult<>), typeof(ListResultDto<>));
        }
    }
}
