using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MapsterMapper;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.SubMenu
{
    public class GetAllSubMenu
    {
        public class Query : IRequest<ICollection<SubMenuDto>> { }

        public class Handler : IRequestHandler<Query, ICollection<SubMenuDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ICollection<SubMenuDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var subMenus = await _unitOfWork.SubMenuRepository.GetAllActiveSubMenusAsync(
                    cancellationToken
                );

                return _mapper.Map<ICollection<SubMenuDto>>(subMenus);
            }
        }
    }
}
