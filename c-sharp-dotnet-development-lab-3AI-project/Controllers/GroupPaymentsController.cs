using System.Net;
using AutoMapper;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment_record;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.payment.dto;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using c_sharp_dotnet_development_lab_3AI_project.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace c_sharp_dotnet_development_lab_3AI_project.Controllers;

[Authorize]
[ApiController]
[Route("/groups/{groupId:guid}/payments")]
public class GroupPaymentsController : ControllerBase
{
    private readonly IMapper _mapper;

    private readonly IRepository _repository;

    public GroupPaymentsController(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("", Name = nameof(GetPayments))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PaymentReadDto> GetPayments(Guid groupId)
    {
        Guid userId = Auth.Jwt.GetUserId(User);
        if (!_repository.UserHasAccessToGroup(userId, groupId))
            return ApiResponse.NotFound;

        IEnumerable<Payment?> payments = _repository.GetPaymentsOfGroup(groupId);

        return Ok(_mapper.Map<IEnumerable<PaymentReadDto>>(payments));
    }

    [HttpGet("{paymentId:guid}", Name = nameof(GetPayment))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PaymentReadDto> GetPayment(Guid groupId, Guid paymentId)
    {
        Guid userId = Auth.Jwt.GetUserId(User);
        if (!_repository.UserHasAccessToGroup(userId, groupId))
            return ApiResponse.NotFound;

        Payment? payment = _repository.GetPaymentWithPaymentRecords(paymentId);
        if (payment == null || payment.GroupId != groupId)
            return ApiResponse.NotFound;

        return Ok(_mapper.Map<PaymentReadDto>(payment));
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PaymentReadDto> CreateGroupPayment(Guid groupId, PaymentWriteDto dto)
    {
        Group? group = _repository.GetGroupWithUsersGroups(groupId);
        if (group == null) return ApiResponse.NotFound;

        Guid userId = Auth.Jwt.GetUserId(User);
        if (!group.IncludesUser(userId)) return ApiResponse.NotFound;

        dto.PaymentRecords = dto.PaymentRecords.Where(record => record.Amount != 0).ToArray();

        if (dto.PaymentRecords.Any(record => group.UserGroups.All(userGroup => userGroup.UserId != record.UserId)))
            return ApiResponse.Create(HttpStatusCode.BadRequest,
                "PaymentRecords cannot contain UserId that is not in the group");

        Payment payment = new()
        {
            Name = dto.Name,
            Type = dto.Type,
            Description = dto.Description,
            GroupId = groupId,
            PaymentRecords = dto.PaymentRecords
                .Select(record => new PaymentRecord
                {
                    Amount = record.Amount,
                    UserId = record.UserId,
                }).ToList(),
        };

        _repository.AddPayment(payment);
        _repository.SaveChanges();

        return CreatedAtRoute(nameof(GetPayment), new { paymentId = payment.Id, groupId },
            _mapper.Map<PaymentReadDto>(payment));
    }

    [HttpPut("{paymentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<PaymentReadDto> UpdateGroupPayment(Guid groupId, Guid paymentId, PaymentWriteDto dto)
    {
        Guid userId = Auth.Jwt.GetUserId(User);

        Group? group = _repository.GetGroupWithUsersGroups(groupId);
        if (group == null || !group.IncludesUser(userId))
            return ApiResponse.NotFound;

        Payment? payment = _repository.GetPayment(paymentId);
        if (payment == null || payment.GroupId != groupId)
            return ApiResponse.NotFound;

        dto.PaymentRecords = dto.PaymentRecords.Where(record => record.Amount != 0).ToArray();

        if (dto.PaymentRecords.Any(record => group.UserGroups.All(userGroup => userGroup.UserId != record.UserId)))
            return ApiResponse.Create(HttpStatusCode.BadRequest,
                "PaymentRecords cannot contain UserId that is not in the group");

        payment.Type = dto.Type;
        payment.Description = dto.Description;
        payment.Name = dto.Name;
        payment.PaymentRecords = dto.PaymentRecords
            .Select(record => new PaymentRecord
            {
                Amount = record.Amount,
                UserId = record.UserId,
            }).ToList();

        _repository.DeletePaymentRecordsOfPayment(paymentId);
        _repository.SaveChanges();
        _repository.UpdatePayment(payment);
        _repository.SaveChanges();

        return Ok(payment);
    }
}