using System.Data;
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

                    if (!IsValidFile(file, out _))
                        return "Invalid file. Only Excel files (.xls or .xlsx) are allowed.";

                    if (file == null)
                        throw new CustomException("File cannot be null.");

                    using var reader = await GetReader(file, cancellationToken);

                    var dataTable = GetDataTable(reader);

                    int uploadedCount = await UploadUsersFromExcel(dataTable, cancellationToken);

                    await _notificationService.SendProgressData(request.UserId);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return $"{uploadedCount} user(s) uploaded successfully";
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error uploading Excel users: {Message}",
                        exception.Message
                    );
                    throw new CustomException("Error uploading Excel users.", exception);
                }
            }

            private bool IsValidFile(IFormFile? file, out string fileExtension)
            {
                fileExtension = string.Empty;
                if (file == null || file.Length == 0)
                    return false;

                var allowedExtensions = new[] { ".xls", ".xlsx" };
                fileExtension = Path.GetExtension(file.FileName).ToLower();

                return allowedExtensions.Contains(fileExtension);
            }

            private async Task<IExcelDataReader> GetReader(
                IFormFile file,
                CancellationToken cancellationToken
            )
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
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

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return ExcelReaderFactory.CreateReader(fileStream);
            }

            private DataTable GetDataTable(IExcelDataReader reader)
            {
                var dataset = reader.AsDataSet(
                    new ExcelDataSetConfiguration
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) =>
                            new ExcelDataTableConfiguration() { UseHeaderRow = true },
                    }
                );

                return dataset.Tables[0];
            }

            private async Task<int> UploadUsersFromExcel(
                DataTable dataTable,
                CancellationToken cancellationToken
            )
            {
                int uploadedCount = 0;
                for (int index = 0; index < dataTable.Rows.Count; index++)
                {
                    var row = dataTable.Rows[index];
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

                    var newUser = _mapper.Map<BillingAndSubscriptionSystem.Entities.Entities.User>(
                        newUserDto
                    );
                    await _unitOfWork.UserRepository.AddUserAsync(newUser, cancellationToken);
                    uploadedCount++;
                }

                return uploadedCount;
            }
        }
    }
}
