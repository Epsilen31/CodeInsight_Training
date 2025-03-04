using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using MapsterMapper;
using MediatR;

namespace BillingAndSubscriptionSystem.Services.Features.Menu
{
    public class GetSidebarMenu
    {
        public class Query : IRequest<ICollection<MenuDto>>
        {
            public string Role { get; }

            public Query(string role)
            {
                Role = role;
            }
        }

        public class Handler : IRequestHandler<Query, ICollection<MenuDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ICollection<MenuDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var menus = await _unitOfWork.MenuRepository.GetSidebarMenuAsync(
                    request.Role,
                    cancellationToken
                );

                return _mapper.Map<ICollection<MenuDto>>(menus);
            }
        }
    }
}
