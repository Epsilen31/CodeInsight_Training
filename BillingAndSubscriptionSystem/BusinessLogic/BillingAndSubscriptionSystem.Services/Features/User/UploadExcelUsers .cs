using BillingAndSubscriptionSystem.Core.Exceptions;
using BillingAndSubscriptionSystem.DataAccess.Contracts;
using BillingAndSubscriptionSystem.Services.Contracts;
using BillingAndSubscriptionSystem.Services.DTOs;
using ExcelDataReader;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BillingAndSubscriptionSystem.Services.Features.User
{
    public static class UploadExcelUsers
    {
        public class Command : IRequest<string>
        {
            public IFormFile? File { get; set; }
            public string UserId { get; set; } = string.Empty;
        }

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;
            private readonly INotificationService _notificationService;

            public Handler(
                IUnitOfWork unitOfWork,
                ILogger<Handler> logger,
                IMapper mapper,
                INotificationService notificationService
            )
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _notificationService = notificationService;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var file = request.File;

                    if (file == null || file.Length == 0)
                        return "File is empty.";

                    var allowedExtensions = new[] { ".xls", ".xlsx" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                        return "Only Excel files (.xls or .xlsx) are allowed.";

                    var uploadsFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "UploadedFiles"
                    );
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var filePath = Path.Combine(uploadsFolder, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream, cancellationToken);
                    }

                    System.Text.Encoding.RegisterProvider(
                        System.Text.CodePagesEncodingProvider.Instance
                    );

                    using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    using var reader = ExcelReaderFactory.CreateReader(fileStream);

                    var dataset = reader.AsDataSet(
                        new ExcelDataSetConfiguration
                        {
                            UseColumnDataType = true,
                            ConfigureDataTable = (tableReader) =>
                                new ExcelDataTableConfiguration() { UseHeaderRow = true },
                        }
                    );

                    var dataTable = dataset.Tables[0];
                    int uploadedCount = 0;

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        var row = dataTable.Rows[i];
                        var email = row[2]?.ToString();

                        if (string.IsNullOrWhiteSpace(email))
                        {
                            _logger.LogWarning("Invalid email: {Email}", email);
                            continue;
                        }

                        var existingUser = (
                            await _unitOfWork.UserRepository.GetAllUsersAsync(cancellationToken)
                        ).FirstOrDefault(user => user.Email == email);

                        if (existingUser != null)
                        {
                            _logger.LogWarning("User already exists: {Email}", email);
                            continue;
                        }

                        var newUserDto = new UserDto
                        {
                            Name = row[1]?.ToString(),
                            Email = email,
                            Password = row[3]?.ToString(),
                            Phone = row[4]?.ToString(),
                            RoleId = int.TryParse(row[5]?.ToString(), out var roleId) ? roleId : -1,
                        };

                        var newUser =
                            _mapper.Map<BillingAndSubscriptionSystem.Entities.Entities.User>(
                                newUserDto
                            );
                        await _unitOfWork.UserRepository.AddUserAsync(newUser, cancellationToken);
                        uploadedCount++;
                    }

                    await _notificationService.SendProgressData(request.UserId);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return $"{uploadedCount} user(s) uploaded successfully";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading Excel users: {Message}", ex.Message);
                    throw new CustomException("Error uploading Excel users.", ex);
                }
            }
        }
    }
}
